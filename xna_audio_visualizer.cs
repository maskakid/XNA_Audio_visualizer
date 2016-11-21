using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Song mySong;
        Texture2D myTexture,backTexture;
        VisualizationData visData = new VisualizationData();
        Color backColor = Color.White;

        int barWidth;
        int barheight = 5;
        int amount;

        public int GetAverage(Point Between, VisualizationData visData)
        {
            int average = 0;
            for (int i = Between.X; i < Between.Y; i++)
            {
                average += Convert.ToInt32(visData.Frequencies[i]); 
            }
            int diff = Between.Y - Between.X + 1;
            return average / diff;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myTexture = new Texture2D(GraphicsDevice,1,1);
            myTexture.SetData(new Color[] {Color.CadetBlue});

            backTexture = Content.Load<Texture2D>("bg");
            mySong = Content.Load<Song>("song1");
            barWidth = graphics.PreferredBackBufferWidth / 256;
            MediaPlayer.IsVisualizationEnabled = true;
            MediaPlayer.Play(mySong);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MediaPlayer.GetVisualizationData(visData);
            amount = GetAverage(new Point(0,256),visData);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(backTexture, new Vector2(0,0), backColor );

            for (int i = 0; i < 256; i++)
            {
                backColor = Color.FromNonPremultiplied(Convert.ToInt32(visData.Frequencies[amount] * 255), Convert.ToInt32(visData.Frequencies[amount] * 255), Convert.ToInt32(visData.Frequencies[amount] * 255), 255);
                spriteBatch.Draw(myTexture, new Rectangle(i*barWidth,(graphics.PreferredBackBufferHeight/2)+Convert.ToInt32(i *visData.Samples[i]), barWidth, barheight),Color.FromNonPremultiplied(255,0,i,255) ) ;
                spriteBatch.Draw(myTexture, new Rectangle(i * barWidth, (graphics.PreferredBackBufferHeight / 2) + Convert.ToInt32(i * visData.Samples[i]) +barheight+2 , barWidth, barheight), Color.FromNonPremultiplied(255, 0, i, 100));
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}