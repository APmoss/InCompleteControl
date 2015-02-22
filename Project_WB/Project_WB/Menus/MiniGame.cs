using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using Microsoft.Xna.Framework.Audio;
using Project_WB.Framework.IO;

namespace Project_WB.Menus {
	class MiniGame : GameScreen {
		/// <summary>
		/// this area is holding all of the variables necessary to the game
		/// includes rectangles, various counter variables, bools
		/// also includes textures necessary to the sprites of the game
		/// </summary>
		/// 
		#region Variables
		Effect effect;
		SpriteFont font;
		Texture2D text;
		Texture2D lightMask;
		Texture2D shade, car, baseText, arrow;
		Texture2D slenderMan, current, dead;
		Animation carAn = new Animation();
		RenderTarget2D lightsTarget;
		RenderTarget2D mainTarget;
		MouseState mouse;
		Rectangle lightRec = new Rectangle(490, 490, 112, 112);
		Rectangle backRec = new Rectangle(0, 0, 8192, 8192);
		Rectangle baseRec = new Rectangle(4300, 6000, 64, 64);
		Rectangle slenderRec = new Rectangle(0, 0, 64, 64);
		Rectangle arrowRec = new Rectangle(506, 535, 32, 32);
		Vector2 arrowDirection = Vector2.Zero;
		Color color = Color.White;
		long delay = 0;
		int direction = 0;
		int slenderKills = 0;
		float intesntisy = 0.02f;
		float i;
		float arrowRotate = 0;
		bool hit = false;
		bool flash = true;
		bool drawSlender = false;

		SoundEffectInstance sound;
		#endregion

		/// <summary>
		/// activates all of the necessary variables for the screen manager. 
		/// also loads all of the content necessary for drawing the textures to the screen.
		/// </summary>
		/// <param name="instancePreserved"></param>
		public override void Activate(bool instancePreserved) {
			var pp = ScreenManager.Game.GraphicsDevice.PresentationParameters;
			lightsTarget = new RenderTarget2D(ScreenManager.Game.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
			mainTarget = new RenderTarget2D(ScreenManager.Game.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
			text = ScreenManager.Game.Content.Load<Texture2D>("minigame/Map1");
			lightMask = ScreenManager.Game.Content.Load<Texture2D>("minigame/lightmask");
			shade = ScreenManager.Game.Content.Load<Texture2D>("minigame/shade");
			effect = ScreenManager.Game.Content.Load<Effect>("minigame/Effect1");//austin graham//
			//Mouse.SetPosition(512, 256);
			font = ScreenManager.Game.Content.Load<SpriteFont>("minigame/SpriteFont1");
			car = ScreenManager.Game.Content.Load<Texture2D>("minigame/RaceCar");
			baseText = ScreenManager.Game.Content.Load<Texture2D>("minigame/Building1_Base");
			carAn.Initialize(car, new Vector2(480, 480), 32, 32, 4, 90, Color.White, 1f, true);
			slenderMan = ScreenManager.Game.Content.Load<Texture2D>("minigame/Slenderman");
			dead = ScreenManager.Game.Content.Load<Texture2D>("minigame/deadSlender");
			current = slenderMan;
			arrow = ScreenManager.Game.Content.Load<Texture2D>("minigame/Arrow");

			sound = ScreenManager.SoundLibrary.GetSound("southsea").CreateInstance();
			sound.Play();
			sound.Volume = IOManager.LoadSettings().MusicVolume;
			
			base.Activate(instancePreserved);
		}

		/// <summary>
		/// updates all of the necessary functions for game to work.
		/// deals with keyboard input- moves the unit around
		/// updates light positions
		/// updates collisions
		/// updates the arrow pointing towards the objective
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="otherScreenHasFocus"></param>
		/// <param name="coveredByOtherScreen"></param>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			// Allows the game to exit
			mouse = Mouse.GetState();
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				sound.Stop();
				ExitScreen();
			}

			//updates the movement for the unit

			if (Keyboard.GetState().IsKeyDown(Keys.Up) && backRec.Y < 480) {
				backRec.Y += 4;
				slenderRec.Y += 4;
				baseRec.Y += 4;
				direction = 3;
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Down) && backRec.Y > -7680) {
				backRec.Y -= 4;
				slenderRec.Y -= 4;
				baseRec.Y -= 4;
				direction = 0;
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Left) && backRec.X < 480) {
				backRec.X += 4;
				slenderRec.X += 4;
				baseRec.X += 4;
				direction = 1;
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Right) && backRec.X > -7680) {
				backRec.X -= 4;
				slenderRec.X -= 4;
				baseRec.X -= 4;
				direction = 2;
			}
			//updates the animation for the unit
			carAn.Update(gameTime);
			//changes the intensity of the light based upon the distance from the goal
			i = (float)(Math.Sqrt((480 - baseRec.X) * (480 - baseRec.X) + (480 - baseRec.Y) * (480 - baseRec.Y)));
			i = i / (float)(Math.Sqrt(((baseRec.X) * (baseRec.X)) + ((baseRec.Y) * (baseRec.Y))));
			intesntisy = (float)(0.03 * i);
			flash = false;
			delay++;
			//changes slenderman's animation when he is hit
			if (delay % 120 == 0) {
				if (drawSlender)
					drawSlender = false;
				else {
					drawSlender = true;
					current = slenderMan;
					hit = false;
				}
				Random r = new Random();
				slenderRec.X = r.Next(0, 1024);
				slenderRec.Y = r.Next(0, 1024);
			}
			if (drawSlender) {
				if (slenderRec.Contains(new Point(480, 480))) {
					current = dead;
					if (!hit) {
						slenderKills += 50;
						ScreenManager.SoundLibrary.GetSound("-ugh").Play();
					}
						
					hit = true;
				}
			}
			//points the arrow toward the objective
			arrowDirection = new Vector2(baseRec.X, baseRec.Y) - new Vector2(480, 480);
			arrowRotate = (float)Math.Atan2(arrowDirection.Y, arrowDirection.X);
			if (baseRec.Contains(new Point(480, 480))) {
				sound.Stop();
				ExitScreen();
			}
			
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}
		/// <summary>
		/// draws all of the necessary game components
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime) {
			ScreenManager.Game.GraphicsDevice.SetRenderTarget(lightsTarget);
			ScreenManager.Game.GraphicsDevice.Clear(Color.Black);
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
			//spriteBatch.Draw(shade, new Rectangle(0, 0, 1024, 512), Color.White);
			//spriteBatch.Draw(lightMask, lightRec, color);
			if(flash)
				ScreenManager.SpriteBatch.Draw(lightMask, new Rectangle(500, 200, 80, 80), Color.Red);
			ScreenManager.SpriteBatch.End();

			// Draw the main scene to the Render Target  
			//draws all of the objects that the lighting applies to 
			ScreenManager.Game.GraphicsDevice.SetRenderTarget(mainTarget);
			ScreenManager.Game.GraphicsDevice.Clear(Color.White);
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(text, backRec, Color.White);
			ScreenManager.SpriteBatch.Draw(baseText, baseRec, Color.White);
			if (drawSlender)
			{
				ScreenManager.SpriteBatch.Draw(current, slenderRec, Color.White);
			}
			carAn.Draw(ScreenManager.SpriteBatch, direction);
			ScreenManager.SpriteBatch.End();

			// Draw the main scene with a pixel  
			ScreenManager.Game.GraphicsDevice.SetRenderTarget(null);
			ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Vector2 pos1 = new Vector2(lightRec.X, lightRec.Y);
			pos1.X /= 1280;
			pos1.Y /= 720;
			Vector2 pos2 = new Vector2(400, 200);
			pos2.X /= 1280;
			pos2.Y /= 720;
			effect.Parameters["pos"].SetValue(pos1);
			Vector2 tempV = new Vector2(baseRec.X, baseRec.Y);
			tempV.X /= 1280;
			tempV.Y /= 720;
			effect.Parameters["pos2"].SetValue(tempV);
			effect.Parameters["intensity"].SetValue(intesntisy);
			//effect.Parameters["lightMask"].SetValue(lightsTarget);
			effect.CurrentTechnique.Passes[0].Apply();
			ScreenManager.SpriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
			ScreenManager.SpriteBatch.End();
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.DrawString(font, "Slender Hits: " + slenderKills, Vector2.Zero, Color.Red);
			//ScreenManager.SpriteBatch.DrawString(font, "X: " + arrowDirection.X + " Y: " + arrowDirection.Y, new Vector2(0, 50), Color.White);
			//ScreenManager.SpriteBatch.DrawString(font, "Rotation: " + arrowRotate, new Vector2(0, 100), Color.Orange);
			ScreenManager.SpriteBatch.Draw(arrow, arrowRec, null, new Color(255, 255, 255, 10), arrowRotate + MathHelper.PiOver2, new Vector2(arrow.Width / 2, arrow.Height / 2), SpriteEffects.None, 0);
			ScreenManager.SpriteBatch.End();
			//austin graham//
			
			base.Draw(gameTime);
		}
	}

	public class Animation {
		// The image representing the collection of images used for animation
		public Texture2D spriteStrip;


		// The scale used to display the sprite strip
		float scale;


		// The time since we last updated the frame
		int elapsedTime;


		// The time we display a frame until the next one
		public int frameTime;


		// The number of frames that the animation contains
		public int frameCount;


		// The index of the current frame we are displaying
		int currentFrame;


		// The color of the frame we will be displaying
		public Color color;


		// The area of the image strip we want to display
		Rectangle sourceRect = new Rectangle();


		// The area where we want to display the image strip in the game
		public Rectangle destinationRect = new Rectangle();


		// Width of a given frame
		public int FrameWidth;


		// Height of a given frame
		public int FrameHeight;


		// The state of the Animation
		public bool Active;


		// Determines if the animation will keep playing or deactivate after one run
		public bool Looping;


		// Width of a given frame
		public Vector2 Position;


		public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping) {
			// Keep a local copy of the values passed in
			this.color = color;
			this.FrameWidth = frameWidth;
			this.FrameHeight = frameHeight;
			this.frameCount = frameCount;
			this.frameTime = frametime;
			this.scale = scale;

			Looping = looping;
			Position = position;
			spriteStrip = texture;


			// Set the time to zero
			elapsedTime = 0;
			currentFrame = 0;
			destinationRect = new Rectangle((int)position.X, (int)position.Y, (int)(FrameWidth * scale), (int)(FrameHeight * scale));
			sourceRect = new Rectangle(0, 0, 88, 30);
			// Set the Animation to active by default
			Active = true;
		}

		public void Update(GameTime gameTime) {
			// Do not update the game if we are not active
			if (Active == false)
				return;


			// Update the elapsed time
			elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;


			// If the elapsed time is larger than the frame time
			// we need to switch frames
			if (elapsedTime > frameTime) {
				// Move to the next frame
				currentFrame++;


				// If the currentFrame is equal to frameCount reset currentFrame to zero
				if (currentFrame == frameCount) {
					currentFrame = 0;
					// If we are not looping deactivate the animation
					if (Looping == false)
						Active = false;
				}


				// Reset the elapsed time to zero
				elapsedTime = 0;
			}


			// Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
			sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
		}

		// Draw the Animation Strip
		public void Draw(SpriteBatch spriteBatch, int direction) {
			// Only draw the animation when we are active
			if (Active) {
				switch (direction) {
					case 0: spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
						break;
					case 1: spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, MathHelper.PiOver2, new Vector2(8, 32), SpriteEffects.None, 0);
						break;
					case 2: spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, -MathHelper.PiOver2, new Vector2(32, 8), SpriteEffects.None, 0);
						break;
					case 3: spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, MathHelper.Pi, new Vector2(32, 32), SpriteEffects.None, 0);
						break;

				}


			}
		}
	}
}
