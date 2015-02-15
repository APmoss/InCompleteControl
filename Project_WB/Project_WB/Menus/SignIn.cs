using System;
using System.Collections.Generic;
using System.IO;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;


namespace Project_WB.Menus {
	/// <summary>
	/// This is the screen seen when signing in, and it handles all
	/// code associated with signing in.
	/// </summary>
	class SignIn : GameScreen {
		#region Fields
		
		#endregion

		#region Initialization
		public SignIn() {
			TransitionOnTime = TimeSpan.FromSeconds(.3);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void Activate(bool instancePreserved) {
			SetGui();

			base.Activate(instancePreserved);
		}
		#endregion

		#region Overridden Methodsa
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			Gui.Update(gameTime);

			if (!coveredByOtherScreen && Gui.Screen != scrn) {
				Gui.Screen = scrn;
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			Gui.Draw(gameTime);
			
			base.Draw(gameTime);
		}
		#endregion

		#region SetGui
		Screen scrn;
		LabelControl versionLabel;
		LabelControl usernameLabel;
		InputControl usernameBox;
		LabelControl passwordLabel;
		PasswordInputControl passwordBox;
		ButtonControl loginButton;
		ButtonControl registerButton;
		WindowControl loginWindow;

		private void SetGui() {
			Gui = ScreenManager.DefaultGui;
			scrn = new Screen(Stcs.XRes, Stcs.YRes);
			Gui.Screen = scrn;
			
			scrn.Desktop.Bounds = new UniRectangle(0, 0, Stcs.XRes, Stcs.YRes);

			versionLabel = new LabelControl(Stcs.InternalVersion.ToString());
			versionLabel.Bounds = new UniRectangle(10, Stcs.YRes - 20, 300, 20);

			System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			//System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es-ES");
			//System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");

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

			registerButton = new ButtonControl();
			registerButton.Text = Strings.Register;
			registerButton.Bounds = new UniRectangle(180, 170, 160, 35);
			registerButton.Pressed += delegate {
				ScreenManager.AddScreen(new Register(), null);
			};

			loginWindow = new WindowControl();
			loginWindow.Bounds = new UniRectangle(Stcs.XRes / 2 - 175, Stcs.YRes - 350, 350, 300);
			loginWindow.Title = "BAKERNET Account Login";
			loginWindow.Children.Add(usernameLabel);
			loginWindow.Children.Add(usernameBox);
			loginWindow.Children.Add(passwordLabel);
			loginWindow.Children.Add(passwordBox);
			loginWindow.Children.Add(loginButton);
			loginWindow.Children.Add(registerButton);
			
			scrn.Desktop.Children.Add(versionLabel);
			scrn.Desktop.Children.Add(loginWindow);
		}
		#endregion
	}
}
