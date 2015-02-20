using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Project_WB.Framework.Entities {
	class HotkeyEventArgs : EventArgs {
		#region Properties
		public Keys[] Keys {
			get; protected set;
		}
		#endregion

		public HotkeyEventArgs(Keys[] keys) {
			this.Keys = keys;
		}
	}
}
