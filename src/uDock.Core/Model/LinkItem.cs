using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiteDB;

namespace uDock.Core.Model
{
    public class LinkItem : INotifyPropertyChanged
    {
        public static int TotalLinksCount = 0;
        private string _title = string.Empty;
        private string _uri;

        public LinkItem(ObjectId parentId)
        {
            TotalLinksCount++;
            ParentId = parentId;
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

        public string Uri
        {
            get => _uri;
            set
            {
                _uri = value;
                OnPropertyChanged(nameof(Display));
            }
        }

        public ObservableCollection<LinkItem> Children { get; } = new ObservableCollection<LinkItem>();

        public ObjectId ParentId { get; set; }

        public ObjectId Id { get; set; } = ObjectId.NewObjectId();

        public int Order { get; set; } = 0;

        public string Display => $"{Title}" + (string.IsNullOrWhiteSpace(Uri) ? "": $" [{Uri}]");

        public static LinkItem FromFileName(string fullFileName, LinkItem parent = null)
        {
            return new LinkItem(parent?.Id)
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
