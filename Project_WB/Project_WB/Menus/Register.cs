using System;
using System.Collections.Generic;
using GameStateManagement;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Microsoft.Xna.Framework;
using Nuclex.Input;
using Nuclex.UserInterface.Controls;
using System.Text.RegularExpressions;

namespace Project_WB.Menus {
	class Register : GameScreen {
		#region Fields
		GuiManager gui;
		#endregion

		public Register() {
			//TODO: change back to .3
			TransitionOnTime = TimeSpan.FromSeconds(1);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		#region Overridden Methods
		public override void Activate(bool instancePreserved) {
			SetGui();

			base.Activate(instancePreserved);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			gui.Update(gameTime);

			//TODO: wither fine tune or remve this
			if (ScreenState == GameStateManagement.ScreenState.TransitionOn || ScreenState == GameStateManagement.ScreenState.TransitionOff) {
				registerWindow.Bounds.Location.X = MathHelper.SmoothStep(-600, Stcs.XRes / 2 - 300, TransitionAlpha);
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			gui.Draw(gameTime);

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, "*", 
												new Vector2(warningStar.Bounds.ToOffset(600, 600).Left + registerWindow.Bounds.Location.X.Offset,
															warningStar.Bounds.ToOffset(600, 600).Top + registerWindow.Bounds.Location.Y.Offset),
												Color.Red, 0, new Vector2(25, 15), .5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

			if (!string.IsNullOrEmpty(warningLabel.Text)) {
				ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, "*",
												new Vector2(warningLabel.Bounds.ToOffset(600, 600).Left + registerWindow.Bounds.Location.X.Offset,
															warningLabel.Bounds.ToOffset(600, 600).Top + registerWindow.Bounds.Location.Y.Offset),
												Color.Red, 0, new Vector2(25, 15), .5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
			}

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
		#endregion

		#region Verification
		protected void VerifyAndCreate() {
			string message = string.Empty;

			if(VerifyFields(out message)) {
				//TODO: databases
				//Good, now submit it!
			}
			else {
				warningLabel.Text = message;
			}
		}

		protected bool VerifyFields(out string message) {
			int result = 0;
			message = string.Empty;

			// No country selected
			if (countryList.SelectedItems.Count < 1) {
				message = "Please select a country.";
				warningStar.Bounds = countryList.Bounds;
				return false;
			}
			// Bad month format
			if (!int.TryParse(monthBox.Text, out result)) {
				message = "Incorrect formatting in month of birth.";
				warningStar.Bounds = monthBox.Bounds;
				return false;
			}
			else {
				// Bad month number
				if (result < 1 || result > 12) {
					message = "Incorrect month input. Please use number 1-12.";
					warningStar.Bounds = monthBox.Bounds;
					return false;
				}
			}
			// Bad date format
			if (!int.TryParse(dateBox.Text, out result)) {
				message = "Incorrect formatting in date of birth.";
				warningStar.Bounds = dateBox.Bounds;
				return false;
			}
			else {
				// Bad date number
				if (result < 1 || result > 31) {
					message = "Incorrect date input. Please use number 1-31.";
					warningStar.Bounds = dateBox.Bounds;
					return false;
				}
			}
			// Bad year format
			if (!int.TryParse(yearBox.Text, out result)) {
				message = "Incorrect formatting in year of birth.";
				warningStar.Bounds = yearBox.Bounds;
				return false;
			}
			else {
				// Bad year number
				if (result < 1990 || result > 2012) {
					message = "Incorrect year input. Please use number 1900-2012.";
					warningStar.Bounds = yearBox.Bounds;
					return false;
				}
			}
			// Date does not exist
			try {
				DateTime validate = new DateTime(int.Parse(yearBox.Text), int.Parse(monthBox.Text), int.Parse(dateBox.Text));
			}
			catch {
				message = "Date entered does not exist. (Ex.- February 31)";
				warningStar.Bounds = dateBox.Bounds;
				return false;
			}
			// Bad email address format
			if (!validateEmail(emailBox.Text)) {
				message = "Invalid email address in email address box.";
				warningStar.Bounds = emailBox.Bounds;
				return false;
			}
			// Emails do not match
			if (emailBox.Text != confirmEmail.Text) {
				message = "Email addresses do not match.";
				warningStar.Bounds = confirmEmail.Bounds;
				return false;
			}
			// Username is too short/long
			if (usernameBox.Text.Length < 6 || usernameBox.Text.Length > 16) {
				message = "Username is too short/long.";
				warningStar.Bounds = usernameBox.Bounds;
				return false;
			}
			// Username has bad characters
			if (!validateUsername(usernameBox.Text)) {
				message = "Username contains invalid characters.";
				warningStar.Bounds = usernameBox.Bounds;
				return false;
			}
			// Usernames do not match
			if (usernameBox.Text != confirmUsername.Text) {
				message = "Usernames do not match.";
				warningStar.Bounds = confirmUsername.Bounds;
				return false;
			}
			// Password is too short/long
			if (passwordBox.GetInternalText().Length < 6 || passwordBox.GetInternalText().Length > 16) {
				message = "Password is too short/long.";
				warningStar.Bounds = passwordBox.Bounds;
				return false;
			}
			// Password is in bad format (not one capital, lowercase, number)
			if (!validatePassword(passwordBox.GetInternalText())) {
				message = "Password must contain at least one capital, one lowercase, and one number.";
				warningStar.Bounds = passwordBox.Bounds;
				return false;
			}
			// Passwords do not match
			if (passwordBox.GetInternalText() != confirmPassword.GetInternalText()) {
				message = "Passwords do not match.";
				warningStar.Bounds = confirmPassword.Bounds;
				return false;
			}
			
			// Aaaand... It's good!
			message = "All good!";
			warningLabel.Text = message;
			return true;
		}

		protected bool validateEmail(string email) {
			try {
				var address = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch {
				return false;
			}
		}

		protected bool validateUsername(string username) {
			Regex regex = new Regex(@"^[a-zA-Z0-9_.-]*$");

			if (regex.IsMatch(username)) {
				return true;
			}
			return false;
		}

		protected bool validatePassword(string password) {
			Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,16}$");

			if (regex.IsMatch(password)) {
				return true;
			}
			return false;
		}
		#endregion

		#region SetGui
		Screen scrn;
		LabelControl countryLabel;
		ListControl countryList;
		LabelControl birthLabel;
		InputControl monthBox, dateBox, yearBox;
		LabelControl nameLabel;
		InputControl firstNameBox, lastNameBox;
		LabelControl emailLabel;
		InputControl emailBox, confirmEmail;
		LabelControl usernameLabel;
		InputControl usernameBox, confirmUsername;
		LabelControl passwordLabel;
		PasswordInputControl passwordBox, confirmPassword;
		ButtonControl confirmButton, cancelButton;
		LabelControl disclaimer;
		LabelControl warningLabel, warningStar;
		WindowControl registerWindow;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.GraphicsDeviceManager, ScreenManager.NuclexInputManager);
			gui.Visualizer = Nuclex.UserInterface.Visuals.Flat.FlatGuiVisualizer.FromFile(ScreenManager.Game.Services, "content/gui/grey/Suave.skin.xml");
			gui.Initialize();

			scrn = new Screen(Stcs.XRes, Stcs.YRes);
			gui.Screen = scrn;
			
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
			countryList.Items.Add("Lingor Island");
			countryList.Items.Add("Fallujah");
			countryList.Items.Add("Takistan");
			countryList.Items.Add("Utes");
			countryList.Items.Add("Panthera");
			
			birthLabel = new LabelControl(Strings.DateOfBirth + " (ex.- 8 | 16 | 1995)");
			birthLabel.Bounds = new UniRectangle(10, 145, 580, 20);

			monthBox = new InputControl();
			monthBox.Text = Strings.Month;
			monthBox.Bounds = new UniRectangle(10, 170, 55, 30);

			dateBox = new InputControl();
			dateBox.Text = Strings.Date;
			dateBox.Bounds = new UniRectangle(75, 170, 55, 30);

			yearBox = new InputControl();
			yearBox.Text = Strings.Year;
			yearBox.Bounds = new UniRectangle(140, 170, 75, 30);

			nameLabel = new LabelControl(Strings.Name + " (ex.- Byran Baker)");
			nameLabel.Bounds = new UniRectangle(10, 210, 580, 20);

			firstNameBox = new InputControl();
			firstNameBox.Text = Strings.FirstName;
			firstNameBox.Bounds = new UniRectangle(10, 235, 285, 30);

			lastNameBox = new InputControl();
			lastNameBox.Text = Strings.LastName;
			lastNameBox.Bounds = new UniRectangle(305, 235, 285, 30);

			emailLabel = new LabelControl("E-Mail Address");
			emailLabel.Bounds = new UniRectangle(10, 275, 580, 20);

			emailBox = new InputControl();
			emailBox.Text = "E-Mail Address";
			emailBox.Bounds = new UniRectangle(10, 300, 285, 30);

			confirmEmail = new InputControl();
			confirmEmail.Text = "Confirm E-Mail Address";
			confirmEmail.Bounds = new UniRectangle(305, 300, 285, 30);

			usernameLabel = new LabelControl(Strings.Username + " (must be 6-16 characters long)");
			usernameLabel.Bounds = new UniRectangle(10, 340, 580, 20);

			usernameBox = new InputControl();
			usernameBox.Text = "BAKERNET Username";
			usernameBox.Bounds =  new UniRectangle(10, 365, 285, 30);

			confirmUsername = new InputControl();
			confirmUsername.Text = "Confirm Username";
			confirmUsername.Bounds = new UniRectangle(305, 365, 285, 30);

			passwordLabel = new LabelControl(Strings.Password + " (6-16 characters, at least one capital, one lowercase, and one number");
			passwordLabel.Bounds = new UniRectangle(10, 405, 580, 20);

			passwordBox = new PasswordInputControl('*');
			passwordBox.Bounds = new UniRectangle(10, 430, 285, 30);

			confirmPassword = new PasswordInputControl('*');
			confirmPassword.Bounds = new UniRectangle(305, 430, 285, 30);

			confirmButton = new ButtonControl();
			confirmButton.Text = "Confirm & Create";
			confirmButton.Bounds = new UniRectangle(10, 480, 285, 35);
			confirmButton.Pressed += delegate {
				VerifyAndCreate();
			};

			cancelButton = new ButtonControl();
			cancelButton.Text = "Cancel";
			cancelButton.Bounds = new UniRectangle(305, 480, 285, 35);
			cancelButton.Pressed += delegate {
				ExitScreen();
			};

			disclaimer = new LabelControl("*By creating a free BAKERNET account, you agree to give us all your money.");
			disclaimer.Bounds = new UniRectangle(10, 540, 580, 20);

			warningLabel = new LabelControl(string.Empty);
			warningLabel.Bounds = new UniRectangle(10, 570, 580, 20);

			warningStar = new LabelControl(string.Empty);
			warningStar.Bounds = disclaimer.Bounds;

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
			registerWindow.Children.Add(confirmEmail);
			registerWindow.Children.Add(usernameLabel);
			registerWindow.Children.Add(usernameBox);
			registerWindow.Children.Add(confirmUsername);
			registerWindow.Children.Add(passwordLabel);
			registerWindow.Children.Add(passwordBox);
			registerWindow.Children.Add(confirmPassword);
			registerWindow.Children.Add(confirmButton);
			registerWindow.Children.Add(cancelButton);
			registerWindow.Children.Add(disclaimer);
			registerWindow.Children.Add(warningLabel);
			registerWindow.Children.Add(warningStar);

			scrn.Desktop.Children.Add(registerWindow);
		}
		#endregion
	}
}
