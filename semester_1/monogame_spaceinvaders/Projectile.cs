using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame_spaceinvaders
{
    class Projectile : GameObject
    {
        // To reference the player that shot the projectile
        private Player player;

        //private SoundEffect soundEffect;

        /// <summary>
        /// Projectile collision box
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
        /// Projectile constructor
        /// </summary>
        /// <param name="level">Level to place projectile in</param>
        /// <param name="player">The player the projectile is shot from</param>
        /// <param name="sprite">Sprite of projectile</param>
        /// <param name="soundEffect">Soundeffect for projectile</param>
        public Projectile(Level level, Player player, Texture2D sprite, SoundEffect soundEffect) : base(level)
        {
            this.player = player;
            this.Sprite = sprite;
            level.AddGameObject(new SoundEffectObject(level, soundEffect));
            Speed = 500;
        }

        /// <summary>
        /// Initialize projectile
        /// </summary>
        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Set origin and start position for projectile
        /// </summary>
        /// <param name="content">ContentManager reference</param>
        public override void LoadContent(ContentManager content)
        {
            //Sprites = new Texture2D[3];

            //Sprites[0] = content.Load<Texture2D>("laserBlue05");
            //Sprites[1] = content.Load<Texture2D>("laserBlue15");
            //Sprites[2] = content.Load<Texture2D>("laserBlue09");

            //Sprite = Sprites[random.Next(0, Sprites.Length - 1)];
            //Sprite = Sprites[2];

            //soundEffect = content.Load<SoundEffect>("audio/blindShift");

            SetOrigin();

            Position = new Vector2(player.Position.X, player.Position.Y - 20);
        }

        /// <summary>
        /// Update projectile.
        /// Move and check bounds
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            CheckBounds();
        }

        /// <summary>
        /// Move projectile up
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Move(GameTime gameTime)
        {
            // Up movement
            velocity.Y = -1;

            velocity.Normalize();

            // Calculate delta time
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the player based on HandleInput, speed and delta time
            Position += ((velocity * Speed) * deltaTime);
        }

        /// <summary>
        /// On Collision method for projectile.
        /// Called when colliding with another gameobject
        /// </summary>
        /// <param name="go">This gameobject reference</param>
        /// <param name="other">Other gameobject reference</param>
        public override void OnCollision(GameObject go, GameObject other)
        {
            if (other is Enemy)
            {
                Delete();
                player.Color = Color.White;
                Level.Points += 1;
                other.Respawn();

            }
        }

        /// <summary>
        /// Delete (destroy) projectile from level
        /// </summary>
        public override void Delete()
        {
            Level.projectileCount--;
            base.Delete();
        }

    }
}
