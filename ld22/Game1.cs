using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ld22
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Random random = new Random();
        public static Game1 instance;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;

        Texture2D playerSprite;
        Texture2D playerBulletSprite;

        Texture2D[] starTex;
        Texture2D[] backTex;
        Texture2D earthTex;

        Texture2D[] enemySprites;
        Texture2D eggSprite;

        Texture2D arrowSprite;

        CharacterManager characterManager;
        InputHandler inputHandler;
        LevelManager levelManager;
        SoundManager soundManager;

        Camera cam;

        SpriteFont font;

        string[] intro;
        string[] end;
        bool gameOver;

        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load sprites and such
            playerSprite = Content.Load<Texture2D>("playership");
            playerBulletSprite = Content.Load<Texture2D>("playerbullet");

            starTex = new Texture2D[3];
            starTex[0] = Content.Load<Texture2D>("star1");
            starTex[1] = Content.Load<Texture2D>("star2");
            starTex[2] = Content.Load<Texture2D>("star3");

            backTex = new Texture2D[4];
            backTex[0] = Content.Load<Texture2D>("background1");
            backTex[1] = Content.Load<Texture2D>("background2");
            backTex[2] = Content.Load<Texture2D>("background3");
            backTex[3] = Content.Load<Texture2D>("background4");

            earthTex = Content.Load<Texture2D>("earth");

            enemySprites = new Texture2D[4];
            enemySprites[0] = Content.Load<Texture2D>("enemy1");
            enemySprites[1] = Content.Load<Texture2D>("enemy2");
            enemySprites[2] = Content.Load<Texture2D>("enemy3");
            enemySprites[3] = Content.Load<Texture2D>("boss");

            eggSprite = Content.Load<Texture2D>("egg");

            arrowSprite = Content.Load<Texture2D>("arrow");

            levelManager = new LevelManager(starTex, backTex, earthTex);

            player = new Player(playerSprite, new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 0.0f), 200, levelManager);
            characterManager = new CharacterManager(player);
            inputHandler = new InputHandler(player);

            player.setCharacterManager(characterManager);
            levelManager.setCharacterManager(characterManager);

            characterManager.setBulletSprite(playerBulletSprite);
            characterManager.setEnemySprites(enemySprites);
            characterManager.setEggSprite(eggSprite);
            characterManager.setLevelManager(levelManager);

            cam = new Camera(GraphicsDevice, new Vector2(0.0f, 0.0f), player, levelManager);
            characterManager.setCam(cam);

            soundManager = new SoundManager(Content);
            levelManager.setSoundManager(soundManager);
            characterManager.setSoundManager(soundManager);

            font = Content.Load<SpriteFont>("Fixedsys");

            intro = new string[6];
            intro[0] = "'This is ground control... your circuit's dead, there's something wrong...'";
            intro[1] = "On a routine mission, you stumble into a strange portal and find yourself";
            intro[2] = "-alone-";
            intro[3] = "in a parallel dimension.";
            intro[4] = "Search for the space eggs to survive and find your way home.";
            intro[5] = "Good luck!";

            end = new string[4];
            end[0] = "In your absence, the earth was destroyed.";
            end[1] = "You are now";
            end[2] = "-alone-";
            end[3] = "\n\nTHE END";

            gameOver = false;

            levelManager.initLevel(0);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            soundManager.stop();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            characterManager.update(gameTime);
            inputHandler.update(gameTime);
            cam.update(gameTime);
            levelManager.update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            float oldZoom = cam.getZoom();

            foreach (float f in levelManager.getBackgroundLevels())
            {
                cam.setZoom(f);
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate,
                    SaveStateMode.None, cam.getTransform());
                // for tiling
                GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
                GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
                levelManager.render(spriteBatch, f);
                spriteBatch.End();
            }

            cam.setZoom(oldZoom);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate,
                SaveStateMode.SaveState, cam.getTransform());
            characterManager.render(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate,
                SaveStateMode.SaveState, Matrix.Identity);
            spriteBatch.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
            spriteBatch.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
            spriteBatch.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
            string health = "HP: " + player.getHP();
            string lives =  "Lives: " + player.getLives();
            spriteBatch.DrawString(font, health, new Vector2(0, 0), Color.Yellow);
            if (player.getLives() >= 0)
            {
                spriteBatch.DrawString(font, lives, new Vector2(0, font.MeasureString(health).Y), Color.Yellow);
            }

            string[] mainText = null;
            if (levelManager.getCurrentLevel() == 0)
            {
                mainText = intro;
            }
            else if (levelManager.getCurrentLevel() == 6)
            {
                mainText = end;
            }

            if (mainText != null)
            {
                Vector2 pos = new Vector2(0.0f, 50.0f);

                for (int i = 0; i < mainText.Length; i++)
                {
                    Color col = Color.Yellow;
                    if (mainText[i].CompareTo("-alone-") == 0)
                    {
                        col = Color.Blue;
                    }
                    pos.X = (graphics.GraphicsDevice.Viewport.Width / 2.0f) - (font.MeasureString(mainText[i]).X / 2.0f);
                    spriteBatch.DrawString(font, mainText[i], pos, col);
                    pos.Y += font.MeasureString(mainText[i]).Y * 1.1f;
                }
            }
            if (gameOver)
            {
                Vector2 pos;
                string g = "GAME OVER";
                pos.X = (graphics.GraphicsDevice.Viewport.Width / 2.0f) - (font.MeasureString(g).X);
                pos.Y = (graphics.GraphicsDevice.Viewport.Height / 2.0f) - (font.MeasureString(g).Y);
                spriteBatch.DrawString(font, g, pos, Color.Red, 0.0f, new Vector2(0.0f, 0.0f), 2.0f, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void setGameOver(bool b)
        {
            gameOver = false;
        }
    }
}
