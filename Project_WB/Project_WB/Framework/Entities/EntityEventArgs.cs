using System;
using System.Collections.Generic;

namespace Project_WB.Framework.Entities {
	class EntityEventArgs : EventArgs {
		#region Fields
		public Entity RecievingEntity;
		#endregion

		public EntityEventArgs(Entity recievingEntity) {
			this.RecievingEntity = recievingEntity;
		}
	}
}
