using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        public LinkService LinkService { get; }
        public WindowService WindowService { get; }

        public ObservableCollection<LinkItem> LinkItems => _app.LinkItems;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ApplicationState app,
            LinkService linkService,
            WindowService windowService)
        {
            _app = app;
            LinkService = linkService;
            WindowService = windowService;
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
            Application.Current.Shutdown();
        });


        public void ExecuteLink()
        {
            if(SelectedLink == null)
                return;

            try
            {
                Process.Start(SelectedLink.Uri);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                MessageBox.Show($"Unable to start link {SelectedLink.Uri}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand RunLinkCommand => new RelayCommand(ExecuteLink);

        private void ExecuteMultiple()
        {
            var links = LinkService.GetChildLinks(SelectedLink).ToArray();

            var proceed = true;

            if (links.Length > 5)
            {
                if (MessageBox.Show($"Are you sure you want to start {links.Length} processes?", "Question",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    proceed = false;
                }
            }

            if (proceed)
            {
                foreach (var link in links)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            Process.Start(link.Uri);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            MessageBox.Show($"Unable to start link {link}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                }
            }
        }

        public ICommand OpenLinkDirectoryCommand => new RelayCommand(() =>
        {
            LinkService.OpenContainingDirectory(SelectedLink);
        });
    }
}