using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;

namespace LostPolygon.AspNetCore.Utility;

public static class ExtendedDisplayNameResolver {
    private static readonly Func<Type, MemberInfo, LambdaExpression, string?> DefaultDisplayNameResolver;
    private static readonly ConcurrentDictionary<MemberInfo, string?> DisplayNameCache = new();

    static ExtendedDisplayNameResolver() {
        Func<Type,MemberInfo,LambdaExpression,string> currentResolver = ValidatorOptions.DisplayNameResolver;
        ValidatorOptions.DisplayNameResolver = null;
        DefaultDisplayNameResolver = ValidatorOptions.DisplayNameResolver!;
        ValidatorOptions.DisplayNameResolver = currentResolver;
    }

    public static void Register() {
        ValidatorOptions.DisplayNameResolver = DisplayNameResolver;
    }

    private static string? DisplayNameResolver(Type type, MemberInfo? memberInfo, LambdaExpression expression) {
        if (memberInfo == null)
            return null;

        return DisplayNameCache.GetOrAdd(memberInfo, memberInfo => {
            if (!AttributeUtility.GetMemberNameFromNameAttributes(memberInfo, out string? name)) {
                name = DefaultDisplayNameResolver(type, memberInfo, expression);
            }

            return name;
        });
    }
}
