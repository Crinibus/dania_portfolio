using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame_spaceinvaders
{
    class Asteroid : Enemy
    {
        /// <summary>
        /// Asteroid contructor
        /// </summary>
        /// <param name="level">Level to place asteriod in</param>
        public Asteroid(Level level) : base(level)
        {
        }

        /// <summary>
        /// Load and set texture for asteroid, set origin and set start position
        /// </summary>
        /// <param name="content">ContentManager reference</param>
        public override void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>("meteorGrey_big3");

            SetOrigin();

            // Start position
            Position = new Vector2(random.Next(Sprite.Width / 2, (int)GameManager.ScreenSize.X), -Sprite.Height);
        }

        /// <summary>
        /// Draw asteroid
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="spriteBatch">SpriteBatch reference</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color, 0, Origin, 1f, SpriteEffects.None, 1f);
        }

    }
}
