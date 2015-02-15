using System;
using System.Collections.Generic;
using GameStateManagement;
using Squared.Tiled;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project_WB.Gameplay {
	class TestThing : GameScreen {
		Camera2D cam;
		MouseState mouse = new MouseState();

		Map map;

		public TestThing() {
			
		}

		public override void Activate(bool instancePreserved) {
			cam = new Camera2D(ScreenManager.Game.GraphicsDevice.Viewport);
			
			map = Map.Load(Path.Combine(ScreenManager.Game.Content.RootDirectory, @"maps\test.tmx"), ScreenManager.Game.Content);
			
			base.Activate(instancePreserved);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			cam.Update();

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			PlayerIndex p;

			if (input.IsKeyPressed(Keys.W, null, out p)) {
				cam.DestPosition.Y -= 5;
			}
			if (input.IsKeyPressed(Keys.A, null, out p)) {
				cam.DestPosition.X -= 5;
			}
			if (input.IsKeyPressed(Keys.S, null, out p)) {
				cam.DestPosition.Y += 5;
			}
			if (input.IsKeyPressed(Keys.D, null, out p)) {
				cam.DestPosition.X += 5;
			}

			if (input.IsKeyPressed(Keys.E, null, out p)) {
				cam.DestRotationDegrees += 1;
			}
			if (input.IsKeyPressed(Keys.Q, null, out p)) {
				cam.DestRotationDegrees -= 1;
			}

			if (Mouse.GetState().ScrollWheelValue > mouse.ScrollWheelValue) {
				cam.DestScale += .1f;
			}
			if (Mouse.GetState().ScrollWheelValue < mouse.ScrollWheelValue) {
				cam.DestScale -= .1f;
			}
			mouse = Mouse.GetState();
			
			base.HandleInput(gameTime, input);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetMatrixTransformation());

			map.Draw(ScreenManager.SpriteBatch, new Rectangle(0, 0, 700, 1200), new Vector2(0, 0));

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
