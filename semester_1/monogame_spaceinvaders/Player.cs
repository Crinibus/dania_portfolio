using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame_spaceinvaders
{
    /// <summary>
    /// GameObject Player
    /// </summary>
    class Player : GameObject
    {
        //private bool SpaceReleased = true;

        private Texture2D projectileTexture;
        private SoundEffect projectileSoundEffect;

        //private int maxProjectileCount = 20;

        // Cooldown until
        private double cooldownUntil;
        // Cooldown time
        private double cooldownTime = 0.2d;

        // keys
        private Keys movementKeyUp = Keys.Up;                   // move up
        private Keys movementKeyDown = Keys.Down;               // move down
        private Keys movementKeyLeft = Keys.Left;               // move left
        private Keys movementKeyRight = Keys.Right;             // move right
        private Keys movementKeySpeedUp = Keys.LeftShift;       // increase movement speed
        private Keys movementKeySpeedDown = Keys.LeftControl;   // decrease movement speed
        private Keys shootKey = Keys.Space;                     // shoot

        /// <summary>
        /// Player collision box
        /// </summary>
        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X + Offset.X),
                    (int)(Position.Y + Offset.Y),
                    Sprite.Width,
                    Sprite.Height - 20
                );
            }
        }

        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="level">Level to place player in</param>
        public Player(Level level) : base(level)
        {
            velocity = Vector2.Zero;
            Speed = 150f;
            Position = new Vector2(GameManager.ScreenSize.X / 2, GameManager.ScreenSize.Y - 300);
        }

        /// <summary>
        /// Initialize player
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// Load textures for player and texture and sound for projectile.
        /// Set start sprite.
        /// Set origin on sprite.
        /// </summary>
        /// <param name="content">ContentManager reference</param>
        public override void LoadContent(ContentManager content)
        {
            // Array with all player sprites
            Sprites = new Texture2D[4];

            // Load all player normal forward sprites in array
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i] = content.Load<Texture2D>($"player/normalFwd/{i + 1}fwd");
            }

            // Random start sprite
            Sprite = Sprites[random.Next(0, Sprites.Length - 1)];

            SetOrigin();

            projectileTexture = content.Load<Texture2D>("laser/laserBlue05");

            projectileSoundEffect = content.Load<SoundEffect>("audio/laserSound1");

        }

        /// <summary>
        /// Update player.
        /// Handle input, move, screenwrap and animate
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Move(gameTime);
            ScreenWrap();
            Animate(gameTime);
        }

        /// <summary>
        /// Draw player
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="spriteBatch">SpriteBatch reference</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color, 0, Origin, 1f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Move player
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Move(GameTime gameTime)
        {
            // Calculate delta time
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the player based on HandleInput, speed and delta time
            Position += ((velocity * Speed) * deltaTime);
        }

        /// <summary>
        /// Handle input for player
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void HandleInput(GameTime gameTime)
        {
            // Reset velocity = stop moving when no keys are pressed
            velocity = Vector2.Zero;

            // Get keyboard state
            KeyboardState kbState = Keyboard.GetState();

            // Check if user press a key:
            // Up arrow
            if (kbState.IsKeyDown(movementKeyUp))
            {
                // Move up
                velocity.Y = -1;
            }
            // Down arrow
            if (kbState.IsKeyDown(movementKeyDown))
            {
                // Move down
                velocity.Y = 1;
            }
            // Left arrow
            if (kbState.IsKeyDown(movementKeyLeft))
            {
                // Move left
                velocity.X = -1;
            }
            // Right arrow
            if (kbState.IsKeyDown(movementKeyRight))
            {
                // Move right
                velocity.X = 1;
            }

            // Left shift
            if (kbState.IsKeyDown(movementKeySpeedUp))
            {
                Speed += 0.1f;
            }
            // Left control
            if (kbState.IsKeyDown(movementKeySpeedDown))
            {
                Speed -= 0.1f;
            }

            // Space key
            if (kbState.IsKeyDown(shootKey) && gameTime.TotalGameTime.TotalSeconds > cooldownUntil)
            {
                //if (SpaceReleased)
                //{
                //    if (Level.projectileCount < maxProjectileCount)
                //    {
                Level.AddGameObject(new Projectile(Level, this, projectileTexture, projectileSoundEffect));
                // Play sound for shooting a projectile
                projectileSoundEffect.Play();
                Level.projectileCount++;
                //        SpaceReleased = false;
                //    }
                //}

                // Set until the player can shoot again
                cooldownUntil = gameTime.TotalGameTime.TotalSeconds + cooldownTime;
            }
            //else
            //{
            //    SpaceReleased = true;
            //}

            // If moving, then normalize vector
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
        }

        /// <summary>
        /// Screenwrap player when player moves off screen
        /// </summary>
        private void ScreenWrap()
        {
            // Check for:
            // Right border
            if (Position.X > GameManager.ScreenSize.X + (Sprite.Width / 2))
            {
                Position = new Vector2(-(Sprite.Width / 2), Position.Y);
            }
            // Left border
            else if (Position.X < -Sprite.Width / 2)
            {
                Position = new Vector2(GameManager.ScreenSize.X + (Sprite.Width / 2), Position.Y);
            }
            // Check for:
            // Buttom border
            if (Position.Y > GameManager.ScreenSize.Y + (Sprite.Height / 2))
            {
                Position = new Vector2(Position.X, -Sprite.Height / 2);
            }
            // Top border
            else if (Position.Y < -(Sprite.Height / 2))
            {
                Position = new Vector2(Position.X, GameManager.ScreenSize.Y + (Sprite.Height / 2));
            }
        }

        /// <summary>
        /// On collision method for player.
        /// Called when colliding with another gameobject
        /// </summary>
        /// <param name="go">This gameobject reference</param>
        /// <param name="other">Other gameobject reference</param>
        public override void OnCollision(GameObject go, GameObject other)
        {
            //if (other is Enemy)
            //{
            //    Color = Color.Red;
            //}
            //Level.Points--;
        }

    }
}
