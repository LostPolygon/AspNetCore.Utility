using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace LostPolygon.AspNetCore.Components; 

public class ExInputTextArea : InputTextArea {
    private string FieldClass => FieldCssClass(EditContext, FieldIdentifier);

    [Parameter]
    public string InvalidCssClass { get; set; } = "is-invalid";

    [Parameter]
    public string ValidCssClass { get; set; } = "is-valid";

    [Parameter]
    public string ModifiedCssClass { get; set; } = "modified";

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

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "textarea");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "class", CssClass);
        builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValue));
        builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString!));
        builder.CloseElement();
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