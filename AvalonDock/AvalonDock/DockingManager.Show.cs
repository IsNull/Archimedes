using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace AvalonDock
{
    public partial class DockingManager
    {
        #region Show Methods

        /// <summary>
        /// Show a dockable content in its container <see cref="Pane"/>
        /// </summary>
        /// <param name="content">Content to show</param>
        internal void Show(DockableContent content) {
            //if desideredState is not defined, use the saved state if exists
            if (content.SavedStateAndPosition != null)
                Show(content, content.SavedStateAndPosition.State);
            else
                Show(content, DockableContentState.Docked);
        }

        /// <summary>
        /// Show a dockable content in its container with a desidered state
        /// </summary>
        /// <param name="content">Content to show</param>
        /// <param name="desideredState">State desidered</param>
        internal void Show(DockableContent content, DockableContentState desideredState) {
            Show(content, desideredState, AnchorStyle.None);
        }

        /// <summary>
        /// Show a dockable content in its container with a desidered state
        /// </summary>
        /// <param name="content">Content to show</param>
        /// <param name="desideredState">State desidered</param>
        /// <param name="desideredAnchor">Border to which anchor the newly created container pane</param>
        /// <remarks></remarks>
        internal void Show(DockableContent content, DockableContentState desideredState, AnchorStyle desideredAnchor) {
            Debug.WriteLine(string.Format("Show Content={0}, desideredState={1}, desideredAnchor={2}", content.Name, desideredState, desideredAnchor));

            if (desideredState == DockableContentState.Hidden)//??!!show hidden?
                Hide(content);

            if (content.State == DockableContentState.AutoHide) {

                (content.ContainerPane as DockablePane).ToggleAutoHide(); // first redock the content
                Show(content, desideredState, desideredAnchor); // then show it as desidered

            } else if (content.State == DockableContentState.Docked || content.State == DockableContentState.Document || content.State == DockableContentState.None) {

                ShowDockedDocumentNone(content, desideredState, desideredAnchor);

            } else if (content.State == DockableContentState.Hidden || content.State == DockableContentState.DockableWindow || content.State == DockableContentState.FloatingWindow) {

                ShowHiddenDockableFloating(content, desideredState, desideredAnchor);
            }
        }

        /// <summary>
        /// Show the Content as standard Window with no docking abilities.
        /// </summary>
        /// <param name="content"></param>
        internal void ShowUnDockableFloatingWindow(DockableContent content) {
            var floatingWindow = SetupUnDockableFloatingWindow(content);
            floatingWindow.Show();
        }

        /// <summary>
        /// Show the DockableContent as Dialoge
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal bool? ShowDialoge(DockableContent content) {
            var floatingWindow = SetupUnDockableFloatingWindow(content);
            return floatingWindow.ShowDialog();
        }

        UnDockableFloatingWindow SetupUnDockableFloatingWindow(DockableContent content) {
            RescueOnNone(content, AnchorStyle.None);
            var floatingWindow = new UnDockableFloatingWindow(this);
            var mainWindow = Window.GetWindow(this);
            if (mainWindow.IsVisible)
                floatingWindow.Owner = mainWindow;

            floatingWindow.IsDockableWindow = false;
            floatingWindow.Content = content;
            SetFloationgWindowProperties(content, floatingWindow);
            return floatingWindow;
        }


        #endregion

        #region DockableContentState:  Docked - Document - None

        void ShowDockedDocumentNone(DockableContent content, DockableContentState desideredState, AnchorStyle desideredAnchor) {

            RescueOnNone(content, desideredAnchor);

            if (content.ContainerPane.GetManager() == null) {
                //disconnect the parent pane from previous panel
                //((Panel)content.ContainerPane.Parent).Children.Remove(content.ContainerPane);
                if (content.ContainerPane.Parent != null) {
                    ((Panel)content.ContainerPane.Parent).Children.Remove(content.ContainerPane);
                }

                Anchor(content.ContainerPane as DockablePane, desideredAnchor);
            }

            if (desideredState == DockableContentState.DockableWindow || desideredState == DockableContentState.FloatingWindow) {

                #region desideredState: DockableWindow + FloatingWindow

                var floatingWindow = new DockableFloatingWindow(this);
                floatingWindow.Content = content;

                var mainWindow = Window.GetWindow(this);
                if (mainWindow.IsVisible)
                    floatingWindow.Owner = mainWindow;

                SetFloationgWindowProperties(content, floatingWindow);
                floatingWindow.Show();

                #endregion

            } else if (desideredState == DockableContentState.AutoHide) {

                #region desideredState: AutoHide

                var paneContainer = content.ContainerPane as DockablePane;
                Debug.Assert(paneContainer != null);

                if (paneContainer != null)
                    paneContainer.ToggleAutoHide();

                content.Activate();
                #endregion

            } else if (desideredState == DockableContentState.Document) {

                #region desideredState: Document

                DocumentPane docPane = MainDocumentPane;
                if (docPane != null) {
                    docPane.Items.Add(content.DetachFromContainerPane());
                    docPane.SelectedItem = content;
                    content.SetStateToDocument();
                }

                #endregion

            } else {

                #region desideredState: Other

                content.ContainerPane.SelectedItem = content;
                content.Activate();

                DockablePane dockParent = content.ContainerPane as DockablePane;
                if (content.ActualWidth == 0.0 && (
                    dockParent.Anchor == AnchorStyle.Left || dockParent.Anchor == AnchorStyle.Right)) {
                    ResizingPanel.SetResizeWidth(dockParent, new GridLength(200));
                    ResizingPanel.SetEffectiveSize(dockParent, new Size(200, 0.0));
                } else if (content.ActualWidth == 0.0 && (
                      dockParent.Anchor == AnchorStyle.Top || dockParent.Anchor == AnchorStyle.Bottom)) {
                    ResizingPanel.SetResizeHeight(dockParent, new GridLength(200));
                    ResizingPanel.SetEffectiveSize(dockParent, new Size(200, 0.0));
                }

                #endregion

            }
        }

        #endregion

        #region DockableContentState: Hidden - DockableWindow - FloatingWindow

        void ShowHiddenDockableFloating(DockableContent content, DockableContentState desideredState, AnchorStyle desideredAnchor) {


            if (content.State != DockableContentState.Hidden) {
                FloatingWindow floatingWindow = null;
                floatingWindow = (content.ContainerPane as FloatingDockablePane).FloatingWindow;
                content.DetachFromContainerPane();

                if (floatingWindow.HostedPane.Items.Count == 0)
                    floatingWindow.Close();
            }


            if (desideredState == DockableContentState.Docked || desideredState == DockableContentState.AutoHide) {

                #region Docked + AutoHide

                if (content.SavedStateAndPosition != null &&
                    content.SavedStateAndPosition.ContainerPane != null &&
                    content.SavedStateAndPosition.ChildIndex >= 0 &&
                    content.SavedStateAndPosition.ContainerPane.GetManager() == this &&
                    desideredState == DockableContentState.Docked) {
                    //ok previous container pane is here..
                    Pane prevPane = content.SavedStateAndPosition.ContainerPane;

                    if (content.SavedStateAndPosition.ChildIndex < prevPane.Items.Count) {
                        prevPane.Items.Insert(content.SavedStateAndPosition.ChildIndex, content);
                    } else {
                        prevPane.Items.Add(content);
                    }

                    if (prevPane.Items.Count == 1) {
                        if (!double.IsNaN(content.SavedStateAndPosition.Width) ||
                            !double.IsInfinity(content.SavedStateAndPosition.Width)) {
                            ResizingPanel.SetResizeWidth(content,
                                new GridLength(content.SavedStateAndPosition.Width));
                        }
                    }

                    DockablePane prevDockablePane = prevPane as DockablePane;
                    if (prevDockablePane != null && prevDockablePane.IsAutoHidden) {
                        prevDockablePane.ToggleAutoHide();
                    }

                    content.SetStateToDock();
                    content.Activate();

                    (prevPane.Parent as UIElement).InvalidateMeasure();
                } else {
                    if (desideredAnchor == AnchorStyle.None &&
                        content.SavedStateAndPosition != null &&
                        content.SavedStateAndPosition.Anchor != AnchorStyle.None)
                        desideredAnchor = content.SavedStateAndPosition.Anchor;

                    if (desideredAnchor == AnchorStyle.None)
                        desideredAnchor = AnchorStyle.Right;

                    DockablePane foundPane = null;

                    if (desideredState == DockableContentState.Docked) {
                        //first not empty panes
                        ILinqToTree<DependencyObject> itemFound = new LogicalTreeAdapter(this).Descendants().FirstOrDefault(el => el.Item is DockablePane && (el.Item as DockablePane).Anchor == desideredAnchor && (el.Item as DockablePane).IsDocked);

                        if (itemFound == null)//look for all panes even empty
                            itemFound = new LogicalTreeAdapter(this).Descendants().FirstOrDefault(el => el.Item is DockablePane && (el.Item as DockablePane).Anchor == desideredAnchor && (el.Item as DockablePane).Items.Count == 0);

                        foundPane = itemFound != null ? itemFound.Item as DockablePane : null;
                    }

                    if (foundPane != null) {
                        content.SetStateToDock();
                        foundPane.Items.Add(content);

                        if ((foundPane.IsAutoHidden && desideredState == DockableContentState.Docked) ||
                             (!foundPane.IsAutoHidden && desideredState == DockableContentState.AutoHide))
                            foundPane.ToggleAutoHide();
                    } else {
                        DockablePane newHostpane = new DockablePane();
                        newHostpane.Items.Add(content);

                        if (desideredAnchor == AnchorStyle.Left ||
                            desideredAnchor == AnchorStyle.Right) {
                            double w = 200;
                            if (content.SavedStateAndPosition != null &&
                                !double.IsInfinity(content.SavedStateAndPosition.Width) &&
                                !double.IsNaN(content.SavedStateAndPosition.Width))
                                w = content.SavedStateAndPosition.Width;

                            ResizingPanel.SetResizeWidth(newHostpane, new GridLength(w));
                            ResizingPanel.SetEffectiveSize(newHostpane, new Size(w, 0.0));
                        } else {
                            double h = 200;
                            if (content.SavedStateAndPosition != null &&
                                !double.IsInfinity(content.SavedStateAndPosition.Height) &&
                                !double.IsNaN(content.SavedStateAndPosition.Height))
                                h = content.SavedStateAndPosition.Height;

                            ResizingPanel.SetResizeHeight(newHostpane, new GridLength(h));
                            ResizingPanel.SetEffectiveSize(newHostpane, new Size(0.0, h));
                        }


                        Anchor(newHostpane, desideredAnchor);

                        if (desideredState == DockableContentState.AutoHide) {
                            ToggleAutoHide(newHostpane);
                        }
                    }
                }

                ActiveContent = content;
                #endregion

            } else if (desideredState == DockableContentState.DockableWindow || desideredState == DockableContentState.FloatingWindow) {

                #region DockableWindow + FloatingWindow

                DockablePane newHostpane = null;
                FloatingDockablePane prevHostpane = null;
                if (content.SavedStateAndPosition != null && content.SavedStateAndPosition.ContainerPane != null && content.SavedStateAndPosition.ContainerPane is FloatingDockablePane) {
                    prevHostpane = content.SavedStateAndPosition.ContainerPane as FloatingDockablePane;
                    if (!prevHostpane.Items.Contains(content))
                        prevHostpane.Items.Add(content);
                } else {
                    newHostpane = new DockablePane();
                    newHostpane.Items.Add(content);
                }

                if (desideredState == DockableContentState.DockableWindow)
                    content.SetStateToDockableWindow();
                else if (desideredState == DockableContentState.FloatingWindow)
                    content.SetStateToFloatingWindow();

                if (prevHostpane != null) {
                    //check to see if floating window that host prevHostPane is already loaded (hosting other contents)
                    var floatingWindow = prevHostpane.Parent as DockableFloatingWindow;
                    if (floatingWindow != null && floatingWindow.IsLoaded) {
                        floatingWindow.Activate();
                    } else {
                        floatingWindow = new DockableFloatingWindow(this);
                        floatingWindow.Content = content;
                        floatingWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                        floatingWindow.Top = prevHostpane.FloatingWindow.Top;
                        floatingWindow.Left = prevHostpane.FloatingWindow.Left;
                        floatingWindow.Width = prevHostpane.FloatingWindow.Width;
                        floatingWindow.Height = prevHostpane.FloatingWindow.Height;

                        var mainWindow = Window.GetWindow(this);
                        if (mainWindow.IsVisible)
                            floatingWindow.Owner = mainWindow;

                        //now I've created a new pane to host the hidden content
                        //if a an hidden content is shown that has prevHostpane as saved pane
                        //I want that it is relocated in this new pane that I've created right now
                        var hiddenContents = DockableContents.Where(c => c.State == DockableContentState.Hidden).ToArray();
                        foreach (var hiddenContent in hiddenContents) {
                            if (hiddenContent.SavedStateAndPosition.ContainerPane == prevHostpane) {
                                hiddenContent.SavedStateAndPosition = new DockableContentStateAndPosition(
                                    (floatingWindow.Content as Pane),
                                    hiddenContent.SavedStateAndPosition.ChildIndex,
                                    hiddenContent.SavedStateAndPosition.Width,
                                    hiddenContent.SavedStateAndPosition.Height,
                                    hiddenContent.SavedStateAndPosition.Anchor,
                                    hiddenContent.SavedStateAndPosition.State);
                            }
                        }
                        SetFloationgWindowProperties(content, floatingWindow);
                        floatingWindow.Show();
                    }
                } else if (newHostpane != null) {
                    var floatingWindow = new DockableFloatingWindow(this);
                    floatingWindow.Content = newHostpane;
                    floatingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    var mainWindow = Window.GetWindow(this);
                    if (mainWindow.IsVisible)
                        floatingWindow.Owner = mainWindow;

                    SetFloationgWindowProperties(content, floatingWindow);
                    floatingWindow.Show();
                }
                
                #endregion

            } else if (desideredState == DockableContentState.Document) {

                DocumentPane docPane = MainDocumentPane;
                if (docPane != null) {
                    docPane.Items.Add(content);
                    docPane.SelectedItem = content;
                    content.SetStateToDocument();
                }
            }
        }

        #endregion


        void SetFloationgWindowProperties(DockableContent content, Window target) {

            target.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var uiView = content.Content as FrameworkElement;

            if (uiView != null) {
                target.Width = uiView.Width + 20 + ((uiView.Width < target.MinWidth) ? target.MinWidth : 0);
                target.Height = uiView.Height + 20 + target.MinHeight;
            }

            if(!content.FloatingWindowSize.IsEmpty){
                target.Width = (content.FloatingWindowSize.Width != 0) ? content.FloatingWindowSize.Width : target.Width;
                target.Height = (content.FloatingWindowSize.Height != 0) ? content.FloatingWindowSize.Height : target.Height;
            }
            target.SizeToContent = content.FloatingWindowSizeToContent;
        }

        void RescueOnNone(DockableContent content, AnchorStyle desideredAnchor) {

            #region Section content.State == DockableContentState.None

            if (content.ContainerPane == null || content.State == DockableContentState.None) {
                //Problem!? try to rescue
                if (content.State == DockableContentState.Docked || content.State == DockableContentState.None) {
                    //find the the pane which the desidered anchor style
                    //DockablePane foundPane = this.FindChildDockablePane(desideredAnchor != AnchorStyle.None ? desideredAnchor : AnchorStyle.Right);
                    //first search for a pane with other contents (avoiding empty panes which are containers for hidden contents)
                    ILinqToTree<DependencyObject> itemFound = new LogicalTreeAdapter(this).Descendants().FirstOrDefault(el => el.Item is DockablePane && (el.Item as DockablePane).Anchor == desideredAnchor && (el.Item as DockablePane).IsDocked);

                    if (itemFound == null)//search for all panes (even empty)
                        itemFound = new LogicalTreeAdapter(this).Descendants().FirstOrDefault(el => el.Item is DockablePane && (el.Item as DockablePane).Anchor == desideredAnchor && (el.Item as DockablePane).Items.Count == 0);

                    DockablePane foundPane = itemFound != null ? itemFound.Item as DockablePane : null;

                    if (foundPane != null) {
                        content.SetStateToDock();
                        foundPane.Items.Add(content);
                        var containerPanel = foundPane.Parent as ResizingPanel;
                        if (containerPanel != null)
                            containerPanel.InvalidateMeasure();
                    } else {
                        //if no suitable pane was found create e new one on the fly
                        if (content.ContainerPane != null) {
                            content.ContainerPane.RemoveContent(content);
                        }

                        DockablePane pane = new DockablePane();
                        pane.Items.Add(content);
                        Anchor(pane, desideredAnchor);
                    }
                } else {
                    //add to main document pane
                    MainDocumentPane.Items.Add(content);
                }
            }

            #endregion
        }

    }
}
