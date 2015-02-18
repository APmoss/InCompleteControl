using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement {
	/// <summary>
	/// Contains a collection of fonts preloaded and ready to use.
	/// </summary>
	public class FontLibrary {
		#region Properties
		// Read only public properties to use for drawing
		public SpriteFont Consolas {
			get; protected set;
		}
		public SpriteFont Centaur {
			get; protected set;
		}
		public SpriteFont HighTowerText {
			get; protected set;
		}
		public SpriteFont SmallSegoeUIMono {
			get; protected set;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Loads the content for all spritefonts
		/// </summary>
		/// <param name="content"></param>
		public void LoadFonts(ContentManager content) {
			if (content != null) {
				Consolas = content.Load<SpriteFont>("fonts/consolas");
				Centaur = content.Load<SpriteFont>("fonts/centaur");
				HighTowerText = content.Load<SpriteFont>("fonts/highTowerText");
				SmallSegoeUIMono = content.Load<SpriteFont>("fonts/smallSegoeUiMono");
			}
		}
		#endregion
	}
}
