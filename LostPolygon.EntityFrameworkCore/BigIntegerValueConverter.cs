using System;
using System.Linq.Expressions;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LostPolygon.EntityFrameworkCore;

public class BigIntegerValueConverter : ValueConverter<BigInteger, string> {
    public BigIntegerValueConverter() : base(Serialize, Deserialize, null)
    {
    }

    private static readonly Expression<Func<string, BigInteger>> Deserialize = x => BigInteger.Parse(x);
    private static readonly Expression<Func<BigInteger, string>> Serialize = x => x.ToString("R", System.Globalization.CultureInfo.InvariantCulture);
}
