using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiteDB;

namespace uDock.Wpf.Model
{
    public class LinkItem : INotifyPropertyChanged
    {
        public static int TotalLinksCount = 0;
        private string _title;

        public LinkItem(LinkItem parent)
        {
            TotalLinksCount++;
            Parent = parent;
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Display));
            }
        }

        public int Kind { get; set; }

        public string Uri { get; set; }

        public ObservableCollection<LinkItem> Children { get; } = new ObservableCollection<LinkItem>();

        public LinkItem Parent { get; set; }

        public ObjectId Id { get; set; } = ObjectId.NewObjectId();

        public string Display => $"{Title}" + (string.IsNullOrWhiteSpace(Uri) ? "": $" [{Uri}]");

        public static LinkItem FromFileName(string fullFileName, LinkItem parent = null)
        {
            return new LinkItem(parent)
            {
                Title = System.IO.Path.GetFileName(fullFileName),
                Uri = fullFileName
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
