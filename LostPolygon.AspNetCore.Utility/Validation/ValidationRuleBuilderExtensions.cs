using System;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;

namespace LostPolygon.AspNetCore.Utility; 

public static class ValidationRuleBuilderExtensions {
    public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilderInitial<T, string> rule) {
        return rule.SetValidator(new GuidValidator());
    }

    public static IRuleBuilderOptions<T, string> IsUrl<T>(this IRuleBuilderInitial<T, string> rule) {
        return rule.SetValidator(new UrlValidator());
    }

    public static IRuleBuilderOptions<T, string> NotNullOrWhitespace<T>(
        this IRuleBuilder<T, string> ruleBuilder) {
        return ruleBuilder
            .Must(s => !String.IsNullOrWhiteSpace(s))
            .WithMessage("'{PropertyName}' must not be empty or contain only whitespace.");
    }

    public static IRuleBuilderOptions<T, TProperty> WithNameFromDataAttribute<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions) {
        // Use reflection since contravariance only works with interface and delegates,
        // and we need it to handle generic collections.
        // Example: RuleBuilder<object, ICollection<object>> can't be cast to RuleBuilder<object, ICollection<object>>
        if (ruleBuilderOptions.GetType().GetGenericTypeDefinition() != typeof(RuleBuilder<,>))
            throw new ValidationException($"Expected a RuleBuilder<{typeof(T).FullName}, {typeof(TProperty).FullName}>, got {ruleBuilderOptions.GetType().FullName}");

        PropertyInfo ruleBuilderRulePropertyInfo = ruleBuilderOptions.GetType().GetProperty(nameof(RuleBuilder<T, TProperty>.Rule))!;
        PropertyRule rule = (PropertyRule) ruleBuilderRulePropertyInfo.GetValue(ruleBuilderOptions)!;

        if (AttributeUtility.GetMemberNameFromNameAttributes(rule.Member, out string? dataName)) {
            ruleBuilderOptions.OverridePropertyName(dataName);
        }

        return ruleBuilderOptions;
    }
}