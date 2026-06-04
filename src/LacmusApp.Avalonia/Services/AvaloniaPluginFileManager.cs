using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using LacmusApp.Avalonia.Services.Files;
using LacmusApp.IO.Interfaces;

namespace LacmusApp.Avalonia.Services
{
    public class AvaloniaPluginDialog : IDialog
    {
        private Window _window;

        public AvaloniaPluginDialog(Window window)
        {
            _window = window;
        }

        public async Task<string> SelectToWrite()
        {
            var file = await StorageDialog.SaveFileAsync(
                "Chose plugin", new[] { StorageDialog.ZipFilter }, _window);

            if (file == null)
                throw new Exception("File is not selected");
            return file;
        }

        public async Task<string> SelectToRead()
        {
            var files = await StorageDialog.PickFilesAsync(
                "Chose plugin", allowMultiple: false,
                filters: new[] { StorageDialog.ZipFilter }, window: _window);

            if (files == null || files.Count == 0)
                throw new Exception("File is not selected");
            var file = files[0];
            if (!File.Exists(file))
                throw new Exception("File is not exists");
            var attributes = File.GetAttributes(file);
            if (attributes.HasFlag(FileAttributes.Directory))
                throw new Exception("Folders are not supported");

            return file;
        }
    }
}
