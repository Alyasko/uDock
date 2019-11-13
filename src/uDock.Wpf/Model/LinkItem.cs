using System.Collections.ObjectModel;
using LiteDB;

namespace uDock.Wpf.Model
{
    public class LinkItem
    {
        public static int TotalLinksCount = 0;

        public LinkItem(LinkItem parent)
        {
            TotalLinksCount++;
            Parent = parent;
        }

        public string Title { get; set; }

        public int Kind { get; set; }

        public string Uri { get; set; }

        public ObservableCollection<LinkItem> Children { get; } = new ObservableCollection<LinkItem>();

        public LinkItem Parent { get; set; }

        public ObjectId Id { get; set; } = ObjectId.NewObjectId();

        public static LinkItem FromFileName(string fullFileName, LinkItem parent = null)
        {
            return new LinkItem(parent)
            {
                Title = System.IO.Path.GetFileName(fullFileName),
                Uri = fullFileName
            };
        }
    }
}
