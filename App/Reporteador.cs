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
            if(_diccionario.TryGetValue(LlaveDiccionario.Evaluación, out IEnumerable<ObjetoEscuelaBase> lista))
                return lista.Cast<Evaluación>();
            else
                return new List<Evaluación>();
                //Escribir en el log de auditoría
        }

        public IEnumerable<string> GetListAsignaturas()
        {
            var listaEvaluaciones = GetListEvaluaciones();

            return (from ev in listaEvaluaciones 
                    select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaliacionesXAsig()
        {
            var dicRta = new Dictionary<string, IEnumerable<Evaluación>>();

            return dicRta;
        }
    }
}