using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace monogame_spaceinvaders
{
    /// <summary>
    /// GameObject Enemy
    /// </summary>
    abstract class Enemy : GameObject
    {
        // Minimum and maximum movement speed of enemy
        private int minSpeed = 20;
        private int maxSpeed = 120;

        /// <summary>
        /// Enemy collision box
        /// </summary>
        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X + Offset.X),
                    (int)(Position.Y + Offset.Y),
                    Sprite.Width,
                    Sprite.Height
                );
            }
        }

        /// <summary>
        /// Enemy constructor
        /// </summary>
        /// <param name="level">Level to place enemy in</param>
        public Enemy(Level level) : base(level)
        {
            Speed = random.Next(minSpeed, maxSpeed);
        }

        /// <summary>
        /// Initialize enemy
        /// </summary>
        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Update enemy. Movement and check bounds
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            CheckBounds();
        }

        /// <summary>
        /// Move enemy down
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Move(GameTime gameTime)
        {
            // Down movement
            velocity.Y = 1;

            velocity.Normalize();

            // Calculate delta time
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the player based on HandleInput, speed and delta time
            Position += ((velocity * Speed) * deltaTime);

        }

        /// <summary>
        /// On collision method for enemy.
        /// Called when colliding with another gameobject
        /// </summary>
        /// <param name="go">This gameobject reference</param>
        /// <param name="other">Other gameobject reference</param>
        public override void OnCollision(GameObject go, GameObject other)
        {
            if (other is Player)
            {
                Respawn();
                other.Color = Color.Red;
                Level.Points--;
            }
            //else if (other is Projectile)
            //{
            //    Respawn();
            //    Level.Points++;
            //}
        }

        /// <summary>
        /// Respawn enemy to over the top of the screen
        /// </summary>
        public override void Respawn()
        {
            Position = new Vector2(random.Next(0, (int)GameManager.ScreenSize.X), -Sprite.Height);
        }

        /// <summary>
        /// Check bounds for enemy.
        /// Respawns enemy if it's over sprite height over screen and under sprite height under screen
        /// </summary>
        public override void CheckBounds()
        {
            // Over sprite height over screen
            if (Position.Y < -Sprite.Height)
            {
                Respawn();
            }
            // Under sprite height under screen
            else if (Position.Y > GameManager.ScreenSize.Y + Sprite.Height)
            {
                Respawn();
            }
        }

    }
}
