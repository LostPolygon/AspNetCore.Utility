using System;

namespace LostPolygon.AspNetCore.Utility.ResultTypes;

public class DescriptiveErrorException : DescriptiveErrorException<IDescriptiveError> {
    public DescriptiveErrorException(IDescriptiveError descriptiveError) : base(descriptiveError) {
    }

    public DescriptiveErrorException(IDescriptiveError descriptiveError, Exception? innerException) : base(descriptiveError, innerException) {
    }
}

public class DescriptiveErrorException<TDescriptiveError> : Exception where TDescriptiveError : IDescriptiveError {
    public TDescriptiveError DescriptiveError { get; }

    public DescriptiveErrorException(TDescriptiveError descriptiveError)
        : base(descriptiveError.Message) {
        DescriptiveError = descriptiveError;
    }

    public DescriptiveErrorException(TDescriptiveError descriptiveError, Exception? innerException)
        : base(descriptiveError.Message, innerException) {
        DescriptiveError = descriptiveError;
    }
}
