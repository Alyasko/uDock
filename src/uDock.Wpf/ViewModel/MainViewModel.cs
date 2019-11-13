using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
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
            return new LinkItem()
            {
                Parent = parent,
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
    }
}