using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace LacmusApp.Avalonia.Services.Files
{
    /// <summary>
    /// Avalonia 11 file/folder pickers via TopLevel.StorageProvider.
    /// Replaces the removed OpenFileDialog/SaveFileDialog/OpenFolderDialog APIs.
    /// </summary>
    public static class StorageDialog
    {
        public static readonly FilePickerFileType ZipFilter = new("ZIP archive") { Patterns = new[] { "*.zip" } };

        private static TopLevel ResolveTopLevel(Window window)
        {
            if (window is not null)
                return window;
            return (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        }

        public static async Task<string> PickFolderAsync(string title, Window window = null)
        {
            var top = ResolveTopLevel(window);
            if (top is null) return null;
            var res = await top.StorageProvider.OpenFolderPickerAsync(
                new FolderPickerOpenOptions { Title = title, AllowMultiple = false });
            return res.Count > 0 ? res[0].Path.LocalPath : null;
        }

        public static async Task<IReadOnlyList<string>> PickFilesAsync(
            string title, bool allowMultiple,
            IReadOnlyList<FilePickerFileType> filters = null, Window window = null)
        {
            var top = ResolveTopLevel(window);
            if (top is null) return new List<string>();
            var res = await top.StorageProvider.OpenFilePickerAsync(
                new FilePickerOpenOptions { Title = title, AllowMultiple = allowMultiple, FileTypeFilter = filters });
            return res.Select(f => f.Path.LocalPath).ToList();
        }

        public static async Task<string> SaveFileAsync(
            string title, IReadOnlyList<FilePickerFileType> filters = null, Window window = null)
        {
            var top = ResolveTopLevel(window);
            if (top is null) return null;
            var res = await top.StorageProvider.SaveFilePickerAsync(
                new FilePickerSaveOptions { Title = title, FileTypeChoices = filters });
            return res?.Path.LocalPath;
        }
    }
}
