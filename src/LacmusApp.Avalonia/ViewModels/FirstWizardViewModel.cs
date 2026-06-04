using System;
using System.IO;
using System.Reactive;
using Avalonia.Controls;
using System.Threading.Tasks;
using LacmusApp.Avalonia.Services;
using LacmusApp.Avalonia.Services.Files;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Serilog;

namespace LacmusApp.Avalonia.ViewModels
{
    public partial class FirstWizardViewModel : ReactiveValidationObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        public ReactiveCommand<Unit, Unit> OpenPhotos { get; }

        [Reactive] private string _inputPath;
        [Reactive] private LocalizationContext _localizationContext;

        public FirstWizardViewModel(IScreen screen, LocalizationContext localizationContext)
        {
            HostScreen = screen;
            LocalizationContext = localizationContext;
            
            this.ValidationRule(
                viewModel => viewModel.InputPath,
                Directory.Exists,
                path => $"Incorrect path {path}");
            
            OpenPhotos = ReactiveCommand.CreateFromTask(Open);
        }

        private async Task Open()
        {
            try
            {
                var dirPath = await StorageDialog.PickFolderAsync("Chose directory image files");
                if (!string.IsNullOrEmpty(dirPath))
                    InputPath = dirPath;
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to setup input path.");
            }
        }
    }
}