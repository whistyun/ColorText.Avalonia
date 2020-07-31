using System;
using System.Collections.Generic;
using System.Text;

namespace ColorTextBlock.Avalonia
{
    public class LineBreak : Run
    {
        public LineBreak()
        {
            Text = "\r\n";
        }
    }
}
