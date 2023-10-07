namespace _02.DOM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design.Serialization;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using _02.DOM.Interfaces;
    using _02.DOM.Models;

    public class DocumentObjectModel : IDocument
    {
        private IHtmlElement child;
        public DocumentObjectModel()
        {
            this.Root = new HtmlElement(ElementType.Document);
            this.Root.Children.Add(new HtmlElement(ElementType.Html));
            this.Root.Children[0].Parent = this.Root;
            //foreach (var item in this.Root.Children[0].Children)
            //{
            //    item.Parent = this.Root.Children[0];
            //}
            this.Root.Children[0].Children.Add(new HtmlElement(ElementType.Head));
            this.Root.Children[0].Children.Add(new HtmlElement(ElementType.Body));


        }
        public DocumentObjectModel(IHtmlElement root)
        {
            this.Root = root;
        }

        

        public IHtmlElement Root { get; private set; }

        public IHtmlElement GetElementByType(ElementType type)
        {
           IHtmlElement element = this.GetElementByType(this.Root,type);
            return element;
        }

        private IHtmlElement GetElementByType(IHtmlElement root, ElementType type)
        {
           Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode.Type == type)
                {
                    return currentNode;
                }
                foreach (var item in currentNode.Children)
                {
                    queue.Enqueue(item);
                }
            }
            return null;
        }

        public List<IHtmlElement> GetElementsByType(ElementType type)
        {
            List<IHtmlElement> elements = new List<IHtmlElement>();
            this.GetElementsByTypeDfs(this.Root, elements, type);
            return elements;
        }

        private void GetElementsByTypeDfs(IHtmlElement node, List<IHtmlElement> elements, ElementType type)
        {
            if(node == null)
            {
                return;
            }
            if(node.Type == type)
            {
                elements.Add(node);
            }
            foreach (var child in node.Children)
            {
                this.GetElementsByTypeDfs(child, elements, type);
            }

            

        }

        public bool Contains(IHtmlElement htmlElement)
        {
            var node = this.Root;
            bool result = this.ContainsWithBfs(node, htmlElement);
            return result;
        }

        private bool ContainsWithBfs(IHtmlElement node, IHtmlElement htmlElement)
        {
            Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode == htmlElement)
                {
                    return true;
                }
                foreach (var child in currentNode.Children)
                {
                    queue.Enqueue(child);
                }
            }
            return false;
        }

        public void InsertFirst(IHtmlElement parent, IHtmlElement child)
        {
           var node = this.Root;
            this.Root = this.InsertFirstWithBfs(node, parent, child);
        }

        private IHtmlElement InsertFirstWithBfs(IHtmlElement node, IHtmlElement parent, IHtmlElement child)
        {
            Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            bool found = false;
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode == parent)
                {
                    found = true;
                    child.Parent = currentNode;
                    currentNode.Children.Insert(0, child);
                    return this.Root;
                }
                foreach (var item in currentNode.Children)
                {
                    queue.Enqueue(item);
                }
            }
            if (!found)
            {
                throw new InvalidOperationException();
            }
            return this.Root;
        }

        public void InsertLast(IHtmlElement parent, IHtmlElement child)
        {
            var node = this.Root;
            this.Root = this.InsertLastWithBfs(node, parent, child);
        }

        private IHtmlElement InsertLastWithBfs(IHtmlElement node, IHtmlElement parent, IHtmlElement child)
        {
            Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            bool found = false;
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode == parent)
                {
                    found = true;
                    child.Parent = currentNode;
                    currentNode.Children.Add(child);
                    return this.Root;
                }
                foreach (var item in currentNode.Children)
                {
                    queue.Enqueue(item);
                }
            }
            if (!found)
            {
                throw new InvalidOperationException();
            }
            return this.Root;
        }

        public void Remove(IHtmlElement htmlElement)
        {
            
           this.Root = this.RemoveWithBfs(this.Root, htmlElement);
        }

        private IHtmlElement RemoveWithBfs(IHtmlElement root, IHtmlElement htmlElement)
        {
            Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            queue.Enqueue(root);
            bool found = false;
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode == htmlElement)
                {
                    found = true;
                    currentNode.Parent.Children.Remove(currentNode);
                    return this.Root;
                }
                foreach (var item in currentNode.Children)
                {
                    queue.Enqueue(item);
                }
            }
            if (!found)
            {
                throw new InvalidOperationException();
            }
            return this.Root;
        }

        public void RemoveAll(ElementType elementType)
        {
            this.Root = this.RemoveAllWithBfs(this.Root, elementType);
        }

        private IHtmlElement RemoveAllWithBfs(IHtmlElement root, ElementType elementType)
        {
            Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode.Type == elementType)
                {
                    if(currentNode.Children.Count > 0)
                    {
                        currentNode.Children.Clear();
                    }
                    currentNode.Parent.Children.Remove(currentNode);
                }
                foreach (var item in currentNode.Children)
                {
                    queue.Enqueue(item);
                }
            }
            return this.Root;
        }

        public bool AddAttribute(string attrKey, string attrValue, IHtmlElement htmlElement)
        {
            IHtmlElement element = this.Find(this.Root, htmlElement);
            if (element == null)
            {
                throw new InvalidOperationException();
            }
            if (element.Attributes.ContainsKey(attrKey))
            {
                return false;
            }
            else
            {
                element.Attributes.Add(attrKey, attrValue);
                return true;
            }
        }

        public bool RemoveAttribute(string attrKey, IHtmlElement htmlElement)
        {
            IHtmlElement element = this.Find(this.Root,htmlElement);
            if (element == null)
            {
                throw new InvalidOperationException();
            }
            if (element.Attributes.ContainsKey(attrKey))
            {
                element.Attributes.Remove(attrKey);
                return true;
            }
            return false;
        }

        private IHtmlElement Find(IHtmlElement node,IHtmlElement htmlElement)
        {
            Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode == htmlElement)
                {
                    return currentNode;
                }
                foreach (var item in currentNode.Children)
                {
                    queue.Enqueue(item);
                }
            }
            return null;

        }

        public IHtmlElement GetElementById(string idValue)
        {
            IHtmlElement element = this.FindById(this.Root, idValue);
            return element;
        }

        private IHtmlElement FindById(IHtmlElement root, string idValue)
        {
            Queue<IHtmlElement> queue = new Queue<IHtmlElement>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode.Attributes.ContainsKey("id"))
                {
                    if (currentNode.Attributes["id"] == idValue)
                    {
                        return currentNode;
                    }
                }
                foreach (var item in currentNode.Children)
                {
                    queue.Enqueue(item);
                }
            }
            return null;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            ToStringHelper(sb, Root, 0);
            return sb.ToString().TrimEnd();
        }

        private void ToStringHelper(StringBuilder sb, IHtmlElement node, int indent)
        {
            sb.AppendLine($"{new string(' ', indent)}{node.GetType().Name}");
            indent += 2;
            foreach (var child in node.Children)
            {
                ToStringHelper(sb, child, indent);
            }
        }

    }
}
