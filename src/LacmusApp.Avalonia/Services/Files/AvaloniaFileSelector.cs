using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace LacmusApp.Avalonia.Services.Files
{
    public class AvaloniaFileSelector : IAvaloniaFileSelector
    {
        private readonly Window _window;

        public AvaloniaFileSelector(Window window) => _window = window;

        public async Task<string> SelectFile(string title = null)
        {
            var files = await StorageDialog.PickFilesAsync(title ?? "Select file", allowMultiple: false, window: _window);
            var path = files.First();

            var attributes = File.GetAttributes(path);
            var isFolder = attributes.HasFlag(FileAttributes.Directory);
            if (isFolder) throw new Exception("Folders are not supported.");
            return path;
        }

        public async Task<string> SelectDir(string title = null)
        {
            var path = await StorageDialog.PickFolderAsync(title ?? "Select folder", _window);

            var attributes = File.GetAttributes(path);
            var isFolder = attributes.HasFlag(FileAttributes.Directory);
            if (!isFolder) throw new Exception("Files are not supported.");
            return path;
        }

        public async Task<IEnumerable<string>> SelectFiles(string title = null)
        {
            var files = await StorageDialog.PickFilesAsync(title ?? "Select files", allowMultiple: true, window: _window);
            return files.Where(x => File.GetAttributes(x).HasFlag(FileAttributes.Directory));
        }

        public async Task<IEnumerable<string>> SelectAllFilesFromDir(string title = null, bool isRecursive = false)
        {
            var dirPath = await StorageDialog.PickFolderAsync(title ?? "Select folder", _window);

            if (string.IsNullOrEmpty(dirPath) || !Directory.Exists(dirPath))
                return Enumerable.Empty<string>();

            return GetFilesFromDir(dirPath, isRecursive);
        }

        private static IEnumerable<string> GetFilesFromDir(string dirPath, bool isRecursive)
        {
            return Directory.GetFiles(dirPath, "*.*",
                isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }
    }
}
