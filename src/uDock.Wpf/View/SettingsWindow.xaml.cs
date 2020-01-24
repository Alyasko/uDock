using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using uDock.Core.Model;
using uDock.Wpf.ViewModel;

namespace uDock.Wpf.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel _settingsViewModel;

        public SettingsWindow()
        {
            InitializeComponent();

            _settingsViewModel = DataContext as SettingsViewModel;
        }

        private void TbTitle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TrvLinks_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _settingsViewModel.SelectedLink = e.NewValue as LinkItem;
        }

        private void TrvLinks_OnDrop(object sender, DragEventArgs e)
        {
            _settingsViewModel.HandleDrop(e);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button btn && btn.DataContext is LinkItem li)
                _settingsViewModel.SelectedLink = li;

            if (_settingsViewModel.SelectedLink == null)
                return;

            _settingsViewModel.ExecuteLink();
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
    }
}
