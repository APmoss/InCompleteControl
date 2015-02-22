using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Squared.Tiled;
using Project_WB.Framework.Entities.Units;
using Project_WB.Framework.Entities;

namespace Project_WB.Gameplay {
	class Multi1v1 : Battle {
		public override void Activate(bool instancePreserved) {
			map = Map.Load(@"maps\snowy.tmx", ScreenManager.Game.Content);

			base.Activate(instancePreserved);

			// Standard zoom
			camera.DestPosition = new Vector2(Stcs.XRes / 2, Stcs.YRes / 2);
			camera.DestScale = 1.4f;

			addEntities();
		}

		void addEntities() {
			// Add starting units to each team
			entityManager.AddEntities(
										// Player 1
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

										// Player 2
										new Accensus() { Tile = new Point(14, 25), Team = 2 },
										new Accensus() { Tile = new Point(15, 24), Team = 2 },
										new Accensus() { Tile = new Point(16, 23), Team = 2 },
										new Accensus() { Tile = new Point(17, 24), Team = 2 },
										new Accensus() { Tile = new Point(18, 25), Team = 2 },

										new Scout() { Tile = new Point(13, 26), Team = 2 },
										new Scout() { Tile = new Point(19, 26), Team = 2 },

										new Ballistarius() { Tile = new Point(16, 25), Team = 2 },

										new Scorpionarius() { Tile = new Point(15, 26), Team = 2 },
										new Scorpionarius() { Tile = new Point(17, 26), Team = 2 },

										new Centurion() { Tile = new Point(16, 26), Team = 2 }
									);

			foreach (var entity in entityManager.GetEntities()) {
				if (entity is Unit) {
					if (((Unit)entity).Team == 2) {
						((Unit)entity).Tint = Color.DarkOrange;
					}
				}
			}
		}
	}
}
