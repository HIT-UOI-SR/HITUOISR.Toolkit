using Microsoft.Extensions.FileProviders;
using System;

namespace HITUOISR.Toolkit.Settings.FileBase
{
    /// <summary>
    /// 基于文件的设置源。
    /// </summary>
    public abstract class FileSettingsSource : ISettingsSource
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="path">文件路径。</param>
        public FileSettingsSource(string path) => Path = path;

        /// <summary>
        /// 文件路径。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 使用的文件提供器。
        /// </summary>
        public IFileProvider? FileProvider { get; set; }

        /// <summary>
        /// 该文件在缺失时是否是可用自动创建。
        /// </summary>
        public bool AutoCreate { get; set; }

        /// <summary>
        /// 异常处理操作。
        /// </summary>
        public Action<FileExceptionContext>? ExceptionHandler { get; set; }

        /// <inheritdoc/>
        public bool IsReadOnly { get; set; }

        /// <inheritdoc/>
        public abstract ISettingsProvider Build(ISettingsBuilder builder);

        /// <summary>
        /// 保证至少能使用默认的属性。
        /// </summary>
        /// <param name="builder"></param>
#if NET5_0_OR_GREATER
        [System.Diagnostics.CodeAnalysis.MemberNotNull(nameof(FileProvider), nameof(ExceptionHandler))]
#endif
        public void EnsureDefaults(ISettingsBuilder builder)
        {
            FileProvider ??= builder.GetFileProvider();
            ExceptionHandler ??= builder.GetileExceptionHandler();
        }
    }
}
