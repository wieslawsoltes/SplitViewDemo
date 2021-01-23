using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace SplitViewDemo
{
    public class MainWindow : Window
    {
        private SplitView _splitView;
        private Button _pinButton;
        private TextBlock _pinTextBlock;
        private LayoutTransformControl _layoutTransform;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _splitView = this.FindControl<SplitView>("SplitView");
            _pinButton = this.FindControl<Button>("PinButton");
            _pinTextBlock = this.FindControl<TextBlock>("PinTextBlock");
            _layoutTransform = this.FindControl<LayoutTransformControl>("LayoutTransform");

            _splitView.GetObservable(SplitView.IsPaneOpenProperty).Subscribe((isPaneOpen) =>
            {
                if (_layoutTransform.LayoutTransform is RotateTransform rotateTransform)
                {
                    rotateTransform.Angle = isPaneOpen ? 0 : 90;
                }
            });

            _splitView.GetObservable(SplitView.DisplayModeProperty).Subscribe((displayMode) =>
            {
                if (_splitView.DisplayMode == SplitViewDisplayMode.Inline)
                {
                    _pinButton.Content = this.Resources["PinIcon"];
                }
                else if (_splitView.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                {
                    _pinButton.Content = this.Resources["PinOffIcon"];
                }
            });

            _pinButton.Click += (sender, e) =>
            {
                if (_splitView.DisplayMode == SplitViewDisplayMode.Inline)
                {
                    _splitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                    _splitView.IsPaneOpen = false;
                }
                else if (_splitView.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                {
                    _splitView.DisplayMode = SplitViewDisplayMode.Inline;
                    _splitView.IsPaneOpen = true;
                }
            };

            _pinTextBlock.PointerPressed += (sender, e) =>
            {
                if (_splitView.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                {
                    _splitView.IsPaneOpen = !_splitView.IsPaneOpen;
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
