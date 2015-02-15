using System;
using System.Collections.Generic;
using GameStateManagement;
using Ruminate.GUI.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Ruminate.GUI.Content;
using Microsoft.Xna.Framework;

namespace Project_WB.Menus {
	/// <summary>
	/// This is the screen seen when signing in, and it handles all
	/// code associated with signing in.
	/// </summary>
	class SignIn : GameScreen {
		// Handles all gui widgets
		Gui gui;

		public SignIn() {
			TransitionOnTime = TimeSpan.FromSeconds(.3);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void Activate(bool instancePreserved) {
			Texture2D imageMap = ScreenManager.Game.Content.Load<Texture2D>("gui/grey/imageMap");

			//string s = Environment.CurrentDirectory;

			string map = File.OpenText(@"C:\Users\Alex\Desktop\Project_WB\Project_WB\Project_WBContent\gui\grey\map.txt").ReadToEnd(); //Path.Combine(Environment.CurrentDirectory, "\\Project_WBContent\\gui\\grey\\map")).ReadToEnd();

			SpriteFont font = ScreenManager.Game.Content.Load<SpriteFont>("fonts/smallSegoeUiMono");
			gui = new Gui(ScreenManager.Game, new Skin(imageMap, map), new TextRenderer(font, Color.Black));

			gui.Widgets = new Widget[] {
				new Button(200, 200, "Login and stuff???"),
				new Panel(400, 400, 200, 200) {
					Children = new Widget[] {
						new TextBox(2, 100)
					}
				}
			};

			base.Activate(instancePreserved);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			gui.Update();

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			gui.Draw();
			
			base.Draw(gameTime);
		}
	}
}
