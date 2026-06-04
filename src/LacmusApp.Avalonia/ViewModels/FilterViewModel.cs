using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace LacmusApp.Avalonia.ViewModels
{
    public partial class FilterViewModel : ReactiveObject
    {
        [Reactive] private int _filterIndex = 0;
        [Reactive] private int _currentPage = 0;
    }
}