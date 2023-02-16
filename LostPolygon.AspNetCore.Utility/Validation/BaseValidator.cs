using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace LostPolygon.AspNetCore.Utility;

public abstract class BaseValidator<T> : AbstractValidator<T> {
    protected BaseValidator() : this(true, true) {
    }

    protected BaseValidator(bool callAddRules, bool checkForMissingRules) {
        // ReSharper disable once VirtualMemberCallInConstructor
        if (callAddRules) {
            AddRules();
        }

        if (checkForMissingRules) {
            CheckMissingRules();
        }
    }

    protected abstract void AddRules();

    private void CheckMissingRules() {
        List<PropertyRule> rules = this.Cast<PropertyRule>().ToList();
        PropertyInfo[] properties = typeof(T).GetProperties();

        List<ValidationFailure> missingRulesFailures = new();
        foreach (PropertyInfo property in properties) {
            bool ruleExists = rules.Any(rule => CheckMemberInfosEqual(rule.Member, property));
            if (!ruleExists) {
                missingRulesFailures.Add(
                    new ValidationFailure(property.Name, $"Public property '{property.Name}' must have a validation rule.")
                );
            }
        }

        if (missingRulesFailures.Count == 0)
            return;

        missingRulesFailures.Insert(0, new ValidationFailure("", $"All public properties must have a validation rule (type '{typeof(T).FullName}')."));
        throw new ValidationException(missingRulesFailures);
    }

    private static bool CheckMemberInfosEqual(MemberInfo x, MemberInfo b) {
        return
            x.DeclaringType == b.DeclaringType &&
            x.MetadataToken == b.MetadataToken;
    }
}
