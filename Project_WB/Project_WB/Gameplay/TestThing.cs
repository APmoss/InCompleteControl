using System;
using System.Collections.Generic;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_WB.Framework;
using Project_WB.Framework.Audio;
using Project_WB.Framework.Entities;
using Project_WB.Framework.Entities.Units;
using Project_WB.Framework.Particles;
using Project_WB.Framework.Pathfinding;
using Project_WB.Framework.Squared.Tiled;
using Project_WB.Framework.IO;

namespace Project_WB.Gameplay {
	class TestThing : GameScreen {
		#region Fields
		Camera2D cam;
		AudioManager audioManager;
		ParticleManager particleManager;
		EntityManager entityManager;
		EnvironmentSound testSound;
		MouseState mouse = new MouseState();
		Point mouseTile = Point.Zero;
		Texture2D maru, baker;
		Texture2D vignette;
		Effect testEffect;
		TimeSpan searchTime = TimeSpan.Zero;
		RenderTarget2D mainTarget;
		RenderTarget2D lightMask;
		Color ambience = new Color(10, 10, 10, 255);

		Map map;

		TestCharacter character;
		bool follow = false;

		Random r = new Random();
		int particleLoops = 1;

		TimeSpan elapsed = TimeSpan.Zero;
		TimeSpan targetElapsed = TimeSpan.FromSeconds(1);

		List<Point> highlightedTiles = new List<Point>();
		List<Point> barrierList = new List<Point>();

		bool partyTime = false;
		TimeSpan partyTimeElapsed = TimeSpan.Zero;
		TimeSpan targetPartyTime = TimeSpan.FromSeconds(.2);
		#endregion

		public TestThing() {
			
		}

		#region Methods
		public override void Activate(bool instancePreserved) {
			cam = new Camera2D(ScreenManager.Game.GraphicsDevice.Viewport);
			audioManager = new AudioManager(cam);
			particleManager = new ParticleManager(ScreenManager.Game.Content.Load<Texture2D>("textures/particles"));
			particleManager.MaxParticleCount = 4096;
			testSound = new EnvironmentSound(ScreenManager.SoundLibrary.GetSound("creepyNoise"), new Vector2(256/32, 1344/32), true, TimeSpan.FromSeconds(30));
			audioManager.AddSounds(testSound);

			map = Map.Load(@"maps\test.tmx", ScreenManager.Game.Content);

			maru = ScreenManager.Game.Content.Load<Texture2D>("textures/maru1");
			baker = ScreenManager.Game.Content.Load<Texture2D>("textures/baker1");
			vignette = ScreenManager.Game.Content.Load<Texture2D>("textures/vignette");

			testEffect = ScreenManager.Game.Content.Load<Effect>("effects/radialLight");

			for (int i = 0; i < map.Layers["collision"].Width; i++) {
				for (int j = 0; j < map.Layers["collision"].Height; j++) {
					if (map.Layers["collision"].GetTile(i, j) == 116) {
						barrierList.Add(new Point(i, j));
					}
				}
			}

			character = new TestCharacter(new Rectangle(128, 256, 32, 32), ScreenManager.Game.Content.Load<Texture2D>("maps/tilesets/tilesetAv1.0"), new Point(17, 15), maru);
			character.LeftClicked += delegate {
				switch (r.Next(6)) {
					case 0:
						//ScreenManager.SoundLibrary.GetSound("-orders").Play();
						break;
					case 1:
						//ScreenManager.SoundLibrary.GetSound("-reporting").Play();
						break;
					case 2:
						//ScreenManager.SoundLibrary.GetSound("-yes").Play();
						break;
					case 3:
						//ScreenManager.SoundLibrary.GetSound("-good").Play();
						break;
					case 4:
						//ScreenManager.SoundLibrary.GetSound("-iread").Play();
						break;
					case 5:
						//ScreenManager.SoundLibrary.GetSound("-what").Play();
						break;
				}
			};
			character.MouseEntered += (s, e) => ScreenManager.SoundLibrary.GetSound("tileChange1").Play();

			entityManager = new EntityManager(ScreenManager.Game.Content.Load<Texture2D>("textures/etc"), ScreenManager.Game.Content.Load<Texture2D>("textures/sprites"), audioManager, ScreenManager.SoundLibrary, particleManager);
			
			entityManager.LoadMap(map);
			Sango sango = new Sango();
			Guy guy = new Guy();
			sango.Position = new Vector2(128, 352);
			guy.Position = new Vector2(128 + 32, 352);
			entityManager.AddEntities(sango, guy);

			mainTarget = new RenderTarget2D(ScreenManager.GraphicsDevice, Stcs.XRes, Stcs.YRes);
			lightMask = new RenderTarget2D(ScreenManager.GraphicsDevice, Stcs.XRes, Stcs.YRes);

			base.Activate(instancePreserved);
		}
		
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			cam.Update();
			audioManager.Update(gameTime);

			//Maybe use
			elapsed += gameTime.ElapsedGameTime;

			character.Update(gameTime);
			entityManager.Update(gameTime);

			if (follow) {
				cam.DestPosition = entityManager.SelectedUnit.Position;
				if (entityManager.SelectedUnit.Waypoints.Count > 0) {
					cam.DestScale = 2;
					//cam.DestXRotation = MathHelper.ToRadians(45);
				}
				else {
					cam.DestScale = 1;
					//cam.DestXRotation = MathHelper.ToRadians(0);
				}
			}

			partyTimeElapsed += gameTime.ElapsedGameTime;
			if (partyTimeElapsed >= targetPartyTime) {
				partyTimeElapsed -= targetPartyTime;

				if (partyTime) {
					if (cam.DestZRotation == MathHelper.ToRadians(-10)) {
						cam.DestZRotation = MathHelper.ToRadians(10);

						ambience.R += 50;
						ambience.G += 50;
						ambience.B += 50;
					}
					else if (cam.DestZRotation == MathHelper.ToRadians(10)) {
						cam.DestZRotation = MathHelper.ToRadians(-10);

						ambience.R -= 50;
						ambience.G -= 50;
						ambience.B -= 50;
					}
					if (cam.DestScale == 3.5) {
						cam.DestScale = 2.5f;
					}
					else if (cam.DestScale == 2.5) {
						cam.DestScale = 3.5f;
					}
				}
			}

			for (int i = 0; i < 3; i++) {
				particleManager.AddParticle(new Particle(new Rectangle(0, 16, 16, 16)) {
					LifeSpan = new TimeSpan(0, 0, 8),
					Position = new Vector2(r.Next(2048), 0),
					Velocity = new Vector2(r.Next(-2, 0), r.Next(7, 15)),
					Tint = Color.White
				});	
			}

			if (mouse.LeftButton == ButtonState.Pressed) {
				for (int i = 0; i < particleLoops; i++) {
					double angle = MathHelper.ToRadians(r.Next(0, 1440) / 4);
					Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					vel *= (float)(r.NextDouble() * 2 + 1);

					particleManager.AddParticle(new Particle(new Rectangle(0, 0, 16, 16)) {
						LifeSpan = new TimeSpan(0, 0, 2),
						Position = cam.ToRelativePosition(new Vector2(mouse.X, mouse.Y)),
						Velocity = vel,
						Tint = new Color() {
							R = (byte)r.Next(50, 200),
							G = (byte)r.Next(50, 200),
							B = (byte)r.Next(50, 200),
							A = (byte)255
						}
					});
				}
			}

			foreach (var entity in entityManager.GetEntities()) {
				if (entity is VerticalCenturion && gameTime.TotalGameTime.TotalMilliseconds % 10 == 0) {
					double angle = MathHelper.ToRadians(r.Next(0, 1440) / 4);
					Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					vel *= (float)(r.NextDouble()  / 2);

					particleManager.AddParticle(new Particle(new Rectangle(0, 0, 16, 16)) {
						LifeSpan = new TimeSpan(0, 0, 0, 0, 500),
						Position = ((VerticalCenturion)entity).Position + new Vector2(9, 9),
						Velocity = vel,
						Scale = .25f,
						Tint = new Color() {
							R = (byte)r.Next(150, 200),
							G = (byte)r.Next(150, 200),
							B = 0,//(byte)r.Next(150, 200),
							A = (byte)255
						}
					});
				}
			}

			particleManager.Update(gameTime);

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
			DebugOverlay.DebugText.AppendFormat("-Mouse X: {0}  |  Y: {1}", mouse.X, mouse.Y).AppendLine();
			DebugOverlay.DebugText.AppendFormat("-MTile X: {0}  |  Y: {1}", mouseTile.X, mouseTile.Y).AppendLine();
			DebugOverlay.DebugText.AppendFormat("-ChrSpd: {0}", character.Speed).AppendLine();
			DebugOverlay.DebugText.AppendFormat("-SecretToUniverse: {0}", new Random().Next(5000)).AppendLine();
			DebugOverlay.DebugText.AppendFormat("-ParticleTotal: {0}", particleManager.ParticleCount).AppendLine();
			DebugOverlay.DebugText.AppendFormat("-ParticleLoops: {0}", particleLoops).AppendLine();
			DebugOverlay.DebugText.AppendFormat("-SearchTime: {0}", searchTime.TotalMilliseconds).AppendLine();

			DebugOverlay.DebugText.AppendFormat("musicTransitionAlpha: {0}", audioManager.musicTransitionAlpha).AppendLine();
			foreach (var sound in audioManager.AudioItems) {
				DebugOverlay.DebugText.AppendFormat("--{0}: {1}", sound.Name, sound.LifeSpan).AppendLine();
			}			
		}
		
		public override void HandleInput(GameTime gameTime, InputState input) {
			PlayerIndex p;

			if (input.IsNewKeyPress(Keys.Escape, null, out p)) {
				audioManager.StopAll();
				ExitScreen();
			}

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
				cam.DestZRotation += MathHelper.ToRadians(10);
			}
			if (input.IsKeyPressed(Keys.Q, null, out p)) {
				cam.DestZRotation -= MathHelper.ToRadians(10);
			}
			if (input.IsButtonPressed(Buttons.DPadUp, null, out p)) {
				cam.DestXRotation -= MathHelper.ToRadians(1);
			}
			if (input.IsButtonPressed(Buttons.DPadDown, null, out p)) {
				cam.DestXRotation += MathHelper.ToRadians(1);
			}
			if (input.IsButtonPressed(Buttons.DPadLeft, null, out p)) {
				cam.DestYRotation -= MathHelper.ToRadians(1);
			}
			if (input.IsButtonPressed(Buttons.DPadRight, null, out p)) {
				cam.DestYRotation += MathHelper.ToRadians(1);
			}

			if (input.IsKeyPressed(Keys.R, null, out p)) {
				cam.DestPosition = Vector2.Zero;
				cam.DestScale = 1;
				cam.DestZRotation = MathHelper.ToRadians(0);
			}
			if (input.IsNewKeyPress(Keys.K, null, out p)) {
				partyTime = !partyTime;
				cam.DestZRotation = MathHelper.ToRadians(10);
				cam.DestScale = 2.5f;
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
			if (input.IsNewKeyPress(Keys.F, null, out p)) {
				follow = !follow;
			}
			if (input.IsNewKeyPress(Keys.B, null, out p)) {
				BakerBot temp = new BakerBot(baker);
				temp.Position = new Vector2(mouseTile.X * 32, mouseTile.Y * 32);
				entityManager.AddEntities(temp);
			}
			if (input.IsNewKeyPress(Keys.N, null, out p)) {
				Centurion temp = new Centurion();
				temp.Position = new Vector2(mouseTile.X * 32, mouseTile.Y * 32);
				entityManager.AddEntities(temp);
			}
			if (input.IsKeyPressed(Keys.M, null, out p)) {
				VerticalCenturion temp = new VerticalCenturion();
				temp.Position = new Vector2(mouseTile.X * 32, mouseTile.Y * 32);
				entityManager.AddEntities(temp);
			}
			if (input.IsNewKeyPress(Keys.Y, null, out p)) {
				int sel = r.Next(entityManager.GetEntities().Count());
				entityManager.SelectedUnit = (Unit)entityManager.GetEntities()[sel];
			}
			if (input.IsNewKeyPress(Keys.Space, null, out p)) {
				if (entityManager.SelectedUnit != null) {
					entityManager.SelectedUnit.Health -= 20;
				}
			}

			if (input.IsKeyPressed(Keys.PageUp, null, out p)) {
				ambience.R += 2;
				ambience.B += 2;
				ambience.G += 2;
			}
			if (input.IsKeyPressed(Keys.PageDown, null, out p)) {
				ambience.R -= 2;
				ambience.B -= 2;
				ambience.G -= 2;
			}
			ambience.R = (byte)MathHelper.Clamp(ambience.R, 2, 253);
			ambience.G = (byte)MathHelper.Clamp(ambience.G, 2, 253);
			ambience.B = (byte)MathHelper.Clamp(ambience.B, 2, 253);

			if (input.IsKeyPressed(Keys.OemPlus, null, out p)) {
				particleLoops++;
			}
			if (input.IsKeyPressed(Keys.OemMinus, null, out p)) {
				particleLoops--;
			}

			if (input.IsNewKeyPress(Keys.OemSemicolon, null, out p)) {
				audioManager.AddSounds(new MusicTrack(ScreenManager.SoundLibrary.GetSound("blaster")));
			}
			if (input.IsNewKeyPress(Keys.OemQuotes, null, out p)) {
				audioManager.AddSounds(new MusicTrack(ScreenManager.SoundLibrary.GetSound("southSea")));
			}

			if (input.IsNewMousePress(InputState.MouseButton.Right)) {
				if (entityManager.SelectedUnit != null) {
					if ((mouseTile.X >= 0 && mouseTile.X < 50 && mouseTile.Y >= 0 && mouseTile.Y < 50) &&
															(!barrierList.Contains(mouseTile))) {
						PathFinder pf = new PathFinder();

						LinkedList<Point> solution = new LinkedList<Point>();

						if (input.IsKeyPressed(Keys.LeftShift, null, out p) && entityManager.SelectedUnit.Waypoints.Count > 0) {
							if (pf.QuickFind(new MapData(50, 50, entityManager.SelectedUnit.Waypoints.Last(), mouseTile, barrierList), out solution)) {
								solution.RemoveFirst();
								foreach (var item in solution) {
									entityManager.SelectedUnit.Waypoints.AddLast(item);
								}
								character.Angry = false;

								switch (r.Next(4)) {
									case 0:
										//ScreenManager.SoundLibrary.GetSound("-affirmative").Play();
										break;
									case 1:
										//ScreenManager.SoundLibrary.GetSound("-recieved").Play();
										break;
									case 2:
										//ScreenManager.SoundLibrary.GetSound("-rightaway").Play();
										break;
									case 3:
										//ScreenManager.SoundLibrary.GetSound("-roger").Play();
										break;
								}
							}
							else {
								character.Angry = true;
							}
						}
						else {
							if (pf.QuickFind(new MapData(50, 50, entityManager.SelectedUnit.Tile, mouseTile, barrierList), out solution)) {
								entityManager.SelectedUnit.Waypoints = solution;
								character.Angry = false;

								switch (r.Next(4)) {
									case 0:
										//ScreenManager.SoundLibrary.GetSound("-affirmative").Play();
										break;
									case 1:
										//ScreenManager.SoundLibrary.GetSound("-recieved").Play();
										break;
									case 2:
										//ScreenManager.SoundLibrary.GetSound("-rightaway").Play();
										break;
									case 3:
										//ScreenManager.SoundLibrary.GetSound("-roger").Play();
										break;
								}
							}
							else {
								character.Angry = true;
							}
						}

						entityManager.SelectedUnit.InvokeWaypointsChanged();
					}
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
			
			Point oldMouseTile = mouseTile;

			mouseTile = new Point((int)Math.Round(v.X / 32),
									(int)Math.Round(v.Y / 32));

			if (oldMouseTile != mouseTile) {
				//ScreenManager.SoundLibrary.GetSound("tileChange1").Play();
			}

			character.UpdateInteraction(gameTime, input, cam);
			entityManager.UpdateInteration(gameTime, input, cam);

			if (input.IsNewKeyPress(Keys.F10, null, out p)) {
				audioManager.StopAll();
				ExitScreen();
			}

			base.HandleInput(gameTime, input);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.GraphicsDevice.SetRenderTarget(mainTarget);
			ScreenManager.GraphicsDevice.Clear(Color.Black);
			ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

			//testEffect.Parameters["mousePos"].SetValue(new Vector2(mouse.X / Stcs.XRes, mouse.Y / Stcs.YRes));

			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, RasterizerState.CullNone, null, cam.GetMatrixTransformation());

			map.Draw(ScreenManager.SpriteBatch, new Rectangle(0, 0, 1600, 1600), new Vector2(0, 0));
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, "!", new Vector2(260, 1354), Color.Blue);

			ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
											new Rectangle(mouseTile.X * 32, mouseTile.Y * 32, 32, 32),
											Color.White * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));
			
			character.Draw(gameTime, ScreenManager);
			entityManager.Draw(gameTime, ScreenManager);

			particleManager.Draw(gameTime, ScreenManager, cam.GetViewingRectangle());

			//ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, DebugOverlay.DebugText, Vector2.Zero, Color.White);

			ScreenManager.SpriteBatch.End();

			ScreenManager.GraphicsDevice.SetRenderTarget(null);
			DrawLightMask(gameTime);

			ScreenManager.GraphicsDevice.Clear(Color.Black);

			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend); //, null, null, null, null, cam.GetMatrixTransformation());
			testEffect.Parameters["lightMask"].SetValue(lightMask);
			testEffect.CurrentTechnique.Passes[0].Apply();
			ScreenManager.SpriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}

		public void DrawLightMask(GameTime gameTime) {
			ScreenManager.GraphicsDevice.SetRenderTarget(lightMask);
			ScreenManager.GraphicsDevice.Clear(Color.Black);

			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cam.GetMatrixTransformation());
			ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, cam.GetViewingRectangle(), ambience);
			ScreenManager.SpriteBatch.End();

			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, cam.GetMatrixTransformation());

			foreach (var particle in particleManager.GetParticles()) {
				ScreenManager.SpriteBatch.Draw(vignette, new Rectangle((int)particle.Position.X - 8, (int)particle.Position.Y - 8, (int)(16 * particle.Scale), (int)(16 * particle.Scale)), particle.Tint);
			}

			Vector2 relativeMouse = cam.ToRelativePosition(mouse.X, mouse.Y);
			float angle = (float)Math.Atan2(mouse.Y - Stcs.YRes / 2, mouse.X - Stcs.XRes / 2);
			float cone = 2 + (Vector2.Distance(new Vector2(mouse.X, mouse.Y), new Vector2(Stcs.XRes / 2, Stcs.YRes / 2)) / 300);
			ScreenManager.SpriteBatch.Draw(vignette, relativeMouse, null, Color.White,
										angle, new Vector2(64), new Vector2(cone, 2), SpriteEffects.None, 0);
			ScreenManager.SpriteBatch.Draw(vignette, character.Position, null, character.flashColor,
										0, new Vector2(48), 1, SpriteEffects.None, 0);

			foreach (var entity in entityManager.GetEntities()) {
				if (entity is VerticalCenturion) {
					VerticalCenturion cent = (VerticalCenturion)entity;

					List<RadialLight> radialLights = (List<RadialLight>)entity.EntityData["radialLights"];

					foreach (var light in radialLights) {
						ScreenManager.SpriteBatch.Draw(vignette, ((Sprite)entity).Position + light.Position, null, light.Tint, 0, Vector2.Zero, light.Scale, 0, 0);
					}
				}
			}
			
			ScreenManager.SpriteBatch.End();

			ScreenManager.GraphicsDevice.SetRenderTarget(null);
		}
		#endregion
	}

	class TestCharacter : Sprite {
		public Point TilePosition = Point.Zero;
		public List<Point> Waypoints = new List<Point>();
		LinkedList<Point> ll = new LinkedList<Point>();
		public bool Angry = false;
		public float Speed = .5f;
		Texture2D maru;
		public Color flashColor = Color.White;
		
		public TestCharacter(Rectangle sourceRectangle, Texture2D spriteSheet, Point tilePosition, Texture2D maru) : base(sourceRectangle, spriteSheet) {
			this.TilePosition = tilePosition;
			Position = new Vector2(TilePosition.X * 32, TilePosition.Y * 32);
			this.maru = maru;

			this.MouseEntered += delegate {
				Tint = Color.Blue;
			};
			this.MouseExited += delegate {
				Tint = Color.White;
			};
			this.LeftClicked += new EventHandler<EntityInputEventArgs>(TestCharacter_LeftClicked);
		}

		void TestCharacter_LeftClicked(object sender, EntityInputEventArgs e) {
			Tint = Color.Red;
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

			flashColor = flashColor == Color.White ? Color.Black : Color.White;
			
			base.Update(gameTime);

			Bounds = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			for (int i = 0; i < Waypoints.Count; i++) {
				Point point = Waypoints[i];

				Color c = (i == Waypoints.Count - 1 ? Color.Blue : Color.Green);

				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, new Rectangle(point.X * 32, point.Y * 32, 32, 32),
											c * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));

				screenManager.SpriteBatch.DrawString(screenManager.FontLibrary.Consolas, i.ToString(), new Vector2(Waypoints[i].X * 32, Waypoints[i].Y * 32),
											Color.Red, 0, Vector2.Zero, .25f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
			}

			if (Angry) {
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, new Rectangle((int)Position.X - 32, (int)Position.Y - 32, 32 * 3, 32 * 3),
												Color.Red * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));
			}

			screenManager.SpriteBatch.Draw(maru, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), Tint);

			base.Draw(gameTime, screenManager);
		}
	}
}
