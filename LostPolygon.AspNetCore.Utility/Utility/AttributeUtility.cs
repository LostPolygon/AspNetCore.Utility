using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Ballast.Atlantis.Utility {
    public static class AttributeUtility {
        private delegate bool GetMemberNameFromAttributeImplementationDelegate(MemberInfo memberInfo, out string? attributeName);
        public static bool GetMemberNameFromJsonProperty<T>(Expression<Func<T>> expression, out string? jsonPropertyName) {
            return GetMemberNameFromAttribute(expression, GetMemberNameFromJsonProperty, out jsonPropertyName);
        }

        public static bool GetMemberNameFromJsonProperty(MemberInfo memberInfo, out string? jsonPropertyName) {
            return GetMemberNameFromAttribute<JsonPropertyNameAttribute>(memberInfo, attribute => attribute.Name, out jsonPropertyName);
        }

        public static bool GetMemberNameFromFromQuery(MemberInfo memberInfo, out string? fromQueryName) {
            return GetMemberNameFromAttribute<FromQueryAttribute>(memberInfo, attribute => attribute.Name, out fromQueryName);
        }

        public static bool GetMemberNameFromNameAttributes(MemberInfo memberInfo, out string? name) {
            name = null;
            if (GetMemberNameFromJsonProperty(memberInfo, out string? jsonPropertyName)) {
                name = jsonPropertyName;
                return true;
            }

            if (GetMemberNameFromFromQuery(memberInfo, out string? fromQueryName)) {
                name = fromQueryName;
                return true;
            }

            return false;
        }

        private static bool GetMemberNameFromAttribute<T>(
            Expression<Func<T>> expression,
            GetMemberNameFromAttributeImplementationDelegate nameFromAttributeImplementationDelegate,
            out string? name) {
            if (!(expression.Body is MemberExpression memberExpression))
                throw new InvalidOperationException("Expression must be a member expression");

            if (!(memberExpression.Member is PropertyInfo propertyInfo))
                throw new InvalidOperationException("Expression must be a property expression");

            return nameFromAttributeImplementationDelegate(propertyInfo, out name);
        }

        private static bool GetMemberNameFromAttribute<TAttribute>(MemberInfo memberInfo, Func<TAttribute, string> getNameFunc, out string? name)
            where TAttribute : Attribute {
            name = null;
            TAttribute? attribute = memberInfo.GetCustomAttribute<TAttribute>();
            if (attribute != null && !String.IsNullOrEmpty(getNameFunc(attribute))) {
                name = getNameFunc(attribute);
                return true;
            }

            return false;
        }
    }
}
