using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using System;
using FWeight = Avalonia.Media.FontWeight;
using FStyle = Avalonia.Media.FontStyle;

namespace ColorTextBlock.Avalonia
{
    public class BoldItalic : Run
    {
        public BoldItalic()
        {
            FontWeight = FWeight.Bold;
            FontStyle = FStyle.Italic;
        }
    }
}
