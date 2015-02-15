using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Ruminate.DataStructures;

namespace Ruminate.GUI.Framework {

    public abstract class WidgetBase<T> : Widget where T : RenderRule {

        protected Gui Owner { get; set; }
        protected T RenderRule { get; set; }

        public override Rectangle Area { get; protected set; }

        public override Rectangle AbsoluteArea {
            get { return RenderRule.Area; } 
            set { RenderRule.Area = value; }
        }
        public override Rectangle AbsoluteInputArea {
            get { return RenderRule.SafeArea; }
        }        

        protected abstract T BuildRenderRule();
        protected WidgetBase() {
            RenderRule = BuildRenderRule();
        }
        
        protected abstract void Attach();
        internal override void Prepare(Gui gui) {

            Owner = gui;
            SissorArea = gui.RenderManager.GraphicsDevice.Viewport.Bounds;
            InputManager = gui.InputManager;
            
            RenderRule.RenderManager = gui.RenderManager;            
            RenderRule.Load();

            Attach();

            AbsoluteArea = Area;
        }        

        internal override void Draw() { RenderRule.Draw(); }

        internal override void DrawNoClipping() { RenderRule.DrawNoClipping(); }     
    }

    public abstract class Widget : ITreeNode<Widget> {

        internal InputManager InputManager { get; set; }

        //Location
        public abstract Rectangle Area { get; protected set; }

        public abstract Rectangle AbsoluteArea { get; set; }
        public abstract Rectangle AbsoluteInputArea { get; }
        public Rectangle SissorArea { get; set; }

        //Dom management
        private TreeNode<Widget> TreeNode { get; set;}
        public TreeNode<Widget> GetTreeNode() {
            return TreeNode;
        }

        public Widget Parent {
            get { return TreeNode.Parent.Data; }
        }        
        public IEnumerable<Widget> Children {
            get {
                return from node in TreeNode.Children select node.Data;
            } set {
                TreeNode.Children.Clear();
                AddWidgets(value);
            }
        }
       

        public void AddWidget(Widget widget) { TreeNode.AddChild(widget); }
        public void AddWidgets(IEnumerable<Widget> widget) { TreeNode.AddChildren(widget); }
        public void RemoveWidget(Widget widget) { TreeNode.RemoveChild(widget); }

        /*####################################################################*/
        /*                                State                               */
        /*####################################################################*/

        internal bool V;
        public bool Visible {
            get {
                return V;
            } set {
                TreeNode.DfsOperation(node => node.Data.V = value);
            }
        }

        internal bool A;
        public bool Active {
            get {
                return A;
            } set {
                TreeNode.DfsOperation(node => node.Data.A = value);
            }
        }

        /// <summary>
        /// When true this widget blocks any other widget from 
        /// receiving input until the mouse leave this widget's
        /// input area.
        /// </summary>
        internal protected bool BlocksInput { get; set; }

        public bool IsPressed { get { return InputManager.PressedWidget == this; } }

        public bool IsFocused { get { return InputManager.FocusedWidget == this; } }

        public bool IsHover { get { return InputManager.HoverWidget == this; } }

        #region Events

        /*####################################################################*/
        /*                                 Events                             */
        /*####################################################################*/

        //Interaction with internal layout system
        protected internal virtual void EnterHover() { }
        protected internal virtual void ExitHover() { }

        protected internal virtual void EnterPressed() { }
        protected internal virtual void ExitPressed() { }

        protected internal virtual void EnterFocus() { }
        protected internal virtual void ExitFocus() { }

        //Interaction with input system        
        protected internal virtual void CharEntered(CharacterEventArgs e) { }
        protected internal virtual void KeyDown(KeyEventArgs e) { }
        protected internal virtual void KeyUp(KeyEventArgs e) { }

        public virtual void MouseHover(MouseEventArgs e) { }
        protected internal virtual void MouseClick(MouseEventArgs e) { }
        protected internal virtual void MouseDoubleClick(MouseEventArgs e) { }

        protected internal virtual void MouseDown(MouseEventArgs e) { }
        protected internal virtual void MouseUp(MouseEventArgs e) { }
        protected internal virtual void MouseMove(MouseEventArgs e) { }
        protected internal virtual void MouseWheel(MouseEventArgs e) { }

        #endregion

        #region Flow

        /*####################################################################*/
        /*                                Flow                                */
        /*####################################################################*/

        protected Widget() {

            TreeNode = new TreeNode<Widget>(this);

            Visible = true;
            Active = true;

            BlocksInput = false;
        }

        internal abstract void Prepare(Gui gui);
        public abstract void Layout();

        protected internal abstract void Update();

        internal abstract void Draw();
        internal abstract void DrawNoClipping();

        #endregion
        
    }
}
