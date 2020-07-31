using Avalonia;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColorTextBlock.Avalonia
{
    public abstract class ArrangeRun : Run
    {
        public abstract void OwnerDraw(DrawingContext context, FormattedText text, IBrush foreground, IBrush background, Rect bounds);
    }
}
