using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using LacmusApp.Avalonia.Services;
using LacmusApp.Avalonia.Services.Files;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Serilog;

namespace LacmusApp.Avalonia.ViewModels
{
    public partial class SecondWizardViewModel : ReactiveValidationObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        public ReactiveCommand<Unit, Unit> SavePhotos { get; }

        [Reactive] private string _outputPath;
        [Reactive] private int _filterIndex = 0;
        [Reactive] private bool _isSaveCrop;
        [Reactive] private bool _isSaveXml;
        [Reactive] private bool _isSaveImage;
        [Reactive] private bool _isSaveDrawImage;
        [Reactive] private bool _isSaveGeoPosition;
        [Reactive] private LocalizationContext _localizationContext;

        public SecondWizardViewModel(IScreen screen, LocalizationContext localizationContext)
        {
            IsSaveXml = true;
            IsSaveImage = true;
            HostScreen = screen;
            LocalizationContext = localizationContext;
            this.ValidationRule(
                viewModel => viewModel.OutputPath,
                Directory.Exists,
                path => $"Incorrect path {path}");
            
            SavePhotos = ReactiveCommand.CreateFromTask(Save);
        }
        private async Task Save()
        {
            try
            {
                var dirPath = await StorageDialog.PickFolderAsync("Select folder to save");
                if (!string.IsNullOrEmpty(dirPath))
                    OutputPath = dirPath;
            }
            catch (Exception e)
            {
                Log.Error("Unable to setup input path.", e);
            }
        }
    }
}