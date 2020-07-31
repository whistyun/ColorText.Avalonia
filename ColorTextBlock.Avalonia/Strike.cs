using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColorTextBlock.Avalonia
{
    public class Strike : ArrangeRun
    {
        public override void OwnerDraw(DrawingContext context, FormattedText text, IBrush foreground, IBrush background, Rect bounds)
        {
            if (background != null)
                context.FillRectangle(background, bounds);

            var pen = new Pen(foreground);

            var point1 = new Point(bounds.X, bounds.Center.Y);
            var point2 = new Point(bounds.Right, bounds.Center.Y);

            context.DrawText(foreground, bounds.Position, text);
            context.DrawLine(pen, point1, point2);
        }
    }
}
