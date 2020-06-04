using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.App;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            /*AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;
            AppDomain.CurrentDomain.ProcessExit += (o, s)=> Printer.Beep(500,1000,1);
            AppDomain.CurrentDomain.ProcessExit -= AccionDelEvento;*/

            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");

            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            var evalList = reporteador.GetListEvaluaciones();
            var listaAsig = reporteador.GetListAsignaturas();
            var listaEvalXAsig = reporteador.GetDicEvaluacionesXAsig();
            var promedioXAsig = reporteador.GetPromedioAlumnoPorAsignatura();
            var topEstudiantes = reporteador.GetTopAlumnosXAsignatura(5);

            // Reto menu consola
            ConsoleKeyInfo opcion;
            // opcion = Console.ReadKey();
            do
            {
                Console.Clear();
                Printer.WriteTitle("BIENVENIDO AL PROGRAMA ESCUELA");
                WriteLine(engine.Escuela);
                WriteLine("1 - Lista de cursos.");
                WriteLine("2 - Reporte de evaluaciones");
                WriteLine("3 - Reporte de asignaturas.");
                WriteLine("4 - Reporte de evaluaciones por asignatura.");
                WriteLine("5 - Reporte del promedio de alumnos por asignatura.");
                WriteLine("6 - Reporte Top X Estudiantes por asignatura.");
                WriteLine("Presione ENTER o ESC para salir");

                opcion = ReadKey();
                Console.Clear();
                switch (opcion.Key)
                {
                    case ConsoleKey.D1:
                        ImprimirCursosEscuela(engine.Escuela);
                        presioneCualquierTecla();
                        break;
                    case ConsoleKey.D2:
                        reporteador.ImprimirEvaluaciones();
                        presioneCualquierTecla();
                        break;
                    case ConsoleKey.D3:
                        reporteador.ImprimirAsignaturas();
                        presioneCualquierTecla();
                        break;
                    case ConsoleKey.D4:
                        reporteador.ImprimirEvaluacionesPorAsignatura();
                        presioneCualquierTecla();
                        break;
                    case ConsoleKey.D5:
                        reporteador.ImprimirPromedioAlumnosPorAsignatura();
                        presioneCualquierTecla();
                        break;
                    case ConsoleKey.D6:
                        int top;
                        string cadenaTop;

                        Write("\nDigite la cantidad de estudiantes, luego ENTER: ");
                        cadenaTop = ReadLine();
                        top = int.Parse(cadenaTop);
                        reporteador.ImprimirTopPromedioAlumnosPorAsignatura(top);
                        presioneCualquierTecla();
                        break;
                }
                
            } while (opcion.Key != ConsoleKey.Escape && opcion.Key != ConsoleKey.Enter);

        }

        private static void presioneCualquierTecla()
        {
            System.Console.WriteLine("\n------ Presione cualquier tecla -------");
            System.Console.WriteLine("         para regresar al Menu");
            Console.ReadKey();
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
