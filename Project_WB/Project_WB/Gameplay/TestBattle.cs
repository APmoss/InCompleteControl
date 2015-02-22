using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Squared.Tiled;
using Microsoft.Xna.Framework.Input;
using Project_WB.Framework.Entities.Units;
using GameStateManagement;
using Project_WB.Framework.Entities;

namespace Project_WB.Gameplay {
	class TestBattle : Battle {
		public override void Activate(bool instancePreserved) {
			map = Map.Load(@"maps\river.tmx", ScreenManager.Game.Content);

			base.Activate(instancePreserved);

			camera.DestPosition = new Vector2(300, 300);
			camera.DestScale = 1.4f;

			Centurion cen = new Centurion();
			Ballistarius bal = new Ballistarius();
			Accensus acc = new Accensus();
			Scout sco = new Scout();
			Scorpionarius scp = new Scorpionarius();
			entityManager.AddEntities(cen, bal, acc, sco, scp);
			cen.Tile = new Point(7, 3);
			bal.Tile = new Point(8, 3);
			acc.Tile = new Point(9, 3);
			sco.Tile = new Point(10, 3);
			scp.Tile = new Point(11, 3);
			cen.Team = bal.Team = acc.Team = sco.Team = scp.Team = 1;

			Centurion cen2 = new Centurion();
			Ballistarius bal2 = new Ballistarius();
			Accensus acc2 = new Accensus();
			Scout sco2 = new Scout();
			Scorpionarius scp2 = new Scorpionarius();
			entityManager.AddEntities(cen2, bal2, acc2, sco2, scp2);
			cen2.Tile = new Point(7, 28);
			bal2.Tile = new Point(8, 28);
			acc2.Tile = new Point(9, 28);
			sco2.Tile = new Point(10, 4);
			scp2.Tile = new Point(11, 28);
			cen2.Team = bal2.Team = acc2.Team = sco2.Team = scp2.Team = 2;

			foreach (var entity in entityManager.GetEntities()) {
				if (entity is Unit) {
					if (((Unit)entity).Team != entityManager.controllingTeam) {
						((Unit)entity).Tint = Color.LightPink;
					}
				}
			}
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			PlayerIndex p = PlayerIndex.One;

			if (input.IsNewKeyPress(Keys.Escape, null, out p)) {
				ExitScreen();
			}
			
			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}
	}
}
