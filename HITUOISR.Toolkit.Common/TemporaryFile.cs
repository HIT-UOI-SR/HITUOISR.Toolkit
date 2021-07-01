using System;
using System.IO;

namespace HITUOISR.Toolkit.Common
{
    /// <summary>
    /// 简易临时文件。
    /// </summary>
    public sealed class TemporaryFile : IDisposable
    {
        /// <summary>
        /// 创建临时文件。
        /// </summary>
        public TemporaryFile() : this(Path.GetTempPath()) { }

        /// <summary>
        /// 在指定文件夹创建临时文件。
        /// </summary>
        /// <param name="directory">临时文件所在文件夹。</param>
        public TemporaryFile(string directory)
        {
            File = new(Path.Combine(directory, Path.GetRandomFileName()));
            File.Create().Dispose();
        }

        /// <summary>
        /// 临时文件。
        /// </summary>
        public FileInfo File { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (File.Exists) File.Delete();
        }
    }
}
