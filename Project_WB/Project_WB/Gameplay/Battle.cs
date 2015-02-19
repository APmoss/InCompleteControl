using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project_WB.Framework;
using Project_WB.Framework.Pathfinding;
using Project_WB.Framework.Squared.Tiled;

namespace Project_WB.Gameplay {
	class Battle : GameScreen {
		#region Fields
		MouseState mouse;
		MouseState oldMouse;
		Camera2D camera;

		Map map;
		#endregion

		#region Overridden Methods
		public override void Activate(bool instancePreserved) {
			mouse = Mouse.GetState();
			oldMouse = mouse;
			camera = new Camera2D(ScreenManager.Game.GraphicsDevice.Viewport);
			
			base.Activate(instancePreserved);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			mouse = Mouse.GetState();

			oldMouse = mouse;

			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			camera.Update();

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			base.Draw(gameTime);
		}
		#endregion

		#region Methods
		protected Point GetMouseTile() {
			int halfTileSize = map.TileWidth / 2;

			Vector2 relMousePos = camera.ToRelativePosition(new Vector2(mouse.X - halfTileSize * camera.GetScale(),
																		mouse.Y - halfTileSize * camera.GetScale()));

			Point mouseTile = new Point((int)Math.Round(relMousePos.X / 32),
										(int)Math.Round(relMousePos.Y / 32));

			return mouseTile;
		}
		#endregion
	}
}
