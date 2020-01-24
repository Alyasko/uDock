using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LiteDB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using uDock.Core;
using uDock.Core.Model;

namespace uDock.Wpf.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly ApplicationState _app;

        public ObservableCollection<LinkItem> LinkItems => _app.LinkItems;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ApplicationState app)
        {
            _app = app;
        }

        private LinkItem _selectedLink;

        public LinkItem SelectedLink
        {
            get { return _selectedLink; }
            set
            {
                _selectedLink = value;
                RaisePropertyChanged();
            }
        }

        private WindowState _windowState;

        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CloseCommand => new RelayCommand(() =>
        {
            App.Current.Shutdown();
        });

        public ICommand TrayIconClickCommand => new RelayCommand(() =>
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Minimized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        });

        private void GetUrisToExecute(LinkItem rootItem, List<string> uris)
        {
            if (rootItem.Children.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(rootItem.Uri))
                    uris.Add(rootItem.Uri);
            }
            else
            {
                foreach (var item in rootItem.Children)
                {
                    GetUrisToExecute(item, uris);
                }
            }
        }

        public void ExecuteLink()
        {
            if (SelectedLink == null)
                return;

            var uris = new List<string>();
            GetUrisToExecute(SelectedLink, uris);

            var proceed = true;

            if (uris.Count > 5)
            {
                if (MessageBox.Show($"Are you sure you want to start {uris.Count} processes?", "Question",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    proceed = false;
                }
            }

            if (proceed)
            {
                foreach (var uri in uris)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            Process.Start(uri);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            MessageBox.Show($"Unable to start link {uri}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                }
            }
        }
    }
}