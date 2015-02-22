using System;
using System.Collections.Generic;
using System.IO;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Project_WB.Framework.Gui;
using Project_WB.Framework.Gui.Controls;
using Project_WB.Gameplay;
using Project_WB.Framework.IO;


namespace Project_WB.Menus {
	/// <summary>
	/// This is the screen seen when signing in, and it handles all
	/// code associated with signing in.
	/// </summary>
	class SignIn : GameScreen {
		#region Fields
		// A gui manager for all gui elements
		GuiManager gui;

		List<Vector2> points1 = new List<Vector2>();
		List<Vector2> points2 = new List<Vector2>();
		List<Vector2> points3 = new List<Vector2>();
		Vector2 titlePos;
		Vector2 baseTitlePos = Vector2.Zero;
		Random r = new Random();

		Texture2D back;
		#endregion

		#region Initialization
		public SignIn() {
			TransitionOnTime = TimeSpan.FromSeconds(2);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void Activate(bool instancePreserved) {
			SetGui();

			var font = ScreenManager.FontLibrary.HighTowerText;
			baseTitlePos = new Vector2(Stcs.XRes / 2 - font.MeasureString("INCOMPLETE CONTROL").X / 2, Stcs.YRes / 2 - font.MeasureString("I").Y / 2 + 10);

			for (int i = 0; i < Stcs.XRes; i++) {
				points1.Add(new Vector2(i, Stcs.YRes * 1 / 4));
				points2.Add(new Vector2(i, Stcs.YRes * 2 / 4));
				points3.Add(new Vector2(i, Stcs.YRes * 3 / 4));
			}

			back = ScreenManager.Game.Content.Load<Texture2D>("textures/back");

			base.Activate(instancePreserved);
		}
		#endregion

		#region Overridden Methods
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			gui.Update(gameTime);

			for (int i = 0; i < points1.Count; i++) {
				var newPos = points1[i];
				newPos.Y = (Stcs.YRes * 1 / 4) + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds + newPos.X) * (float)(40 * (Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1));
				points1[i] = newPos;

				newPos = points2[i];
				newPos.Y = (Stcs.YRes * 2 / 4) + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds + newPos.X) * (float)(50 * (Math.Cos(gameTime.TotalGameTime.TotalSeconds) + 1.5));
				points2[i] = newPos;

				newPos = points3[i];
				newPos.Y = (Stcs.YRes * 3 / 4) + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds + newPos.X) * (float)(40 * (Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1));
				points3[i] = newPos;
			}
			
			titlePos.X = r.Next((int)baseTitlePos.X - 1, (int)baseTitlePos.X + 2);
			titlePos.Y = r.Next((int)baseTitlePos.Y - 1, (int)baseTitlePos.Y + 2);

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			gui.UpdateInteraction(input);
			
			base.HandleInput(gameTime, input);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(back, new Rectangle(0, 0, Stcs.XRes, Stcs.YRes), Color.White * TransitionAlpha);

			for (int i = 0; i < points1.Count; i++) {
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points1[i].X, (int)points1[i].Y, 2, 2), Color.PapayaWhip * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points2[i].X, (int)points2[i].Y, 2, 2), Color.SkyBlue * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points3[i].X, (int)points3[i].Y, 2, 2), Color.Blue * TransitionAlpha);
			}

			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "INCOMPLETE CONTROL", titlePos - Vector2.One, Color.LightCyan * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "INCOMPLETE CONTROL", titlePos + Vector2.One, Color.Cyan * TransitionAlpha);

			gui.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();
			
			base.Draw(gameTime);
		}
		#endregion

		#region SetGui
		Label versionLabel;

		Label languageLabel;
		Button englishButton;
		Button spanishButton;
		Button frenchButton;
		Panel languagePanel;

		Label usernameLabel;
		InputBox usernameBox;
		Label passwordLabel;
		InputBox passwordBox;
		Button loginButton;
		Button registerButton;
		Button offlineButton;
		Panel loginPanel;

		Button creditsButton;
		Button optionsButton;
		Button quitButton;
		Panel otherPanel;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);

			versionLabel = new Label(35, Stcs.YRes - 40, "Version: " + Stcs.InternalVersion.ToString());

			languageLabel = new Label(10, 10, Strings.SwitchLanguage + ":");
			languageLabel.Bounds.Width = 200;

			englishButton = new Button(10, 60, 200, string.Format("English ({0})", Strings.English));
			englishButton.LeftClicked += delegate {
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
				ResetText();
			};
			spanishButton = new Button(10, 110, 200, string.Format("Spanish ({0})", Strings.Spanish));
			spanishButton.LeftClicked += delegate {
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es-ES");
				ResetText();
			};
			frenchButton = new Button(10, 160, 200, string.Format("French ({0})", Strings.French));
			frenchButton.LeftClicked += delegate {
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
				ResetText();
			};

			languagePanel = new Panel(Stcs.XRes - 230, Stcs.YRes - 250, 220, 210);
			languagePanel.AddChild(languageLabel);
			languagePanel.AddChild(englishButton);
			languagePanel.AddChild(spanishButton);
			languagePanel.AddChild(frenchButton);

			usernameLabel = new Label(10, 10, Strings.Username);

			usernameBox = new InputBox(120, 10, 220, 35, false, "Enter a username", ScreenManager);
			
			passwordLabel = new Label(10, 60, Strings.Password);

			passwordBox = new InputBox(120, 60, 220, 35, true, "Enter a password", ScreenManager);

			loginButton = new Button(10, 110, 160, Strings.SignIn);
			loginButton.LeftClicked += delegate {
				bool success = false;

				if (IOManager.LogIn(usernameBox.Text, passwordBox.internalText, out success)) {
					if (success) {
						ExitScreen();
						ScreenManager.AddScreen(new MainMenu(), null);
					}
					else {
						ScreenManager.AddScreen(new MessageBox("Bad Login",
												string.Format("Could not log in under user \"{0}\" and the password entered.", usernameBox.Text)), null);
					}
				}
				else {
					ScreenManager.AddScreen(new MessageBox("Bad Connection", "Could not access the database. Please use offline mode."), null);
				}
			};

			registerButton = new Button(180, 110, 160, Strings.Register);
			registerButton.LeftClicked += delegate {
				ScreenManager.AddScreen(new Register(), null);
			};

			offlineButton = new Button(10, 170, 330, "Offline Login");
			offlineButton.Tint = new Color(50, 30, 30);
			offlineButton.LeftClicked += delegate {
				Session.OfflineMode = true;
				Session.LoggedIn = true;
				Session.LoginTime = DateTime.Now;

				ExitScreen();
				ScreenManager.AddScreen(new MainMenu(), null);
			};

			loginPanel = new Panel(Stcs.XRes / 2 - 175, Stcs.YRes - 235, 350, 225);
			loginPanel.AddChild(usernameLabel);
			loginPanel.AddChild(usernameBox);
			loginPanel.AddChild(passwordLabel);
			loginPanel.AddChild(passwordBox);
			loginPanel.AddChild(loginButton);
			loginPanel.AddChild(registerButton);
			loginPanel.AddChild(offlineButton);

			creditsButton = new Button(10, 10, 180, "Credits");
			creditsButton.LeftClicked += delegate {
				ScreenManager.AddScreen(new Credits(), null);
			};

			optionsButton = new Button(10, 60, 180, "Options");
			optionsButton.LeftClicked += delegate {
				ScreenManager.AddScreen(new Options(), null);
			};

			quitButton = new Button(10, 110, 180, "Quit Game");
			quitButton.LeftClicked += delegate {
				ScreenManager.Game.Exit();
			};

			otherPanel = new Panel(30, Stcs.YRes - 205, 200, 160);
			otherPanel.AddChild(creditsButton);
			otherPanel.AddChild(optionsButton);
			otherPanel.AddChild(quitButton);
			
			gui.AddControl(versionLabel);
			gui.AddControl(languagePanel);
			gui.AddControl(loginPanel);
			gui.AddControl(otherPanel);
		}
		#endregion

		#region Methods
		void ResetText() {
			languageLabel.Text = Strings.SwitchLanguage + ":";

			englishButton.Text = string.Format("English ({0})", Strings.English);
			spanishButton.Text = string.Format("Spanish ({0})", Strings.Spanish);
			frenchButton.Text = string.Format("French ({0})", Strings.French);

			usernameLabel.Text = Strings.Username;

			passwordLabel.Text = Strings.Password;

			loginButton.Text = Strings.SignIn;

			registerButton.Text = Strings.Register;
		}
		#endregion
	}
}
