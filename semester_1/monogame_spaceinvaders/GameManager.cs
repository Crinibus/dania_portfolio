using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame_spaceinvaders
{
    public class GameManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Size used for the window
        public static Rectangle screenBounds = new Rectangle(0, 0, 800, 500);

        // Used to get screen size
        public static Vector2 ScreenSize;

        // The current level
        private Level currentLevel;

        // Used to calculate fps
        double fpsTimer = 0;
        int frameCounter = 0;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            ////Set window size
            _graphics.PreferredBackBufferWidth = screenBounds.Width;
            _graphics.PreferredBackBufferHeight = screenBounds.Height;

            ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            //Disable 60 fps limit
            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            currentLevel = new Level();
            currentLevel.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            currentLevel.LoadContent(this.Content, _spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            currentLevel.Update(gameTime);

            //Calculate fps and update every second
            fpsTimer += gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter++;
            if (fpsTimer > 1.0)
            {
                Window.Title = frameCounter + " FPS - " + _graphics.GraphicsDevice.Adapter.Description;
                fpsTimer -= 1.0;
                frameCounter = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            currentLevel.Draw(gameTime);

            base.Draw(gameTime);

        }
    }
}
