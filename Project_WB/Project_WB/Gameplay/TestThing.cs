using System;
using System.Collections.Generic;
using System.Linq;
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

		TestCharacter character;
		bool follow = false;

		//TODO: remove this
		TimeSpan elapsed = TimeSpan.Zero;
		TimeSpan targetElapsed = TimeSpan.FromSeconds(1);

		List<Point> highlightedTiles = new List<Point>();
		List<Point> barrierList = new List<Point>();

		public TestThing() {
			
		}

		public override void Activate(bool instancePreserved) {
			cam = new Camera2D(ScreenManager.Game.GraphicsDevice.Viewport);
			
			map = Map.Load(Path.Combine(ScreenManager.Game.Content.RootDirectory, @"maps\test.tmx"), ScreenManager.Game.Content);

			for (int i = 0; i < map.Layers["collision"].Width; i++) {
				for (int j = 0; j < map.Layers["collision"].Height; j++) {
					if (map.Layers["collision"].GetTile(i, j) == 116) {
						barrierList.Add(new Point(i, j));
					}
				}
			}

			character = new TestCharacter(new Rectangle(128, 256, 32, 32), ScreenManager.Game.Content.Load<Texture2D>("maps/tilesets/tilesetAv1.0"), new Point(17, 15));

			base.Activate(instancePreserved);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			cam.Update();

			//Maybe use
			elapsed += gameTime.ElapsedGameTime;

			character.Update(gameTime);

			if (follow) {
				cam.DestPosition = character.Position;
			}

			DebugOverlay.DebugText.AppendFormat("Mouse X: {0}  |  Y: {1}", mouse.X, mouse.Y).AppendLine();
			DebugOverlay.DebugText.AppendFormat("MTile X: {0}  |  Y: {1}", mouseTile.X, mouseTile.Y).AppendLine();
			DebugOverlay.DebugText.AppendFormat("ChrSpd: {0}", character.Speed).AppendLine();
			DebugOverlay.DebugText.AppendFormat("SecretToUniverse: {0}", new Random().Next(5000)).AppendLine();

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

			if (input.IsKeyPressed(Keys.R, null, out p)) {
				cam.DestPosition = Vector2.Zero;
				cam.DestScale = 1;
				cam.DestRotationDegrees = 0;
			}
			if (input.IsNewKeyPress(Keys.H, null, out p)) {
				map.Layers["collision"].Opacity = (map.Layers["collision"].Opacity == .65f ? 0 : .65f);
			}

			if (input.IsNewKeyPress(Keys.O, null, out p)) {
				character.Speed -= .1f;
			}
			if (input.IsNewKeyPress(Keys.P, null, out p)) {
				character.Speed += .1f;
			}
			if (input.IsKeyPressed(Keys.F, null, out p)) {
				follow = !follow;
			}

			if (input.IsNewKeyPress(Keys.T, null, out p) || (Mouse.GetState().RightButton == ButtonState.Pressed && mouse.RightButton == ButtonState.Released)) {
				if (mouseTile.X >= 0 && mouseTile.X < 50 && mouseTile.Y >= 0 && mouseTile.Y < 50) {
					//Update pathfinding
					PathMap m = new PathMap();
					m.SetMaps(0, new MapData(50, 50, character.TilePosition, mouseTile, barrierList));
					m.ReloadMap();

					PathFinder pf = new PathFinder();
					pf.Initialize(m);
					pf.Reset();

					pf.IsSearching = true;

					while (pf.SearchStatus != SearchStatus.PathFound && pf.SearchStatus != SearchStatus.NoPath) {
						pf.Update(gameTime);
					}

					if (pf.SearchStatus == SearchStatus.PathFound) {
						character.Waypoints = pf.FinalPath().ToList();
						character.Angry = false;
					}
					else {
						character.Angry = true;
					}
				}
				else {
					character.Angry = true;
				}
			}

			if (Mouse.GetState().ScrollWheelValue > mouse.ScrollWheelValue) {
				cam.DestScale += .1f;
			}
			if (Mouse.GetState().ScrollWheelValue < mouse.ScrollWheelValue) {
				cam.DestScale -= .1f;
			}
			mouse = Mouse.GetState();

			var v = cam.ToRelativePosition(new Vector2(mouse.X - 16 * cam.GetScale(), mouse.Y - 16 * cam.GetScale()));

			mouseTile = new Point((int)Math.Round(v.X / 32),
									(int)Math.Round(v.Y / 32));

			base.HandleInput(gameTime, input);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetMatrixTransformation());
			//ScreenManager.SpriteBatch.Begin();

			map.Draw(ScreenManager.SpriteBatch, new Rectangle(0, 0, 1600, 1600), new Vector2(0, 0));
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Centaur, "???", Vector2.Zero, Color.Blue);

			ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
											new Rectangle(mouseTile.X * 32, mouseTile.Y * 32, 32, 32),
											Color.White * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));

			character.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}

	class TestCharacter : Sprite {
		public Point TilePosition = Point.Zero;
		public List<Point> Waypoints = new List<Point>();
		public bool Angry = false;
		public float Speed = .5f;

		public TestCharacter(Rectangle sourceRectangle, Texture2D spriteSheet, Point tilePosition) : base(sourceRectangle, spriteSheet) {
			this.TilePosition = tilePosition;
			Position = new Vector2(TilePosition.X * 32, TilePosition.Y * 32);
		}

		public override void Update(GameTime gameTime) {
			if(Waypoints.Count > 0) {
				if (Vector2.Distance(Position, new Vector2(Waypoints[0].X * 32, Waypoints[0].Y * 32)) < 2) {
					TilePosition = Waypoints[0];
					Waypoints.RemoveAt(0);
				}
				else {
					Position = new Vector2(MathHelper.Lerp(Position.X, Waypoints[0].X * 32, Speed),
											MathHelper.Lerp(Position.Y, Waypoints[0].Y * 32, Speed));
				}
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			for (int i = 0; i < Waypoints.Count; i++) {
				Point point = Waypoints[i];

				Color c = (i == Waypoints.Count - 1 ? Color.Blue : Color.Green);

				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, new Rectangle(point.X * 32, point.Y * 32, 32, 32),
											c * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));
			}

			if (Angry) {
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, new Rectangle((int)Position.X - 32, (int)Position.Y - 32, 32 * 3, 32 * 3),
												Color.Red * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));
			}
			
			base.Draw(gameTime, screenManager);
		}
	}
}
