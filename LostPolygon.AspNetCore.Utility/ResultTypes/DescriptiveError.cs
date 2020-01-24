namespace Ballast.Atlantis.Utility {
    public interface IDescriptiveError {
        string Key { get; }
        string Message { get; }
        object? AttemptedValue { get; }

        string? ToStringInternal() {
            string result = $"{Key}: {Message}";
            if (AttemptedValue != null) {
                result += $" (attempted value {AttemptedValue})";
            }

            return result;
        }
    }

    public struct DescriptiveError : IDescriptiveError {
        public string Key { get; }
        public string Message { get; }
        public object? AttemptedValue { get; }

        public DescriptiveError(string key, string message) {
            Key = key;
            Message = message;
            AttemptedValue = null;
        }

        public DescriptiveError(string key, string message, object attemptedValue) : this(key, message) {
            AttemptedValue = attemptedValue;
        }

        public override string? ToString() {
            return ((IDescriptiveError) this).ToStringInternal();
        }
    }

    public struct DescriptiveError<T> : IDescriptiveError {
        public string Key { get; }
        public string Message { get; }
        public object? AttemptedValue { get; }

        public DescriptiveError(string key, string message) {
            Key = key;
            Message = message;
            AttemptedValue = null;
        }

        public DescriptiveError(string key, string message, object attemptedValue) : this(key, message) {
            AttemptedValue = attemptedValue;
        }

        public override string? ToString() {
            return ((IDescriptiveError) this).ToStringInternal();
        }
    }
}
