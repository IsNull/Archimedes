/******************************************************
 * 
 * TreeViewItem Template Class (Generic)
 * 
 * Base Class for TreeViewModels
 * 
 * ****************************************************
 * */

namespace Archimedes.Patterns.WPF.ViewModels.Trees
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// TreeViewItem template class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeViewItemViewModel<T> : WorkspaceViewModel, ITreeViewItemViewModel<T>
        where T : class, ITreeViewItemViewModel<T>, new()
    {
        #region Fields

        T _parent;
        ObservableCollection<T> _children;
        bool _isSelected;
        bool _isExpanded;

        #endregion

        #region Constructors

        public TreeViewItemViewModel()
            : this(null, false) { }

        public TreeViewItemViewModel(T parent)
            : this(parent, false) { }

        public TreeViewItemViewModel(T parent, bool lazyLoadChildren) {
            _parent = parent;
            _children = new ObservableCollection<T>();

            if(lazyLoadChildren)
                _children.Add(DummyChild);
        }

        #endregion

        #region Properties

        public ObservableCollection<T> Children {
            get { return _children; }
        }


        public bool IsSelected {
            get { return _isSelected; }
            set {
                if(value != _isSelected) {
                    _isSelected = value;
                    OnPropertyChanged(() => IsSelected);
                }
            }
        }

        /// <summary>
        /// Returns the parent of this node or null, if this is the root
        /// </summary>
        public T Parent {
            get { return _parent; }
        }

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }


        public bool IsExpanded {
            get { return _isExpanded; }
            set {
                if(value != _isExpanded) {
                    _isExpanded = value;
                    OnPropertyChanged(() => IsExpanded);
                }

                // expand all higher nodes
                if(_isExpanded && _parent != null)
                    _parent.IsExpanded = true;

                // Lazy load all childs
                if(this.HasDummyChild) {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren() { }

        #endregion

        #region Dummychild

        static T _dummychild = new T() { };
        public static T DummyChild {
            get {
                return _dummychild;
            }
        }

        #endregion
    }
}
