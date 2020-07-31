using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace ColorTextBlock.Avalonia
{
    public class ColorTextBlock : Control
    {
        public static readonly StyledProperty<IBrush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<ColorTextBlock>();

        public static readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<ColorTextBlock, IBrush>(
                nameof(Foreground), defaultValue: Brushes.Black);

        public static readonly StyledProperty<FontFamily> FontFamilyProperty =
            AvaloniaProperty.Register<ColorTextBlock, FontFamily>(
                nameof(FontFamily), defaultValue: FontFamily.Default);

        public static readonly StyledProperty<double> FontSizeProperty =
            AvaloniaProperty.Register<ColorTextBlock, double>(
                nameof(FontSize), defaultValue: 12);

        public static readonly StyledProperty<FontStyle> FontStyleProperty =
            AvaloniaProperty.Register<ColorTextBlock, FontStyle>(
                nameof(FontStyle), defaultValue: FontStyle.Normal);

        public static readonly StyledProperty<FontWeight> FontWeightProperty =
            AvaloniaProperty.Register<ColorTextBlock, FontWeight>(
                nameof(FontWeight), defaultValue: FontWeight.Normal);

        public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
            AvaloniaProperty.Register<TextBlock, TextWrapping>(nameof(TextWrapping));

        public static readonly DirectProperty<ColorTextBlock, IEnumerable<Run>> ContentProperty =
            AvaloniaProperty.RegisterDirect<ColorTextBlock, IEnumerable<Run>>(
                nameof(Content),
                    o => o.Content,
                    (o, v) => o.Content = v);

        public static readonly DirectProperty<ColorTextBlock, string> TextProperty =
            AvaloniaProperty.RegisterDirect<ColorTextBlock, string>(
                nameof(Text),
                    o => o.Text,
                    (o, v) => o.Text = v);

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

        public double FontSize
        {
            get { return GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public FontStyle FontStyle
        {
            get { return GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public FontWeight FontWeight
        {
            get { return GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public TextWrapping TextWrapping
        {
            get { return GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        [Content]
        public IEnumerable<Run> Content
        {

            get => _content;
            set
            {
                var olds = _content;

                if (SetAndRaise(ContentProperty, ref _content, value))
                {
                    // remove change listener
                    foreach (var oldrun in olds)
                        oldrun.PropertyChanged -= OnRunPropertiewChanged;


                    // set change listener
                    foreach (var newrun in _content)
                        newrun.PropertyChanged += OnRunPropertiewChanged;
                }
            }
        }

        public string Text
        {
            get => String.Concat(_content.Select(run => run.Text));
            set => Content = new List<Run>(new[] { new Run { Text = value } });
        }

        private IEnumerable<Run> _content = new List<Run>();
        private List<TextLayout> layouts = new List<TextLayout>();

        static ColorTextBlock()
        {
            ClipToBoundsProperty.OverrideDefaultValue<ColorTextBlock>(true);

            AffectsRender<ColorTextBlock>(
                BackgroundProperty,
                ForegroundProperty,
                FontWeightProperty,
                FontSizeProperty,
                FontStyleProperty);

            Observable.Merge(
                ContentProperty.Changed,
                FontSizeProperty.Changed,
                FontStyleProperty.Changed,
                FontWeightProperty.Changed,
                TextWrappingProperty.Changed
            ).AddClassHandler<ColorTextBlock>((x, _) => x.OnTextPropertiesChanged());
        }

        private void OnRunPropertiewChanged(object sender, AvaloniaPropertyChangedEventArgs args)
        {
            var prop = args.Property;

            if (prop == Run.FontFamilyProperty
                || prop == Run.FontSizeProperty
                || prop == Run.FontStyleProperty
                || prop == Run.FontWeightProperty
                || prop == Run.TextProperty)
            {
                OnTextPropertiesChanged();
            }

            if (prop == Run.TextProperty)
            {
                var oldTxt = new StringBuilder();
                var newTxt = new StringBuilder();

                foreach (var o in Content)
                {
                    if (Object.ReferenceEquals(o, sender))
                    {
                        oldTxt.Append(args.OldValue);
                        newTxt.Append(args.NewValue);
                    }
                    else
                    {
                        oldTxt.Append(o.Text);
                        newTxt.Append(o.Text);
                    }
                }

                RaisePropertyChanged(TextProperty, oldTxt.ToString(), newTxt.ToString());
            }
        }

        private void OnTextPropertiesChanged()
        {
            InvalidateMeasure();
        }


        protected override Size MeasureOverride(Size availableSize)
        {
            if (Content == null) return Size.Empty;

            double maxWid = 0;
            double maxHei = 0;
            layouts = new List<TextLayout>();

            if (TextWrapping == TextWrapping.Wrap)
            {
                Point point = new Point();

                var nextPointY = 0d;
                var entireWidth = availableSize.Width;

                foreach (var run in Content)
                {
                    var runTxt = run.Text;
                    var fmt = run.Measure(Size.Infinity, FontFamily, FontSize, FontStyle, FontWeight, TextWrapping);

                    if (runTxt == "\r\n" | runTxt == "\r" | runTxt == "\n")
                    {
                        if (point.X != 0d)
                        {
                            point = new Point(0, nextPointY);
                        }
                        else
                        {
                            fmt.Text = " ";
                            point = new Point(0, point.Y + fmt.Bounds.Bottom);
                        }
                        continue;
                    }

                    /*
                     * It is hacking-resolution for 'line breaking rules'.
                     * 
                     * TODO 後で、英訳する。
                     * 
                     * Avalonia(9.11)のFormattedTextでは、
                     * 矩形範囲に単一のスタイルで文字描画したときの改行位置しか計算できません。
                     * 
                     * そのため、 既に適当な文字を入力した後に、追加で別の文言を描画しようとした時、
                     * 以下のどちらの理由で改行が発生したか判断ができません。
                     * 
                     * 　理由1.余白が小さすぎるため改行が行われた
                     * 　理由2.描画領域が狭く(あるいは単語が長すぎるため)無理やり改行が行われた
                     * 
                     * 先頭にスペースを入れて改行位置を計算させることで、
                     * 理由1でも理由2でも先頭で改行が行われるようにしています。
                     * (この場合、スペース1文字を追加したために理由1に該当してしまう可能性がありますが、
                     *  スペースの横幅は小さいため、不自然には見えないと期待しています)
                     */
                    if (point.X != 0d)
                    {
                        var remainWidth = entireWidth - point.X;
                        fmt.Text = " " + runTxt;
                        fmt.Constraint = new Size(remainWidth, Double.PositiveInfinity);


                        if (fmt.GetLines().First().Length == 1)
                        {
                            point = new Point(0, nextPointY);
                        }
                        else
                        {
                            fmt.Text = runTxt;
                            fmt.Constraint = new Size(remainWidth, Double.PositiveInfinity);

                            var lines = fmt.GetLines().ToArray();

                            var firstLine = lines[0];

                            var subTxt = runTxt.Substring(0, firstLine.Length);
                            if (subTxt.EndsWith("\r\n")) subTxt = subTxt.Substring(0, subTxt.Length - 2);
                            else if (subTxt.EndsWith("\r")) subTxt = subTxt.Substring(0, subTxt.Length - 1);
                            else if (subTxt.EndsWith("\n")) subTxt = subTxt.Substring(0, subTxt.Length - 1);

                            var layout = new TextLayout(run, fmt, subTxt, point);

                            layouts.Add(layout);
                            maxWid = Math.Max(maxWid, layout.Bounds.Right);
                            maxHei = Math.Max(maxHei, layout.Bounds.Bottom);


                            if (lines.Length == 1)
                            {
                                point = new Point(layout.Bounds.Right, point.Y);
                                continue;
                            }
                            else
                            {
                                point = new Point(0, layout.Bounds.Bottom);
                                runTxt = runTxt.Substring(firstLine.Length);
                            }
                        }
                    }


                    fmt.Text = runTxt;
                    fmt.Constraint = new Size(entireWidth, Double.PositiveInfinity);

                    var first = true;
                    int stIdx = 0;
                    foreach (var line in fmt.GetLines().ToArray())
                    {
                        if (!first)
                            point = new Point(0, layouts[layouts.Count - 1].Bounds.Bottom);

                        var subTxt = runTxt.Substring(stIdx, line.Length);
                        if (subTxt.EndsWith("\r\n")) subTxt = subTxt.Substring(0, subTxt.Length - 2);
                        else if (subTxt.EndsWith("\r")) subTxt = subTxt.Substring(0, subTxt.Length - 1);
                        else if (subTxt.EndsWith("\n")) subTxt = subTxt.Substring(0, subTxt.Length - 1);

                        var layout = new TextLayout(run, fmt, subTxt, point);

                        layouts.Add(layout);
                        maxWid = Math.Max(maxWid, layout.Bounds.Right);
                        maxHei = Math.Max(maxHei, layout.Bounds.Bottom);

                        point = new Point(
                            point.X + layout.Bounds.Width,
                            point.Y);
                        stIdx += line.Length;

                        first = false;

                        nextPointY = layout.Bounds.Bottom;
                    }
                }
            }
            else
            {
                Point point = new Point();

                foreach (var run in Content)
                {
                    var fmt = run.Measure(Size.Infinity, FontFamily, FontSize, FontStyle, FontWeight, TextWrapping);
                    var runTxt = run.Text;

                    var first = true;
                    int stIdx = 0;
                    foreach (var line in fmt.GetLines())
                    {
                        if (!first)
                            point = new Point(0, layouts[layouts.Count - 1].Bounds.Bottom);

                        var subTxt = runTxt.Substring(stIdx, line.Length);
                        if (subTxt.EndsWith("\r\n")) subTxt = subTxt.Substring(0, subTxt.Length - 2);
                        else if (subTxt.EndsWith("\r")) subTxt = subTxt.Substring(0, subTxt.Length - 1);
                        else if (subTxt.EndsWith("\n")) subTxt = subTxt.Substring(0, subTxt.Length - 1);

                        var layout = new TextLayout(run, fmt, subTxt, point);

                        layouts.Add(layout);

                        maxWid = Math.Max(maxWid, layout.Bounds.Right);
                        maxHei = Math.Max(maxHei, layout.Bounds.Bottom);

                        point = new Point(
                            point.X + layout.Bounds.Width,
                            point.Y);
                        stIdx += line.Length;

                        first = false;
                    }
                }
            }

            return new Size(maxWid, maxHei);
        }

        public override void Render(DrawingContext context)
        {
            var background = Background;
            if (background != null)
            {
                context.FillRectangle(background, new Rect(Bounds.Size));
            }

            if (layouts != null)
            {
                foreach (var line in layouts)
                {
                    var run = line.Run;
                    var formatForeground = run.Foreground ?? Foreground;
                    var formatBackground = run.Background ?? background;

                    if (run is ArrangeRun arrange)
                    {
                        arrange.OwnerDraw(context, line.Format, formatForeground, formatBackground, line.Bounds);
                    }
                    else
                    {
                        if (formatBackground != null)
                            context.FillRectangle(formatBackground, line.Bounds);

                        context.DrawText(formatForeground, line.Bounds.Position, line.Format);
                    }
                }
            }
        }
    }

    class TextLayout
    {
        private FormattedText _Format;
        public FormattedText Format
        {
            get
            {
                _Format.Text = Text;
                return _Format;
            }
        }
        public Run Run { get; }
        public String Text { get; }
        public Rect Bounds { get; }

        public TextLayout(Run run, FormattedText fmt, string txt, Point startAt)
        {
            _Format = fmt;
            Run = run;
            Text = txt;

            Bounds = new Rect(startAt, Format.Bounds.Size);
        }
    }
}