using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace LostPolygon.AspNetCore.Components {
    public class ExValidationMessage<TValue> : ValidationMessage<TValue> {
        private static readonly PropertyInfo _currentEditContextPropertyInfo =
            typeof(ValidationMessage<TValue>).GetProperty(nameof(CurrentEditContext), BindingFlags.Instance | BindingFlags.NonPublic)!;

        private static readonly FieldInfo _fieldIdentifierFieldInfo =
            typeof(ValidationMessage<TValue>).GetField("_fieldIdentifier", BindingFlags.Instance | BindingFlags.NonPublic)!;

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            foreach (string message in CurrentEditContext.GetValidationMessages(FieldIdentifier))
            {
                builder.OpenElement(0, "div");
                builder.AddMultipleAttributes(1, AdditionalAttributes);
                builder.AddContent(2, message);
                builder.CloseElement();
            }
        }

        public FieldIdentifier FieldIdentifier => (FieldIdentifier) _fieldIdentifierFieldInfo.GetValue(this)!;

        public EditContext CurrentEditContext => (EditContext) _currentEditContextPropertyInfo.GetValue(this)!;
    }
}
