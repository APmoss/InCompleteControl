using System;
using System.Collections.Generic;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project_WB.Framework;

namespace Project_WB {
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class WB_Game : Microsoft.Xna.Framework.Game {
		GraphicsDeviceManager graphics;
		ScreenManager screenManager;

		public WB_Game() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			Stcs.InternalVersion = new Version("1.0.0.1");

			// Make the mouse visible
			IsMouseVisible = true;

			// Set framerate to 60fps
			TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

			// Set the default resolution (changeable in options) to 1280x720 (720p)
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;

			// Set up the screen manager and add some screens
			screenManager = new ScreenManager(this, graphics);
			Components.Add(screenManager);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			AddInitialScreens();
			
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {			
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			// Clear the buffers with black
			GraphicsDevice.Clear(Color.Black);

			base.Draw(gameTime);
		}

		/// <summary>
		/// Adds the initial screens that show when starting up
		/// </summary>
		private void AddInitialScreens() {
			screenManager.AddScreen(new DebugOverlay(), null);
			screenManager.AddScreen(new Menus.Splash(), null);
			//screenManager.AddScreen(new Menus.Title(), null);
			//screenManager.AddScreen(new Menus.SignIn(), null);
			//screenManager.AddScreen(new Menus.Register(), null);
		}
	}
}
