using System;
using System.Collections.Generic;
using GameStateManagement;
using Squared.Tiled;
using Project_WB.Gameplay.Pathfinding;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project_WB.Gameplay {
	class TestThing : GameScreen {
		Camera2D cam;
		MouseState mouse = new MouseState();
		Point mouseTile = Point.Zero;

		Map map;

		//TODO: remove this
		TimeSpan elapsed = TimeSpan.Zero;
		TimeSpan targetElapsed = TimeSpan.FromSeconds(1);

		List<Point> highlightedTiles = new List<Point>();

		public TestThing() {
			
		}

		public override void Activate(bool instancePreserved) {
			cam = new Camera2D(ScreenManager.Game.GraphicsDevice.Viewport);
			
			map = Map.Load(Path.Combine(ScreenManager.Game.Content.RootDirectory, @"maps\test.tmx"), ScreenManager.Game.Content);
			
			base.Activate(instancePreserved);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			cam.Update();

			//elapsed += gameTime.ElapsedGameTime;

			if (elapsed > targetElapsed) {
				elapsed -= targetElapsed;

				//Point mouseTile = new Point((int)Math.Round(mouse.X / 32.0) * 32, (int)Math.Round(mouse.Y / 32.0) * 32);

				//Update pathfinding
				PathMap m = new PathMap();
				//m.SetMaps(0, new MapData(15, 15, 8, ))

				PathFinder p = new PathFinder();
				//p.Initialize(
			}
			
			DebugOverlay.DebugText.AppendFormat("Mouse X: {0}  |  Y: {1}", mouse.X, mouse.Y).AppendLine();
			DebugOverlay.DebugText.AppendFormat("MTile X: {0}  |  Y: {1}", mouseTile.X, mouseTile.Y).AppendLine();

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

			mouseTile = new Point((int)Math.Round((mouse.X - 16) / 32.0),
									(int)Math.Round((mouse.Y - 16) / 32.0));
			
			base.HandleInput(gameTime, input);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetMatrixTransformation());
			//ScreenManager.SpriteBatch.Begin();

			map.Draw(ScreenManager.SpriteBatch, new Rectangle(0, 0, 700, 1200), new Vector2(0, 0));
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Centaur, "???", Vector2.Zero, Color.Blue);

			ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(mouseTile.X * 32, mouseTile.Y * 32, 32, 32),
											Color.White * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
