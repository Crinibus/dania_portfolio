using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace monogame_spaceinvaders
{
    class Level
    {
        /// <summary>
        /// Gameobjects in the game.
        /// </summary>
        private List<GameObject> gameObjects = new List<GameObject>();

        /// <summary>
        /// Used for adding gameobjects on the next update.
        /// </summary>
        private List<GameObject> gameObjectsToAdd = new List<GameObject>();

        /// <summary>
        /// Used for removing gameobjects on the next update
        /// </summary>
        private List<GameObject> gameObjectsToRemove = new List<GameObject>();

        /// <summary>
        /// SpriteBatch reference used for drawing gameobjects in the level
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Collision texture (1 white pixel)
        /// </summary>
        public Texture2D collisionTexture;

        /// <summary>
        /// Background texture
        /// </summary>
        public Texture2D backgroundTexture;

        /// <summary>
        /// Background music
        /// </summary>
        public Song backgroundMusic;

        /// <summary>
        /// Standard font
        /// </summary>
        public SpriteFont standardFont;

        /// <summary>
        /// Content manager reference used for loading objects at runtime.
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// Number of player points
        /// </summary>
        private int points;

        /// <summary>
        /// Number of projectiles
        /// </summary>
        public int projectileCount = 0;

        /// <summary>
        /// Property for player points (cannot get under zero)
        /// </summary>
        public int Points
        {
            get { return points; }
            set
            {
                if (value < 0)
                {
                    points = 0;
                }
                else
                {
                    points = value;
                }
            }
        }

        /// <summary>
        /// Initialize level and gameobjects in the level
        /// </summary>
        public void Initialize()
        {
            // Add player
            gameObjects.Add(new Player(this));

            // Add all start asteroids
            for (int i = 0; i < 2; i++)
            {
                gameObjects.Add(new Asteroid(this));
            }

            // Add all start enemy ships
            for (int i = 0; i < 2; i++)
            {
                gameObjects.Add(new EnemySpaceship(this));
            }

            // Initialize every gameobject at the start of the level
            foreach (GameObject go in gameObjects)
            {
                go.Initialize();
            }
        }

        /// <summary>
        /// Load level and gameobjects in the level
        /// </summary>
        /// <param name="content">ContentManager</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.content = content;

            // Load standard font
            standardFont = content.Load<SpriteFont>("standardFont");

            // Load background texture
            backgroundTexture = content.Load<Texture2D>("backgrounds/purple");

            // Load and play background music
            backgroundMusic = content.Load<Song>("audio/BlindShift");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

            // Load every gameobject
            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(content);
            }

            // Load collision texture
            collisionTexture = content.Load<Texture2D>("CollisionTexture");

        }

        /// <summary>
        /// Update level and the gameobjects in the level
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Update(GameTime gameTime)
        {
            // Add entities created in previous frame.
            foreach (GameObject go in gameObjectsToAdd)
            {
                gameObjects.Add(go);
                go.Initialize();
                go.LoadContent(content);
            }

            gameObjectsToAdd.Clear();

            foreach (GameObject go in gameObjectsToRemove)
            {
                gameObjects.Remove(go);
            }

            gameObjectsToRemove.Clear();

            // Update every gameobject in level
            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);

                // Check collision for all gameobjects
                foreach (GameObject other in gameObjects)
                {
                    go.CheckCollision(go, other);
                }
            }

        }

        /// <summary>
        /// Adds a gameobject to the level.
        /// Will be addded before the next frame.
        /// </summary>
        /// <param name="go">Gameobject to add</param>
        public void AddGameObject(GameObject go)
        {
            gameObjectsToAdd.Add(go);
        }

        /// <summary>
        /// Removes a gameobject from the level
        /// </summary>
        /// <param name="go">Gameobject to remove</param>
        public void RemoveGameObject(GameObject go)
        {
            gameObjectsToRemove.Add(go);
        }

        /// <summary>
        /// Draw level and the gameobjects in the level
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, (int)GameManager.ScreenSize.X, (int)GameManager.ScreenSize.Y), Color.White);

            // Draw every gameobject in level
            foreach (GameObject go in gameObjects)
            {
                go.Draw(gameTime, spriteBatch);
                DrawCollisionBox(go);
            }

            // Draw how many points and the projectile count on screen
            spriteBatch.DrawString(standardFont, $"Points: {points}", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(standardFont, $"Projectile count: {projectileCount}", new Vector2(0, 15), Color.White);

#if DEBUG
            // Draw y-position of each gameobject on the screen
            for (int i = 0; i < gameObjects.Count; i++)
            {
                spriteBatch.DrawString(standardFont, $"{i} > {gameObjects[i].GetType().Name} y-pos: {(int)gameObjects[i].Position.Y}", new Vector2(0, i * 15 + 30), Color.White);
            }
#endif

            spriteBatch.End();
        }

        /// <summary>
        /// Draw collision box of gameobject
        /// </summary>
        /// <param name="go">Gameobject to draw collision box for</param>
        private void DrawCollisionBox(GameObject go)
        {
            Rectangle topLine = new Rectangle(go.CollisionBox.X, go.CollisionBox.Y, go.CollisionBox.Width, 1);
            Rectangle buttomLine = new Rectangle(go.CollisionBox.X, go.CollisionBox.Y + go.CollisionBox.Height, go.CollisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(go.CollisionBox.X + go.CollisionBox.Width, go.CollisionBox.Y, 1, go.CollisionBox.Height);
            Rectangle leftLine = new Rectangle(go.CollisionBox.X, go.CollisionBox.Y, 1, go.CollisionBox.Height);

            spriteBatch.Draw(collisionTexture, topLine, Color.Red);
            spriteBatch.Draw(collisionTexture, buttomLine, Color.Red);
            spriteBatch.Draw(collisionTexture, rightLine, Color.Red);
            spriteBatch.Draw(collisionTexture, leftLine, Color.Red);
        }

    }
}
