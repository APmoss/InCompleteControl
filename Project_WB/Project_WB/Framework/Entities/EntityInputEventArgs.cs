using System;
using System.Collections.Generic;
using GameStateManagement;

namespace Project_WB.Framework.Entities {
	class EntityInputEventArgs : EventArgs {
		#region Properties
		public InputState Input {
			get; protected set;
		}
		#endregion

		public EntityInputEventArgs(InputState input) {
			this.Input = input;
		}
	}
}
