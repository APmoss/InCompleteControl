using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Project_WB.Framework.Gui;
using Project_WB.Framework.Gui.Controls;
using Project_WB.Framework.IO;

namespace Project_WB.Menus {
	class Register : GameScreen {
		#region Fields
		GuiManager gui;
		#endregion

		public Register() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);
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
			ScreenManager.SpriteBatch.Begin();

			gui.Draw(gameTime, ScreenManager);

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
				if (IOManager.CreateNewUser(usernameBox.Text, passwordBox.internalText, firstNameBox.Text, lastNameBox.Text, emailBox.Text)) {
					message = "New user added successfully!";
					warningLabel.Text = message;

					ScreenManager.AddScreen(new MessageBox("New User",
											string.Format("New User {0} has been added to the database successfully!", usernameBox.Text)), null);
				}
			}
			else {
				warningLabel.Text = message;
			}
		}

		protected bool VerifyFields(out string message) {
			message = "";
			//return false;
			
			int result = 0;
			message = string.Empty;

			// No country selected
			//if (countryList.SelectedItems.Count < 1) {
			//    message = "Please select a country.";
			//    warningStar.Bounds = countryList.Bounds;
			//    return false;
			//}
			// Bad month format
			if (!int.TryParse(monthBox.Text, out result)) {
				message = "Incorrect formatting in month of birth.";
				//warningStar.Bounds = monthBox.Bounds;
				return false;
			}
			else {
				// Bad month number
				if (result < 1 || result > 12) {
					message = "Incorrect month input. Please use number 1-12.";
					//warningStar.Bounds = monthBox.Bounds;
					return false;
				}
			}
			// Bad date format
			if (!int.TryParse(dateBox.Text, out result)) {
				message = "Incorrect formatting in date of birth.";
				//warningStar.Bounds = dateBox.Bounds;
				return false;
			}
			else {
				// Bad date number
				if (result < 1 || result > 31) {
					message = "Incorrect date input. Please use number 1-31.";
					//warningStar.Bounds = dateBox.Bounds;
					return false;
				}
			}
			// Bad year format
			if (!int.TryParse(yearBox.Text, out result)) {
				message = "Incorrect formatting in year of birth.";
				//warningStar.Bounds = yearBox.Bounds;
				return false;
			}
			else {
				// Bad year number
				if (result < 1990 || result > 2012) {
					message = "Incorrect year input. Please use number 1900-2012.";
					//warningStar.Bounds = yearBox.Bounds;
					return false;
				}
			}
			// Date does not exist
			try {
				DateTime validate = new DateTime(int.Parse(yearBox.Text), int.Parse(monthBox.Text), int.Parse(dateBox.Text));
			}
			catch {
				message = "Date entered does not exist. (Ex.- February 31)";
				//warningStar.Bounds = dateBox.Bounds;
				return false;
			}
			// No first name
			if (firstNameBox.Text.Length <= 0) {
				message = "First name is blank.";
			}
			// No last name
			if (lastNameBox.Text.Length <= 0) {
				message = "Last name is blank.";
			}
			// Bad email address format
			if (!validateEmail(emailBox.Text)) {
				message = "Invalid email address in email address box.";
				//warningStar.Bounds = emailBox.Bounds;
				return false;
			}
			// Emails do not match
			if (emailBox.Text != confirmEmail.Text) {
				message = "Email addresses do not match.";
				//warningStar.Bounds = confirmEmail.Bounds;
				return false;
			}
			// Username is too short/long
			if (usernameBox.Text.Length < 6 || usernameBox.Text.Length > 16) {
				message = "Username is too short/long.";
				//warningStar.Bounds = usernameBox.Bounds;
				return false;
			}
			// Username has bad characters
			if (!validateUsername(usernameBox.Text)) {
				message = "Username contains invalid characters.";
				//warningStar.Bounds = usernameBox.Bounds;
				return false;
			}
			// Usernames do not match
			if (usernameBox.Text != confirmUsername.Text) {
				message = "Usernames do not match.";
				//warningStar.Bounds = confirmUsername.Bounds;
				return false;
			}
			// Username already taken
			bool exists = false;
			if (IOManager.CheckUsername(usernameBox.Text, out exists)) {
				if(exists) {
					message = "Username already exists.";
					return false;
				}
			}
			// Cannot connect to database
			else {
				message = "Cannot connect to database. Try again later.";
				return false;
			}
			// Password is too short/long
			if (passwordBox.internalText.Length < 6 || passwordBox.internalText.Length > 16) {
				message = "Password is too short/long.";
				//warningStar.Bounds = passwordBox.Bounds;
				return false;
			}
			// Password is in bad format (not one capital, lowercase, number)
			if (!validatePassword(passwordBox.internalText)) {
				message = "Password must be 6-16 chars, 1 CAPS, 1 lower, and 1 numb3r";
				//warningStar.Bounds = passwordBox.Bounds;
				return false;
			}
			// Passwords do not match
			if (passwordBox.internalText != confirmPassword.internalText) {
				message = "Passwords do not match.";
				//warningStar.Bounds = confirmPassword.Bounds;
				return false;
			}
			
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
		Label countryLabel;
		Label countryDefault;
		Label birthLabel;
		InputBox monthBox, dateBox, yearBox;
		Label nameLabel;
		InputBox firstNameBox, lastNameBox;
		Label emailLabel;
		InputBox emailBox, confirmEmail;
		Label usernameLabel;
		InputBox usernameBox, confirmUsername;
		Label passwordLabel;
		InputBox passwordBox, confirmPassword;
		Button confirmButton, cancelButton;
		//Label disclaimer;
		Label warningLabel;//, warningStar;
		Panel registerPanel;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);
			
			countryLabel = new Label(10, 10, "Country of Residence");

			countryDefault = new Label(300, 10, "US (Default)");

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

			birthLabel = new Label(10, 60, Strings.DateOfBirth + " (ex.- 8 | 16 | 1995)");

			monthBox = new InputBox(10, 110, 55, 35, false, Strings.Month, ScreenManager);
			monthBox.Text = "0";

			dateBox = new InputBox(75, 110, 55, 35, false, Strings.Date, ScreenManager);
			dateBox.Text = "0";

			yearBox = new InputBox(140, 110, 75, 35, false, Strings.Year, ScreenManager);
			yearBox.Text = "0";

			nameLabel = new Label(10, 160, "*" + Strings.Name + " (ex.- Byran Baker)");

			firstNameBox = new InputBox(10, 210, 285, 35, false, Strings.FirstName, ScreenManager);
			firstNameBox.Text = "First Name";

			lastNameBox = new InputBox(305, 210, 285, 35, false, Strings.LastName, ScreenManager);
			lastNameBox.Text = "Last Name";

			emailLabel = new Label(10, 260, "*E-Mail Address");

			emailBox = new InputBox(10, 310, 285, 35, false, "E-Mail Address", ScreenManager);
			emailBox.Text = "E-Mail";

			confirmEmail = new InputBox(305, 310, 285, 35, false, "Confirm E-Mail", ScreenManager);
			confirmEmail.Text = "Confirm E-Mail";

			usernameLabel = new Label(10, 360, "*" + Strings.Username + " (must be 6-16 characters long)");

			usernameBox = new InputBox(10, 410, 285, 35, false, "*" + Strings.Username, ScreenManager);
			usernameBox.Text = "Username";

			confirmUsername = new InputBox(305, 410, 285, 35, false, "Confirm Username", ScreenManager);
			confirmUsername.Text = "Confirm Username";

			passwordLabel = new Label(10, 460, "*" + Strings.Password + " (6-16 chars, 1 CAPS, 1 lower, and 1 numb3r");

			passwordBox = new InputBox(10, 510, 285, 35, true, "*" + Strings.Password, ScreenManager);
			passwordBox.Text = "Password";

			confirmPassword = new InputBox(305, 510, 285, 35, true, "Confirm Password", ScreenManager);
			confirmPassword.Text = "Confirm Password";

			confirmButton = new Button(10, 560, 285, "Confirm & Create");
			confirmButton.LeftClicked+= delegate {
				VerifyAndCreate();
			};

			cancelButton = new Button(305, 560, 285, "Cancel");
			cancelButton.LeftClicked += delegate {
			    ExitScreen();
			};

			//disclaimer = new Label(10, 540, "*By creating a free BAKERNET account, you agree to give us all your money.");

			warningLabel = new Label(10, 610, "Items with * are bound to your acct, others are not.");
			warningLabel.Bounds.Width = 580;
			warningLabel.BackgroundTint = Color.Red * .2f;

			//warningStar = new Label(disclaimer.AbsoluteArea.X, disclaimer.AbsoluteArea.Y, string.Empty);

			registerPanel = new Panel(Stcs.XRes / 2 - 300, Stcs.YRes / 2 - 330, 600, 660);
			//registerPanel = new Panel(Stcs.XRes / 2 - 300, Stcs.YRes / 2 - 300, 600, 600);
			////registerWindow.Title = "Register a new BAKERNET Account";
			registerPanel.AddChild(countryLabel);
			registerPanel.AddChild(countryDefault);
			//registerPanel.AddChild(countryComboBox);
			registerPanel.AddChild(birthLabel);
			registerPanel.AddChild(monthBox);
			registerPanel.AddChild(dateBox);
			registerPanel.AddChild(yearBox);
			registerPanel.AddChild(nameLabel);
			registerPanel.AddChild(firstNameBox);
			registerPanel.AddChild(lastNameBox);
			registerPanel.AddChild(emailLabel);
			registerPanel.AddChild(emailBox);
			registerPanel.AddChild(confirmEmail);
			registerPanel.AddChild(usernameLabel);
			registerPanel.AddChild(usernameBox);
			registerPanel.AddChild(confirmUsername);
			registerPanel.AddChild(passwordLabel);
			registerPanel.AddChild(passwordBox);
			registerPanel.AddChild(confirmPassword);
			registerPanel.AddChild(confirmButton);
			registerPanel.AddChild(cancelButton);
			//registerPanel.AddChild(disclaimer);
			registerPanel.AddChild(warningLabel);
			//registerPanel.AddChild(warningStar);

			gui.AddControl(registerPanel);
		}
		#endregion
	}
}
