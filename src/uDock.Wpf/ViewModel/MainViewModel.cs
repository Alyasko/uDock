using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using uDock.Wpf.Model;

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
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public ObservableCollection<LinkItem> LinkItems { get; set; } = new ObservableCollection<LinkItem>();

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

        private LinkItem CreateNewLinkItem(LinkItem parent)
        {
            return new LinkItem(parent)
            {
                Title = $"Link {LinkItem.TotalLinksCount}"
            };
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

        public ICommand RemoveCategoryCommand => new RelayCommand(() =>
        {
            if (SelectedLink?.Parent != null)
                SelectedLink.Parent.Children.Remove(SelectedLink);
            else
                LinkItems.Remove(SelectedLink);
        });

        public void ExecuteLink()
        {
            if (SelectedLink == null)
                return;

            if(SelectedLink.Children.Count == 0 && string.IsNullOrWhiteSpace(SelectedLink.Uri))
                return;

            try
            {
                if (SelectedLink.Children.Count > 0)
                {
                    foreach (var item in SelectedLink.Children)
                    {
                        Process.Start(item.Uri);
                    }
                }
                else
                {
                    Process.Start(SelectedLink.Uri);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                MessageBox.Show("Unable to start link", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        li.Children.Add(LinkItem.FromFileName(fileName, li));
                    }
                }
                else
                {
                    foreach (var fileName in fileNames)
                    {
                        LinkItems.Add(LinkItem.FromFileName(fileName));
                    }
                }
            }
        }
    }
}