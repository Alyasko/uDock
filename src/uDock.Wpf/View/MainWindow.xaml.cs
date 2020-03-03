using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;
using uDock.Core.Model;
using uDock.Wpf.ViewModel;

namespace uDock.Wpf.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _mainViewModel = DataContext as MainViewModel;
        }

        private void TbTitle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TrvLinks_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _mainViewModel.SelectedLink = e.NewValue as LinkItem;
        }

        private void BtnSettings_OnClick(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private void PositionWindow()
        {
            var location = _mainViewModel.WindowService.LoadLocation();
            if (location == null)
                return;

            Top = location.Top;
            Left = location.Left;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            PositionWindow();

            var wWidth = SystemParameters.PrimaryScreenWidth;
            var wHeight = SystemParameters.PrimaryScreenHeight;

            _mainViewModel.WindowService.ScreenSize = new SizeF((float)wWidth, (float)wHeight);

            _mainViewModel.WindowService.WindowPositionChangeRequested += WindowServiceWindowPositionChangeRequested;
        }

        private void WindowServiceWindowPositionChangeRequested(object sender, WindowPositionChangeRequestedEventArgs e)
        {
            if (e.NewTop != null)
                Top = e.NewTop.Value;

            if (e.NewLeft != null)
                Left = e.NewLeft.Value;
        }

        private void GridLinkItem_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (e.OriginalSource is TextBlock btn && btn.DataContext is LinkItem li)
                    _mainViewModel.SelectedLink = li;

                if (_mainViewModel.SelectedLink == null)
                    return;

                _mainViewModel.ExecuteLink();
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _mainViewModel.WindowService.SaveLocation(Top, Left);
        }

        private void TrayIcon_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TrayIcon_OnTrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            _mainViewModel.WindowService.Toggle();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.WindowService.ResetPosition();
        }

        private void MainWindow_OnLocationChanged(object sender, EventArgs e)
        {
            _mainViewModel.WindowService.WindowLocation = new PointF((float)Left, (float)Top);
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _mainViewModel.WindowService.WindowSize = new SizeF((float)ActualWidth, (float)ActualHeight);
        }
    }
}
