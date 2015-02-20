using System;
using System.Collections.Generic;

namespace Project_WB.Framework.Entities {
	class EntityEventArgs : EventArgs {
		#region Fields
		public List<Entity> EntityList;
		#endregion

		public EntityEventArgs(List<Entity> entityList) {
			this.EntityList = entityList;
		}
	}
}
