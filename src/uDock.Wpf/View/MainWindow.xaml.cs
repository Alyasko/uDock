using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MediatR;
using uDock.Wpf.Model;
using uDock.Wpf.ViewModel;

namespace uDock.Wpf.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

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

        private void TrvLinks_OnDrop(object sender, DragEventArgs e)
        {
            _mainViewModel.HandleDrop(e);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button btn && btn.DataContext is LinkItem li)
                _mainViewModel.SelectedLink = li;

            if (_mainViewModel.SelectedLink == null)
                return;

            _mainViewModel.ExecuteLink();
        }

        private void BtnSetupMode_OnClick(object sender, RoutedEventArgs e)
        {
            if (BtnSetupMode.IsChecked != null)
            {
                var gridWidth = GridSetup.Width;
                if (BtnSetupMode.IsChecked.Value)
                {
                    GridSetup.Visibility = Visibility.Visible;
                    WindowMain.Width += gridWidth;
                }
                else
                {
                    GridSetup.Visibility = Visibility.Collapsed;
                    WindowMain.Width -= gridWidth;
                }

            }
        }
    }
}
