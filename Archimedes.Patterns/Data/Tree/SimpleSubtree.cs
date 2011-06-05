// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System;
using System.Text;

namespace Archimedes.Patterns.Data.Tree
{
    /// <summary>
    /// Represents a hierarchy of objects or data.  SimpleSubtree is a root-level alias for SimpleTree and SimpleTreeNode.
    /// </summary>
    public class SimpleSubtree<T> : SimpleTreeNode<T>
    {
        public SimpleSubtree() { }
    }
}