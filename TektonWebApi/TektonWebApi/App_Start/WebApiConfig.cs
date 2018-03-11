using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace TektonWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "asignarCapacidad",
                routeTemplate: "api/{controller}/asignarCapacidad/{idSala}/{capacidad}",
                defaults: new { idSala = RouteParameter.Optional, capacidad = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "registrarCharla",
                routeTemplate: "api/{controller}/registrarCharla/{idSala}/{idSpeaker}/{horarioInicio}/                                          {horarioFin}/{nombreCharla}",
                defaults: new { idSala = RouteParameter.Optional, idSpeaker = RouteParameter.Optional, 
                                horarioInicio = RouteParameter.Optional, horarioFin = RouteParameter.Optional,
                                nombreCharla = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            // Quite los comentarios de la siguiente línea de código para habilitar la compatibilidad de consultas para las acciones con un tipo de valor devuelto IQueryable o IQueryable<T>.
            // Para evitar el procesamiento de consultas inesperadas o malintencionadas, use la configuración de validación en QueryableAttribute para validar las consultas entrantes.
            // Para obtener más información, visite http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // Para deshabilitar el seguimiento en la aplicación, incluya un comentario o quite la siguiente línea de código
            // Para obtener más información, consulte: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
