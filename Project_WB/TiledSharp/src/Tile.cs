using System;
using System.Collections.Generic;

namespace TiledSharp {
	public class TmxLayerTile {
		// Tile flip bit flags
		const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
		const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
		const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

		public uint GID { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public bool HorizontalFlip { get; private set; }
		public bool VerticalFlip { get; private set; }
		public bool DiagonalFlip { get; private set; }

		public TmxLayerTile(uint id, int x, int y) {
			GID = id;
			X = x;
			Y = y;

			// Scan for tile flip bit flags
			if ((GID & FLIPPED_HORIZONTALLY_FLAG) != 0)
				HorizontalFlip = true;
			else
				HorizontalFlip = false;

			if ((GID & FLIPPED_VERTICALLY_FLAG) != 0)
				VerticalFlip = true;
			else
				VerticalFlip = false;

			if ((GID & FLIPPED_DIAGONALLY_FLAG) != 0)
				DiagonalFlip = true;
			else
				DiagonalFlip = false;

			// Zero the bit flags
			GID &= ~(FLIPPED_HORIZONTALLY_FLAG |
					 FLIPPED_VERTICALLY_FLAG |
					 FLIPPED_DIAGONALLY_FLAG);
		}
	}
}
