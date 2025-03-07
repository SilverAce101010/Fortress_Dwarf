using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dwarven_Fortress
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Texture2D rect;

        int[,] grid;
        int[,] grid_buffer;

        Random rng;

        int[,] R;
        int[,] G;
        int[,] B;

        List<int> tect_point_width;
        List<int> tect_point_height;

        private int _smoothness = 5;

        private int smooth_average = 0;

        private int _width = 200;
        private int _height = 100;

        private int _pixelWidth = 8;
        private int _pixelHeight = 8;

        private int _everest = 255;
        private int _peaks = 195;
        private int _mountains = 185;
        private int _cliffs = 175;
        private int _hills = 160;
        private int _forest = 140;
        private int _wilds = 120;
        private int _plains = 100;
        private int _sands = 95;
        private int _puddles = 85;
        private int _shallows = 80;
        private int _ocean = 30;
        private int _depths = 15;
        private int _trench = 0;

        private int tect_points = 800;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            rng = new Random(); 
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);
            int num_neighbours;
            int border_sea_width;
            int border_sea_height;

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _width * (_pixelWidth);
            _graphics.PreferredBackBufferHeight = _height * (_pixelWidth);
            _graphics.ApplyChanges();

            grid = new int[_width, _height];
            grid_buffer = new int[_width, _height];

            R = new int[_width, _height];
            G = new int[_width, _height];
            B = new int[_width, _height];

            border_sea_height = _height / 9;
            border_sea_width = _width / 10;

            tect_point_width = new List<int>();
            tect_point_height = new List<int>();

            for (int x = 0; x < tect_points; x++)
            {
                tect_point_width.Add(rng.Next(_width));
                tect_point_height.Add(rng.Next(_height));
            }

            // create grid
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (i >= 0 && i < border_sea_width || j >= 0 && j < border_sea_height || i <= _width && i > _width - border_sea_width || j <= _height && j > _height - border_sea_height)
                    {
                        grid[i, j] = rng.Next(_shallows - 25, _shallows);
                    }
                    else
                    {
                        grid[i, j] = rng.Next(255);
                    }
                }
            }

            // create mountains
            for (int i = 0; i < tect_point_height.Count; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    for (int l = -1; l <= 1; l++)
                    {
                        int nk = tect_point_width[i] + k;
                        int nl = tect_point_height[i] + l;
                        if (nk >= 0 && nk < _width && nl >= 0 && nl < _height)
                        {
                            grid[nk, nl] = rng.Next(_cliffs + 10, 255);
                        }
                    }
                }
            }

            // smooth grid
            for (int x = 0; x < _smoothness; x++)
            {
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        smooth_average = 0;
                        num_neighbours = 0;

                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                int ni = i + di;
                                int nj = j + dj;
                                if (ni >= 0 && ni < _width && nj >= 0 && nj < _height)
                                {
                                    smooth_average = smooth_average + grid[ni, nj] + rng.Next(5);
                                    num_neighbours++;
                                }
                                
                            }
                        }
                        smooth_average = smooth_average / num_neighbours;
                        grid_buffer[i, j] = smooth_average;
                    }
                }
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        grid[i, j] = grid_buffer[i, j];
                    }
                }
            }

            // color grid
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if( grid[i, j] > _everest || grid[i, j] < _trench)
                    {
                        R[i, j] = 240;
                        G[i, j] = 0;
                        B[i, j] = 0;
                    }
                    else if (grid[i, j] == _everest)
                    {
                        R[i, j] = 250;
                        G[i, j] = 250;
                        B[i, j] = 250;
                    }
                    else if (grid[i, j] >= _peaks)
                    {
                        R[i, j] = 215;
                        G[i, j] = 215;
                        B[i, j] = 215;
                    }
                    else if (grid[i, j] >= _mountains)
                    {
                        R[i, j] = 137;
                        G[i, j] = 137;
                        B[i, j] = 137;
                    }
                    else if (grid[i,j] >= _cliffs)
                    {
                        R[i, j] = 100;
                        G[i, j] = 100;
                        B[i, j] = 100;
                    }
                    else if (grid[i, j] >= _hills)
                    {
                        R[i, j] = 5;
                        G[i, j] = 106;
                        B[i, j] = 5;
                    }
                    else if (grid[i, j] >= _forest)
                    {
                        R[i, j] = 10;
                        G[i, j] = 128;
                        B[i, j] = 9;
                    }
                    else if (grid[i, j] >= _wilds)
                    {
                        R[i, j] = 18;
                        G[i, j] = 140;
                        B[i, j] = 16;
                    }
                    else if (grid[i, j] >= _plains)
                    {
                        R[i, j] = 5;
                        G[i, j] = 163;
                        B[i, j] = 4;
                    }
                    else if (grid[i, j] >= _sands)
                    {
                        R[i, j] = 194;
                        G[i, j] = 178;
                        B[i, j] = 128;
                    }
                    else if (grid[i, j] >= _puddles)
                    {
                        R[i, j] = 58;
                        G[i, j] = 135;
                        B[i, j] = 167;
                    }
                    else if (grid[i, j] >= _shallows)
                    {
                        R[i, j] = 28;
                        G[i, j] = 105;
                        B[i, j] = 137;
                    }
                    else if (grid[i, j] >= _ocean)
                    {
                        R[i, j] = 47;
                        G[i, j] = 47;
                        B[i, j] = 143;
                    }
                    else if (grid[i, j] >= _depths)
                    {
                        R[i, j] = 48;
                        G[i, j] = 40;
                        B[i, j] = 136;
                    }
                    else if (grid[i, j] == _trench)
                    {
                        R[i, j] = 25;
                        G[i, j] = 25;
                        B[i, j] = 132;
                    }
                }
            }

                    base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            // draw grid
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _spriteBatch.Draw(rect, new Rectangle(i * (_pixelWidth), j * (_pixelHeight), _pixelWidth, _pixelHeight), new Color(R[i, j], G[i, j], B[i, j]));
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}