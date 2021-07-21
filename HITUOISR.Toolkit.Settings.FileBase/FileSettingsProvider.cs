using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;

namespace HITUOISR.Toolkit.Settings.FileBase
{
    /// <summary>
    /// 基于文件的设置提供器。
    /// </summary>
    public abstract class FileSettingsProvider : SettingsProvider
    {
        private bool _loaded = false;

        /// <summary>
        /// 设置源。
        /// </summary>
        public new FileSettingsSource Source => (FileSettingsSource)base.Source;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="source"></param>
        public FileSettingsProvider(FileSettingsSource source) : base(source) { }

        /// <inheritdoc/>
        public override string ToString() => $"{GetType().Name}: {Source.Path}";

        /// <inheritdoc/>
        public override bool TryLoad(ISettingsKeyInfo key, bool reloadRequest = false)
        {
            if (reloadRequest || !_loaded)
            {
                try
                {
                    Preload(reloadRequest);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    return false;
                }
                _loaded = true;
            }
            return TryLoadKey(key);
        }

        /// <inheritdoc/>
        public override bool Save()
        {
            IFileInfo? file = Source.FileProvider?.GetFileInfo(Source.Path);
            if (file is null) return false;
            try
            {
                using var stream = OpenWrite(file);
                Save(stream);
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private void Preload(bool reloading)
        {
            IFileInfo? file = Source.FileProvider?.GetFileInfo(Source.Path);
            if (file is null || !file.Exists)
            {
                Data = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                if (Source.AutoCreate && file != null && Source.FileProvider is PhysicalFileProvider provider)
                {
                    System.IO.File.Create(Path.Combine(provider.Root, file.Name)).Dispose();
                }
                else
                {
                    throw new NotSupportedException($"无法在非物理文件系统 {Source.FileProvider} 中自动创建设置文件！");
                }
            }
            else
            {
                using var stream = OpenRead(file);
                try
                {
                    Preload(stream);
                }
                catch
                {
                    if (reloading)
                    {
                        Data = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// 从流中预加载数据。
        /// </summary>
        /// <param name="stream">要加载的流。</param>
        protected abstract void Preload(Stream stream);

        /// <summary>
        /// 根据键加载数据。
        /// </summary>
        /// <remarks>
        /// 保证在预加载完成后进行。
        /// </remarks>
        /// <param name="key">要加载的设置键键信息。</param>
        /// <returns>指示是否成功。</returns>
        protected abstract bool TryLoadKey(ISettingsKeyInfo key);

        /// <summary>
        /// 保存设置到流。
        /// </summary>
        /// <param name="stream">要写入的流。</param>
        protected abstract void Save(Stream stream);

        private void HandleException(Exception ex)
        {
            bool ignoreException = false;
            if (Source.ExceptionHandler != null)
            {
                FileExceptionContext context = new(this, ex);
                Source.ExceptionHandler.Invoke(context);
                ignoreException = context.Ignore;
            }
            if (!ignoreException)
            {
                ExceptionDispatchInfo.Throw(ex);
            }
        }

        private static Stream OpenRead(IFileInfo fileInfo)
        {
            if (fileInfo.PhysicalPath != null)
            {
                // IFileInfo.CreateReadStream 默认使用异步IO，有额外开销
                return new FileStream(
                    fileInfo.PhysicalPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 1,
                    FileOptions.SequentialScan);
            }

            return fileInfo.CreateReadStream();
        }

        private static Stream OpenWrite(IFileInfo fileInfo)
        {
            if (fileInfo.PhysicalPath != null)
            {
                return new FileStream(
                    fileInfo.PhysicalPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None);
            }
            throw new NotSupportedException($"Cannot write to the non-physical file {fileInfo.Name}.");
        }
    }
}
