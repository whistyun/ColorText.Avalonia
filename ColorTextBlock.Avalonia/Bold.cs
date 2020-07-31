using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using System;
using FWeight = Avalonia.Media.FontWeight;

namespace ColorTextBlock.Avalonia
{
    public class Bold : Run
    {
        public Bold()
        {
            FontWeight = FWeight.Bold;
        }
    }
}
