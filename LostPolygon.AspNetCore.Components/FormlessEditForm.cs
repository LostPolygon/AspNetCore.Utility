using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace LostPolygon.AspNetCore.Components; 

public class FormlessEditForm : EditForm {
    protected override void OnParametersSet() {
        base.OnParametersSet();

        EditContext = EditContext ?? throw new ArgumentNullException(nameof(EditContext));
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        // If _fixedEditContext changes, tear down and recreate all descendants.
        // This is so we can safely use the IsFixed optimization on CascadingValue,
        // optimizing for the common case where _fixedEditContext never changes.
        builder.OpenRegion(EditContext!.GetHashCode());

        builder.OpenElement(0, "span");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.OpenComponent<CascadingValue<EditContext>>(3);
        builder.AddAttribute(4, "IsFixed", true);
        builder.AddAttribute(5, "Value", EditContext);
        builder.AddAttribute(6, "ChildContent", ChildContent?.Invoke(EditContext));
        builder.CloseComponent();
        builder.CloseElement();

        builder.CloseRegion();
    }
}