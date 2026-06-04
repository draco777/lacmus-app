using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace LacmusApp.Avalonia.Models
{
    public partial class MetaData : ReactiveObject
    {
        [Reactive] private string _group;
        [Reactive] private string _tagName;
        [Reactive] private string _description;

        public MetaData(string group, string tagName, string description)
        {
            Group = group;
            TagName = tagName;
            Description = description;
        }
    }
}