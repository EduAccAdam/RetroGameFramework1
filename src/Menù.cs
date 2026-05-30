
using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace RetroGameFramework.src
{
    public class Menù
    {
        public static void Menu()
        {
           



            bool continua = true;

            while (continua)
            {
                Console.Clear();

                int width = Console.WindowWidth;
                int height = Console.WindowHeight;
                int CenterX = width / 2;
                int CenterY = height / 2;

                string play = "Play";
                string start = "Click Enter to start";
                string esc = "Click ESC to leave";


                Console.SetCursorPosition(CenterX, CenterY);
                foreach (char a in play)
                {
                    Console.Write(a);
                    Thread.Sleep(50);
                }
                Console.SetCursorPosition(CenterX - 7, CenterY + 1);
                foreach (char b in start)
                {
                    Console.Write(b);
                    Thread.Sleep(50);
                }
                Console.SetCursorPosition(0, 0);
                foreach (char c in esc)
                {
                    Console.Write(c);
                    Thread.Sleep(50);
                }

                Console.CursorVisible = false;

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    continua = false;
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            }

        }
        
    }
}
