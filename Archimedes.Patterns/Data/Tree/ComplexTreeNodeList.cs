// This collection of non-binary tree data structures created by Dan Vanderboom.
// Extendend by P.Buettiker 2010/2011 aka IsNull
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System;
using System.Text;
using System.Collections.Generic;

namespace Archimedes.Patterns.Data.Tree
{
    /// <summary>
    /// Contains a list of ComplexTreeNode (or ComplexTreeNode-derived) objects, with the capability of linking parents and children in both directions.
    /// </summary>
    public class ComplexTreeNodeList<T> : List<ComplexTreeNode<T>> where T : ComplexTreeNode<T>
    {
        public T Parent;

        public ComplexTreeNodeList(ComplexTreeNode<T> Parent)
        {
            this.Parent = (T)Parent;
        }

        public T Add(T Node)
        {
            if (Node == null)
                throw new ArgumentNullException();

            base.Add(Node);
            Node.Parent = Parent;
            return Node;
        }

        public override string ToString()
        {
            return "Count=" + Count.ToString();
        }
    }
}