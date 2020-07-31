# ColorTextBlock.Avalonia

This is an experimental code and contains a wrong method about linebreak.

## sample

**MainWindow.axaml**
```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ctxt="clr-namespace:ColorTextBlock.Avalonia;assembly=ColorTextBlock.Avalonia"
        x:Class="ColorTextBlock.Sample.Views.MainWindow"
        Title="ColorTextBlock.Sample">

  <StackPanel Orientation="Vertical">
    <TextBlock>English</TextBlock>
    <ctxt:ColorTextBlock TextWrapping="Wrap" xml:space="preserve" Margin="10,0,0,0">
      <ctxt:Strike>strikethrough</ctxt:Strike> <ctxt:Underline>underline</ctxt:Underline>
      <ctxt:Bold>bold</ctxt:Bold> <ctxt:Italic>italic</ctxt:Italic>
      <ctxt:BoldItalic>bold-italic</ctxt:BoldItalic>
      <ctxt:LineBreak/>
      <ctxt:Run Foreground="Brown" FontWeight="Bold" >The quick brown fox</ctxt:Run>
      <ctxt:Run FontWeight="Bold">jumps over</ctxt:Run>
      <ctxt:Run Foreground="Turquoise" Background="Brown">the lazy brown dog</ctxt:Run>
      <ctxt:LineBreak/>
      <ctxt:LineBreak/>

      Where is WhitespaceSignificantCollectionAttribute?
      How do i get space between an element and an element?      
    </ctxt:ColorTextBlock>

    <TextBlock>Japanese</TextBlock>
    <ctxt:ColorTextBlock TextWrapping="Wrap" xml:space="preserve" Margin="10,0,0,0">
      吾輩は猫である。名前はまだ無い。
      どこで生れたかとんと見当がつかぬ。何でも薄暗いじめじめした所で<ctxt:Bold Foreground="Blue">ニャーニャー泣いていた</ctxt:Bold>事だけは記憶している。
      吾輩はここで始めて人間というものを見た。しかもあとで聞くとそれは書生という人間中で一番獰悪どうあくな種族であったそうだ。
    </ctxt:ColorTextBlock>
  </StackPanel>
  </Window>
```

![ColorTextBlock.Demo.png](ColorTextBlock.Demo.png)