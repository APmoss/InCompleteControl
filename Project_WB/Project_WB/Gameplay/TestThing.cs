using System;
using System.Collections.Generic;
using GameStateManagement;
using TiledSharp;

namespace Project_WB.Gameplay {
	class TestThing : GameScreen {
		TmxMap map = new TmxMap(@"C:\Users\211980\Desktop\test.tmx");
		int i = 0;

		public override void Activate(bool instancePreserved) {
			base.Activate(instancePreserved);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			//var t = ((TmxLayer)map.Layers["Asd"]).Tiles[0];
			//((TmxTileset)map.Tilesets["asd"]).
			base.Draw(gameTime);
		}
	}
}
