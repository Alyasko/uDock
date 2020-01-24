using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LiteDB;
using Microsoft.Win32;
using uDock.Core;
using uDock.Core.Model;

namespace uDock.Wpf.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ApplicationState _app;

        public SettingsViewModel(ApplicationState app)
        {
            _app = app;
        }

        public ObservableCollection<LinkItem> LinkItems => _app.LinkItems;
        private readonly Dictionary<ObjectId, LinkItem> _linksDict = new Dictionary<ObjectId, LinkItem>();

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

        private LinkItem CreateNewLinkItem(LinkItem parent)
        {
            var link = new LinkItem(parent?.Id)
            {
                Title = $"Link {LinkItem.TotalLinksCount}"
            };
            _linksDict.Add(link.Id, link);
            return link;
        }

        public ICommand AddCategoryCommand => new RelayCommand(() =>
        {
            LinkItems.Add(CreateNewLinkItem(null));
        });

        public ICommand AddSubCategoryCommand => new RelayCommand(() =>
        {
            if (SelectedLink == null)
                return;

            SelectedLink.Children.Add(CreateNewLinkItem(SelectedLink));
        });

        public ICommand CloseCommand => new RelayCommand(() =>
        {
            App.Current.Shutdown();
        });

        public ICommand LoadProjectCommand => new RelayCommand(() =>
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "uDock project files | *.udock";
            if (ofd.ShowDialog().Value)
            {
                var file = ofd.FileName;
                _app.Project = Project.Load(file);
                foreach (var item in _app.Project.Items)
                {
                    LinkItems.Add(item);
                }
            }
        });

        public ICommand SaveProjectCommand => new RelayCommand(() =>
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "uDock project files | *.udock";
            if (sfd.ShowDialog().Value)
            {
                var file = sfd.FileName;
                var _project = Project.Save(LinkItems.ToList(), file);
            }
        });

        public ICommand RemoveCategoryCommand => new RelayCommand(() =>
        {
            if (SelectedLink?.ParentId != null)
            {
                if (!_linksDict.TryGetValue(SelectedLink?.ParentId, out var linkId))
                    return;
                linkId.Children.Remove(SelectedLink);
            }
            else
                LinkItems.Remove(SelectedLink);
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

        public void HandleDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (e.OriginalSource is TextBlock tb && tb.DataContext is LinkItem li)
                {
                    foreach (var fileName in fileNames)
                    {
                        var link = LinkItem.FromFileName(fileName, li);
                        _linksDict.Add(link.Id, link);
                        li.Children.Add(link);
                    }
                }
                else
                {
                    foreach (var fileName in fileNames)
                    {
                        var link = LinkItem.FromFileName(fileName);
                        _linksDict.Add(link.Id, link);
                        LinkItems.Add(link);
                    }
                }
            }
        }

    }
}
