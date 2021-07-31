using Microsoft.Extensions.FileProviders;
using System;

namespace HITUOISR.Toolkit.Settings.FileBase
{
    /// <summary>
    /// 用于 <seealso cref="FileSettingsProvider"/> 的 <seealso cref="ISettingsBuilder"/> 扩展方法。
    /// </summary>
    public static class FileSettingsBuilderExtensions
    {
        private static readonly string FileProviderKey = "FileProvider";
        private static readonly string FileExceptionHandlerKey = "FileExceptionHandler";

        /// <summary>
        /// 设置文件提供器。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileProvider">要使用的文件提供器。</param>
        /// <returns></returns>
        public static ISettingsBuilder SetFileProvider(this ISettingsBuilder builder, IFileProvider fileProvider)
        {
            builder.Properties[FileProviderKey] = fileProvider;
            return builder;
        }

        /// <summary>
        /// 获取文件提供器。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>使用的文件提供器。</returns>
        public static IFileProvider GetFileProvider(this ISettingsBuilder builder)
        {
            if (builder.Properties.TryGetValue(FileProviderKey, out object? value) && value is IFileProvider provider)
            {
                return provider;
            }
            return new PhysicalFileProvider(AppContext.BaseDirectory ?? string.Empty);
        }

        /// <summary>
        /// 设置基础路径。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="basePath">要使用的基础路径。</param>
        /// <returns></returns>
        public static ISettingsBuilder SetBasePath(this ISettingsBuilder builder, string basePath) =>
            builder.SetFileProvider(new PhysicalFileProvider(basePath));

        /// <summary>
        /// 设置处理文件异常的操作。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="handler">异常处理操作。</param>
        /// <returns></returns>
        public static ISettingsBuilder SetFileExceptionHandler(this ISettingsBuilder builder, Action<FileExceptionContext> handler)
        {
            builder.Properties[FileExceptionHandlerKey] = handler;
            return builder;
        }

        /// <summary>
        /// 获取处理文件异常的操作。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>使用的异常处理。</returns>
        public static Action<FileExceptionContext> GetileExceptionHandler(this ISettingsBuilder builder)
        {
            if (builder.Properties.TryGetValue(FileExceptionHandlerKey, out object? value) && value is Action<FileExceptionContext> handler)
            {
                return handler;
            }
            return _ => { };
        }
    }
}
