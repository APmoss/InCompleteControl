using System;
using System.Collections.Generic;
using GameStateManagement;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Microsoft.Xna.Framework;
using Nuclex.Input;
using Nuclex.UserInterface.Controls;

namespace Project_WB.Menus {
	class Register : GameScreen {
		public Register() {
			//TODO: change back to .3
			TransitionOnTime = TimeSpan.FromSeconds(1);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void Activate(bool instancePreserved) {
			SetGui();

			base.Activate(instancePreserved);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			Gui.Update(gameTime);

			if (!coveredByOtherScreen && Gui.Screen != scrn) {
				Gui.Screen = scrn;
			}

			//TODO: wither fine tune or remve this
			if (ScreenState == GameStateManagement.ScreenState.TransitionOn || ScreenState == GameStateManagement.ScreenState.TransitionOff) {
				registerWindow.Bounds.Location.X = MathHelper.SmoothStep(-600, Stcs.XRes / 2 - 300, TransitionAlpha);
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			//TODO: remove this
			//ScreenManager.FadeBackBuffer(TransitionAlpha, Color.Black);

			Gui.Draw(gameTime);
			
			base.Draw(gameTime);
		}

		#region SetGui
		Screen scrn;
		LabelControl countryLabel;
		ListControl countryList;
		LabelControl birthLabel;
		InputControl monthBox, dateBox, yearBox;
		LabelControl nameLabel;
		InputControl firstNameBox, lastNameBox;
		LabelControl emailLabel;
		InputControl emailBox, confirmEmailBox;
		LabelControl usernameLabel;
		InputControl usernameBox, confirmUsername;
		LabelControl passwordLabel;
		InputControl passwordBox, confirmPassword;
		ButtonControl cancelButton;
		WindowControl registerWindow;

		private void SetGui() {
			Gui = ScreenManager.DefaultGui;
			scrn = new Screen(Stcs.XRes, Stcs.YRes);
			Gui.Screen = scrn;

			scrn.Desktop.Bounds = new UniRectangle(0, 0, Stcs.XRes, Stcs.YRes);

			countryLabel = new LabelControl("Country of Residence");
			countryLabel.Bounds = new UniRectangle(10, 35, 580, 20);

			countryList = new ListControl();
			countryList.Bounds = new UniRectangle(10, 60, 580, 75);
			countryList.SelectionMode = ListSelectionMode.Single;
			countryList.Items.Add("United States");
			countryList.Items.Add("Not the United States");
			countryList.Items.Add("Narnia");
			countryList.Items.Add("Chernarus");

			birthLabel = new LabelControl("Date of Birth (ex.- 8 | 16 | 1995)");
			birthLabel.Bounds = new UniRectangle(10, 145, 580, 20);

			monthBox = new InputControl();
			monthBox.Text = "Month";
			monthBox.Bounds = new UniRectangle(10, 170, 55, 30);

			dateBox = new InputControl();
			dateBox.Text = "Date";
			dateBox.Bounds = new UniRectangle(75, 170, 55, 30);

			yearBox = new InputControl();
			yearBox.Text = "Year";
			yearBox.Bounds = new UniRectangle(140, 170, 75, 30);

			nameLabel = new LabelControl("Name (ex.- Byran Baker");
			nameLabel.Bounds = new UniRectangle(10, 210, 580, 20);

			firstNameBox = new InputControl();
			firstNameBox.Text = "First";
			firstNameBox.Bounds = new UniRectangle(10, 235, 285, 30);

			lastNameBox = new InputControl();
			lastNameBox.Text = "Last";
			lastNameBox.Bounds = new UniRectangle(305, 235, 285, 30);

			emailLabel = new LabelControl("E-Mail Address");
			emailLabel.Bounds = new UniRectangle(10, 275, 580, 20);

			emailBox = new InputControl();
			emailBox.Text = "E-Mail Address";
			emailBox.Bounds = new UniRectangle(10, 300, 285, 30);

			confirmEmailBox = new InputControl();
			confirmEmailBox.Text = "Confirm E-Mail Address";
			confirmEmailBox.Bounds = new UniRectangle(305, 300, 285, 30);

			usernameLabel = new LabelControl("Username (must be 2-16 characters)");
			usernameLabel.Bounds = new UniRectangle(10, 340, 580, 20);

			usernameBox = new InputControl();
			usernameBox.Text = "BAKERNET Username";
			usernameBox.Bounds =  new UniRectangle(10, 365, 285, 30);

			confirmUsername = new InputControl();
			confirmUsername.Text = "Confirm Username";
			confirmUsername.Bounds = new UniRectangle(305, 365, 285, 30);

			passwordLabel = new LabelControl("Password (must be at least 6 characters & contain at least 1 number");
			passwordLabel.Bounds = new UniRectangle(10, 405, 580, 20);

			passwordBox = new InputControl();
			passwordBox.Text = "Password";
			passwordBox.Bounds = new UniRectangle(10, 430, 285, 30);

			confirmPassword = new InputControl();
			confirmPassword.Text = ""

			cancelButton = new ButtonControl();
			cancelButton.Text = "Cancel";
			cancelButton.Bounds = new UniRectangle(10, 400, 250, 35);
			cancelButton.Pressed += delegate {
				ExitScreen();
			};

			registerWindow = new WindowControl();
			registerWindow.Title = "Register a new BAKERNET Account";
			registerWindow.Bounds = new UniRectangle(Stcs.XRes / 2 - 300, Stcs.YRes / 2 - 300, 600, 600);
			registerWindow.Children.Add(countryLabel);
			registerWindow.Children.Add(countryList);
			registerWindow.Children.Add(birthLabel);
			registerWindow.Children.Add(monthBox);
			registerWindow.Children.Add(dateBox);
			registerWindow.Children.Add(yearBox);
			registerWindow.Children.Add(nameLabel);
			registerWindow.Children.Add(firstNameBox);
			registerWindow.Children.Add(lastNameBox);
			registerWindow.Children.Add(emailLabel);
			registerWindow.Children.Add(emailBox);
			registerWindow.Children.Add(confirmEmailBox);
			registerWindow.Children.Add(usernameLabel);
			registerWindow.Children.Add(usernameBox);
			registerWindow.Children.Add(confirmUsername);
			registerWindow.Children.Add(cancelButton);

			scrn.Desktop.Children.Add(registerWindow);
		}
		#endregion
	}
}
