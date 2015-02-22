using System;
using System.Collections.Generic;
using System.IO;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Microsoft.Xna.Framework.Audio;


namespace Project_WB.Menus {
	/// <summary>
	/// This is the screen seen when signing in, and it handles all
	/// code associated with signing in.
	/// </summary>
	class SignIn : GameScreen {
		#region Fields
		//TODO: finalize background
		List<Vector2> points1 = new List<Vector2>();
		List<Vector2> points2 = new List<Vector2>();
		List<Vector2> points3 = new List<Vector2>();
		Vector2 titlePos;
		Vector2 baseTitlePos = new Vector2(230, 333);
		Random r = new Random();
		SoundEffectInstance song;
		#endregion

		#region Initialization
		public SignIn() {
			TransitionOnTime = TimeSpan.FromSeconds(2);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void Activate(bool instancePreserved) {
			SetGui();

			//TODO:remove and stuff
			for (int i = 0; i < Stcs.XRes; i++) {
				points1.Add(new Vector2(i, Stcs.YRes * 1 / 4));
				points2.Add(new Vector2(i, Stcs.YRes * 2 / 4));
				points3.Add(new Vector2(i, Stcs.YRes * 3 / 4));
			}

			song = ScreenManager.Game.Content.Load<SoundEffect>("audio/effects/flicker").CreateInstance();
			song.Play();

			base.Activate(instancePreserved);
		}
		#endregion

		#region Overridden Methods
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			Gui.Update(gameTime);

			if (!coveredByOtherScreen && Gui.Screen != scrn) {
				Gui.Screen = scrn;
			}

			//TODO: finalize background
			song.Volume = TransitionAlpha;
			
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

		public override void Draw(GameTime gameTime) {
			//TODO: remove and stuff
			ScreenManager.SpriteBatch.Begin();

			for (int i = 0; i < points1.Count; i++) {
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points1[i].X, (int)points1[i].Y, 2, 2), Color.Red * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points2[i].X, (int)points2[i].Y, 2, 2), Color.Green * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points3[i].X, (int)points3[i].Y, 2, 2), Color.Blue * TransitionAlpha);
			}

			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "RESONANT FREQUENCY", titlePos - Vector2.One, new Color(60, 0, 0) * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "RESONANT FREQUENCY", titlePos + Vector2.One, Color.Maroon * TransitionAlpha);

			ScreenManager.SpriteBatch.End();
			
			Gui.Draw(gameTime);

			base.Draw(gameTime);
		}
		#endregion

		#region SetGui
		Screen scrn;
		LabelControl versionLabel;
		LabelControl languageLabel;
		ListControl languageList;
		WindowControl languageWindow;

		LabelControl usernameLabel;
		InputControl usernameBox;
		LabelControl passwordLabel;
		PasswordInputControl passwordBox;
		ButtonControl loginButton;
		ButtonControl registerButton;
		WindowControl loginWindow;

		ButtonControl creditsButton;
		ButtonControl optionsButton;
		ButtonControl quitButton;
		WindowControl otherWindow;

		private void SetGui() {
			Gui = ScreenManager.DefaultGui;
			scrn = new Screen(Stcs.XRes, Stcs.YRes);
			Gui.Screen = scrn;
			
			scrn.Desktop.Bounds = new UniRectangle(0, 0, Stcs.XRes, Stcs.YRes);

			versionLabel = new LabelControl(Stcs.InternalVersion.ToString());
			versionLabel.Bounds = new UniRectangle(10, Stcs.YRes - 20, 300, 20);

			languageLabel = new LabelControl(Strings.SwitchLanguage + ":");
			languageLabel.Bounds = new UniRectangle(10, 35, 180, 20);

			languageList = new ListControl();
			languageList.Bounds = new UniRectangle(10, 60, 180, 100);
			languageList.SelectionMode = ListSelectionMode.Single;
			languageList.Items.Add(Strings.English + " (English)");
			languageList.Items.Add(Strings.Spanish + string.Format(" (Espa{0}ol)", (char)164));
			languageList.Items.Add(Strings.French + string.Format(" (Fran{0}ais)", (char)135));
			languageList.Items.Add("Murrikan");
			languageList.SelectionChanged += delegate {
				string culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

				if (languageList.SelectedItems.Count == 1) {
					if (languageList.SelectedItems[0] == 0) {
						System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
					}
					else if (languageList.SelectedItems[0] == 1) {
						System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es-ES");
					}
					else if (languageList.SelectedItems[0] == 2) {
						System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
					}

					ResetText();
				}
			};

			languageWindow = new WindowControl();
			languageWindow.Bounds = new UniRectangle(Stcs.XRes - 230, Stcs.YRes - 200, 200, 175);
			languageWindow.Title = "Language Options";
			languageWindow.Children.Add(languageLabel);
			languageWindow.Children.Add(languageList);

			usernameLabel = new LabelControl(Strings.Username);
			usernameLabel.Bounds = new UniRectangle(10, 35, 330, 20);

			usernameBox = new InputControl();
			usernameBox.Bounds = new UniRectangle(10, 60, 330, 30);
			
			passwordLabel = new LabelControl(Strings.Password);
			passwordLabel.Bounds = new UniRectangle(10, 100, 330, 20);
			
			passwordBox = new PasswordInputControl('*');
			passwordBox.Bounds = new UniRectangle(10, 125, 330, 30);

			loginButton = new ButtonControl();
			loginButton.Text = Strings.SignIn;
			loginButton.Bounds = new UniRectangle(10, 170, 160, 35);
			loginButton.Pressed += delegate {
				ScreenManager.AddScreen(new Gameplay.TestThing(), null);
				song.Stop();
			};

			registerButton = new ButtonControl();
			registerButton.Text = Strings.Register;
			registerButton.Bounds = new UniRectangle(180, 170, 160, 35);
			registerButton.Pressed += delegate {
				ScreenManager.AddScreen(new Register(), null);
			};

			loginWindow = new WindowControl();
			loginWindow.Bounds = new UniRectangle(Stcs.XRes / 2 - 175, Stcs.YRes - 235, 350, 225);
			loginWindow.Title = "BAKERNET Account Login";
			loginWindow.Children.Add(usernameLabel);
			loginWindow.Children.Add(usernameBox);
			loginWindow.Children.Add(passwordLabel);
			loginWindow.Children.Add(passwordBox);
			loginWindow.Children.Add(loginButton);
			loginWindow.Children.Add(registerButton);

			creditsButton = new ButtonControl();
			creditsButton.Bounds = new UniRectangle(10, 35, 180, 35);
			creditsButton.Text = "Credits";

			optionsButton = new ButtonControl();
			optionsButton.Bounds = new UniRectangle(10, 80, 180, 35);
			optionsButton.Text = "Options";

			quitButton = new ButtonControl();
			quitButton.Bounds = new UniRectangle(10, 125, 180, 35);
			quitButton.Text = "Quit Game";
			quitButton.Pressed += delegate {
				ScreenManager.Game.Exit();
			};

			otherWindow = new WindowControl();
			otherWindow.Bounds = new UniRectangle(30, Stcs.YRes - 205, 200, 175);
			otherWindow.Title = "Other";
			otherWindow.Children.Add(creditsButton);
			otherWindow.Children.Add(optionsButton);
			otherWindow.Children.Add(quitButton);
			
			scrn.Desktop.Children.Add(versionLabel);
			scrn.Desktop.Children.Add(languageWindow);
			scrn.Desktop.Children.Add(loginWindow);
			scrn.Desktop.Children.Add(otherWindow);
		}
		#endregion

		#region Methods
		void ResetText() {
			languageLabel = new LabelControl(Strings.SwitchLanguage + ":");

			languageList.Items.Clear();
			languageList.Items.Add(Strings.English + " (English)");
			languageList.Items.Add(Strings.Spanish + string.Format(" (Espa{0}ol)", (char)164));
			languageList.Items.Add(Strings.French + string.Format(" (Fran{0}ais)", (char)135));
			languageList.Items.Add("MURRIKANNNN");

			usernameLabel.Text = Strings.Username;

			passwordLabel.Text = Strings.Password;

			loginButton.Text = Strings.SignIn;

			registerButton.Text = Strings.Register;
		}
		#endregion
	}
}
