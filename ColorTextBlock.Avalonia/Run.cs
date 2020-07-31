using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using System;
using System.ComponentModel;

namespace ColorTextBlock.Avalonia
{
    [TypeConverter(typeof(StringToRunConverter))]
    public class Run : AvaloniaObject
    {
        public static readonly StyledProperty<IBrush> BackgroundProperty =
            AvaloniaProperty.Register<Run, IBrush>(nameof(Foreground));

        public static readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<Run, IBrush>(nameof(Foreground));

        public static readonly StyledProperty<FontFamily> FontFamilyProperty =
            AvaloniaProperty.Register<Run, FontFamily>(nameof(FontFamily));

        public static readonly StyledProperty<double?> FontSizeProperty =
            AvaloniaProperty.Register<Run, double?>(nameof(FontSize));

        public static readonly StyledProperty<FontStyle?> FontStyleProperty =
            AvaloniaProperty.Register<Run, FontStyle?>(nameof(FontStyle));

        public static readonly StyledProperty<FontWeight?> FontWeightProperty =
            AvaloniaProperty.Register<Run, FontWeight?>(nameof(FontWeight));

        public static readonly StyledProperty<String> TextProperty =
            AvaloniaProperty.Register<Run, String>(nameof(Text));

        public IBrush Background
        {
            get { return GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public IBrush Foreground
        {
            get { return GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public FontFamily FontFamily
        {
            get { return GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public double? FontSize
        {
            get { return GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public FontStyle? FontStyle
        {
            get { return GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public FontWeight? FontWeight
        {
            get { return GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        [Content]
        public String Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        internal FormattedText Measure(
            Size constraint,
            FontFamily parentFontFamily,
            double parentFontSize,
            FontStyle parentFontStyle,
            FontWeight parentFontWeight,
            TextWrapping parentWrapping)
        {
            var typeface = new Typeface(
                    FontFamily ?? parentFontFamily,
                    FontSize.HasValue ? FontSize.Value : parentFontSize,
                    FontStyle.HasValue ? FontStyle.Value : parentFontStyle,
                    FontWeight.HasValue ? FontWeight.Value : parentFontWeight);

            return new FormattedText
            {
                Constraint = constraint,
                Typeface = typeface,
                Text = Text ?? string.Empty,
                TextAlignment = TextAlignment.Left,
                Wrapping = parentWrapping
            };
        }
    }
}
