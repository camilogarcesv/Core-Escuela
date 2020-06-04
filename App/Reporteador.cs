using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;


namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc == null)
                throw new ArgumentNullException(nameof(dicObsEsc));

            _diccionario = dicObsEsc;
        }

        public IEnumerable<Evaluación> GetListEvaluaciones()
        {
            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluación, out IEnumerable<ObjetoEscuelaBase> lista))
                return lista.Cast<Evaluación>();
            else
                return new List<Evaluación>();
            //Escribir en el log de auditoría
        }

        public void ImprimirEvaluaciones()
        {
            Printer.WriteTitle("Evaluaciones");
            var lisEval = GetListEvaluaciones();

            foreach (var ev in lisEval)
            {
                WriteLine($"Evaluación: {ev.Nombre}");
            }
        }

        public IEnumerable<string> GetListAsignaturas()
        {
            return GetListAsignaturas(out var dummy);
        }

        public IEnumerable<string> GetListAsignaturas(out IEnumerable<Evaluación> listaEvaluaciones)
        {
            listaEvaluaciones = GetListEvaluaciones();

            return (from ev in listaEvaluaciones
                    select ev.Asignatura.Nombre).Distinct();
        }

        internal void ImprimirAsignaturas()
        {
            Printer.WriteTitle("Asignaturas");
            var lisAsig = GetListAsignaturas();

            foreach (var asig in lisAsig)
            {
                WriteLine($"Asignatura: {asig}");
            }
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaluacionesXAsig()
        {
            var dicRta = new Dictionary<string, IEnumerable<Evaluación>>();
            var listaAsig = GetListAsignaturas(out var listaEvaluaciones);
            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEvaluaciones
                                where eval.Asignatura.Nombre == asig
                                select eval;
                dicRta.Add(asig, evalsAsig);
            }

            return dicRta;
        }

        public void ImprimirEvaluacionesPorAsignatura()
        {
            Printer.WriteTitle("Evaluaciones por Asignatura");
            var listaEvalXAsig = GetDicEvaluacionesXAsig();

            foreach (var evAsig in listaEvalXAsig)
            {
                WriteLine($"Evaluacion por asignatura: {evAsig.Key}");
                foreach (var item in evAsig.Value)
                {
                    var tmp = item as Evaluación;
                    WriteLine(tmp.Nombre);
                }
            }
        }



        public Dictionary<string, IEnumerable<AlumnoPromedio>> GetPromedioAlumnoPorAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<AlumnoPromedio>>();
            var dicEvalXAsig = GetDicEvaluacionesXAsig();

            foreach (var asigConEval in dicEvalXAsig)
            {
                //elemento anónimo
                var promediosAlumn = from eval in asigConEval.Value
                                     group eval by new
                                     {
                                         eval.Alumno.UniqueID,
                                         eval.Alumno.Nombre
                                     }
                            into grupoEvalsAlumno
                                     select new AlumnoPromedio
                                     {
                                         AlumnoId = grupoEvalsAlumno.Key.UniqueID,
                                         AlumnoNombre = grupoEvalsAlumno.Key.Nombre,
                                         Promedio = grupoEvalsAlumno.Average(evaluacion => evaluacion.Nota)
                                     };
                rta.Add(asigConEval.Key, promediosAlumn);
            }

            return rta;
        }



        internal void ImprimirPromedioAlumnosPorAsignatura()
        {
            Printer.WriteTitle("Promedio Alumnos por Asignatura");
            var lisPromedioXAsig = GetPromedioAlumnoPorAsignatura();
            foreach (var item in lisPromedioXAsig)
            {
                WriteLine($"Asignatura: {item.Key}");
                foreach (var alum in item.Value)
                {
                    var tmp = alum as AlumnoPromedio;
                    WriteLine($"Nombre {tmp.AlumnoNombre} Promedio: {tmp.Promedio}");
                }
            }
        }


        public Dictionary<string, IEnumerable<AlumnoPromedio>> GetTopAlumnosXAsignatura(int top)
        {
            var rta = new Dictionary<string, IEnumerable<AlumnoPromedio>>();

            var PromAlumnXAsig = GetPromedioAlumnoPorAsignatura();
            foreach (var asig in PromAlumnXAsig)
            {
                var TopAlumnos = from alumn in
                                asig.Value.OrderByDescending(al => al.Promedio).Take(top)
                                 select alumn;
                rta.Add(asig.Key, TopAlumnos);
            }
            return rta;
        }

        internal void ImprimirTopPromedioAlumnosPorAsignatura(int top)
        {
            Printer.WriteTitle("Top Promedio de Alumnos Por Asignatura");
            var listaTopEstudiantes = GetTopAlumnosXAsignatura(top);

            foreach (var topEst in listaTopEstudiantes)
            {
                WriteLine($"Asignatura: {topEst.Key}");
                foreach (var item in topEst.Value)
                {
                    var tmp = item as AlumnoPromedio;
                    WriteLine($"Nombre {tmp.AlumnoNombre} Promedio: {tmp.Promedio}");
                }
            }
        }
    }
}