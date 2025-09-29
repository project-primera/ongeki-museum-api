namespace OngekiMuseumApi.Utils;

using System;
using System.Security.Cryptography;

public static class Uuid
{
    /// <summary>
    /// Create a UUID version 7 (time-ordered) according to the UUIDv7 draft convention.
    /// This implementation encodes the unix epoch milliseconds into the first 6 bytes
    /// (big-endian), then fills the remainder with cryptographically secure random bytes,
    /// setting the version and RFC4122 variant bits.
    /// </summary>
    public static Guid CreateVersion7()
    {
        long unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Span<byte> bytes = stackalloc byte[16];

        // Timestamp (48 bits) big-endian
        bytes[0] = (byte)((unixMs >> 40) & 0xFF);
        bytes[1] = (byte)((unixMs >> 32) & 0xFF);
        bytes[2] = (byte)((unixMs >> 24) & 0xFF);
        bytes[3] = (byte)((unixMs >> 16) & 0xFF);
        bytes[4] = (byte)((unixMs >> 8) & 0xFF);
        bytes[5] = (byte)(unixMs & 0xFF);

        Span<byte> rnd = stackalloc byte[10];
        RandomNumberGenerator.Fill(rnd);

        // Set version (7) in the high nibble of byte 6
        bytes[6] = (byte)((rnd[0] & 0x0F) | 0x70);
        bytes[7] = rnd[1];

        // Set RFC4122 variant (0b10xxxxxx) in the high bits of byte 8
        bytes[8] = (byte)((rnd[2] & 0x3F) | 0x80);
        bytes[9] = rnd[3];

        // remaining 6 bytes of node/random
        for (int i = 0; i < 6; i++) bytes[10 + i] = rnd[4 + i];

        return new Guid(bytes);
    }
}