namespace LostPolygon.AspNetCore.Utility;

public interface IDescriptiveError {
    string Key { get; }
    string Message { get; }
    object? AttemptedValue { get; }

    string ToStringInternal() {
        string result = $"{Key}: {Message}";
        if (AttemptedValue != null) {
            result += $" (attempted value {AttemptedValue})";
        }

        return result;
    }
}

public readonly record struct DescriptiveError(string Key, string Message, object? AttemptedValue = null) : IDescriptiveError {
    public override string ToString() {
        return ((IDescriptiveError) this).ToStringInternal();
    }
}

public readonly record struct DescriptiveError<T>(string Key, string Message, object? AttemptedValue = null) : IDescriptiveError {
    public override string ToString() {
        return ((IDescriptiveError) this).ToStringInternal();
    }
}
