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


namespace Project_WB.Menus {
	/// <summary>
	/// This is the screen seen when signing in, and it handles all
	/// code associated with signing in.
	/// </summary>
	class SignIn : GameScreen {
		#region Fields
		// A gui manager for all gui elements
		GuiManager gui;
		//TODO: finalize background
		List<Vector2> points1 = new List<Vector2>();
		List<Vector2> points2 = new List<Vector2>();
		List<Vector2> points3 = new List<Vector2>();
		Vector2 titlePos;
		Vector2 baseTitlePos = new Vector2(230, 333);
		Random r = new Random();
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

			base.Activate(instancePreserved);
		}
		public override void Unload() {			
			base.Unload();
		}
		#endregion

		#region Overridden Methods
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			gui.Update(gameTime);

			//TODO: finalize background
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
			//TODO: remove and stuff
			ScreenManager.SpriteBatch.Begin();

			for (int i = 0; i < points1.Count; i++) {
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points1[i].X, (int)points1[i].Y, 2, 2), Color.Red * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points2[i].X, (int)points2[i].Y, 2, 2), Color.Green * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle((int)points3[i].X, (int)points3[i].Y, 2, 2), Color.Blue * TransitionAlpha);
			}

			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "InComplete Control", titlePos - Vector2.One, new Color(60, 0, 0) * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "InComplete Control", titlePos + Vector2.One, Color.Maroon * TransitionAlpha);

			gui.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();
			
			base.Draw(gameTime);
		}
		#endregion

		#region SetGui
		Label versionLabel;
		Button testButton;

		Label languageLabel;
		//ComboBox languageComboBox;
		Panel languagePanel;

		Label usernameLabel;
		//TextBox usernameBox;
		Label passwordLabel;
		//TextBox passwordBox;
		Button loginButton;
		Button registerButton;
		Panel loginPanel;

		Button creditsButton;
		Button optionsButton;
		Button quitButton;
		Panel otherPanel;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);

			versionLabel = new Label(10, Stcs.YRes - 40, Stcs.InternalVersion.ToString());

			testButton = new Button(1000, 200, 100, "TEST :D");
			testButton.LeftClicked += delegate {
				ScreenManager.AddScreen(new TestBattle(), null);
			};

			languageLabel = new Label(10, 35, Strings.SwitchLanguage + ":");

			//languageComboBox = new ComboBox(10, 60, 180, "Language");
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem(Strings.English + " (English)"));
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem(Strings.Spanish + string.Format(" (Espa{0}ol)", (char)164)));
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem(Strings.French + string.Format(" (Fran{0}ais)", (char)135)));
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Murrikan"));
			//languageList.SelectionChanged += delegate {
			//    string culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

			//    if (languageList.SelectedItems.Count == 1) {
			//        if (languageList.SelectedItems[0] == 0) {
			//            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			//        }
			//        else if (languageList.SelectedItems[0] == 1) {
			//            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es-ES");
			//        }
			//        else if (languageList.SelectedItems[0] == 2) {
			//            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
			//        }

			//        ResetText();
			//    }
			//};

			languagePanel = new Panel(Stcs.XRes - 230, Stcs.YRes - 200, 200, 175);
			languagePanel.AddChild(languageLabel);
			//languagePanel.AddWidget(languageComboBox);

			usernameLabel = new Label(10, 35, Strings.Username);

			//usernameBox = new TextBox(2, 16);
			////usernameBox.Bounds = new UniRectangle(10, 60, 330, 30);
			
			passwordLabel = new Label(10, 100, Strings.Password);
			
			//passwordBox = new TextBox(2, 16);
			//passwordBox.Bounds = new UniRectangle(10, 125, 330, 30);

			loginButton = new Button(10, 170, 160, Strings.SignIn);
			loginButton.LeftClicked += delegate {
				ScreenManager.AddScreen(new Gameplay.TestThing(), null);
			};

			registerButton = new Button(180, 170, 160, Strings.Register);
			//registerButton.Pressed += delegate {
			//    ScreenManager.AddScreen(new Register(), null);
			//};

			loginPanel = new Panel(Stcs.XRes / 2 - 175, Stcs.YRes - 235, 350, 225);
			////loginWindow.Title = "BAKERNET Account Login";
			loginPanel.AddChild(usernameLabel);
			//loginPanel.AddWidget(usernameBox);
			loginPanel.AddChild(passwordLabel);
			//loginPanel.AddWidget(passwordBox);
			loginPanel.AddChild(loginButton);
			loginPanel.AddChild(registerButton);

			creditsButton = new Button(10, 35, 180, "Credits");

			optionsButton = new Button(10, 80, 180, "Options");
			optionsButton.LeftClicked += delegate {
				ScreenManager.AddScreen(new Options(), null);
			};

			quitButton = new Button(10, 125, 180, "Quit Game");
			quitButton.LeftClicked += delegate {
				ScreenManager.Game.Exit();
			};

			otherPanel = new Panel(30, Stcs.YRes - 205, 200, 175);
			otherPanel.AddChild(creditsButton);
			otherPanel.AddChild(optionsButton);
			otherPanel.AddChild(quitButton);
			
			gui.AddControl(versionLabel);
			gui.AddControl(testButton);
			gui.AddControl(languagePanel);
			gui.AddControl(loginPanel);
			gui.AddControl(otherPanel);
			
			//TEMP REMOVE
			gui.AddControl(new Slider(0, 0, 1280, 75));
			gui.AddControl(new DialogBox(0, 100, 800, 300, "This is a test where I write lots of text in order to fill in a lot of empty space " +
															"and stuff am,dadslkaf asdf as df df adsfdsafas asdf asdf asd dd as dfasdf asdf sadf " +
															"asdf asdff f asd fas d d d asdfasdfasdf asdf asdfasdfasdfasdfasdfa sdfasdfasdfasdfadfasdfasdfsadfasdfasdfasdfasdfasdfasdf asdfasdfasdfasdf" +
															"as df asdf sad f ds f sad fasfdasdfasdfasdfasdfsadfasdfasdfasd fasdfasdfasdfasdfasdfsadfsadfsadfasdfasdf sadfasdfsadf"));
		}
		#endregion

		#region Methods
		void ResetText() {
			//languageLabel.Text = Strings.SwitchLanguage + ":";

			//languageComboBox.DropDownItems.Clear();
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem(Strings.English + " (English)"));
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem(Strings.Spanish + string.Format(" (Espa{0}ol)", (char)164)));
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem(Strings.French + string.Format(" (Fran{0}ais)", (char)135)));
			//languageComboBox.DropDownItems.Add(new ComboBox.DropDownItem("MURRIKANNNN"));

			//usernameLabel.Text = Strings.Username;

			//passwordLabel.Text = Strings.Password;

			loginButton.Text = Strings.SignIn;

			registerButton.Text = Strings.Register;
		}
		#endregion
	}
}
