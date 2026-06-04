using System.IO;
using LacmusApp.Screens.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace LacmusApp.Screens.ViewModels;

public partial class LogViewModel : ReactiveObject, ILogViewModel, ILogEventSink
{
    private readonly ITextFormatter _formatter;
    
    public LogViewModel()
    {
        LogText = "";
        var template = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        _formatter = new MessageTemplateTextFormatter(template);
    }

    [Reactive] private string _logText;
    
    public void Emit(LogEvent logEvent)
    {
        var sw = new StringWriter();
        _formatter.Format(logEvent, sw);
        LogText += sw.ToString();
    }
}