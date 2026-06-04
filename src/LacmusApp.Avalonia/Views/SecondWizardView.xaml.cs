using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using LacmusApp.Avalonia.ViewModels;

namespace LacmusApp.Avalonia.Views
{
    public partial class SecondWizardView : ReactiveUserControl<SecondWizardViewModel>
    {
        public SecondWizardView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}