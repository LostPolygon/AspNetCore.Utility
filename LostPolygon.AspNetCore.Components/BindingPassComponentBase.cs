using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace LostPolygon.AspNetCore.Components {
    public class BindingPassComponentBase<T> : ComponentBase {
        [Parameter]
        public T Value { get; set; } = default!;

        [Parameter]
        public EventCallback<T> ValueChanged { get; set; }

        [Parameter]
        public Expression<Func<T>> ValueExpression { get; set; } = null!;
    }
}
