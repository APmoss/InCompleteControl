using System;
using System.Collections.Generic;
using Project_WB.Framework.Squared.Tiled;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Gui.Controls;
using Project_WB.Framework.Entities.Units;
using Project_WB.Framework.Entities;
using System.Linq;

namespace Project_WB.Gameplay {
	class Tutorial : Battle {
		DialogBox welcomeDialog;
		DialogBox guiDialog;
		DialogBox youDialog;
		DialogBox enemyDialog;

		public Tutorial() {
			combatLocked = true;
		}

		public override void Activate(bool instancePreserved) {
			map = Map.Load(@"maps\river.tmx", ScreenManager.Game.Content);
			
			base.Activate(instancePreserved);

			camera.DestPosition = new Vector2(Stcs.XRes / 2, Stcs.YRes / 2);
			camera.DestScale = 1.4f;

			notificationBox.Text = "Objective: Read the tutorial and learn the ropes.";
			notificationBox.HasButton = false;

			addEntities();
			
			welcomeDialog = new DialogBox(300, 300, 500, 300, "Welcome to INCOMPLETE CONTROL. In this level, you will learn the basics of combat. Press the OK button to advance the dialog.");
			guiDialog = new DialogBox(500, 300, 500, 300, "Let's learn some basic controls. The controls are locked at first, but will be unlocked after the tutorial is up. " +
														"First, the camera can be controlled by moving to the sides of the screen, or by using the " +
														"directional keys. You can use this to look around the map and get a better view of your battlefield. You may also zoom using the " +
														"scroll wheel on your mouse.");
			youDialog = new DialogBox(100, 500, 900, 220, "These units you see here are yours. You will be able to control them and use them in combat soon. Each different unit has different " +
														"benefits and disadvantages when it comes to combat, so be sure to check the HUD on the bottom of the screen when learning. " +
														"You may select a unit by left clicking on them. This will show the unit's movement range (blue) and attack range (red). Attack and/or " +
														"move using right click. If there is a blue square available, you are able to travel to it, no matter the obstacles. You may " +
														"only attack and move once per unit per turn, so choose your strategy wisely.");
			enemyDialog = new DialogBox(100, 500, 800, 200, "These units here are your enemy! They are slightly tinted red, and have an orange highlight if you select them. " +
														"You cannot control these units, but you may select them, check their range, statistics, and even health. They aren't expecting an " +
														"so now would be a good time to strike! Defeat the enemies to continue, good luck.");
			guiManager.AddControl(welcomeDialog);
			welcomeDialog.OkButton.LeftClicked += delegate {
				camera.DestScale += .5f;
				camera.DestPosition += new Vector2(50, 50);
				guiManager.AddControl(guiDialog);

				guiDialog.OkButton.LeftClicked += delegate {
					camera.DestScale += .5f;
					camera.DestPosition = new Vector2(530, 250);
					guiManager.AddControl(youDialog);

					youDialog.OkButton.LeftClicked += delegate {
						camera.DestPosition = new Vector2(530, 24 * 32);
						guiManager.AddControl(enemyDialog);
						enemyDialog.OkButton.LeftClicked += delegate {
							camera.DestScale = 1.2f;
							combatLocked = false;
							notificationBox.Text = "Defeat the enemies!";
						};
					};
				};
			};
		}

		void addEntities() {
			entityManager.AddEntities(
										new Accensus() { Tile = new Point(14, 6), Team = 1 },
										new Accensus() { Tile = new Point(15, 7), Team = 1 },
										new Accensus() { Tile = new Point(16, 8), Team = 1 },
										new Accensus() { Tile = new Point(17, 7), Team = 1 },
										new Accensus() { Tile = new Point(18, 6), Team = 1 },

										new Scout() { Tile = new Point(13, 5), Team = 1 },
										new Scout() { Tile = new Point(19, 5), Team = 1 },

										new Ballistarius() { Tile = new Point(16, 6), Team = 1 },

										new Scorpionarius() { Tile = new Point(15, 5), Team = 1 },
										new Scorpionarius() { Tile = new Point(17, 5), Team = 1 },

										new Centurion() { Tile = new Point(16, 5), Team = 1 },

										new Accensus() { Tile = new Point(14, 24), Team = 2 },
										new Accensus() { Tile = new Point(15, 24), Team = 2 },
										new Accensus() { Tile = new Point(16, 24), Team = 2 },
										new Accensus() { Tile = new Point(17, 24), Team = 2 },
										new Accensus() { Tile = new Point(18, 24), Team = 2 }
									);

			foreach (var entity in entityManager.GetEntities()) {
				if (entity is Unit) {
					if (((Unit)entity).Team == 2) {
						((Unit)entity).Tint = Color.DarkOrange;
					}
				}
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			if (entityManager.controllingTeam == 2) {
				bool check = false;
				foreach (var entity in entityManager.GetEntities()) {
					if (entity is Unit) {
						Unit unit = entity as Unit;
						if (unit.Team == 2 && !unit.Moved) {
							if(!entityManager.mapData.Barriers.Contains(new Point(unit.Tile.X, unit.Tile.Y - 1))) {
								unit.Waypoints.AddFirst(new Point(unit.Tile.X, unit.Tile.Y - 1));
							}
							unit.Moved = true;
							check = true;
							break;
						}
					}
				}

				if (!check) {
					entityManager.controllingTeam = 1;
					entityManager.ResetEntities();
				}
			}
			
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}
	}
}
