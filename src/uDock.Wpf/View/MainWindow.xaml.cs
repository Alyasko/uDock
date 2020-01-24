using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using MediatR;
using uDock.Core.Model;
using uDock.Wpf.ViewModel;
using Button = System.Windows.Controls.Button;
using DragEventArgs = System.Windows.DragEventArgs;
using MessageBox = System.Windows.MessageBox;

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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var width = this.Width;
            //var height = this.Height;

            //var wWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            //var wHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            //var r = Screen.PrimaryScreen.WorkingArea;

            ////Top = 
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
    }
}
