using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace LostPolygon.AspNetCore.Components {
    public class ExInputNumber<TValue> : InputNumber<TValue> {
        private static readonly FieldInfo _stepAttributeValueFieldInfo =
            typeof(InputNumber<TValue>).GetField("_stepAttributeValue", BindingFlags.Static | BindingFlags.NonPublic)!;

        private string FieldClass => FieldCssClass(EditContext, FieldIdentifier);

        [Parameter]
        public string InvalidCssClass { get; set; } = "is-invalid";

        [Parameter]
        public string ValidCssClass { get; set; } = "is-valid";

        [Parameter]
        public string ModifiedCssClass { get; set; } = "modified";

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "step", (string) _stepAttributeValueFieldInfo.GetValue(null)!);
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            builder.AddAttribute(3, "type", "number");
            builder.AddAttribute(4, "class", CssClass);
            builder.AddAttribute(5, "value", BindConverter.FormatValue(CurrentValueAsString));
            builder.AddAttribute(6, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
            builder.CloseElement();
        }

        protected new string CssClass {
            get {
                if (AdditionalAttributes != null &&
                    AdditionalAttributes.TryGetValue("class", out var @class) &&
                    !string.IsNullOrEmpty(Convert.ToString(@class))) {
                    return $"{@class} {FieldClass}";
                }

                return FieldClass; // Never null or empty
            }
        }

        private string FieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier) {
            bool isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();
            if (editContext.IsModified(fieldIdentifier)) {
                return isValid ? ModifiedCssClass + " " + ValidCssClass : ModifiedCssClass + " " + InvalidCssClass;
            }
            else {
                return isValid ? "" : InvalidCssClass;
            }
        }
    }
}