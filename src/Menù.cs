
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
            bool scrivi = true;

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
                string info = "Click I to see the Guide";

                if (scrivi)
                {
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
                    Console.SetCursorPosition(width - 25, 0);
                    foreach (char d in info)
                    {
                        Console.Write(d);
                        Thread.Sleep(50);
                    }
                    scrivi = false;
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
                else if (key.Key == ConsoleKey.I)
                {
                    Console.WriteLine("=== ADAM SNAKE ===");
                    Console.WriteLine("OGGETTI:");
                    Console.WriteLine("  Mela normale  = +1 punto");
                    Console.WriteLine("  Mela d'oro    = +2 punti, immunita 8s, 9 mele, velocita aumentata");
                    Console.WriteLine("  Fantasma      = morte istantanea, dura 3 secondi in campo");
                    Console.WriteLine("");
                    Console.WriteLine("CONTROLLI:");
                    Console.WriteLine("  Frecce / WASD = muovi il serpente");
                    Console.WriteLine("  P             = pausa / riprendi");
                    Console.WriteLine("  C             = cambia colore serpente");
                    Console.WriteLine("  ESC           = esci (solo dopo game over)");
                    Console.WriteLine("");
                    Console.WriteLine("REGOLE:");
                    Console.WriteLine("  - Non puoi tornare indietro nella direzione opposta");
                    Console.WriteLine("  - Mordere la coda = game over (tranne durante immunita)");
                    Console.WriteLine("  - Il fantasma mostra un conto alla rovescia di 3 secondi");
                    Console.WriteLine("  - Durante immunita ogni mela vale 2 punti");
                    Console.WriteLine("==================");
                    Console.SetCursorPosition(0, height);
                    Console.WriteLine("Clicca qualsiasi pulsante per tornare indietro");
                    Console.ReadKey(true);
                    scrivi = true;

                }
            }

        }
        
    }
}
