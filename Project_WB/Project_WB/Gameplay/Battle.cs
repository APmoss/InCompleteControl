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

namespace Project_WB.Gameplay {
	class Battle : GameScreen {
		#region Fields
		Camera2D camera;
		Map map;

		GuiManager guiManager;
		AudioManager audioManager;
		ParticleManager particleManager;
		EntityManager entityManager;

		PathFinder pathFinder;
		#endregion

		#region Overridden Methods
		public override void Activate(bool instancePreserved) {
			SetGui();
			camera = new Camera2D(ScreenManager.GraphicsDevice.Viewport);

			audioManager = new AudioManager(camera);
			particleManager = new ParticleManager(ScreenManager.Game.Content.Load<Texture2D>("textures/particles"));
			entityManager = new EntityManager(ScreenManager.Game.Content.Load<Texture2D>("textures/etc"),
												ScreenManager.Game.Content.Load<Texture2D>("textures/sprites"),
												audioManager, ScreenManager.SoundLibrary, particleManager);

			pathFinder = new PathFinder();

			base.Activate(instancePreserved);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			entityManager.UpdateInteration(gameTime, input, camera);
			guiManager.UpdateInteraction(input);

			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			camera.Update();

			audioManager.Update(gameTime);
			particleManager.Update(gameTime);
			entityManager.Update(gameTime);
			guiManager.Update(gameTime);

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, RasterizerState.CullNone, null, camera.GetMatrixTransformation());

			//map.Draw(ScreenManager.SpriteBatch, new Rectangle(0, 0, map.Width, map.Height), Vector2.Zero);
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
		#endregion

		#region SetGui
		Panel entityHud;

		private void SetGui() {
			guiManager = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);

			entityHud = new Panel(0, Stcs.YRes - 150, Stcs.XRes, 150);

			guiManager.AddControl(entityHud);
		}
		#endregion
	}
}
