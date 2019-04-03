﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class TreeNode<T>
    {
        public TreeNode<T> parent;
        public T data;
        public List<TreeNode<T>> children;
        public bool isSolved = false;

        public TreeNode(TreeNode<T> parent, T data)
        {
            this.parent = parent;
            this.data = data;
            children = new List<TreeNode<T>>();
        }

        public void AddChild(T child)
        {
            children.Add(new TreeNode<T>(this, child));
        }

        public void AddChild(TreeNode<T> treeNode)
        {
            children.Add(treeNode);
        }

        public bool AreAllChildrenSolved()
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (!children[i].isSolved)
                    return false;
            }
            return true;
        }

    }
}
