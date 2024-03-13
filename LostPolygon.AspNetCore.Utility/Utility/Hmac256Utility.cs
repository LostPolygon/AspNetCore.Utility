using System;
using System.Security.Cryptography;
using System.Text;

namespace LostPolygon.AspNetCore.Utility;

public static class Hmac256Utility {
    public static string ComputeUtf8StringHashAsHex(string data, byte[] secretKey) {
        Span<byte> output = stackalloc byte[256 / 8];

        ComputeUtf8StringHash(data, secretKey, output);
        return Convert.ToHexString(output);
    }

    public static string ComputeHashAsHex(ReadOnlySpan<byte> data, byte[] secretKey) {
        Span<byte> output = stackalloc byte[256 / 8];

        ComputeHash(data, secretKey, output);
        return Convert.ToHexString(output);
    }

    public static void ComputeUtf8StringHash(string data, byte[] secretKey, Span<byte> output) {
        int dataMaxByteCount = Encoding.UTF8.GetMaxByteCount(data.Length);
        Span<byte> dataBytes = stackalloc byte[dataMaxByteCount];
        int dataByteSize = Encoding.UTF8.GetBytes(data, dataBytes);
        dataBytes = dataBytes[..dataByteSize];

        ComputeHash(dataBytes, secretKey, output);
    }

    public static void ComputeHash(ReadOnlySpan<byte> data, byte[] secretKey, Span<byte> output) {
        using HMACSHA256 hmacsha256 = new(secretKey);
        bool success = hmacsha256.TryComputeHash(data, output, out int bytesWritten);
        if (!success || bytesWritten != output.Length)
            throw new Exception("HMACSHA256.TryComputeHash failed");
    }
}
