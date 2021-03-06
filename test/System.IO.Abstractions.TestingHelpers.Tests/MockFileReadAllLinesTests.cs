using FluentAssertions;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Xunit;

    using Text;

    using XFS = MockUnixSupport;

    public class MockFileReadAllLinesTests {
        [Fact]
        public void MockFile_ReadAllLines_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllLines(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            result.ShouldBeEquivalentTo(new[] { "Demo", "text", "content", "value" }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void MockFile_ReadAllLines_ShouldReturnOriginalDataWithCustomEncoding()
        {
            // Arrange
            string text = "Hello\r\nthere\rBob\nBob!";
            var encodedText = Encoding.BigEndianUnicode.GetBytes(text);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData(encodedText) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllLines(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);

            // Assert
            result.ShouldBeEquivalentTo(new[] { "Hello", "there", "Bob", "Bob!" }, options => options.WithStrictOrdering());
        }
    }
}