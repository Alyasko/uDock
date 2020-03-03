using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using uDock.Core.Model;

namespace uDock.Core
{
    public class LinkService
    {
        public IEnumerable<LinkItem> GetChildLinks(LinkItem linkItem)
        {
            if (linkItem == null)
                return null;

            var uris = new List<LinkItem>();
            GetChildrenRecursive(linkItem, uris);

            return uris;
        }

        public void OpenContainingDirectory(LinkItem linkItem)
        {
            if (linkItem == null)
                return;

            var link = linkItem.Uri;
            var dir = Path.GetDirectoryName(link);
            if (string.IsNullOrWhiteSpace(dir))
                return;

            Process.Start(dir);
        }


        private void GetChildrenRecursive(LinkItem rootItem, List<LinkItem> uris)
        {
            if (rootItem.Children.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(rootItem.Uri))
                    uris.Add(rootItem);
            }
            else
            {
                foreach (var item in rootItem.Children)
                {
                    GetChildrenRecursive(item, uris);
                }
            }
        }
    }
}
