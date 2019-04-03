using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Linq;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class Tree<T>
    {
        public TreeNode<T> root { get; }

        public Tree(T rootData)
        {
            root = new TreeNode<T>(null, rootData);
        }

        #region PRINTING
        public int HeightOfTree(TreeNode<T> root)
        {
            if (root == null)
                return 0;

            List<int> heights = new List<int>();

            for (int i = 0; i < root.children.Count; i++)
            {
                heights.Add(HeightOfTree(root.children[i]));
            }
            if(heights.Count == 0)
                return 1;
            else
                return heights.Max(i=>i) + 1;
        }

        public void PrintLevelOrder()
        {
            int h = HeightOfTree(root);
            for (int i = 1; i <= h; i++)
            {
                PrintGivenLevel(root, i);
                WriteLine();
            }
        }

        public void PrintGivenLevel(TreeNode<T> root, int level)
        {
            if (root == null)
                return;
            if (level == 1)
                Write(root.data.ToString()+" | "); 
            else if (level > 1)
            {
                for (int i = 0; i < root.children.Count; i++)
                {
                    PrintGivenLevel(root.children[i], level - 1);
                }
                
            }
        }
        #endregion
    }
}
