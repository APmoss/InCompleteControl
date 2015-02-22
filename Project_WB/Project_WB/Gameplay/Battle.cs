using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Project_WB.Framework;
using Project_WB.Framework.Squared.Tiled;
using Project_WB.Framework.Gui;
using Project_WB.Framework.Audio;
using Project_WB.Framework.Entities;
using Project_WB.Framework.Particles;
using Project_WB.Framework.Pathfinding;
using Microsoft.Xna.Framework.Graphics;
using Project_WB.Framework.Gui.Controls;
using Microsoft.Xna.Framework.Input;
using Project_WB.Menus;

namespace Project_WB.Gameplay {
	/// <summary>
	/// The base class for all combat and battles/engagements.
	/// </summary>
	class Battle : GameScreen {
		#region Fields
		// The camera used in battle
		protected Camera2D camera;
		// The map loaded for this level
		protected Map map;

		// The gui manager for all gui elements
		protected GuiManager guiManager;
		// The audio manager for all sounds
		protected AudioManager audioManager;
		// The particle manager used for all partice effects
		protected ParticleManager particleManager;
		// The entity manager used for entity handling
		protected EntityManager entityManager;

		// Camera boundaries
		Rectangle leftCameraMovementBound;
		Rectangle rightCameraMovementBound;
		Rectangle topCameraMovementBound;
		Rectangle bottomCameraMovementBound;

		// The mouse the tile is placed on
		protected Point mouseTile = Point.Zero;
		// Whether the user can control combat elements
		protected bool combatLocked = false;
		// Shows or hides the bottom gui
		bool showGui = false;
		// The total teams in the current battle
		int totalTeams = 2;
		#endregion

		public Battle() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);
		}

		#region Overridden Methods
		public override void Activate(bool instancePreserved) {
			SetGui();
			
			camera = new Camera2D(ScreenManager.GraphicsDevice.Viewport);

			audioManager = new AudioManager(camera);
			particleManager = new ParticleManager(ScreenManager.Game.Content.Load<Texture2D>("textures/particles"));
			entityManager = new EntityManager(ScreenManager.Game.Content.Load<Texture2D>("textures/etc"),
												ScreenManager.Game.Content.Load<Texture2D>("textures/sprites"),
												audioManager, ScreenManager.SoundLibrary, particleManager);
			entityManager.LoadMap(map);

			leftCameraMovementBound = new Rectangle(0, 0, 5, Stcs.YRes);
			rightCameraMovementBound = new Rectangle(Stcs.XRes - 5, 0, 5, Stcs.YRes);
			topCameraMovementBound = new Rectangle(0, 0, Stcs.XRes, 5);
			bottomCameraMovementBound = new Rectangle(0, Stcs.YRes - 5, Stcs.XRes, 5);

			base.Activate(instancePreserved);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			guiManager.UpdateInteraction(input);

			if (!combatLocked) {
				entityManager.UpdateInteration(gameTime, input, camera);

				PlayerIndex p = PlayerIndex.One;

				int x = (int)MathHelper.Clamp(Mouse.GetState().X, 0, 1279);
				int y = (int)MathHelper.Clamp(Mouse.GetState().Y, 0, 719);
				Mouse.SetPosition(x, y);

				// Unselects units and hides the gui
				if (input.IsNewKeyPress(Keys.Escape, null, out p)) {
					entityManager.SelectedUnit = null;
				}

				int negate = 1;
				if (camera.DestZRotation == MathHelper.Pi) {
					negate = -1;
				}

				// Check the mouse positions to move the camera (camera will move if mouse is on the borders)
				if (leftCameraMovementBound.Contains(x, y)) {
					camera.DestPosition.X -= 5 * negate;
				}
				if (rightCameraMovementBound.Contains(x, y)) {
					camera.DestPosition.X += 5 * negate;
				}
				if (topCameraMovementBound.Contains(x, y)) {
					camera.DestPosition.Y -= 5 * negate;
				}
				if (bottomCameraMovementBound.Contains(x, y)) {
					camera.DestPosition.Y += 5 * negate;
				}

				// Check for directional keys to move the camera
				if (input.IsKeyPressed(Keys.Left, null, out p)) {
					camera.DestPosition.X -= 5 * negate;
				}
				if (input.IsKeyPressed(Keys.Right, null, out p)) {
					camera.DestPosition.X += 5 * negate;
				}
				if (input.IsKeyPressed(Keys.Up, null, out p)) {
					camera.DestPosition.Y -= 5 * negate;
				}
				if (input.IsKeyPressed(Keys.Down, null, out p)) {
					camera.DestPosition.Y += 5 * negate;
				}

				camera.DestPosition.X = (float)MathHelper.Clamp(camera.DestPosition.X, 100, map.Width * map.TileWidth - 100);
				camera.DestPosition.Y = (float)MathHelper.Clamp(camera.DestPosition.Y, 100, map.Height * map.TileHeight - 100);

				// Check for mouse scroll delta for camera zooming
				if (input.CurrentMouseState.ScrollWheelValue > input.LastMouseState.ScrollWheelValue) {
					camera.DestScale += .001f * (input.CurrentMouseState.ScrollWheelValue - input.LastMouseState.ScrollWheelValue);
				}
				if (input.CurrentMouseState.ScrollWheelValue < input.LastMouseState.ScrollWheelValue) {
					camera.DestScale += .001f * (input.CurrentMouseState.ScrollWheelValue - input.LastMouseState.ScrollWheelValue);
				}
			}

			// Update the tile the mouse is currently on
			var relativeMouse = camera.ToRelativePosition(input.CurrentMouseState.X, input.CurrentMouseState.Y);
			if (camera.DestZRotation == MathHelper.Pi) {
				relativeMouse.X -= entityManager.tileSize / 2;
				relativeMouse.Y -= entityManager.tileSize / 2;
			}
			else {
				relativeMouse.X -= entityManager.tileSize / 2;
				relativeMouse.Y -= entityManager.tileSize / 2;
			}
			mouseTile = new Point((int)Math.Round(relativeMouse.X / 32),
								(int)Math.Round(relativeMouse.Y / 32));

			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			camera.Update();

			audioManager.Update(gameTime);
			particleManager.Update(gameTime);
			entityManager.Update(gameTime);
			guiManager.Update(gameTime);

			UpdateHud();
			
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, RasterizerState.CullNone, null, camera.GetMatrixTransformation());

			map.Draw(ScreenManager.SpriteBatch, new Rectangle(0, 0, map.Width * map.TileWidth, map.Height * map.TileHeight), Vector2.Zero);
			Rectangle mouseTileRec = new Rectangle(mouseTile.X * entityManager.tileSize, mouseTile.Y * entityManager.tileSize, entityManager.tileSize, entityManager.tileSize);
			ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, mouseTileRec, Color.White * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)));
			entityManager.Draw(gameTime, ScreenManager);
			particleManager.Draw(gameTime, ScreenManager, camera.GetViewingRectangle());

			ScreenManager.SpriteBatch.End();

			ScreenManager.SpriteBatch.Begin();

			guiManager.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
		#endregion

		#region Methods
		protected void UpdateHud() {
			// Changes the positions and data for the gui and visual elements
			if (showGui) {
				entityHudPanel.Bounds.Y = (int)MathHelper.Lerp((float)entityHudPanel.Bounds.Y, Stcs.YRes - 150, .1f);
			}
			else {
				entityHudPanel.Bounds.Y = (int)MathHelper.Lerp((float)entityHudPanel.Bounds.Y, Stcs.YRes, .1f);
			}
			entityHudPanel.Bounds.Y = (int)MathHelper.Clamp(entityHudPanel.Bounds.Y, Stcs.YRes - 150, Stcs.YRes);

			if (entityManager.SelectedUnit != null) {
				showGui = true;

				var unit = entityManager.SelectedUnit;

				unitNameLabel.Text = unit.Name;
				unitGreenBar.Bounds.Width = (int)(100 * (unit.Health / unit.MaxHealth));
				unitHealthCount.Text = string.Format("{0}/{1}", unit.Health, unit.MaxHealth);
				unitDamageLabel.Text = string.Format("Damage: {0}", unit.Damage);
				unitSpeedLabel.Text = string.Format("Speed: {0}", unit.Speed);
				unitTravelDistance.Text = string.Format("Travel Dist.: {0}", unit.distance);
				unitAttackDistance.Text = string.Format("Attack Dist.: {0}", unit.attackDistance);
				unitTile.Text = string.Format("Tile: {0} - {1}", unit.Tile.X, unit.Tile.Y);
			}
			else {
				showGui = false;
			}
		}
		#endregion

		#region SetGui
		protected DialogBox notificationBox;

		Label unitNameLabel;
		Label unitHealthLabel;
		Bar unitRedBar;
		Bar unitGreenBar;
		Label unitHealthCount;
		Label unitDamageLabel;
		Label unitSpeedLabel;
		Label unitTravelDistance;
		Label unitAttackDistance;
		Label unitTile;

		Button endButton;
		Button exitButton;
		Panel entityHudPanel;

		private void SetGui() {
			guiManager = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);

			notificationBox = new DialogBox(10, 10, Stcs.XRes - 20, 40, "Notification:");
			notificationBox.BackgroundTint = new Color(30, 40, 30, 200);
			notificationBox.HasButton = false;
			
			unitNameLabel = new Label(10, 10, "");
			unitNameLabel.Bounds.Width = 200;
			unitNameLabel.Bounds.Height = 35;
			unitNameLabel.BackgroundTint = Color.DarkCyan;
			unitNameLabel.TextTint = Color.LightCyan;

			unitHealthLabel = new Label(10, 50, "Health");
			unitHealthLabel.Bounds.Width = 90;
			
			unitRedBar = new Bar(110, 50, 100, 35, Color.Red);

			unitGreenBar = new Bar(110, 50, 100, 35, Color.Green);

			unitHealthCount = new Label(115, 50, "100/100");
			unitHealthCount.BackgroundTint = Color.Transparent;

			unitDamageLabel = new Label(220, 10, "Damage:");
			unitDamageLabel.Bounds.Width = 200;
			unitDamageLabel.BackgroundTint = Color.Maroon;

			unitSpeedLabel = new Label(220, 50, "Speed:");
			unitSpeedLabel.Bounds.Width = 200;
			unitSpeedLabel.BackgroundTint = Color.DarkGreen;

			unitTravelDistance = new Label(430, 10, "Travel Dist.:");
			unitTravelDistance.Bounds.Width = 200;
			unitTravelDistance.BackgroundTint = Color.DarkBlue;

			unitAttackDistance = new Label(430, 50, "Attack Dist.:");
			unitAttackDistance.Bounds.Width = 200;
			unitAttackDistance.BackgroundTint = Color.DarkRed;

			unitTile = new Label(640, 10, "Tile:");
			unitTile.Bounds.Width = 200;

			exitButton = new Button(Stcs.XRes - 110, 60, 100, "Surrender");
			exitButton.Tint = Color.Red;
			exitButton.LeftClicked += delegate {
				ExitScreen();
				ScreenManager.AddScreen(new MainMenu(), null);
			};

			endButton = new Button(Stcs.XRes - 110, 10, 100, "End Turn");
			endButton.Tint = Color.DarkOrange;
			endButton.LeftClicked += delegate {
				entityManager.controllingTeam++;
				if (entityManager.controllingTeam > totalTeams) {
					entityManager.controllingTeam = 1;
					entityManager.ResetEntities();
				}
				if (entityManager.controllingTeam == 1) {
					camera.DestZRotation = 0;
					notificationBox.Text = "Player 1 is now in control!";
				}
				else {
					camera.DestZRotation = MathHelper.Pi;
					notificationBox.Text = "Player 2 is now in control!";
				}
			};
			
			entityHudPanel = new Panel(0, Stcs.YRes - 150, Stcs.XRes, 150);
			entityHudPanel.Tint = new Color(10, 10, 10, 256);
			entityHudPanel.AddChild(unitNameLabel);
			entityHudPanel.AddChild(unitRedBar);
			entityHudPanel.AddChild(unitGreenBar);
			entityHudPanel.AddChild(unitHealthCount);
			entityHudPanel.AddChild(unitHealthLabel);
			entityHudPanel.AddChild(unitDamageLabel);
			entityHudPanel.AddChild(unitSpeedLabel);
			entityHudPanel.AddChild(unitTravelDistance);
			entityHudPanel.AddChild(unitAttackDistance);
			entityHudPanel.AddChild(unitTile);
			entityHudPanel.AddChild(endButton);
			entityHudPanel.AddChild(exitButton);

			guiManager.AddControl(notificationBox);
			guiManager.AddControl(entityHudPanel);
		}
		#endregion
	}
}
