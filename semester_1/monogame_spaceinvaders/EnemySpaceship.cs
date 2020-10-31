using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame_spaceinvaders
{
    class EnemySpaceship : Enemy
    {
        /// <summary>
        /// Enemy spaceship constructor
        /// </summary>
        /// <param name="level">Level to place enemy spaceship in</param>
        public EnemySpaceship(Level level) : base(level)
        {
        }

        /// <summary>
        /// Load textures for enemy spaceships, set start sprite, set origin and set start position
        /// </summary>
        /// <param name="content">ContentManager reference</param>
        public override void LoadContent(ContentManager content)
        {
            // Array with sprites for enemy spaceship
            Sprites = new Texture2D[5];

            // Load all enemy spaceship sprites in array
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i] = content.Load<Texture2D>($"enemies/black/enemyBlack{i + 1}");
            }

            // Random start sprite
            Sprite = Sprites[random.Next(0, Sprites.Length - 1)];

            SetOrigin();

            // Start position
            Position = new Vector2(random.Next(Sprite.Width / 2, (int)GameManager.ScreenSize.X), -Sprite.Height);
        }
    }
}
