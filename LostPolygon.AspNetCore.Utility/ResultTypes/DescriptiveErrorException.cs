using System;

namespace LostPolygon.AspNetCore.Utility.ResultTypes;

public class DescriptiveErrorException : Exception {
    public IDescriptiveError DescriptiveError { get; }

    public DescriptiveErrorException(IDescriptiveError descriptiveError)
        : base(descriptiveError.Message) {
        DescriptiveError = descriptiveError;
    }

    public DescriptiveErrorException(IDescriptiveError descriptiveError, Exception? innerException)
        : base(descriptiveError.Message, innerException) {
        DescriptiveError = descriptiveError;
    }
}
