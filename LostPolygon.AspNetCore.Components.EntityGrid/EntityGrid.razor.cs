using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridBlazor;
using GridBlazor.Pages;
using GridMvc.Server;
using GridShared;
using Microsoft.AspNetCore.Components;

namespace LostPolygon.AspNetCore.Components.EntityGrid {
    public abstract partial class EntityGrid<T> where T : notnull {
        [Parameter]
        public Func<IReadOnlyList<T>> Items { get; set; } = null!;

        [Parameter]
        public bool ShowHeaderFilters { get; set; } = false;

        [Parameter]
        public RenderFragment HeaderFilters { get; set; } = null!;

        [Parameter]
        public RenderFragment BeforeTableContent { get; set; } = null!;

        [Parameter]
        public RenderFragment AfterTableContent { get; set; } = null!;

        [Parameter]
        public RenderFragment AfterCardContent { get; set; } = null!;

        [Parameter]
        public bool StickyFirstColumn { get; set; } = true;

        public CGrid<T> ItemsGrid { get; private set; } = null!;

        private GridComponent<T> ItemsGridComponent = null!;
        private List<T> FilteredItems { get; set; } = null!;
        private List<EntityGridFilterBase<T>> Filters { get; set; } = new List<EntityGridFilterBase<T>>();

        protected abstract void CnCreatingColumnsConfiguration(IGridColumnCollection<T> columns);

        public void AddFilter(EntityGridFilterBase<T> filter) {
            Filters.Add(filter);
        }

        protected override async Task OnInitializedAsync() {
            Items = Items ?? throw new ArgumentNullException(nameof(Items));
            FilteredItems = new List<T>(Items().Count);

            IGridClient<T> itemsGridClient =
                await GridBlazorUtility.CreateBasicGrid(
                    () => FilteredItems,
                    CnCreatingColumnsConfiguration,
                    server => CreateServer(server),
                    client => CreateClient(client)
                );

            ItemsGrid = itemsGridClient.Grid;
            await ApplyFilters();
        }

        protected virtual IGridClient<T> CreateClient(IGridClient<T> client) {
            return client.Sortable();
        }

        protected virtual IGridServer<T> CreateServer(IGridServer<T> server) {
            return server.Sortable();
        }

        public async Task UpdateGrid() {
            await ApplyFilters();
        }

        protected async Task ApplyFilters() {
            IReadOnlyList<T> items = Items();
            IEnumerable<T> filteredItems = items;
            foreach (EntityGridFilterBase<T> filter in Filters) {
                filter.OnBeforeFilter(items);
            }

            foreach (EntityGridFilterBase<T> filter in Filters) {
                filteredItems = filteredItems.Where(filter.Filter);
            }

            FilteredItems.Clear();
            FilteredItems.AddRange(filteredItems);

            if (ItemsGridComponent != null) {
                await ItemsGridComponent.UpdateGrid();
            }
            else {
                await ItemsGrid.UpdateGrid();
            }
        }
    }
}
