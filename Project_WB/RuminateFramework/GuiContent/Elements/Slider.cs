using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public sealed class Slider : WidgetBase<SliderRenderRule> {

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        private int _travelStart = -1;

        private Pin Pin { get; set; }

        /// <summary>
        /// The Percentage of the value the slider is at. 100% is all the way to the right.
        /// 0% is all the way to the left.
        /// </summary>
        public float Value {
            get {
                return RenderRule.Percentage;
            } set {
                RenderRule.Percentage = value;
            }
        }

        /// <summary>
        /// Event fired when the slider is dragged.
        /// </summary>
        public WidgetEvent ValueChanged { get; set; }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        protected override SliderRenderRule BuildRenderRule() {
            return new SliderRenderRule();
        }

        public Slider(int x, int y, int width) {

            Pin = new Pin();
            Area = new Rectangle(x, y, width, 0);
        }

        public Slider(int x, int y, int width, WidgetEvent onValueChanged) {

            Pin = new Pin();
            Area = new Rectangle(x, y, width, 0);
            ValueChanged = onValueChanged;
        } 

        protected override void Attach() {

            Area = new Rectangle(Area.X, Area.Y, Area.Width, RenderRule.IconSize.Y);
        }

        public override void Layout() { }

        protected internal override void Update() {

            if (_travelStart == -1) { return; }

            RenderRule.Travel = _travelStart - (int)Pin.Shift.X;
            ValueChanged(this);
        }

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        protected internal override void MouseDown(MouseEventArgs e) {

            var l = InputSystem.MouseLocation;

            if (!RenderRule.GripArea.Contains(l)) { return; }

            _travelStart = RenderRule.Travel;
            Pin.Push();
        }

        protected internal override void MouseUp(MouseEventArgs e) {

            _travelStart = -1;
            Pin.Pull();
        }
    }
}