using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace LostPolygon.AspNetCore.Components.EntityGrid; 

public abstract class EntityGridFilterBase<T> : ComponentBase where T : notnull {
    [CascadingParameter]
    protected EntityGrid<T> Parent { get; set; } = null!;

    protected override void OnInitialized() {
        if (Parent == null)
            throw new ArgumentNullException(
                nameof(Parent),
                $@"{nameof(EntityGridFilterBase<T>)} must exist within a {nameof(EntityGrid<T>)}"
            );

        Parent.AddFilter(this);
    }

    public virtual void OnBeforeFilter(IReadOnlyList<T> items) {
    }

    public abstract bool Filter(T item);

    public virtual void NotifyGridInitialized() {
    }
}