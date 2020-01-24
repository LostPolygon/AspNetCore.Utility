using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ballast.Atlantis.Utility {
    public static class ModelStateDictionaryExtensions {
        public static void AddModelError<TError>(this ModelStateDictionary modelState, TError error) where TError : IDescriptiveError {
            modelState.AddModelError(error.Key, error.Message);
            if (error.AttemptedValue != null) {
                modelState.TryGetValue(error.Key, out ModelStateEntry entry);
                Debug.Assert(entry != null, nameof(entry) + " != null");
                entry.RawValue = error.AttemptedValue;
                entry.AttemptedValue = error.AttemptedValue.ToString();
            }
        }
    }
}
