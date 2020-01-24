using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace LostPolygon.AspNetCore.Utility {
    public class RequiredBindingMetadataProvider : IBindingMetadataProvider {
        public void CreateBindingMetadata(BindingMetadataProviderContext context) {
            if (context.PropertyAttributes != null && context.PropertyAttributes.OfType<RequiredAttribute>().Any()) {
                context.BindingMetadata.IsBindingRequired = true;
            }
        }
    }
}
