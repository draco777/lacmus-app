using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using Avalonia.Media;
using LacmusApp.Avalonia.Models;
using ReactiveUI;
using LacmusApp.Image.Models;

namespace LacmusApp.Avalonia.ViewModels
{
    public class PhotoViewModel : Image<ImageBrush>
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<BoundBox>> _boundBoxes;
        
        public PhotoViewModel(int index)
        {
            Index = index;

            _boundBoxes = this.WhenAnyValue(x => x.Detections)
                .Select(x =>
                    x.Select(
                        obj => new BoundBox(obj.XMin, obj.YMin, obj.Height, obj.Width))
                        .ToList())
                .ToProperty(this, x => x.BoundBoxes);
        }
        public new string Name { get; set; }
        public int Index { get; }
        public IEnumerable<BoundBox> BoundBoxes => _boundBoxes.Value;
    }
}