using System;
using System.Collections.Generic;
using System.IO;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Content;
using Ruminate.GUI.Framework;

namespace Project_WB.Menus {
	/// <summary>
	/// This is the screen seen when signing in, and it handles all
	/// code associated with signing in.
	/// </summary>
	class SignIn : GameScreen {
		#region Fields
		TextBox usernameBox = new TextBox(2, 15);
		TextBox passwordBox = new TextBox(2, 15);
		#endregion


		public SignIn() {
			TransitionOnTime = TimeSpan.FromSeconds(.3);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void Activate(bool instancePreserved) {
			Gui = ScreenManager.DefaultGui;
			
			Gui.AddWidget(new Panel(Stcs.XRes / 2 - 125, Stcs.YRes - 600, 250, 550) {
				Children = new Widget[] { 
					new Label(2, 2, "Username:"),
					new Panel(2, 32, 220, 35) {
						Children = new Widget[] {
							usernameBox
						}
					},
					new Label(2, 82, "Password:"),
					new Panel(2, 112, 220, 35) {
						Children = new Widget[] {
							passwordBox
						}
					}
				}
			});

			base.Activate(instancePreserved);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			Gui.Update();

			DebugOverlay.DebugText.Append(usernameBox.IsFocused).AppendLine();

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			Gui.Draw();
			
			base.Draw(gameTime);
		}
	}
}
