namespace LostPolygon.AspNetCore.Utility {
    public interface IDescriptiveError {
        string Key { get; }
        string Message { get; }
        object? AttemptedValue { get; }
    }

    public abstract class DescriptiveErrorBase : IDescriptiveError {
        public string Key { get; }
        public string Message { get; }
        public object? AttemptedValue { get; }

        protected DescriptiveErrorBase(string key, string message) {
            Key = key;
            Message = message;
            AttemptedValue = null;
        }

        protected DescriptiveErrorBase(string key, string message, object? attemptedValue) : this(key, message) {
            AttemptedValue = attemptedValue;
        }

        public override string? ToString() {
            string result = $"{Key}: {Message}";
            if (AttemptedValue != null) {
                result += $" (attempted value {AttemptedValue})";
            }

            return result;
        }
    }

    public class DescriptiveError : DescriptiveErrorBase {
        public DescriptiveError(string key, string message) : base(key, message) {
        }

        public DescriptiveError(string key, string message, object? attemptedValue) : base(key, message, attemptedValue) {
        }
    }

    public class DescriptiveError<T> : DescriptiveErrorBase {
        public DescriptiveError(string key, string message) : base(key, message) {
        }

        public DescriptiveError(string key, string message, object? attemptedValue) : base(key, message, attemptedValue) {
        }
    }
}
