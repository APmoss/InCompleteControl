using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Project_WB.Gameplay {
	abstract class Entity {
		//TODO: finish documentation

		#region Fields
		public string Name = string.Empty;
		#endregion

		#region Methods
		public virtual void Update(GameTime gameTime) { }

		public virtual void Draw(GameTime gameTime, ScreenManager screenManager) { }
		#endregion
	}
}
