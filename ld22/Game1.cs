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

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        Texture2D playerSprite;
        Texture2D enemy1Sprite;
        Texture2D playerBulletSprite;
        Texture2D[] starTex;
        Texture2D[] backTex;

        CharacterManager characterManager;
        InputHandler inputHandler;
        LevelManager levelManager;

        Camera cam;

        public Game1()
        {
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
            enemy1Sprite = Content.Load<Texture2D>("enemy1");
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

            levelManager = new LevelManager(starTex, backTex);

            player = new Player(playerSprite, new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 0.0f), 100, levelManager);
            characterManager = new CharacterManager(player);
            inputHandler = new InputHandler(player);

            player.setCharacterManager(characterManager);

            characterManager.setBulletSprite(playerBulletSprite);
            characterManager.setEnemySprite(enemy1Sprite);
            characterManager.setLevelManager(levelManager);

            cam = new Camera(GraphicsDevice, new Vector2(0.0f, 0.0f), player, levelManager);

            for (int i = 0; i < 20; i++)
            {
                characterManager.addEnemy();
            }

            levelManager.initLevel(4);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            //spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
            //    SaveStateMode.SaveState, cam.getTransform());
            
            //spriteBatch.End();

            cam.setZoom(oldZoom);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.SaveState, cam.getTransform());
            characterManager.render(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
