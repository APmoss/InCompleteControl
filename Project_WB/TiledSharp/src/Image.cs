using System;
using System.Xml.Linq;
using System.Globalization;

namespace TiledSharp {
	public class TmxImage {
		public string Source { get; private set; }
		public uint? Trans { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public TmxImage(XElement xImage) {
			Source = (string)xImage.Attribute("source");

			var xTrans = (string)xImage.Attribute("trans");
			if (xTrans != null)
				Trans = UInt32.Parse(xTrans, NumberStyles.HexNumber);

			Width = (int)xImage.Attribute("width");
			Height = (int)xImage.Attribute("height");
		}
	}
}
