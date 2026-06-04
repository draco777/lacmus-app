using System;
using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using LacmusApp.Avalonia.Extensions;
using LacmusApp.Avalonia.Managers;
using LacmusApp.Avalonia.Models;

namespace LacmusApp.Avalonia.ViewModels
{
    public partial class ApplicationStatusViewModel : ReactiveObject
    {
        public ApplicationStatusViewModel(
            ApplicationStatusManager applicationStatusManager)
        {
            applicationStatusManager.AppStatusInfoObservable
                .Subscribe(UpdateStatus);
        }
        
        [Reactive] private ISolidColorBrush _statusColor;
        [Reactive] private string _stringStatus;
        
        private void UpdateStatus(AppStatusInfo status)
        {
            StatusColor = status.GetColor();
            StringStatus = status.StringStatus;
        }
    }
}