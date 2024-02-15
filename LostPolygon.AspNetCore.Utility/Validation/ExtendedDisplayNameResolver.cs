using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;

namespace LostPolygon.AspNetCore.Utility;

public static class ExtendedDisplayNameResolver {
    private static Func<Type, MemberInfo, LambdaExpression, string?> _defaultDisplayNameResolver;
    private static ConcurrentDictionary<MemberInfo, string?> _displayNameCache = new();

    static ExtendedDisplayNameResolver() {
        Func<Type,MemberInfo,LambdaExpression,string> currentResolver = ValidatorOptions.DisplayNameResolver;
        ValidatorOptions.DisplayNameResolver = null;
        _defaultDisplayNameResolver = ValidatorOptions.DisplayNameResolver!;
        ValidatorOptions.DisplayNameResolver = currentResolver;
    }

    public static void Register() {
        ValidatorOptions.DisplayNameResolver = DisplayNameResolver;
    }

    private static string? DisplayNameResolver(Type type, MemberInfo? memberInfo, LambdaExpression expression) {
        if (memberInfo == null)
            return null;

        return _displayNameCache.GetOrAdd(memberInfo, memberInfo => {
            if (!AttributeUtility.GetMemberNameFromNameAttributes(memberInfo, out string? name)) {
                name = _defaultDisplayNameResolver(type, memberInfo, expression);
            }

            return name;
        });
    }
}
