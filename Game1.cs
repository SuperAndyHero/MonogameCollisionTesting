using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonogameCollisionTesting
{
    public class Game1 : Game
    {
        private static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static List<Entity> entities;
        public static Random random;

        public static SpriteFont font_Arial;
        public static Texture2D debugTexture;
        public static Texture2D gridTexture;

        public static int screenWidth;
        public static int screenHeight;

        public static Vector2 ScreenSize
        {
            get { return new Vector2(screenWidth, screenHeight); }
        }

        public static Vector2 screenPosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            screenPosition = ScreenSize / 2;

            random = new Random();
            entities = new List<Entity>();

            base.Initialize();
        }

        public int nextShape = 9;

        protected override void LoadContent()
        {
            GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font_Arial = Content.Load<SpriteFont>("Arial");
            debugTexture = Content.Load<Texture2D>("Debug");
            gridTexture = Content.Load<Texture2D>("Floor");

            SetupEffect();

            entities.Add(new Entity(ScreenSize / 1.5f));

            //entities.Add(new Entity(ScreenSize / 1.5f, new Polygon(new Vector2[4] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) }, 48)));

            entities.Add(new Entity(ScreenSize / 2, new Polygon(new Vector2[8] { new Vector2(0, 0), new Vector2(0.3f, -0.3f), new Vector2(1, 0), new Vector2(1.3f, 0.3f), new Vector2(1, 1), new Vector2(0.3f, 1.3f), new Vector2(0, 1), new Vector2(-0.3f, 0.3f) }), 48));

            entities.Add(new Entity(ScreenSize / 3, Helper.CreateCircle(nextShape), 78));

            base.LoadContent();
        }

        private BasicEffect basicEffect;
        private void SetupEffect()
        {
            basicEffect = new BasicEffect(graphics.GraphicsDevice)
            {
                VertexColorEnabled = true,
                Alpha = 1.0f,
            };
            basicEffect.Projection = Matrix.CreateOrthographic(screenWidth, screenHeight, 0, 1000);
        }

        public static float zoom = 1f;



        private bool ButtonJustPressed(Keys keyType)
        {
            if (oldKeyState.IsKeyUp(keyType) && keyState.IsKeyDown(keyType))//is tracking bool is true, and key down: invert bools and return true
            {
                return true;
            }
            return false;
        }

        public KeyboardState keyState;
        public KeyboardState oldKeyState;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.OemMinus))
            {
                zoom -= 0.010f;
                if (zoom < 1)
                    zoom = 1;
            }
            else if (keyState.IsKeyDown(Keys.OemPlus))
            {
                zoom += 0.010f;
            }

            if (keyState.IsKeyDown(Keys.Right))
                entities[1].position.X += 1f;
            if (keyState.IsKeyDown(Keys.Left))
                entities[1].position.X -= 1f;
            if (keyState.IsKeyDown(Keys.Up))
                entities[1].position.Y -= 1f;
            if (keyState.IsKeyDown(Keys.Down))
                entities[1].position.Y += 1f;

            if(ButtonJustPressed(Keys.OemComma))
            {
                nextShape++;
                entities[2].SetHitbox(Helper.CreateCircle(nextShape));
            }
            if (ButtonJustPressed(Keys.OemPeriod))
            {
                nextShape--;
                entities[2].SetHitbox(Helper.CreateCircle(nextShape));
            }

            if (keyState.IsKeyDown(Keys.OemOpenBrackets))
            {
                entities[2].size += 0.1f;
            }
            if (keyState.IsKeyDown(Keys.OemCloseBrackets))
            {
                entities[2].size -= 0.1f;
            }

            if (keyState.IsKeyDown(Keys.Divide))
            {
                entities[2].rotation += 0.01f;
            }
            if (keyState.IsKeyDown(Keys.Multiply))
            {
                entities[2].rotation -= 0.01f;
            }

            foreach (Entity npc in entities)
            {
                npc.Update();
            }

            screenPosition = entities[1].Center;

            oldKeyState = keyState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(zoom) * Matrix.CreateTranslation(new Vector3(screenWidth * 0.5f, screenHeight * 0.5f, 0)));
            spriteBatch.Draw(gridTexture, Vector2.Zero - screenPosition, null, Color.White, 0f, Vector2.Zero, 4, SpriteEffects.None, default);
            spriteBatch.End();

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                foreach (Entity ent in entities)
                {
                    ent.DrawHitboxFill(basicEffect, pass, GraphicsDevice);
                }

                foreach (Entity ent in entities)
                {
                    ent.DrawHitboxOutline(basicEffect, pass, GraphicsDevice);
                }
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(zoom) * Matrix.CreateTranslation(new Vector3(screenWidth * 0.5f, screenHeight * 0.5f, 0)));

            foreach (Entity ent in entities)
            {
                ent.Draw(spriteBatch);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
