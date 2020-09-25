using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SaveThePrincess.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaveThePrincess
{
    public class SaveThePrincess : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D characterTexture;

        private const int LINK_DEFAULT_HEIGHT = 23;
        private const int LINK_DEFAULT_WIDTH = 17;

        Vector2 linkPosition;
        Vector2 zeldaPosition;

        private Sprite link;
        private Sprite zelda;

        private float timeSinceLastMovement = 0f;

        private Keys lastKeyPressed;

        private Rectangle linkPreviousRectangle; // spacebar is released, if no other arrow keys pressed, return to previous sprite

        List<Sprite> zoras = new List<Sprite>();
        List<Vector2> zoraPositions = new List<Vector2>();

        Random random = new Random();

        public SaveThePrincess()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()// Add your initialization logic here
        {
            // set screen width/height
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            link = new Sprite()
            {
                Height = 22,
                Width = 16,
                Speed = 300f
            };
            link.SourceRectangle = new Rectangle(153, 655, link.Width, link.Height);
            linkPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - link.Width, _graphics.PreferredBackBufferHeight / 2 - link.Height);
            linkPreviousRectangle = link.SourceRectangle;

            zelda = new Sprite()
            {
                Height = 23,
                Width = 16,
                Speed = 0
            };
            zelda.SourceRectangle = new Rectangle(38, 4, zelda.Width, zelda.Height);
            zeldaPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - zelda.Width, _graphics.PreferredBackBufferHeight / 2 - zelda.Height);

            for (int i = 0; i < random.Next(5, 20); i++)
            {
                var zora = new Sprite()
                {
                    Height = 23,
                    Width = 16,
                    Speed = 0
                };
                zora.SourceRectangle = new Rectangle(112, 179, zora.Width, zora.Height);
                var zoraPosition = new Vector2(random.Next(50, 750), random.Next(50, 550));
                zoraPositions.Add(zoraPosition);
                zoras.Add(zora);
            }

            base.Initialize();
        }

        protected override void LoadContent() // use this.Content to load your game content here
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            characterTexture = Content.Load<Texture2D>("characters");

        }

        protected override void Update(GameTime gameTime) // Add your update logic here
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            #region Controlls
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up))
            {
                linkPosition.Y -= link.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                link.SourceRectangle = new Rectangle(201, 654, link.Width = LINK_DEFAULT_WIDTH, link.Height = LINK_DEFAULT_HEIGHT);
                linkPreviousRectangle = link.SourceRectangle;
                lastKeyPressed = Keys.Up;
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                linkPosition.Y += link.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                link.SourceRectangle = new Rectangle(153, 655, link.Width = LINK_DEFAULT_WIDTH, link.Height = LINK_DEFAULT_HEIGHT);
                linkPreviousRectangle = link.SourceRectangle;
                lastKeyPressed = Keys.Down;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                linkPosition.X -= link.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                link.SourceRectangle = new Rectangle(226, 653, link.Width = LINK_DEFAULT_WIDTH + 5, link.Height = LINK_DEFAULT_HEIGHT);
                linkPreviousRectangle = link.SourceRectangle;
                lastKeyPressed = Keys.Left;
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                linkPosition.X += link.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                link.SourceRectangle = new Rectangle(173, 653, link.Width = LINK_DEFAULT_WIDTH + 5, link.Height = LINK_DEFAULT_HEIGHT);
                linkPreviousRectangle = link.SourceRectangle;
                lastKeyPressed = Keys.Right;
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                if (lastKeyPressed == Keys.Up)
                {
                    link.SourceRectangle = new Rectangle(243, 690, link.Width = 20, link.Height = 32); // sword, up

                    if (keyState.IsKeyDown(Keys.Space) && keyState.IsKeyDown(Keys.Up))
                        linkPosition.Y -= 8f;
                }

                else if (lastKeyPressed == Keys.Left)
                {
                    link.SourceRectangle = new Rectangle(59, 736, link.Width = 29, link.Height = 21); // sword, left 

                    if (keyState.IsKeyDown(Keys.Space) && keyState.IsKeyDown(Keys.Left))
                        linkPosition.X -= 8f;
                }

                else if (lastKeyPressed == Keys.Right)
                {
                    link.SourceRectangle = new Rectangle(279, 742, link.Width = 29, link.Height = 21); // sword, right

                    if (keyState.IsKeyDown(Keys.Space) && keyState.IsKeyDown(Keys.Right))
                        linkPosition.X += 8f;
                }
                else if (lastKeyPressed == Keys.Down)
                {
                    link.SourceRectangle = new Rectangle(105, 689, link.Width = 23, link.Height = 29); // sword, down

                    if (keyState.IsKeyDown(Keys.Space) && keyState.IsKeyDown(Keys.Down))
                        linkPosition.Y += 8f;
                }
            }

            if (keyState.IsKeyUp(Keys.Space))
            {
                link.SourceRectangle = linkPreviousRectangle;
            }
            #endregion

            #region Boundaries
            // boundaries link
            if (linkPosition.X >= _graphics.PreferredBackBufferWidth - link.Width * 2) // right
                linkPosition.X = _graphics.PreferredBackBufferWidth - link.Width * 2;
            else if (linkPosition.X <= link.Width * 4) // left
                linkPosition.X = link.Width * 4;

            if (linkPosition.Y >= _graphics.PreferredBackBufferHeight - link.Height * 2) // bottom
                linkPosition.Y = _graphics.PreferredBackBufferHeight - link.Height * 2;
            else if (linkPosition.Y <= link.Height * 2.5f) // top
                linkPosition.Y = link.Height * 2.5f;

            // zora movement & boundaries
            ZoraUpdates();

            #endregion

            timeSinceLastMovement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        private void ZoraUpdates()
        {
            for (int i = 0; i < zoraPositions.Count - 1; i++)
            {
                var newPosition = new Vector2();
                if (timeSinceLastMovement > 1 / 120f)
                {
                    var xDirection = zoraPositions[i].X;
                    var yDirection = zoraPositions[i].Y;


                    var randomSignX = random.Next(-100, 100) > 0;
                    var randomSignY = random.Next(-100, 100) > 0;

                    xDirection = randomSignX ? xDirection + random.Next(1, 3) * -1.5f : xDirection + random.Next(1, 3) * 1.5f;
                    yDirection = randomSignY ? yDirection + random.Next(1, 3) * -1 : yDirection + random.Next(1, 3);

                    // boundaries zora
                    if (xDirection >= _graphics.PreferredBackBufferWidth - zoras[i].Width * 2) // right
                        xDirection = _graphics.PreferredBackBufferWidth - zoras[i].Width * 2 - 200;
                    else if (xDirection <= zoras[i].Width * 4) // left
                        xDirection = zoras[i].Width * 4 + 200;

                    if (yDirection >= _graphics.PreferredBackBufferHeight - zoras[i].Height * 2) // bottom
                        yDirection = _graphics.PreferredBackBufferHeight - zoras[i].Height * 2 - 150;
                    else if (yDirection <= zoras[i].Height * 2.5f) // top
                        yDirection = zoras[i].Height * 2.5f + 150;

                    newPosition = new Vector2(xDirection, yDirection);
                    zoraPositions[i] = newPosition;

                    if (i == zoraPositions.Count - 2)
                    {
                        timeSinceLastMovement = 0f;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime) // Add your drawing code here
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _spriteBatch.Draw(characterTexture, new Vector2(linkPosition.X - zelda.Width * 2, linkPosition.Y), link.SourceRectangle, Color.White, 0, new Vector2(link.Width / 2, link.Height / 2), 3.0f, SpriteEffects.None, 1);
            _spriteBatch.Draw(characterTexture, new Vector2(zeldaPosition.X + zelda.Width * 2, zeldaPosition.Y), zelda.SourceRectangle, Color.White, 0, new Vector2(zelda.Width / 2, zelda.Height / 2), 3.0f, SpriteEffects.None, 1);

            // draw the zoras
            for (int i = 0; i < zoraPositions.Count - 1; i++)
            {
                _spriteBatch.Draw(characterTexture, zoraPositions[i], zoras[i].SourceRectangle, Color.White, 0, new Vector2(zoras[i].Width / 2, zoras[i].Height / 2), 3.0f, SpriteEffects.None, 1);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
