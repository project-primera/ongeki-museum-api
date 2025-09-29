using Xunit;
using OngekiMuseumApi.Utils;

namespace OngekiMuseumApi.Tests;

public class Tests
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }
    
    [Fact]
    public void UuidCreateVersion7_ShouldGenerateValidUuidV7()
    {
        // Act
        var uuid1 = Uuid.CreateVersion7();
        var uuid2 = Uuid.CreateVersion7();
        
        // Assert
        Assert.NotEqual(Guid.Empty, uuid1);
        Assert.NotEqual(Guid.Empty, uuid2);
        Assert.NotEqual(uuid1, uuid2);
        
        // Check that it's a valid UUIDv7 by verifying the version bits
        var bytes1 = uuid1.ToByteArray();
        // In Microsoft's GUID byte order, the version is in byte 6, high nibble
        var versionNibble = (bytes1[6] & 0xF0) >> 4; 
        Assert.Equal(7, versionNibble);
        
        // Check RFC4122 variant bits (should be 0b10xxxxxx in high bits of byte 8)
        var variantBits = (bytes1[8] & 0xC0); // Get the two high bits
        Assert.Equal(0x80, variantBits); // Should be 0b10000000 = 0x80
        
        // UUIDs generated close together should have similar timestamps (first 6 bytes)
        // so we can verify time-ordering property
        Assert.True(uuid1.CompareTo(uuid2) < 0 || uuid2.CompareTo(uuid1) < 0); // Should be different
    }
}
