using System.IO;
using Xunit;

namespace HITUOISR.Toolkit.Common.Tests
{
    public class TemporaryFileTests
    {
        [Fact()]
        public void TemporaryFile_ShouldDeleteAfterDispose()
        {
            TemporaryFile file = new();
            Assert.True(File.Exists(file.File.FullName), "Temporary file doesnot created");
            file.Dispose();
            Assert.False(File.Exists(file.File.FullName), "Temporary file doesnot deleted after Dispose.");
        }
    }
}
