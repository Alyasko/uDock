using System.Collections.ObjectModel;

namespace uDock.Wpf.Model
{
    public class LinkItem
    {
        public static int TotalLinksCount = 0;

        public LinkItem()
        {
            TotalLinksCount++;
        }

        public string Title { get; set; }

        public int Kind { get; set; }

        public string Uri { get; set; }

        public ObservableCollection<LinkItem> Children { get; } = new ObservableCollection<LinkItem>();

        public LinkItem Parent { get; set; }
    }
}
