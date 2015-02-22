using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Project_WB.Framework.Gui;
using Project_WB.Framework.Gui.Controls;

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
			
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			gui.UpdateInteraction(input);
			
			base.HandleInput(gameTime, input);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			gui.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.Begin();

			//ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, "*", 
			//                                    new Vector2(warningStar.Bounds.ToOffset(600, 600).Left + registerWindow.Bounds.Location.X.Offset,
			//                                                warningStar.Bounds.ToOffset(600, 600).Top + registerWindow.Bounds.Location.Y.Offset),
			//                                    Color.Red, 0, new Vector2(25, 15), .5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

			//if (!string.IsNullOrEmpty(warningLabel.Text)) {
			//    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, "*",
			//                                    new Vector2(warningLabel.Bounds.ToOffset(600, 600).Left + registerWindow.Bounds.Location.X.Offset,
			//                                                warningLabel.Bounds.ToOffset(600, 600).Top + registerWindow.Bounds.Location.Y.Offset),
			//                                    Color.Red, 0, new Vector2(25, 15), .5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
			//}

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
				//warningLabel.Text = message;
			}
		}

		protected bool VerifyFields(out string message) {
			message = "";
			return false;
			/*
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
			*/
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
		//Label countryLabel;
		//ComboBox countryComboBox;
		//Label birthLabel;
		//TextBox monthBox, dateBox, yearBox;
		//Label nameLabel;
		//TextBox firstNameBox, lastNameBox;
		//Label emailLabel;
		//TextBox emailBox, confirmEmail;
		//Label usernameLabel;
		//TextBox usernameBox, confirmUsername;
		//Label passwordLabel;
		//TextBox passwordBox, confirmPassword;
		Button confirmButton, cancelButton;
		//Label disclaimer;
		//Label warningLabel, warningStar;
		//Panel registerPanel;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.FontLibrary.Consolas);

			//countryLabel = new Label(10, 35, "Country of Residence");

			//countryComboBox = new ComboBox(10, 60, 580, "United -------States");
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("United States"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Not the United States"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Narnia"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Chernarus"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Lingor Island"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Fallujah"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Takistan"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Utes"));
			//countryComboBox.DropDownItems.Add(new ComboBox.DropDownItem("Panthera"));

			//birthLabel = new Label(10, 145, Strings.DateOfBirth + " (ex.- 8 | 16 | 1995)");

			//monthBox = new TextBox(2, 2);
			////monthBox.Text = Strings.Month;
			////monthBox.Bounds = new UniRectangle(10, 170, 55, 30);

			//dateBox = new TextBox(2, 2);
			////dateBox.Text = Strings.Date;
			////dateBox.Bounds = new UniRectangle(75, 170, 55, 30);

			//yearBox = new TextBox(2, 4);
			////yearBox.Text = Strings.Year;
			////yearBox.Bounds = new UniRectangle(140, 170, 75, 30);

			//nameLabel = new Label(10, 210, Strings.Name + " (ex.- Byran Baker)");

			//firstNameBox = new TextBox(2, 20);
			////firstNameBox.Text = Strings.FirstName;
			////firstNameBox.Bounds = new UniRectangle(10, 235, 285, 30);

			//lastNameBox = new TextBox(2, 20);
			////lastNameBox.Text = Strings.LastName;
			////lastNameBox.Bounds = new UniRectangle(305, 235, 285, 30);

			//emailLabel = new Label(10, 275, "E-Mail Address");

			//emailBox = new TextBox(2, 20);
			////emailBox.Text = "E-Mail Address";
			////emailBox.Bounds = new UniRectangle(10, 300, 285, 30);

			//confirmEmail = new TextBox(2, 20);
			////confirmEmail.Text = "Confirm E-Mail Address";
			////confirmEmail.Bounds = new UniRectangle(305, 300, 285, 30);

			//usernameLabel = new Label(10, 340, Strings.Username + " (must be 6-16 characters long)");

			//usernameBox = new TextBox(2, 20);
			////usernameBox.Text = "BAKERNET Username";
			////usernameBox.Bounds =  new UniRectangle(10, 365, 285, 30);

			//confirmUsername = new TextBox(2, 20);
			////confirmUsername.Text = "Confirm Username";
			////confirmUsername.Bounds = new UniRectangle(305, 365, 285, 30);

			//passwordLabel = new Label(10, 405, Strings.Password + " (6-16 characters, at least one capital, one lowercase, and one number");

			//passwordBox = new TextBox(2, 20);
			////passwordBox = new PasswordInputControl('*');
			////passwordBox.Bounds = new UniRectangle(10, 430, 285, 30);

			//confirmPassword = new TextBox(2, 20);
			////confirmPassword = new PasswordInputControl('*');
			////confirmPassword.Bounds = new UniRectangle(305, 430, 285, 30);

			confirmButton = new Button(10, 480, 285, "Confirm & Create");
			//confirmButton.Pressed += delegate {
			//    VerifyAndCreate();
			//};

			cancelButton = new Button(305, 480, 285, "Cancel");
			//cancelButton.Pressed += delegate {
			//    ExitScreen();
			//};

			//disclaimer = new Label(10, 540, "*By creating a free BAKERNET account, you agree to give us all your money.");

			//warningLabel = new Label(10, 570, string.Empty);

			//warningStar = new Label(disclaimer.AbsoluteArea.X, disclaimer.AbsoluteArea.Y, string.Empty);

			//registerPanel = new Panel(Stcs.XRes / 2 - 300, Stcs.YRes / 2 - 300, 600, 600);
			////registerWindow.Title = "Register a new BAKERNET Account";
			//registerPanel.AddWidget(countryLabel);
			//registerPanel.AddWidget(countryComboBox);
			//registerPanel.AddWidget(birthLabel);
			//registerPanel.AddWidget(monthBox);
			//registerPanel.AddWidget(dateBox);
			//registerPanel.AddWidget(yearBox);
			//registerPanel.AddWidget(nameLabel);
			//registerPanel.AddWidget(firstNameBox);
			//registerPanel.AddWidget(lastNameBox);
			//registerPanel.AddWidget(emailLabel);
			//registerPanel.AddWidget(emailBox);
			//registerPanel.AddWidget(confirmEmail);
			//registerPanel.AddWidget(usernameLabel);
			//registerPanel.AddWidget(usernameBox);
			//registerPanel.AddWidget(confirmUsername);
			//registerPanel.AddWidget(passwordLabel);
			//registerPanel.AddWidget(passwordBox);
			//registerPanel.AddWidget(confirmPassword);
			//registerPanel.AddWidget(confirmButton);
			//registerPanel.AddWidget(cancelButton);
			//registerPanel.AddWidget(disclaimer);
			//registerPanel.AddWidget(warningLabel);
			//registerPanel.AddWidget(warningStar);

			//gui.AddWidget(registerPanel);

			//temp, remove once panels are implemented
			gui.AddControl(confirmButton);
			gui.AddControl(cancelButton);
		}
		#endregion
	}
}
