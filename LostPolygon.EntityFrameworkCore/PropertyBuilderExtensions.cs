using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostPolygon.EntityFrameworkCore;

public static class PropertyBuilderExtensions {
    public static PropertyBuilder<TProperty> WithSimpleName<TProperty>(this PropertyBuilder<TProperty> propertyBuilder, string prefix = "") {
        return propertyBuilder.HasColumnName(prefix + propertyBuilder.Metadata.GetDefaultColumnName());
    }
}
