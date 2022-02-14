using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LostPolygon.AspNetCore.Utility; 

public static class StreamExtensions {
    public static async Task CopyToWithNotificationAsync(
        this Stream source,
        Stream destination,
        int bufferSize = 256 * 1024,
        CancellationToken cancellationToken = default,
        Action<long>? totalReadBytesNotificationCallback = null) {
        totalReadBytesNotificationCallback?.Invoke(0);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        try {
            long totalBytesRead = 0;
            while (true)
            {
                int bytesRead = await source.ReadAsync(new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                totalReadBytesNotificationCallback?.Invoke(totalBytesRead);
                if (bytesRead == 0)
                    break;

                await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead), cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}