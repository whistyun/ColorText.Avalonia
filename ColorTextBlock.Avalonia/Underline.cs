using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColorTextBlock.Avalonia
{
    public class Underline : ArrangeRun
    {
        public override void OwnerDraw(DrawingContext context, FormattedText text, IBrush foreground, IBrush background, Rect bounds)
        {
            if (background != null)
                context.FillRectangle(background, bounds);

            var pen = new Pen(foreground);


            context.DrawText(foreground, bounds.Position, text);
            context.DrawLine(pen, bounds.BottomLeft, bounds.BottomRight);
        }
    }
}
