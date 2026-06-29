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
using RetroGameFramework.src;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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

        Random random = new Random();

        int meleMangiate = 0;

        int ballColor = 1;

        int[] melaX = new int[] {-1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] melaY = new int[] { -1, -1, -1, -1 ,-1, -1, -1, -1, -1 };
        int MaxMele = 1;

        int[] FantasmaX = new int[] { -1 };
        int[] FantasmaY = new int[] { -1 };

        int N = 0;

        float[] TestaX = new float[9999];
        float[] TestaY = new float[9999];

        int probMelaOro = 0;

        int FantasmaInt = 0;

        bool Immunita = false;
        int immunitaFrames = 0;
        int FramesFantasma = 0;
        bool FantasmaBool = false;
        

        bool end = false;

        int secondiRimanenti = 0;

        int[] allowedColor = new int[] { 2, 3, 5, 6, 7, 8 };
        int indexColor = 0;

        int occhi = 9;
        int bocca = 2;

        GameImage melaImage = new GameImage(new int[,]
        {
            {0,0,0,5,0,0,0,0,0},  // gambo
            {0,0,5,0,3,3,0,0,0},  // gambo + foglia
            {0,1,1,1,3,3,3,0,0},  // spalle + foglia
            {1,2,2,1,1,1,1,1,0},  // luccichio
            {1,2,2,1,1,1,1,1,1},  // luccichio
            {1,1,1,1,1,1,1,1,1},  // corpo pieno
            {1,1,1,1,1,1,1,1,1},  // corpo pieno
            {0,1,1,1,1,1,1,1,0},  // si restringe
            {0,1,1,1,1,1,1,1,0},  // si restringe
            {0,0,1,1,1,1,1,0,0},  // fondo
            {0,0,0,1,1,1,0,0,0},  // punta fondo
        }, AnchorType.Center);

        PaintStyle MelaStyle = PaintStyle.Default;

        GameImage Fantasma = new GameImage(new int[,]
        {
            {0,0,1,1,1,1,1,0,0},  // testa cima
            {0,1,2,2,1,1,1,1,0},  // luccichio
            {1,1,2,2,1,1,1,1,1},  // luccichio
            {1,1,1,1,1,1,1,1,1},  // corpo pieno
            {1,5,5,1,1,1,5,5,1},  // occhi
            {1,5,6,1,1,1,6,5,1},  // pupille rosse
            {1,1,1,1,1,1,1,1,1},  // corpo pieno
            {1,1,1,4,4,4,1,1,1},  // bocca
            {1,1,3,3,1,3,3,1,1},  // ombra fondo
            {1,3,0,1,3,1,0,3,1},  // frange ondulate
            {1,0,0,1,0,1,0,0,1},
        },
            AnchorType.Center);

        PaintStyle FantasmaStyle = PaintStyle.Default;

        
        PaintStyle MelaGoldenStyle = PaintStyle.Default;
        

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



        PaintStyle textStyle = PaintStyle.Default;
        //
        // 
        //
        // Initialization call, used to customize GameConfig data (used to customize the engine behaviour)
        protected override void OnInitGameConfig(GameConfig GameConfig)
        {
            GameConfig.Title = "Adam_Snake";

            GameConfig.PixelsMatrixWidth = 160;
            GameConfig.PixelsMatrixHeight = 111;
            GameConfig.PixelSize = 8;

            GameConfig.FrameRate = 20;

            Menù.Menu();

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
                System.Drawing.Color.Purple,
                System.Drawing.Color.Black,
                System.Drawing.Color.Gold,
                System.Drawing.Color.White,
                System.Drawing.Color.HotPink,
                System.Drawing.Color.LimeGreen,
                System.Drawing.Color.FromArgb(255, 109, 76, 65),
                System.Drawing.Color.LightGoldenrodYellow,
                System.Drawing.Color.FromArgb(255, 220, 230, 255),
                System.Drawing.Color.FromArgb(255, 255, 255, 255),
                System.Drawing.Color.FromArgb(255, 100,  80, 160),
                System.Drawing.Color.FromArgb(255, 140, 110, 200),
                System.Drawing.Color.FromArgb(255,  20,  20,  40),
                System.Drawing.Color.FromArgb(255, 220,  40,  40)

           };

            int melaX = random.Next(6, 151);
            int melaY = random.Next(7, 101);
        }

        // Called at the start of the first frame of the game.
        // It's main purpose it's to setup the scene.
        private void FirstFrameLoop ()
        {

            
            // set the ball in the center of the screen
            ballPosition = new float[] { GameConfig.PixelsMatrixWidth / 2, GameConfig.PixelsMatrixHeight / 2 };

            // give the fall a speed
            ballSpeed = new float[] { 0,0 };

            for (int i = 0; i < TestaX.Length; i++)
            {
                TestaX[i] = ballPosition[0];
                TestaY[i] = ballPosition[1];
            }

            ballStyle.SetColorRemap(1, 2); // start from first additional color;

            MelaStyle.EnsureColorRemapSize(1);
            MelaStyle.SetColorRemap(1,2);
            MelaStyle.EnsureColorRemapSize(2);
            MelaStyle.SetColorRemap(2,12);
            MelaStyle.EnsureColorRemapSize(3);
            MelaStyle.SetColorRemap(3, 13);
            MelaStyle.EnsureColorRemapSize(5);
            MelaStyle.SetColorRemap(5, 14);

            MelaGoldenStyle.EnsureColorRemapSize(1);
            MelaGoldenStyle.SetColorRemap(1,15);
            MelaGoldenStyle.EnsureColorRemapSize(2);
            MelaGoldenStyle.SetColorRemap(2, 16);
            MelaGoldenStyle.EnsureColorRemapSize(3);
            MelaGoldenStyle.SetColorRemap(3, 13);
            MelaGoldenStyle.EnsureColorRemapSize(5);
            MelaGoldenStyle.SetColorRemap(5, 14);

            FantasmaStyle.EnsureColorRemapSize(1);
            FantasmaStyle.SetColorRemap(1, 16);  // corpo → #DCE6FF bianco-viola
            FantasmaStyle.EnsureColorRemapSize(2);
            FantasmaStyle.SetColorRemap(2, 17);  // luccichio → #FFFFFF bianco puro
            FantasmaStyle.EnsureColorRemapSize(3);
            FantasmaStyle.SetColorRemap(3, 18);  // ombra → #6450A0 viola scuro
            FantasmaStyle.EnsureColorRemapSize(4);
            FantasmaStyle.SetColorRemap(4, 19);  
            FantasmaStyle.EnsureColorRemapSize(5);
            FantasmaStyle.SetColorRemap(5, 20); 
            FantasmaStyle.EnsureColorRemapSize(6);
            FantasmaStyle.SetColorRemap(6, 21);

            textStyle.SetColorRemap(1,11);

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

            for (int m = 0; m < MaxMele; m++)
            {
                if (melaX[m] >= 0 && melaY[m] >= 0)
                {
                    if (probMelaOro == 1 && m == 0)
                    {
                        GameUtils.DrawImageOnScreen(pixels, melaImage, new Point(melaX[m], melaY[m]), MelaGoldenStyle);
                    }
                    else if (FantasmaInt == 1 && m == 0)
                    {
                        GameUtils.DrawImageOnScreen(pixels, Fantasma, new Point(melaX[m], melaY[m]), FantasmaStyle);
                    }
                    else
                    {
                        GameUtils.DrawImageOnScreen(pixels, melaImage, new Point(melaX[m], melaY[m]), MelaStyle);
                    }
                }
            }
           
            

            


            DrawBall(pixels, ballColor);

            if (Immunita == true)
            {
                secondiRimanenti = (GameConfig.FrameRate * 8 - immunitaFrames) / GameConfig.FrameRate;
                Writing.Print(pixels, secondiRimanenti.ToString(), Writing.Top_Left,textStyle);
            }
            if (FantasmaBool== true)
            {
                secondiRimanenti = (GameConfig.FrameRate * 3 - FramesFantasma) / GameConfig.FrameRate;
                Writing.Print(pixels, secondiRimanenti.ToString(), Writing.Top_Right, textStyle);
            }
            if (end == true)
            {
                GameConfig.FrameRate = 0;
                GameUtils.ClearScreen(pixels);
                Writing.Print(pixels, $"Hai perso punteggio di: {meleMangiate}{Environment.NewLine}{Environment.NewLine}" +
                    $"ESC per uscire{Environment.NewLine}{Environment.NewLine}", Writing.Top_Left,textStyle);

            }
        }

        // Called at the end of the last frame of the game.
        // Its main purpose it's to dispose resources, as the game will end immediately after this call.
        protected override void OnEndGame()
        {
            end = true;
            FantasmaBool = false;
            Immunita = false;
        }

        private void UpdateBallPosition()
        {
            if (Immunita)
            {
                MaxMele = 9;
            }
            else
            {
                MaxMele = 1;
            }

            for (int i = N - 1; i > 0; i--)
            {
                TestaX[i] = TestaX[i - 1];
                TestaY[i] = TestaY[i - 1];
            }
            if (N > 0)
            {
                TestaX[0] = ballPosition[0];
                TestaY[0] = ballPosition[1];
            }

            for (int m = 0; m < MaxMele; m++)
            {
                if (melaX[m] < 0 || melaY[m] < 0)
                {
                    melaX[m] = random.Next(6, 151);
                    melaY[m] = random.Next(7, 101);
                    if (m == 0)
                    {
                        probMelaOro = random.Next(1, 5);
                        FantasmaInt = random.Next(1, 4);

                        FantasmaBool = false;
                        FramesFantasma = 0;
                        FantasmaX[0] = -1;
                        FantasmaY[0] = -1;
                        if (FantasmaInt == 1 && probMelaOro != 1)
                        {
                            FantasmaX[0] = melaX[0];
                            FantasmaY[0] = melaY[0];
                            FantasmaBool = true;
                            FramesFantasma = 0;
                        }
                        else
                        {
                            FantasmaInt = 0;
                            FantasmaX[0] = -1;
                            FantasmaY[0] = -1;
                        }
                    }
                    
                }
            }
            
            for (int m = MaxMele; m < melaX.Length; m++)
            {
                melaX[m] = -1;
                melaY[m] = -1;
            }


            ballPosition[0] += ballSpeed[0];
            ballPosition[1] += ballSpeed[1];

            for (int i = 0; i < N; i++)
            {
                if (ballPosition[0] == TestaX[i] && ballPosition[1] == TestaY[i] && Immunita == false)
                {
                    OnEndGame();
                }
            }

            if (Immunita)
            {
                probMelaOro = 0;
                immunitaFrames++;
                if (immunitaFrames == GameConfig.FrameRate * 8)
                {
                    Immunita = false;
                    immunitaFrames = 0;
                }
            }
            if (FantasmaBool)
            {
                FramesFantasma++;
                if (FramesFantasma == GameConfig.FrameRate * 3)
                {
                    FantasmaBool = false;
                    FramesFantasma = 0;
                    FantasmaInt = 0;
                    FantasmaX[0] = -1; 
                    FantasmaY[0] = -1; 
                    melaX[0] = -1;     
                    melaY[0] = -1;
                }
            }

            // Check hits with screen bounds to make the ball bounce
            // The bounce is cheched with a margin to consider the ball dimension
            // In the collision checkings, the radius is always reduced by 0.5 beceuse the center pixel should not be computed.

            if (ballPosition[0] <= 0)
            {
                ballPosition[0] = GameConfig.PixelsMatrixWidth - 1;
            }
                
            else if (ballPosition[0] >= GameConfig.PixelsMatrixWidth)
            {
                ballPosition[0] = 0;
            }
                
            if (ballPosition[1] <= 0)
            {
                ballPosition[1] = GameConfig.PixelsMatrixHeight - 1;
            }
                
            else if (ballPosition[1] >= GameConfig.PixelsMatrixHeight)
            {
                ballPosition[1] = 0;
            }
                
            for (int m = 0; m < MaxMele; m++)
            {
                if ((melaX[m] >= 0 && melaY[m] >= 0 &&
                    ballPosition[0] >= melaX[m] - 9 &&
                    ballPosition[0] <= melaX[m] + 9 &&
                    ballPosition[1] >= melaY[m] - 11 &&
                    ballPosition[1] <= melaY[m] + 11))
                {
                    if (N > 0)
                    {
                        TestaX[N] = TestaX[N-1];
                        TestaY[N] = TestaY[N-1];
                        TestaX[N + 1] = TestaX[N - 1];
                        TestaY[N + 1] = TestaY[N - 1];
                    }
                    else
                    {
                        TestaX[N] = ballPosition[0];
                        TestaY[N] = ballPosition[1];
                        TestaX[N + 1] = ballPosition[0];
                        TestaY[N + 1] = ballPosition[1];
                    }
                    if (Immunita == false)
                    {
                        meleMangiate++;
                        N += 1;
                    }
                    else
                    {
                        meleMangiate += 2;
                        N += 2;
                    }

                    if (probMelaOro == 1 && m == 0)
                    {
                        Immunita = true;
                        immunitaFrames = 0;
                        FantasmaBool = false;  
                        FramesFantasma = 0;    
                        FantasmaInt = 0;       
                        FantasmaX[0] = -1;     
                        FantasmaY[0] = -1;
                    }
                    else if (FantasmaInt == 1 && m == 0)
                    {
                        FantasmaBool = true;
                        FramesFantasma = 0;
                        meleMangiate--;
                        OnEndGame();
                    }
                    melaX[m] = -1;
                    melaY[m] = -1;
                }


            }
            for (int f = 0; f < FantasmaX.Length; f++)
            {
                if ((FantasmaX[f] >= 0 && FantasmaY[f] >= 0 &&
                    ballPosition[0] >= FantasmaX[f] - 9 &&
                    ballPosition[0] <= FantasmaX[f] + 9 &&
                    ballPosition[1] >= FantasmaY[f] - 11 &&
                    ballPosition[1] <= FantasmaY[f] + 11))
                {
                    OnEndGame();
                }

            }
        }

            
    

        private void DrawBall(int[,] pixels, int color)
        {
            // BALL EXAMPLE:    718 
            //                  234 
            //                  659

            if (ballSpeed[0] > 0)
            {
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 1, occhi);  // 1
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 1, occhi);  // 5
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1], color);  // 2
                DrawPixel(pixels, ballPosition[0], ballPosition[1], color);  // 3
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1], color);  // 4
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 1, color);  // 6
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] - 1, color);  // 7
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 1, color);  // 9
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] - 1, color);  // 8
                DrawPixel(pixels, ballPosition[0] + 2, ballPosition[1], bocca);
                DrawPixel(pixels, ballPosition[0] + 3, ballPosition[1], bocca);// a
                DrawPixel(pixels, ballPosition[0] + 4, ballPosition[1] + 1, bocca);  // b
            }
            if (ballSpeed[0] < 0)
            {
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 1, occhi);  // 1
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 1, occhi);  // 5
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1], color);  // 2
                DrawPixel(pixels, ballPosition[0], ballPosition[1], color);  // 3
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1], color);  // 4
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 1, color);  // 6
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] - 1, color);  // 7
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 1, color);  // 9
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] - 1, color);  // 8
                DrawPixel(pixels, ballPosition[0] - 2, ballPosition[1], bocca);
                DrawPixel(pixels, ballPosition[0] - 3, ballPosition[1], bocca);// a
                DrawPixel(pixels, ballPosition[0] - 4, ballPosition[1] + 1, bocca);  // b

            }
            if (ballSpeed[1] > 0)
            {
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1], occhi);  // 2
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] , occhi);  // 5
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 1, color);  // 1
                DrawPixel(pixels, ballPosition[0], ballPosition[1], color);  // 3
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 1, color);  // 5
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 1, color);  // 6
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] - 1, color);  // 7
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 1, color);  // 9
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] - 1, color);  // 8
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 2, bocca);
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 3, bocca);// a
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 4, bocca);  // b
            }
            if (ballSpeed[1] < 0)
            {
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1], occhi);  // 2
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1], occhi);  // 4
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 1, color);  // 1
                DrawPixel(pixels, ballPosition[0], ballPosition[1], color);  // 3
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 1, color);  // 5
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 1, color);  // 6
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] - 1, color);  // 7
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 1, color);  // 9
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] - 1, color);  // 8
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 2, bocca);
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 3, bocca);// a
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] - 4, bocca);  // b

            }
            else if (ballSpeed[0] == 0 && ballSpeed[1] ==0)
            {
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1], occhi);  // 2
                DrawPixel(pixels, ballPosition[0], ballPosition[1] - 1, color);  // 1
                DrawPixel(pixels, ballPosition[0], ballPosition[1], color);  // 3
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 1, color);  // 5
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1], occhi);  // 4
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] + 1, color);  // 6
                DrawPixel(pixels, ballPosition[0] - 1, ballPosition[1] - 1, color);  // 7
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 1, color);  // 9
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] - 1, color);  // 8
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 2, bocca);
                DrawPixel(pixels, ballPosition[0], ballPosition[1] + 3, bocca);// a
                DrawPixel(pixels, ballPosition[0] + 1, ballPosition[1] + 4, bocca);  // b
            }



            for (int i = 0; i < N; i++)
            {
                DrawPixel(pixels, TestaX[i] - 1, TestaY[i], color); 
                DrawPixel(pixels, TestaX[i], TestaY[i] - 1, color); 
                DrawPixel(pixels, TestaX[i], TestaY[i], color); 
                DrawPixel(pixels, TestaX[i], TestaY[i] + 1, color); 
                DrawPixel(pixels, TestaX[i] + 1, TestaY[i], color); 
                DrawPixel(pixels, TestaX[i] - 1, TestaY[i] + 1, color); 
                DrawPixel(pixels, TestaX[i] - 1, TestaY[i] - 1, color);
                DrawPixel(pixels, TestaX[i] + 1, TestaY[i] + 1, color);
                DrawPixel(pixels, TestaX[i] + 1, TestaY[i] - 1, color);
            }

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
                    if (ballSpeed[1] != 2)
                    {
                        ballSpeed = new float[] { 0, -2 };
                    }
                    if (Immunita)
                    {
                        ballSpeed = new float[] { 0, -3 };
                    }
                }
                else if (KeyCode == Keys.Down || KeyCode == Keys.S)
                {
                    if (ballSpeed[1] != -2)
                    {
                        ballSpeed = new float[] { 0, 2 };
                    }
                    if (Immunita)
                    {
                        ballSpeed = new float[] { 0, 3 };
                    }
                    
                }
                else if (KeyCode == Keys.Right || KeyCode == Keys.D)
                {
                    if (ballSpeed[0] != -2)
                    {
                        ballSpeed = new float[] { 2, 0 };
                    }
                    if (Immunita)
                    {
                        ballSpeed = new float[] { 3, 0 };
                    }
                    
                }
                else if (KeyCode == Keys.Left || KeyCode == Keys.A)
                {
                    if (ballSpeed[0] != 2)
                    {
                        ballSpeed = new float[] { -2, 0 };
                    }
                    if (Immunita)
                    {
                        ballSpeed = new float[] { -3, 0 };
                    }
                    
                }
                if (end == true && KeyCode == Keys.Escape)
                {
                    Environment.Exit(0);
                }

                if (KeyCode == Keys.P)
                {
                    SetPaused(true);
                }
                if (KeyCode == Keys.C)
                {
                    indexColor++;
                    if (indexColor >= allowedColor.Length)
                    {
                        indexColor = 0;
                    }
                    ballColor = allowedColor[indexColor];
                    ballStyle.SetColorRemap(1, ballColor);
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
