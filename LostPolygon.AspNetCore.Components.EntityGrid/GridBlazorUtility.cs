using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GridBlazor;
using GridBlazor.Columns;
using GridCore.Server;
using GridMvc.Server;
using GridShared;
using GridShared.Columns;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace LostPolygon.AspNetCore.Components.EntityGrid {
    public static class GridBlazorUtility {
        public static IReadOnlyDictionary<T, IReadOnlyList<(IGridColumn<T> column, object displayValue)>>
            GetGridDisplayValues<T>(IGrid grid, IReadOnlyList<T> items, Dictionary<IGridColumn<T>, Func<T, string>>? customColumnValueGetters = null) where T : notnull {
            IGridColumnCollection columns = grid.Columns;

            var rawResult = new List<List<(IGridColumn<T> column, object displayValue)>>(items.Count);
            foreach (T unused in items) {
                rawResult.Add(new List<(IGridColumn<T> column, object displayValue)>());
            }

            foreach (GridColumnBase<T> gridColumn in columns) {
                for (int i = 0; i < items.Count; i++) {
                    T item = items[i];
                    string value = "";
                    if (gridColumn.HasConstraint) {
                        value = gridColumn.GetValue(item).Value;
                    }
                    else if (customColumnValueGetters != null) {
                        if (customColumnValueGetters.TryGetValue(gridColumn, out Func<T, string>? customValueGetter)) {
                            value = customValueGetter(item);
                        }
                    }

                    rawResult[i].Add((gridColumn, value));
                }
            }

            var result = new Dictionary<T, IReadOnlyList<(IGridColumn<T> column, object displayValue)>>(items.Count);
            for (int i = 0; i < items.Count; i++) {
                T item = items[i];
                result.Add(item, rawResult[i]);
            }

            return result;
        }

        public static async Task<IGridClient<T>> CreateBasicGrid<T>(
            Func<IEnumerable<T>> itemsGetter,
            Action<IGridColumnCollection<T>> columnsConfigureAction,
            Action<IGridServer<T>>? serverConfigureAction = null,
            Action<IGridClient<T>>? clientConfigureAction = null,
            string? gridName = null
        ) {
            gridName ??= $"Grid.{typeof(T).Name}.{Guid.NewGuid()}";
            QueryDictionary<StringValues> query = new QueryDictionary<StringValues>();

            Func<QueryDictionary<StringValues>, ItemsDTO<T>> dataService = queryDictionary => {
                IEnumerable<T> items = itemsGetter();
                if (items == null)
                    throw new InvalidOperationException("items == null");

                IGridServer<T> server =
                    new GridServer<T>(
                        items,
                        new QueryCollection(query),
                        false,
                        gridName,
                        columnsConfigureAction
                    );

                serverConfigureAction?.Invoke(server);
                return server.ItemsToDisplay;
            };

            IGridClient<T> client = new GridClient<T>(
                dataService,
                query,
                false,
                gridName,
                columnsConfigureAction
            );
            clientConfigureAction?.Invoke(client);

            // Set new items to grid
            await client.UpdateGrid();
            return client;
        }
    }
}
