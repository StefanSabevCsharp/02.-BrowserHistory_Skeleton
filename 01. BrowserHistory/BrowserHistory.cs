namespace _01._BrowserHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using _01._BrowserHistory.Interfaces;

    public class BrowserHistory : IHistory
    {
        private List<ILink> links = new List<ILink>();
        private HashSet<ILink> hashLinks = new HashSet<ILink>();

        public int Size => links.Count;

        public void Clear()
        {
            links = new List<ILink>();
        }

        public bool Contains(ILink link)
        {
            return hashLinks.Contains(link);
        }

        public ILink DeleteFirst()
        {
            if (links.Count == 0)
            {
                throw new InvalidOperationException();
            }
            ILink link = links[0];
            links.RemoveAt(0);
            hashLinks.Remove(link);
            return link;
        }

        public ILink DeleteLast()
        {
            if (links.Count == 0)
            {
                throw new InvalidOperationException();
            }
            ILink link = links[links.Count - 1];
            links.RemoveAt(links.Count - 1);
            hashLinks.Remove(link);
            return link;
        }

        public ILink GetByUrl(string url)
        {
            ILink link = links.FirstOrDefault(x => x.Url == url);
            return link;
        }

        public ILink LastVisited()
        {
            if (links.Count == 0)
            {
                throw new InvalidOperationException();
            }
            return links[links.Count - 1];
        }

        public void Open(ILink link)
        {
            links.Add(link);
            hashLinks.Add(link);
        }

        public int RemoveLinks(string url)
        {
            ILink[] linksToRemove = links.Where(x => x.Url.ToLower().Contains(url.ToLower())).ToArray();
            if (linksToRemove.Length == 0)
            {
                throw new InvalidOperationException();
            }

            foreach (var link in linksToRemove)
            {
                links.Remove(link);
                hashLinks.Remove(link);
            }

            return linksToRemove.Length;

        }

        public ILink[] ToArray()
        {
            ILink[] linksArray = links.ToArray();
            linksArray = linksArray.Reverse().ToArray();
            return linksArray;
        }

        public List<ILink> ToList()
        {
            return links.Reverse<ILink>().ToList();
        }

        public string ViewHistory()
        {
            StringBuilder sb = new StringBuilder();
            List<ILink> history = links.Reverse<ILink>().ToList();
            foreach (var link in history)
            {
                sb.AppendLine(link.ToString());
            }
            if (sb.Length == 0)
            {
                return "Browser history is empty!";
            }
            else
            {

                return sb.ToString();
            }
        }
    }
}
