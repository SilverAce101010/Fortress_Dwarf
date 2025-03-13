using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography;
using System.Transactions;

namespace Player_movement
{

    public class Game1 : Game
    {

        private Point GetMousePosition()
        {
            MouseState mouseState = Mouse.GetState();
            return new Point(mouseState.X, mouseState.Y);
        }


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Texture2D rect;

        private int screen_width = 500;
        private int screen_hight = 250;

        private int pixel_width = 10;
        private int pixel_hight = 10;

        private int x = 10;
        private int y = 10;

        private int xdirection = 1;
        private int ydirection = 1;

        private int speed = 2;

        private int mousex;
        private int mousey;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = screen_width;
            _graphics.PreferredBackBufferHeight = screen_hight;
            _graphics.ApplyChanges();


            base.Initialize();
        }



        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.Chocolate });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouse = Mouse.GetState(); // Add this line to get the current mouse state
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = GetMousePosition();
                mousex = mousePosition.X;
                mousey = mousePosition.Y;
            }

            if (x < mousex) { xdirection = 1; }
            if (x == mousex) { xdirection = 0; }
            if (x > mousex) { xdirection = -1; }

            if (y < mousey) { ydirection = 1; }
            if (y == mousey) { ydirection = 0; }
            if (y > mousey) { ydirection = -1; }

            x += (xdirection * speed);
            y += (ydirection * speed);


            // TODO: Add your update logic here 

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(rect, new Rectangle(x, y, 10, 10), Color.Chocolate);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
