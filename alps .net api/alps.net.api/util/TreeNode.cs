using System;
using System.Collections.Generic;

namespace alps.net.api
{
    /// <summary>
    /// Class that represents a tree node
    /// </summary>
    public class TreeNode<T> : ITreeNode<T>
    {
        private ITreeNode<T> parentNode;
        private List<ITreeNode<T>> childNodes = new List<ITreeNode<T>>();
        private T content;

        /// <summary>
        /// Constructor that creates a empty tree node
        /// </summary>
        public TreeNode()
        {

        }

        /// <summary>
        /// Constructor creating a TreeNode with content
        /// </summary>
        /// <param name="content">A string that is contained in the tree node</param>
        public TreeNode(T content)
        {
            setContent(content);
        }

        /// <summary>
        /// Constructor creating a TreeNode with content, a parent and a child
        /// </summary>
        /// <param name="content">A string that is contained in the tree node</param>
        /// <param name="parent">A node that is parent of this node</param>
        /// <param name="child">A node that is a child of this node</param>
        public TreeNode(T content, ITreeNode<T> parent, List<ITreeNode<T>> childNodes)
        {
            setContent(content);
            setParentNode(parent);
            setChildNodes(childNodes);
        }

        /// <summary>
        /// Constructor creating a TreeNode with content, a parent and a child
        /// </summary>
        /// <param name="childNodes">the child nodes</param>
        public TreeNode(List<ITreeNode<T>> childNodes)
        {
            setChildNodes(childNodes);
        }


        public void setParentNode(ITreeNode<T> parent)
        {
            parentNode = parent;
        }


        public void setChildNodes(List<ITreeNode<T>> childNodes)
        {
            // The  list of children should never be null
            if (childNodes is null)
            {
                this.childNodes = new List<ITreeNode<T>>();
            }
            else
            {
                this.childNodes = childNodes;
            }
        }


        public void addChild(ITreeNode<T> child)
        {
            if (!(child is null))
            {
                childNodes.Add(child);
                child.setParentNode(this);
            }
        }


        public ITreeNode<T> getParentNode()
        {
            return parentNode;
        }


        public List<ITreeNode<T>> getChildNodes()
        {
            return childNodes;
        }


        public T getContent()
        {
            return content;
        }


        public override string ToString()
        {
            string result = "";

            if (childNodes != null)
            {

                foreach (ITreeNode<T> i in childNodes)
                {
                    result += i.getContent();

                    Console.WriteLine(i.getContent());
                    i.ToString();

                }
            }

            return result;
        }


        public bool containsContent(T content)
        {
            bool test = false;

            foreach (ITreeNode<T> t in childNodes)
            {
                if (t.Equals(content))
                {
                    test = true;
                    //TreeNode newChild = new TreeNode(compare);
                    //childNodes.Add(newChild);
                    break;
                }
                else
                {
                    test = t.containsContent(content);
                }
            }

            return test;
        }

        public void setContent(T content)
        {
            this.content = content;
        }

        public bool isSubClassOf(ITreeNode<T> parent, bool direct = false)
        {
            if (parentNode is null) return false;
            if (parentNode.Equals(parent)) return true;
            if (!direct) return parentNode.isSubClassOf(parent);
            return false;
        }
    }
}
