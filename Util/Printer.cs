using System;
using static System.Console;

namespace CoreEscuela.Util
{
    public static class Printer
    {
        public static void DrawLine(int tamanio = 10)
        {
            WriteLine("".PadLeft(tamanio, '='));
        }
        public static void WriteTitle(string titulo)
        {
            var tamanio = titulo.Length + 4;
            DrawLine(tamanio);
            WriteLine($"| {titulo} |");
            DrawLine(tamanio);
        }

        public static void Beep(int hz = 1000, int tiempo = 500, int cantidadVeces = 1)
        {
            while (cantidadVeces-- > 0)
            {
                Console.Beep(hz, tiempo);
            }
        }
    }
}