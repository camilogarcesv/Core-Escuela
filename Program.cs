using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;
            AppDomain.CurrentDomain.ProcessExit += (o, s)=> Printer.Beep(500,1000,1);
            AppDomain.CurrentDomain.ProcessExit -= AccionDelEvento;

            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");
            // Printer.Beep();
            ImprimirCursosEscuela(engine.Escuela);

            var dictmp = engine.GetDiccionarioObjetos();

            engine.ImprimirDiccionario(dictmp, true);
        }

        private static void AccionDelEvento(object sender, EventArgs e)
        {
            Printer.WriteTitle("SALIENDO");
            Printer.Beep(500, 500, 3);
            Printer.WriteTitle("SALIÓ");
        }

        private static void ImprimirCursosEscuela(Escuela escuela)
        {
            Printer.WriteTitle("Cursos de la Escuela");

            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre}, Id {curso.UniqueID}");
                }
            }
        }
    }
}
