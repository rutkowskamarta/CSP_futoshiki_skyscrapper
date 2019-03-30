using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class Tree<T>
    {
        private TreeNode<T> root; //początkowo wypełniona plansza

        public Tree(T rootData)
        {
            root = new TreeNode<T>(rootData);
        }

    }
}
