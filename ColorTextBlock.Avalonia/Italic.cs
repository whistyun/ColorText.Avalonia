using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;
using FStyle = Avalonia.Media.FontStyle;

namespace ColorTextBlock.Avalonia
{
    public class Italic : Run
    {
        public Italic()
        {
            FontStyle = FStyle.Italic;
        }
    }
}
