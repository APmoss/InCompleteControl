// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;

namespace TiledSharp
{
	public class TmxLayer : ITmxElement
	{
		public string Name {get; private set;}
		public double Opacity {get; private set;}
		public bool Visible {get; private set;}
		
		public List<TmxLayerTile> Tiles {get; private set;}
		public PropertyDict Properties {get; private set;}
		
		public TmxLayer(XElement xLayer, int width, int height)
		{
			Name = (string)xLayer.Attribute("name");
			
			var xOpacity = xLayer.Attribute("opacity");
			if (xOpacity == null)
				Opacity = 1.0;
			else
				Opacity = (double)xOpacity;
			
			var xVisible = xLayer.Attribute("visible");
			if (xVisible == null)
				Visible = true;
			else
				Visible = (bool)xVisible;
			
			var xData = xLayer.Element("data");
			var encoding = (string)xData.Attribute("encoding");
			
			Tiles = new List<TmxLayerTile>();
			if (encoding == "base64")
			{
				var base64data = Convert.FromBase64String((string)xData.Value);
				Stream stream = new MemoryStream(base64data, false);
				
				var compression = (string)xData.Attribute("compression");
				if (compression == "gzip")
					stream = new GZipStream(stream, CompressionMode.Decompress,
											false);
				else if (compression == "zlib")
					stream = new Ionic.Zlib.ZlibStream(stream,
								Ionic.Zlib.CompressionMode.Decompress, false);
				else if (compression != null)
					throw new Exception("Tiled: Unknown compression.");
				
				using (stream)
				using (var br = new BinaryReader(stream))
					for (int j = 0; j < height; j++)
						for (int i = 0; i < width; i++)
							Tiles.Add(new TmxLayerTile(br.ReadUInt32(), i, j));
			}
			else if (encoding == "csv")
			{
				var csvData = (string)xData.Value;
				int k = 0;
				foreach (var s in csvData.Split(','))
				{
					var gid = uint.Parse(s.Trim());
					var x = k % width;
					var y = k / width;
					Tiles.Add(new TmxLayerTile(gid, x, y));
					k++;
				}
			}
			else if (encoding == null)
			{
				int k = 0;
				foreach (var e in xData.Elements("tile"))
				{
					var gid = (uint)e.Attribute("gid");
					var x = k % width;
					var y = k / width;
					Tiles.Add(new TmxLayerTile(gid, x, y));
					k++;
				}
			}
			else throw new Exception("Tiled: Unknown encoding.");
			
			Properties = new PropertyDict(xLayer.Element("properties"));
		}

		public void Draw(SpriteBatch) {

		}
	}
}
