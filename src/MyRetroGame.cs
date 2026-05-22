/*
    Framework to write a simple retro game in pixel art.
    Copyright (C) 2026  Giovanni Volpintesta

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
    
    Contact the author to: john.foxinhead@gmail.com

*/

using RetroGameFramework;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;

namespace RetroGameDemo
{
    internal class MyRetroGame : GameLogic
    {

        public MyRetroGame(GameConfig GameConfig) : base(GameConfig) { }

        // GameConfig is a variable already accessible in methods to retrieve the game configs
        // bool IsPaused() is a function already accessible in methods to check if the game is paused
        // void SetPaused(bool) is a function already accessible in methods to set the game in pause and to resume it

        // GAME DATA
        // Declare here game-specific data that should survive the frame
        float[] ballPosition; // ball position in screen pixels (float to consider also half pixels)
        float[] ballSpeed; // ball speed in pixels per frame (float to consider also half pixels)

        Random random = new Random(Seed:123);

        int meleContatore = 0;
        int meleMangiate = 0;

        float[] melaPosition;

        int ballColor = 1;

        int melaX = -1;
        int melaY = -1;

        GameImage melaImage = new GameImage(new int[,]
        {
            {1,1,1},
            {1,1,1},
            {1,1,1}
        }, AnchorType.Center);

        PaintStyle MelaStyle = PaintStyle.Default;
        
        //
        //
        //
        //
    GameImage ballImage = new GameImage(new int[,] {
            { 0, 1, 0},
            { 1, 1, 1},
            { 1, 1, 0}
        }, AnchorType.Center);
        PaintStyle ballStyle = PaintStyle.Default;

        GameImage starImage = new GameImage(new int[,] {
            { 0,0,0,0,0,1,0,0,0,0,0 },
            { 0,0,0,0,0,1,0,0,0,0,0 },
            { 0,0,0,0,1,1,1,0,0,0,0 },
            { 1,1,1,1,1,1,1,1,1,1,1 },
            { 0,0,1,1,1,1,1,1,1,0,0 },
            { 0,0,0,1,1,1,1,1,0,0,0 },
            { 0,0,0,1,1,1,1,1,0,0,0 },
            { 0,0,1,1,0,0,0,1,1,0,0 },
            { 0,0,1,0,0,0,0,0,1,0,0 }
        }, AnchorType.Center);
        PaintStyle starStyle = PaintStyle.Default;

 
        PaintStyle squareStyle1 = PaintStyle.Default;

        GameImage squareImage2 = GameImage.CreateFromString(
            "*********\n" +
            "*       *\r\n"+
            "* $$$$$ *\n"+
            "* $   $ *\r\n"+
            "* $ . $ *\n"+
            "* $   $ *\r\r\n"+
            "* $$$$$ *\r\r\r\r\n"+
            "*       *\r\r\r\n"+
            "*********",
        new char[] { ' ', '*', '$', '.' }, AnchorType.Center);
        PaintStyle squareStyle2 = PaintStyle.Default;

        GameImage hearthImage = GameImage.CreateFromResource("hearth", AnchorType.Center);
        PaintStyle hearthStyle = PaintStyle.Default;
        //
        // 
        //
        // Initialization call, used to customize GameConfig data (used to customize the engine behaviour)
        protected override void OnInitGameConfig(GameConfig GameConfig)
        {
            GameConfig.Title = "Adam_Snake";

            GameConfig.PixelsMatrixWidth = 70;
            GameConfig.PixelsMatrixHeight = 48;
            GameConfig.PixelSize = 15;

            GameConfig.FrameRate = 20;

            GameConfig.BackgroundColor = System.Drawing.Color.FromArgb(255, 34, 139, 34);
            
            //GameForm.Initializer.ForegroundColor = System.Drawing.Color.White;
            GameConfig.ForegroundColor = System.Drawing.Color.Cyan;

            GameConfig.AdditionalColors = new System.Drawing.Color[] {
                System.Drawing.Color.Red,
                System.Drawing.Color.Orange,
                System.Drawing.Color.Yellow,
                System.Drawing.Color.Purple,
                System.Drawing.Color.Cyan,
                System.Drawing.Color.Blue,
                System.Drawing.Color.Violet,

           };

            int melaX = random.Next(1, 60);
            int melaY = random.Next(1, 40);
        }

        // Called at the start of the first frame of the game.
        // It's main purpose it's to setup the scene.
        private void FirstFrameLoop ()
        {
            // set the ball in the center of the screen
            ballPosition = new float[] { GameConfig.PixelsMatrixWidth / 2, GameConfig.PixelsMatrixHeight / 2 };

            // give the fall a speed
            ballSpeed = new float[] { 0,0 };

            ballStyle.SetColorRemap(1, 2); // start from first additional color;

            squareStyle1.EnsureColorRemapSize(4);

            squareStyle2.SetColorRemap(1, 4);
            squareStyle2.SetColorRemap(2, 5);
            squareStyle2.SetColorRemap(3, 6);

            hearthStyle.SetColorRemap(1, 2);
            hearthStyle.SetColorRemap(2, 8);

            MelaStyle.EnsureColorRemapSize(1);

            MelaStyle.SetColorRemap(1,2);

        }

        // Called once per frame, BEFORE the OnLoopGame event.
        protected override void OnClear(int[,] pixels)
        {
            GameUtils.ClearScreen(pixels);
        }

        // Called once per frame.
        // Here the actual logic happens.
        protected override void OnLoopGame(float deltaTime)
        {
            if (FrameCount == 0)
            {
                FirstFrameLoop();
            }
            else
            {
                UpdateBallPosition();
            }

        }

        // Called once per frame, AFTER the OnLoopGame event.
        protected override void OnDraw(int[,] pixels)
        {
            int screenWidth = pixels.GetLength(0);
            int screenHeight = pixels.GetLength(1);

            //Console.WriteLine(meleContatore);

            melaPosition = new float[] {melaX,melaY};

            // set the foregorund color in the current ball location
            //GameUtils.DrawImageOnScreen(pixels, ballImage, new Point((int)ballPosition[0], (int)ballPosition[1]), ballStyle);

            GameUtils.DrawImageOnScreen(pixels, melaImage, new Point((int)melaPosition[0], (int)melaPosition[1]), MelaStyle);

            DrawBall(pixels, ballColor);

        }

        // Called at the end of the last frame of the game.
        // Its main purpose it's to dispose resources, as the game will end immediately after this call.
        protected override void OnEndGame()
        {
            //Thread.Sleep(2000);
            Console.WriteLine(meleMangiate);
            Environment.Exit(0);
        }

        private void UpdateBallPosition()
        {
            if (meleContatore == 0 && melaX < 0 && melaY < 0)
            {
                melaX = random.Next(1, 70);
                if (ballPosition[0] - melaX < 8)
                {
                    melaX = random.Next(1, 70);
                }
                melaY = random.Next(1, 48);
                if (ballPosition[1] - melaY < 8)
                {
                    melaY = random.Next(1, 48);
                }
                meleContatore++;

            }

            ballPosition[0] += ballSpeed[0];
            ballPosition[1] += ballSpeed[1];

            // Check hits with screen bounds to make the ball bounce
            // The bounce is cheched with a margin to consider the ball dimension
            // In the collision checkings, the radius is always reduced by 0.5 beceuse the center pixel should not be computed.

            if (ballPosition[0] == 1) // horizontal check to the left
            {
                // if the ball is going to the left and it went outside the left screen bound,
                //ballPosition[0] += (ballRadius - 0.5f) - ballPosition[0]; // correct the position after the bounce
                //ballSpeed[0] *= -1; // flip the speed direction
                OnEndGame();
            }
            else if (ballPosition[0] == GameConfig.PixelsMatrixWidth-2) // horizontal check to the right
            {
                // if the ball is going to the right and it went outside the right screen bound,
                //ballPosition[0] -= ballPosition[0] - (GameConfig.PixelsMatrixWidth - 1 - (ballRadius - 0.5f)); // correct the position after the bounce
                //ballSpeed[0] *= -1; // flip the speed direction
                OnEndGame();
            }

            if (ballPosition[1] == 1) // vertical check to the top
            {
                // if the ball is going up and it went outside the top screen bound,
                //ballPosition[1] += (ballRadius - 0.5f) - ballPosition[1]; // correct the position after the bounce
                //ballSpeed[1] *= -1; // flip the speed direction
                OnEndGame();
            }
            else if (ballPosition[1] == GameConfig.PixelsMatrixHeight-2) // vertical check to the bottom
            {
                // if the ball is going down and it went outside the bottom screen bound,
                //ballPosition[1] -= ballPosition[1] - (GameConfig.PixelsMatrixHeight - 1 - (ballRadius - 0.5f)); // correct the position after the bounce
                //ballSpeed[1] *= -1; // flip the speed direction
                OnEndGame();
            }
            if ((ballPosition[0] >= melaPosition[0] - 3 &&
                 ballPosition[0] <= melaPosition[0] + 3 &&
                 ballPosition[1] >= melaPosition[1] - 3 &&
                 ballPosition[1] <= melaPosition[1] + 3))
            {
                meleMangiate++;
                meleContatore--;
                melaX = random.Next(1, 70);
                if (ballPosition[0] - melaX < 8)
                {
                    melaX = random.Next(1, 70);
                }
                melaY = random.Next(1, 47);
                if (ballPosition[1] - melaY < 6)
                {
                    melaY = random.Next(1, 47);
                }
                meleContatore++;
            }


        }

        private void DrawBall(int[,] pixels, int color)
        {
            // BALL EXAMPLE:    718 
            //                  234 
            //                  659  
            
            //                  abc
            //                  def
            //                  ghj

            //int contatore = 0;
            //if (contatore == 0)
            //{
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1], color);  // 2
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 1, color);  // 1
                DrawPixel(pixels, ballPosition[0], ballPosition[1], color);  // 3
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 1, color);  // 5
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1], color);  // 4
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 1, color);  // 6
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] - 1, color);  // 7
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 1, color);  // 9
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] - 1, color);  // 8
                //contatore++;
            //}
            /*else if (contatore == 1)
            {
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 3, color);  // d
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 2, color);  // b
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 3, color);  // e
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 4, color);  // h
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 3, color);  // f
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 4, color);  // g
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 2, color);  // a
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 4, color);  // j
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 2, color);  // c
            }*/          

        }

        private static void DrawPixel(int[,] pixels, float x, float y, int color)
        {
            int posX = (int)x;
            int posY = (int)y;

            if (posX >= 0 && posX < pixels.GetLength(0)
                && posY >= 0 && posY < pixels.GetLength(1))
            {
                // X coordinate is the column index, while Y coordinate is the row index
                pixels[posX, posY] = color;
            }
            
        }

        // Called the first frame a key is pressed, and not called anymore unless the key is released
        protected override void OnKeyDown(Keys KeyCode)
        {
            if (!IsPaused())
            {
                float[] ballSpeedAbs = new float[] { Math.Abs(ballSpeed[0]), Math.Abs(ballSpeed[1]) };

                if (KeyCode == Keys.Up || KeyCode == Keys.W)
                {
                    ballSpeed = new float[] { 0, -1 };
                }
                else if (KeyCode == Keys.Down || KeyCode == Keys.S)
                {
                    ballSpeed = new float[] { 0, 1 };
                }
                else if (KeyCode == Keys.Right || KeyCode == Keys.D)
                {
                    ballSpeed = new float[] { 1, 0 };
                }
                else if (KeyCode == Keys.Left || KeyCode == Keys.A)
                {
                    ballSpeed = new float[] { -1, 0 };
                }
                if (KeyCode == Keys.P)
                {
                    SetPaused(true);
                }
                if (KeyCode == Keys.C)
                {
                    int tmpColor = ballStyle.GetRemappedColor(PaintStyle.FOREGROUND_COLOR_INDEX);
                    tmpColor++;
                    if (tmpColor >= GameConfig.AdditionalColors.Length + 2)
                        tmpColor = 2;
                    ballStyle.SetColorRemap(PaintStyle.FOREGROUND_COLOR_INDEX, tmpColor);

                    ballColor++;
                    if (ballColor >= GameConfig.AdditionalColors.Length + 2)
                        ballColor = 2;
                }  
            }
            else
            {
                if (KeyCode == Keys.P)
                {
                    SetPaused(false);
                }

            }
        }

        // Called if a key has been released (even in the same frame it has been released)
        protected override void OnKeyUp(Keys KeyCode)
        {

        }

        // Called during the frame a key is pressed and in all the following frames until it's released (excluding the frame it's released)
        protected override void OnKeyPress(Keys KeyCode)
        {
            
            
            
        }

    }
}
