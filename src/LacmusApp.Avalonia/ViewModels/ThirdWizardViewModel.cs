using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using Avalonia.Threading;
using LacmusApp.Appearance.Enums;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using LacmusApp.Avalonia.Managers;
using LacmusApp.Avalonia.Models;
using LacmusApp.Avalonia.Services;
using LacmusApp.Avalonia.Services.Plugin;
using LacmusApp.Avalonia.Views;
using LacmusApp.Screens.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Serilog;
using OperatingSystem = LacmusPlugin.OperatingSystem;

namespace LacmusApp.Avalonia.ViewModels
{
    public partial class ThirdWizardViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ApplicationStatusManager _applicationStatusManager;
        private WizardWindow _window;
        private SettingsViewModel _settingsViewModel;
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        [Reactive] private string _name = "None";
        [Reactive] private string _author = "None";
        [Reactive] private string _company = "None";
        [Reactive] private string _description = "None";
        [Reactive] private string _tag = "None";
        [Reactive] private string _inferenceType = "None";
        [Reactive] private string _version = "None";
        [Reactive] private string _url = "None";
        [Reactive] private string _operatingSystems = "None";
        [Reactive] private string _status = "Not ready";
        [Reactive] private string _error;
        [Reactive] private bool _isError = false;
        [Reactive] private bool _isShowLoadModelButton = false;
        [Reactive] private LocalizationContext _localizationContext;
        
        public ReactiveCommand<Unit, Unit> LoadModelCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateModelStatusCommand { get; }

        public ThirdWizardViewModel(IScreen screen, WizardWindow window, SettingsViewModel settingsViewModel, ApplicationStatusManager manager, LocalizationContext localizationContext)
        {
            _applicationStatusManager = manager;
            _window = window;
            LocalizationContext = localizationContext;
            HostScreen = screen;
            _settingsViewModel = settingsViewModel;
            LoadModelCommand = ReactiveCommand.Create(LoadModel);
            UpdateModelStatusCommand = ReactiveCommand.Create(UpdateModelStatus);
        }

        public async void UpdateModelStatus()
        {
            _applicationStatusManager.ChangeCurrentAppStatus(Enums.Status.Working, "Working | loading model...");
            //get the last version of ml model with specific config
            try
            {
                Log.Information("Loading ml model.");
                Status = "Loading ml model...";

                var plugin = _settingsViewModel.Plugin;
                if (plugin.HasErrorMessage)
                    throw new Exception("No such plugin");
                
                Dispatcher.UIThread.Post(() =>
                {
                    Name = plugin.Name;
                    Author = plugin.Author;
                    Company = plugin.Company;
                    Description = plugin.Description;
                    Tag = plugin.Tag;
                    InferenceType = plugin.InferenceType.ToString();
                    Version = plugin.Version.ToString();
                    Url = plugin.Url;
                    OperatingSystems = ConvertOperatingSystemsToString(plugin.OperatingSystems);
                    Status = $"Ready";
                });
                IsError = false;
                Log.Information("Successfully loads ml model.");
            }
            catch (Exception e)
            {
                Status = $"Not ready.";
                IsError = true;
                Error = $"Error: {e.Message}";
                IsShowLoadModelButton = true;
                Log.Error(e, "Unable to load model.");
            }
            _applicationStatusManager.ChangeCurrentAppStatus(Enums.Status.Ready, "");
        }

        private async void LoadModel()
        {
            Settings settingsWindow = new Settings();
            settingsWindow.DataContext = _settingsViewModel;
            var themeManager = new ThemeManager(settingsWindow);
            themeManager.UseTheme(_settingsViewModel.Theme);
            _settingsViewModel.OnRequestClose += (s, e) => settingsWindow.Close();
            _settingsViewModel.OnRequestRestart += (sender, args) => RestartApp();
            settingsWindow.Show();
        }
        private string ConvertOperatingSystemsToString(IEnumerable<OperatingSystem> operatingSystems)
        {
            var result = "";
            foreach (var os in operatingSystems)
            {
                switch (os)
                {
                    case OperatingSystem.AndroidArm:
                        result += "Android";
                        break;
                    case OperatingSystem.IosArm:
                        result += "IOS";
                        break;
                    case OperatingSystem.LinuxAmd64:
                        result += "Linux";
                        break;
                    case OperatingSystem.LinuxArm:
                        result += "Linux (ARM)";
                        break;
                    case OperatingSystem.OsxAmd64:
                        result += "OSX (amd64)";
                        break;
                    case OperatingSystem.OsxArm:
                        result += "OSX (Apple Silicon)";
                        break;
                    case OperatingSystem.WindowsAmd64:
                        result += "Windows";
                        break;
                    case OperatingSystem.WindowsArm:
                        result += "Windows (ARM)";
                        break;
                    default:
                        result += os.ToString();
                        break;
                }
                result += ";";
            }

            return result;
        }
        
        private async void RestartApp()
        {
            var msg = "To apply settings you need to restart application.";
            if (LocalizationContext.Language == Language.Russian)
                msg = "Чтобы применить настройки необходим перезапуск программы.";
            var msgbox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Need to restart",
                ContentMessage = msg,
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                ShowInCenter = true
            });
            var result = await msgbox.ShowAsync();
            Environment.Exit(0);
        }
    }
}