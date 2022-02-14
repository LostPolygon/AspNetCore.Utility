using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostPolygon.EntityFrameworkCore; 

public static class DateTimeUtcPropertyBuilderExtensions {
    private static DateTime FromCodeToData(DateTime fromCode, string name)
        => fromCode.Kind == DateTimeKind.Utc ? fromCode : throw new InvalidOperationException($"Column {name} only accepts UTC date-time values");

    private static DateTime FromDataToCode(DateTime fromData)
        => fromData.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(fromData, DateTimeKind.Utc) : fromData.ToUniversalTime();

    public static PropertyBuilder<DateTime?> UsesUtc(this PropertyBuilder<DateTime?> property) {
        string? name = property.Metadata.Name;
        return property.HasConversion<DateTime?>(
            fromCode => fromCode != null ? FromCodeToData(fromCode.Value, name) : default,
            fromData => fromData != null ? FromDataToCode(fromData.Value) : default
        );
    }

    public static PropertyBuilder<DateTime> UsesUtc(this PropertyBuilder<DateTime> property) {
        string? name = property.Metadata.Name;
        return property.HasConversion(fromCode => FromCodeToData(fromCode, name), fromData => FromDataToCode(fromData));
    }
}