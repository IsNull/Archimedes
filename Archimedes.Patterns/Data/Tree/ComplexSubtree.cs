// This collection of non-binary tree data structures created by Dan Vanderboom.
// Extendend by P.Buettiker 2010/2011 aka IsNull
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/


namespace Archimedes.Patterns.Data.Tree
{
    using System;
    using System.Text;

    /// <summary>
    /// Represents a hierarchy of objects or data.  ComplexSubtree is an alias for ComplexTree and ComplexTreeNode.
    /// </summary>
    public abstract class ComplexSubtree<T> : ComplexTreeNode<T> where T : ComplexTreeNode<T>
    {
        public ComplexSubtree() { }
    }

}