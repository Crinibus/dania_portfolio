using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace monogame_spaceinvaders
{
    /// <summary>
    /// GameObject
    /// </summary>
    abstract class GameObject
    {

        // Used in animation method
        private float animationFPS = 10;
        private float timeElapsed = 0;
        private int currentIndex;

        // Velocity direction of gameobject
        protected Vector2 velocity;

        // Speed of gameobject
        private float speed;

        /// <summary>
        /// Property array of sprites avaliable to gameobject
        /// </summary>
        protected Texture2D[] Sprites { get; set; }

        /// <summary>
        /// Property sprite of gameobject
        /// </summary>
        protected Texture2D Sprite { get; set; }

        /// <summary>
        /// Property speed of gameobject.
        /// Cannot be under 10
        /// </summary>
        protected float Speed
        {
            get { return speed; }
            set { if (value < 10) { speed = 10; } else { speed = value; } }
        }
        protected Random random { get; set; } = new Random();

        protected Vector2 Origin { get; set; }

        /// <summary>
        /// Property offset of gameobject
        /// </summary>
        protected Vector2 Offset { get; set; }

        /// <summary>
        /// Property color of gameobject
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Property level that the gameobject is in
        /// </summary>
        public Level Level { get; protected set; }

        /// <summary>
        /// Property position of gameobject
        /// </summary>
        public Vector2 Position { get; protected set; }

        //public Rectangle CollisionBox
        //{
        //    get
        //    {
        //        return new Rectangle(
        //            (int)(Position.X + Offset.X),
        //            (int) (Position.Y + Offset.Y),
        //            Sprite.Width,
        //            Sprite.Height
        //        );
        //    }
        //}

        /// <summary>
        /// Property collition box for gameobject
        /// </summary>
        public virtual Rectangle CollisionBox { get; }

        /// <summary>
        /// Gameobject contructor
        /// </summary>
        /// <param name="level">Level to place gameobject in</param>
        public GameObject(Level level)
        {
            this.Level = level;
        }

        /// <summary>
        /// Initialize gameobject
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Load gameobject
        /// </summary>
        /// <param name="content">ContentManager reference</param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Set origin to the middle of the gameobjects sprite.
        /// </summary>
        public void SetOrigin()
        {
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);

            Offset = new Vector2(-Origin.X, -Origin.Y);
        }

        /// <summary>
        /// Update gameobject
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw gameobject
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="spriteBatch">Spribatch reference</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color, 0, Origin, 1f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Move gameobject
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public abstract void Move(GameTime gameTime);

        /// <summary>
        /// Check if this GameObject has collided with another GameObject
        /// </summary>
        /// <param name="other">The object we collided with</param>
        public void CheckCollision(GameObject go, GameObject other)
        {
            if (CollisionBox.Intersects(other.CollisionBox))
            {
                OnCollision(go, other);
            }
        }

        /// <summary>
        /// Is executed whenever a collision occurs
        /// </summary>
        /// <param name="other">The object we collided with</param>
        public abstract void OnCollision(GameObject go, GameObject other);

        /// <summary>
        /// Animate gameobject with sprites in sprites-array
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Animate(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * animationFPS);

            // Test
            if (currentIndex > Sprites.Length - 1)
            {
                currentIndex = Sprites.Length - 1;
            }

            Sprite = Sprites[currentIndex];

            if (currentIndex >= Sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }
        }

        /// <summary>
        /// CheckBounds for gameobject
        /// </summary>
        public virtual void CheckBounds()
        {
            if (Position.Y < -Sprite.Height)
            {
                Delete();
            }
            else if (Position.Y > GameManager.ScreenSize.Y + Sprite.Height)
            {
                Delete();
            }
        }

        /// <summary>
        /// Respawn gameobject
        /// </summary>
        public virtual void Respawn()
        {

        }

        /// <summary>
        /// Delete gameobject from level
        /// </summary>
        public virtual void Delete()
        {
            Level.RemoveGameObject(this);
        }
    }
}
