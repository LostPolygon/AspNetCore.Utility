using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LostPolygon.EntityFrameworkCore;

public static class ModelBuilderExtensions {
    public static string HasCaseInsensitiveCollation(this ModelBuilder builder, DatabaseFacade database) {
        string caseInsensitiveCollation = "";
        if (database.IsSqlite()) {
            caseInsensitiveCollation = "NOCASE";
        } else if (database.IsNpgsql()) {
            caseInsensitiveCollation = "custom-ci-latin-collation";
            builder.HasCollation(caseInsensitiveCollation, locale: "en-u-ks-primary", provider: "icu", deterministic: false);
        }

        return caseInsensitiveCollation;
    }
}
