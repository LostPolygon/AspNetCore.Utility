using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Ballast.Atlantis.Web.Components {
    public class InjectingOwningComponentBase : OwningComponentBase {
        private List<PropertyInfo> _props = null!;

        protected override void OnInitialized() {
            InjectTransient();
        }

        private void InjectTransient() {
            _props = GetType().GetProperties()
                .Where(property => property.GetCustomAttribute<InjectTransientAttribute>() != null)
                .Where(property => {
                    // We don't support set only, non public, or indexer properties
                    if (property.GetMethod == null ||
                        !property.GetMethod.IsPublic ||
                        property.GetMethod.GetParameters().Length > 0)
                    {
                        return false;
                    }

                    bool hasSetter = property.SetMethod != null && property.SetMethod.IsPublic;

                    if (!hasSetter)
                        return false;

                    return true;
                })
                .ToList();

            foreach (PropertyInfo propertyInfo in _props) {
                object service = ScopedServices.GetService(propertyInfo.PropertyType);
                propertyInfo.SetValue(this, service);
            }
        }

        protected T GetScopedService<T>() {
            return ScopedServices.GetService<T>();
        }
    }
}
