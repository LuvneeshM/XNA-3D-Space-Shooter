using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace XNA_Project_3D
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState lastKeyState = Keyboard.GetState(PlayerIndex.One);

        //Camera/View information
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        Matrix projectionMatrix;
        Matrix viewMatrix;

        //Audio Components
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;
        SoundEffect soundExplosion2;
        SoundEffect soundExplosion3;
        SoundEffect soundWeaponsFire;

        //Music
        SoundEffect gamePlaySong;
        SoundEffectInstance gamePlaySongInstance;

        //Visual components
        Ship ship = new Ship();

        //asteroid 
        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Random random = new Random();
        //asteroid speed
        float AsteroidSpeed = GameConstants.AsteroidMinSpeed;
        float updateAsteroidSpeed = 75f;

        //bullet
        Model bulletModel;
        Matrix[] bulletTransforms;
        Bullet[] bulletList = new Bullet[GameConstants.NumBullets];

        //earth
        Planet earth = new Planet();
        Planet jupiter = new Planet();

        //score stuff
        Texture2D stars;
        SpriteFont kootenay;
        SpriteFont GameOver;
        int score;
        Vector2 scorePosition = new Vector2(50, 50);
        int screenHeight, screenWidth;

        //mouse control
        Boolean useMouse = false;

        //hud stuff
        HUD hud = new HUD();
        HUD mana = new HUD();

        //difficulty increase over time
        float elapsedAsteroid;
        float elapsedShip;
        float delay = 5000f;

        //more asteroids
        float elapsedNew;
        float delayNew = 1000;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set preferred resolution
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                GraphicsDevice.DisplayMode.AspectRatio,
                1.0f, 100000000.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition,
                Vector3.Zero, Vector3.Up);

            //Planet variables
            earth.aspectRatio = 200f;
            earth.modelPosition = new Vector3(30, -11, 30);
            earth.rotate = 0.04f;
            jupiter.aspectRatio = 50f;
            jupiter.modelPosition = new Vector3(-1000, 400, 2000);
            jupiter.rotate = .1f;

            ResetAsteroids();

            base.Initialize();
        }

        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            float zStart;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (!asteroidList[i].isActive)
                {
                    if (random.Next(2) == 0)
                    {
                        xStart = -GameConstants.PlayfieldSizeX;
                    }
                    else
                    {
                        xStart = GameConstants.PlayfieldSizeX;
                    }

                    yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                    zStart = (float)random.NextDouble() * GameConstants.PlayFieldSizeZ;
                    asteroidList[i].position = new Vector3(xStart, yStart, GameConstants.StartZ + zStart);
                    double angle = random.NextDouble() * 2 * Math.PI;
                    //asteroidList[i].direction.X = (float)Math.Sin(angle);
                    // asteroidList[i].direction.Y = (float)Math.Cos(angle);
                    asteroidList[i].direction.Z = -(float)10f;
                    //minimum set speed
                    asteroidList[i].speed = AsteroidSpeed;
                    //inrease asteroid speed as game moves on
                    if (elapsedAsteroid >= delay)
                    {
                        AsteroidSpeed += updateAsteroidSpeed;
                        elapsedAsteroid = 0;
                    }
                    //(float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
                    asteroidList[i].isActive = true;
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Player
            ship.Model = Content.Load<Model>("Models/Ship_FBX_Recommended");
            ship.Transforms = SetupEffectDefaults(ship.Model);

            //asteroid
            asteroidModel = Content.Load<Model>("Models/Astroid");
            asteroidTransforms = SetupEffectDefaults(asteroidModel);

            //Planets
            //earth 
            earth.Model = Content.Load<Model>("Models/NightEarth");
            earth.Transforms = SetupEffectDefaults(earth.Model);

            jupiter.Model = Content.Load<Model>("Models/21jupiter");
            jupiter.Transforms = SetupEffectDefaults(jupiter.Model);

            //sound effects
            soundExplosion2 = Content.Load<SoundEffect>("Audio/Waves/explosion2");
            soundExplosion3 = Content.Load<SoundEffect>("Audio/Waves/explosion3");
            soundWeaponsFire = Content.Load<SoundEffect>("Audio/Waves/tx0_fire1");
            //check these sound effects
            soundEngine = Content.Load<SoundEffect>("Audio/Waves/engine_2");
            soundEngineInstance = soundEngine.CreateInstance();
            soundHyperspaceActivation =
                Content.Load<SoundEffect>("Audio/Waves/hyperspace_activate");

            //song
            gamePlaySong = Content.Load<SoundEffect>("Audio/300_Violin_Orchestra-Jorge_Quintero");
            gamePlaySongInstance = gamePlaySong.CreateInstance();

            bulletModel = Content.Load<Model>("Models/pea_proj");
            bulletTransforms = SetupEffectDefaults(bulletModel);

            stars = Content.Load<Texture2D>("Textures/space");

            kootenay = Content.Load<SpriteFont>("Fonts/Lucida Console");
            GameOver = Content.Load <SpriteFont>("Fonts/Game Over");

            hud.texture = Content.Load<Texture2D>("Textures/health");
            hud.Initialize(spriteBatch, 1, 50, 25, 10);

            mana.texture = Content.Load<Texture2D>("Textures/mana");
            mana.Initialize(spriteBatch, 10, 50, 70, 20);

        }

        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }


            // Get some input.
            UpdateInput(gameTime);
            
            if (ship.isActive)
            {
            earth.Update(gameTime);
            jupiter.Update(gameTime);

            hud.Update(gameTime, 1);
            mana.Update(gameTime, 1);

            //check to see time between increase in level
            elapsedAsteroid += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            elapsedShip += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //speed up the ship
            if (elapsedShip >= delay)
            {
                ship.speed += 5;
                elapsedShip = 0;
            }

            elapsedNew += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedNew >= delayNew)
            {
               // ResetAsteroids();
                elapsedNew = 0;
            }

            // Add velocity to the current position.
            ship.Position += ship.Velocity;

            //update the asteroid
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (asteroidList[i].isActive)
                {
                    asteroidList[i].Update(timeDelta);
                }
                else
                {
                    soundExplosion3.Play();
                    hud.Update(gameTime, 0);
                    ResetAsteroids();
                }
            }

            //update the bullet
            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    bulletList[i].Update(timeDelta);
                }
            }

            //bullet-asteroid collision check
            for (int i = 0; i < asteroidList.Length; i++)
            {
                if (asteroidList[i].isActive)
                {
                    BoundingSphere asteroidSphere =
                      new BoundingSphere(asteroidList[i].position,
                               asteroidModel.Meshes[0].BoundingSphere.Radius *
                               GameConstants.AsteroidBoundingSphereScale +1000);
                    for (int j = 0; j < bulletList.Length; j++)
                    {
                        if (bulletList[j].isActive)
                        {
                            BoundingSphere bulletSphere = new BoundingSphere(
                              bulletList[j].position,
                              bulletModel.Meshes[0].BoundingSphere.Radius);

                            if (asteroidSphere.Intersects(bulletSphere))
                            {
                                soundExplosion2.Play();
                                asteroidList[i].isActive = false;
                                ResetAsteroids();
                                bulletList[j].isActive = false;
                                score += GameConstants.KillBonus;
                                break; //no need to check other bullets
                            }
                        }
                    }
                }
            }


            //ship-asteroid collision check
            if (ship.isActive)
            {
                BoundingSphere shipSphere = new BoundingSphere(
                    ship.Position, ship.Model.Meshes[0].BoundingSphere.Radius *
                                         GameConstants.ShipBoundingSphereScale);
                for (int i = 0; i < asteroidList.Length; i++)
                {
                    if (asteroidList[i].isActive)
                    {
                        BoundingSphere b = new BoundingSphere(asteroidList[i].position,
                        asteroidModel.Meshes[0].BoundingSphere.Radius *
                        GameConstants.AsteroidBoundingSphereScaleShip);

                        if (b.Intersects(shipSphere))
                        {
                            //blow up ship
                            asteroidList[i].isActive = false;
                            ResetAsteroids();
                            soundExplosion3.Play();
                            //update health when hit
                            hud.Update(gameTime, 0);
                            if (hud.rectangle.Width <= 0)
                                ship.isActive = false;
                            break; //exit the loop
                        }
                    }
                }
            }

                //check to see if gg
            if (hud.rectangle.Width <= 0)
                ship.isActive = false;

        }

            gamePlaySongInstance.Play();

            base.Update(gameTime);
        }

        protected void MouseInput()
        {
            MouseState mState = Mouse.GetState();

            ship.Position = new Vector3(-mState.X, -mState.Y, ship.Position.Z);
        }

        protected void UpdateInput(GameTime gameTime)
        {
            // Get the game pad state.
            KeyboardState currentKeyState = Keyboard.GetState();

            if (ship.isActive)
            {
                ship.Update(currentKeyState);

                //Play engine sound only when the engine is on.
                if (currentKeyState.IsKeyDown(Keys.W))
                {

                    if (soundEngineInstance.State == SoundState.Stopped)
                    {
                        soundEngineInstance.Volume = 0.75f;
                        soundEngineInstance.IsLooped = true;
                        soundEngineInstance.Play();
                    }
                    else
                        soundEngineInstance.Resume();
                }
                else if (currentKeyState.IsKeyDown(Keys.S))
                {

                    if (soundEngineInstance.State == SoundState.Stopped)
                    {
                        soundEngineInstance.Volume = 0.75f;
                        soundEngineInstance.IsLooped = true;
                        soundEngineInstance.Play();
                    }
                    else
                        soundEngineInstance.Resume();
                }
                else
                {
                    if (soundEngineInstance.State == SoundState.Playing)
                        soundEngineInstance.Pause();
                }

                //are we shooting?9
                if (ship.isActive && currentKeyState.IsKeyDown(Keys.Space) && lastKeyState.IsKeyUp(Keys.Space) && (mana.rectangle.Width > mana.loseHealth))
                {
                    mana.Update(gameTime, 0);
                    //add another bullet.  Find an inactive bullet slot and use it
                    //if all bullets slots are used, ignore the user input
                    for (int i = 0; i < GameConstants.NumBullets; i++)
                    {
                        if (!bulletList[i].isActive)
                        {
                            bulletList[i].direction = -ship.RotationMatrix.Forward;
                            bulletList[i].speed = GameConstants.BulletSpeedAdjustment;
                            bulletList[i].position = ship.Position + (200 * bulletList[i].direction);
                            bulletList[i].isActive = true;
                            soundWeaponsFire.Play();
                            break; //exit the loop
                        }
                    }
                }

                if (ship.isActive && currentKeyState.IsKeyDown(Keys.R))
                {
                    useMouse = !useMouse;
                }
                if (useMouse == true)
                    MouseInput();
            }

            //NEW GAME
            // In case you get lost, press A to warp back to the center.
            if (currentKeyState.IsKeyDown(Keys.Q))
            {
                //remove the asteroid and bullets
               for (int i = 0; i < GameConstants.NumAsteroids; i++)
                {
                    asteroidList[i].isActive = false;
                }
               for (int i = 0; i < GameConstants.NumBullets; i++)
               {
                   bulletList[i].isActive = false;
               }

                //go back to beginning state of game
               ship.Position = Vector3.Zero;
               ship.Velocity = Vector3.Zero;
               ship.Rotation = 0.0f;
               ship.speed = 0f;

               hud.rectangle.Width = hud.texture.Width;
               mana.rectangle.Width = mana.texture.Width;

               score = 0;

               AsteroidSpeed = GameConstants.AsteroidMinSpeed;
                //play on
               soundHyperspaceActivation.Play();
               ship.isActive = true;
               ResetAsteroids();
            }
                lastKeyState = currentKeyState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            if (ship.isActive)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                spriteBatch.Draw(stars, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(kootenay, "Score: " + score,
                                       scorePosition, Color.LightGreen);
                spriteBatch.End();


                hud.Draw();
                mana.Draw();

                earth.Draw();
                jupiter.Draw();


                //asteroids
                for (int i = 0; i < GameConstants.NumAsteroids; i++)
                {
                    Matrix asteroidTransform =
                       Matrix.CreateScale(50f) * Matrix.CreateTranslation(asteroidList[i].position);
                    if (asteroidList[i].isActive)
                    {
                        DrawModel(asteroidModel, asteroidTransform, asteroidTransforms);
                    }
                }

                //bullets
                for (int i = 0; i < GameConstants.NumBullets; i++)
                {
                    if (bulletList[i].isActive)
                    {
                        Matrix bulletTransform =
                          Matrix.CreateTranslation(bulletList[i].position);
                        DrawModel(bulletModel, bulletTransform, bulletTransforms);
                    }
                    //if (bulletList[i].position.Z < Asteroid.getPosition.Z)
                    //bulletList[i].isActive = false;
                    if (bulletList[i].position.Z > (Asteroid.getPosition.Z + 5000) && bulletList[i].position.Z > GameConstants.BulletFieldSize)
                        bulletList[i].isActive = false;
                }


                //ship
                Matrix shipTransformMatrix = ship.RotationMatrix
                       * Matrix.CreateTranslation(ship.Position);
                if (ship.isActive)
                {
                    DrawModel(ship.Model, shipTransformMatrix, ship.Transforms);
                }
                //if (ship.isActive)
                //    ship.Draw();
            }
            if (!ship.isActive)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                spriteBatch.DrawString(GameOver, "Game Over", new Vector2(screenWidth / 2, screenHeight / 2), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }



        //method to draw the different models 
        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        absoluteBoneTransforms[mesh.ParentBone.Index] *
                        modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

    }
}
/*
 * 1. User friendly interface with at least two 3D objects [5 points]
 * 2. User key controls and mouse controls input that can move the 3D objects in all directions inclduing zoom in and out [15 points]
 * 3. User key controls and mouse controls input that can make at least one of the object spin [5 points]
 * 4. Simple but interesting game play [5 points]
 * 5. music and sound effect [5 points]
 * 6. Some 3D collision detection [5 points]
 * 
 * */

