// This collection of non-binary tree data structures created by Dan Vanderboom.
// Extendend by P.Buettiker 2010/2011 aka IsNull
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System;
using System.Text;

namespace Archimedes.Patterns.Data.Tree
{
    /// <summary>
    /// Represents a hierarchy of objects or data.  ComplexTree is a root-level alias for ComplexSubtree and ComplexTreeNode.
    /// </summary>
    public abstract class ComplexTree<T> : ComplexTreeNode<T> where T : ComplexTreeNode<T>
    {
        public ComplexTree() { }
    }
}