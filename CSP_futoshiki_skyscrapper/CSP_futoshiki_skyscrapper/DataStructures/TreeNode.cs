using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class TreeNode<T>
    {
        public T data;
        public List<TreeNode<T>> children;

        public TreeNode(T data)
        {
            this.data = data;
            children = new List<TreeNode<T>>();
        }

        public void AddChild(T child)
        {
            children.Add(new TreeNode<T>(child));
        }

    }
}
