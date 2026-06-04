using System.Collections.Generic;
using System.Threading.Tasks;

namespace LacmusApp.Avalonia.Services.Files
{
    public interface IAvaloniaFileSelector
    {
        Task<string> SelectFile(string title = null);
        Task<string> SelectDir(string title = null);
        Task<IEnumerable<string>> SelectFiles(string title = null);
        Task<IEnumerable<string>> SelectAllFilesFromDir(string title = null, bool isRecursive = false);
    }
}
