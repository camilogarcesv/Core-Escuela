using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;

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
    }
}