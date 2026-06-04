using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using LacmusApp.Avalonia.Managers;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using ReactiveUI;
using LacmusApp.Avalonia.Models;
using LacmusApp.Avalonia.Services;
using LacmusApp.Avalonia.Services.Files;
using LacmusApp.Avalonia.ViewModels;

namespace LacmusApp.Avalonia.Views
{
    public sealed partial class WizardWindow : ReactiveWindow<WizardWindowViewModel>
    { 
        public LocalizationContext LocalizationContext { get; }
        public ThemeManager ThemeManager { get; }
        public WizardWindow(LocalizationContext localizationContext, ThemeManager themeManager)
        {
            LocalizationContext = localizationContext;
            ThemeManager = themeManager;
            var localThemeManager = new ThemeManager(this);
            localThemeManager.UseTheme(themeManager.CurrentTheme);
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
        public WizardWindow() { }
    }
}