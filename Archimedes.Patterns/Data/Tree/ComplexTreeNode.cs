
// This collection of non-binary tree data structures created by Dan Vanderboom.
// Extendend by P.Buettiker 2010/2011 aka IsNull
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/


namespace Archimedes.Patterns.Data.Tree
{
    using System;
    using System.Text;


    /// <summary>
    /// Represents a node in a Tree structure, with a parent node and zero or more child nodes.
    /// (This can also be the root of this tree, that doesn't matter)
    /// </summary>
    public abstract class ComplexTreeNode<T> : IDisposable, ICloneable, IEquatable<T> where T : ComplexTreeNode<T>
    {
        #region Fields

        private T _parent;
        private ComplexTreeNodeList<T> _children;
        private TreeTraversalDirection _DisposeTraversal = TreeTraversalDirection.BottomUp;

        #endregion

        #region Events

        public event EventHandler Disposing;

        #endregion

        #region Constructors

        public ComplexTreeNode() {
            Parent = null;
            Children = new ComplexTreeNodeList<T>(this);
        }

        public ComplexTreeNode(T Parent) {
            this.Parent = Parent;
            Children = new ComplexTreeNodeList<T>(this);
        }

        public ComplexTreeNode(ComplexTreeNodeList<T> Children) {
            Parent = null;
            this.Children = Children;
            Children.Parent = (T)this;
        }

        public ComplexTreeNode(T Parent, ComplexTreeNodeList<T> Children) {
            this.Parent = Parent;
            this.Children = Children;
            Children.Parent = (T)this;
        }

        #endregion

        #region Propertys

        public T Parent {
            get { return _parent; }
            set {
                if (value == _parent) {
                    return;
                }

                if (_parent != null) {
                    _parent.Children.Remove(this);
                }

                if (value != null && !value.Children.Contains(this)) {
                    value.Children.Add(this);
                }

                _parent = value;
            }
        }

        public T Root {
            get {
                ComplexTreeNode<T> node = this;
                while (node.Parent != null) {
                    node = node.Parent;
                }
                return node as T;
            }
        }

        public virtual ComplexTreeNodeList<T> Children {
            get { return _children; }
            private set { _children = value; }
        }

        
        /// <summary>
        /// Specifies the pattern for traversing the Tree for disposing of resources. Default is BottomUp.
        /// </summary>
        public TreeTraversalDirection DisposeTraversal {
            get { return _DisposeTraversal; }
            set { _DisposeTraversal = value; }
        }


        /// <summary>
        /// Reports a depth of nesting in the tree, starting at 0 for the root.
        /// </summary>
        public int Depth {
            get {
                int depth = 0;
                ComplexTreeNode<T> node = this;
                while (node.Parent != null) {
                    node = node.Parent;
                    depth++;
                }
                return depth;
            }
        }
        #endregion

        // Extend Tree with Merge and Walking

        #region Merge Nodes

        public void MergeNodeIntoChildren(T source) {
            MergeNodeIntoChildren(source, (this as T));
        }
        /// <summary>Merges the source node into the destnode childs
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public void MergeNodeIntoChildren(T source, T dest) {
            int childindex = 0;
            if ((childindex = IsNodeInChildern(source, dest)) != -1) {
                MergeNodes(source, (dest.Children[childindex] as T));
            } else {
                dest.Children.Add(source);
            }
        }


        public void MergeNodes(T source) {
            MergeNodeIntoChildren(source, (this as T));
        }
        /// <summary>Merges every sourcechild from source node into the dest node childs.
        /// 
        /// </summary>
        /// <param name="Node2Merge"></param>
        /// <param name="DestinationNode"></param>
        public void MergeNodes(T source, T dest) {
            int childindex = 0;
            foreach (T sourcechild in source.Children) {
                if ((childindex = IsNodeInChildern(sourcechild, dest)) != -1) {
                    MergeNodes(sourcechild, (dest.Children[childindex] as T));
                } else {
                    dest.Children.Add((sourcechild.Clone() as T));
                }
            }

        }
        #endregion

        #region Walker

        /// <summary>Checks if the node exists in this childern
        /// 
        /// </summary>
        /// <param name="scannode"></param>
        /// <returns></returns>
        public int IsNodeInChildern(T node) {
            return IsNodeInChildern(node, (this as T));
        }
        /// <summary>Checks if the node exists in the scannodes childern
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="scannode"></param>
        /// <returns>Returns the Postition if the node was found, otherwise -1</returns>
        private int IsNodeInChildern(T node, T scannode) {
            int i = 0;
            foreach (T scannodechild in scannode.Children) {
                if (node.Equals(scannodechild)) {
                    return i;
                }
                ++i;
            }
            return -1;
        }


        /// <summary>Walks in this node along the given Path (SimpleNode) and returns the node which the path points to.
        /// 
        /// </summary>
        /// <param name="SimpleNode"></param>
        /// <returns></returns>
        public T WalkLast(T SimpleNode) {
            return WalkLast((this as T), SimpleNode);
        }

        /// <summary>Walks in MasterNode along the SimpleNode Path and returns the subnode from MasterNode where SimpleNode (the path) points to. Got it? :)
        /// 
        /// </summary>
        /// <param name="MasterNode">Complex Tree to search/walk in</param>
        /// <param name="SimpleNode">Flat Tree which is used as Path</param>
        /// <returns>node from Masternode</returns>
        public T WalkLast(T MasterNode, T SimpleNode) {
            int index;

            if (SimpleNode.Children.Count == 0) {
                return MasterNode;
            }

            if ((index = MasterNode.IsNodeInChildern((SimpleNode.Children[0] as T))) != -1) {
                if (SimpleNode.Children[0].Children.Count != 0) {
                    return WalkLast((MasterNode.Children[index] as T), (SimpleNode.Children[0] as T));
                } else {
                    return (MasterNode.Children[index] as T);
                }
            } else {
                return null;
            }
        }

        #endregion

        #region Flat Tree

        /// <summary>Returns the deepest (Last) Element of a flat Tree
        /// 
        /// </summary>
        /// <returns></returns>
        public T WalkLastFlat() {
            if (this.Children.Count == 0)
                return (this as T);
            else
                return this.Children[0].WalkLastFlat();
        }


        #endregion

        public abstract bool Equals(T other);
        public abstract object Clone();

        #region IDisposable

        private bool _IsDisposed;
        public bool IsDisposed {
            get { return _IsDisposed; }
        }

        public virtual void Dispose() {
            CheckDisposed();

            // clean up contained objects (in Value property)
            if (DisposeTraversal == TreeTraversalDirection.BottomUp) {
                foreach (ComplexTreeNode<T> node in Children) {
                    node.Dispose();
                }
            }

            OnDisposing();

            if (DisposeTraversal == TreeTraversalDirection.TopDown) {
                foreach (ComplexTreeNode<T> node in Children) {
                    node.Dispose();
                }
            }

            // TODO: clean up the tree itself

            _IsDisposed = true;
        }

        

        protected void OnDisposing() {
            if (Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }
        }

        protected void CheckDisposed() {
            if (IsDisposed) {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion

        public override string ToString() {
            string Description = "Depth=" + Depth.ToString() + ", Children=" + Children.Count.ToString();
            if (this == Root) {
                Description += " (Root)";
            }
            return Description;
        }

        public override bool Equals(object obj) {
            return Equals(obj as T);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}