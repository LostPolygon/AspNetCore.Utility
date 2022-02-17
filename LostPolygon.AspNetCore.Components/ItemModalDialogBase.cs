using System.Diagnostics;
using System.Threading.Tasks;
using BlazorStrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace LostPolygon.AspNetCore.Components;

public abstract class ItemModalDialogBase<TItemViewModel> : ComponentBase where TItemViewModel : class {
    [Parameter]
    public EventCallback<TItemViewModel> OnCommitted { get; set; }

    protected TItemViewModel? ViewModel { get; private set; } = null!;

    protected BSModal Modal { get; set; } = null!;

    protected EditContext? EditContext { get; private set; }

    protected void Open() {
        ViewModel = CreateItemViewModel();
        EditContext = new EditContext(ViewModel);
        Modal.ShowAsync();
    }

    protected abstract TItemViewModel CreateItemViewModel();

    protected abstract Task<bool> Commit();

    protected virtual void OnCloseClicked() {
        Modal.HideAsync();
    }

    protected virtual async Task OnCommitClicked() {
        Debug.Assert(EditContext != null, "EditContext != null");
        bool valid = EditContext.Validate();
        if (!valid)
            return;

        bool success = await Commit();
        await Modal.HideAsync();

        Debug.Assert(ViewModel != null, "ViewModel != null");
        if (success) {
            await OnCommitted.InvokeAsync(ViewModel);
        }
    }
}
