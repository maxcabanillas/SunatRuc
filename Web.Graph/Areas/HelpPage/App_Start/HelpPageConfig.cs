// Uncomment the following to provide samples for PageResult<T>. Must also add the Microsoft.AspNet.WebApi.OData
// package to your project.
////#define Handle_PageResultOfT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

#if Handle_PageResultOfT
using System.Web.Http.OData;
#endif

namespace Web.Graph.Areas.HelpPage
{
    /// <summary>
    /// Use this class to customize the Help Page.
    /// For example you can set a custom <see cref="System.Web.Http.Description.IDocumentationProvider"/> to supply the documentation
    /// or you can provide the samples for the requests/responses.
    /// </summary>
    public static class HelpPageConfig
    {
        /// <summary>
        /// Register Help customize
        /// </summary>
        /// <param name="config"></param>
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "Web.Graph.Areas.HelpPage.TextSample.#ctor(System.String)",
            Justification = "End users may choose to merge this string with existing localized resources.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "bsonspec",
            Justification = "Part of a URI.")]
        public static void Register(HttpConfiguration config)
        {
            //// Uncomment the following to use the documentation from XML documentation file.
            config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/ApiGraph.xml")));

            //// Uncomment the following to use "sample string" as the sample for all actions that have string as the body parameter or return type.
            //// Also, the string arrays will be used for IEnumerable<string>. The sample objects will be serialized into different media type 
            //// formats by the available formatters.
            //config.SetSampleObjects(new Dictionary<Type, object>
            //{
            //    {typeof(string), "fads"}
            //});
            // Extend the following to provide factories for types not handled automatically (those lacking parameterless
            // constructors) or for which you prefer to use non-default property values. Line below provides a fallback
            // since automatic handling will fail and GeneratePageResult handles only a single type.
#if Handle_PageResultOfT
            config.GetHelpPageSampleGenerator().SampleObjectFactories.Add(GeneratePageResult);
#endif

            // Extend the following to use a preset object directly as the sample for all actions that support a media
            // type, regardless of the body parameter or return type. The lines below avoid display of binary content.
            // The BsonMediaTypeFormatter (if available) is not used to serialize the TextSample object.
            //config.SetSampleForMediaType(
            //    new TextSample("Binary JSON content. See http://bsonspec.org for details."),
            //    new MediaTypeHeaderValue("application/bson"));

            //// Uncomment the following to use "[0]=foo&[1]=bar" directly as the sample for all actions that support form URL encoded format
            //// and have IEnumerable<string> as the body parameter or return type.
            //config.SetSampleForType("[0]=foo&[1]=bar", new MediaTypeHeaderValue("application/x-www-form-urlencoded"), typeof(IEnumerable<string>));

            //// Uncomment the following to use "1234" directly as the request sample for media type "text/plain" on the controller named "Values"
            //// and action named "Put".

            #region RUC
            var input = new GraphSample(
 @"
query {
	empresa(ruc:""XXXXXXXXXXX"") {
        ruc
        nombre
        tipo_contribuyente
        profesion
        nombre_comercial
        condicion_contribuyente
        estado_contribuyente
        fecha_inscripcion
        fecha_inicio
        departamento
        provincia
        distrito
        direccion
        telefono
        fax
        comercio_exterior
        principal
        secundario1
        secundario2
        rus
        buen_contribuyente
        retencion
        percepcion_vinterna
        percepcion_cliquido
    }
}");
            config.SetSampleRequest(input, new MediaTypeHeaderValue("text/json"), "Ruc", "GET", "query");
            config.SetSampleRequest(input, new MediaTypeHeaderValue("application/x-www-form-urlencoded"), "Ruc", "POST");
            config.SetSampleRequest(null, new MediaTypeHeaderValue("text/json"), "Ruc", "POST");
            config.SetSampleRequest(null, new MediaTypeHeaderValue("application/json"), "Ruc", "POST");

            var resp = new TextSample(
@"
{
  ""data"": {
    ""empresa"": {
      ""ruc"": ""XXXXXXXXXXX"",
      ""nombre"": ""EMPRESA SOCIEDAD ANONIMA 'O ' S.P.S.A."",
      ""tipo_contribuyente"": ""SOCIEDAD ANONIMA"",
      ""profesion"": ""-"",
      ""nombre_comercial"": ""-"",
      ""condicion_contribuyente"": ""HABIDO"",
      ""estado_contribuyente"": ""ACTIVO"",
      ""fecha_inscripcion"": ""09/10/1992"",
      ""fecha_inicio"": ""01/06/1979"",
      ""departamento"": ""LIMA"",
      ""provincia"": ""LIMA"",
      ""distrito"": ""JESUS MARIA"",
      ""direccion"": ""CAL. MORELLI ..."",
      ""telefono"": ""-"",
      ""fax"": ""-"",
      ""comercio_exterior"": ""SIN ACTIVIDAD"",
      ""principal"": ""VTA. MIN. ..."",
      ""secundario1"": ""VENTA ..."",
      ""secundario2"": ""VENTA ..."",
      ""rus"": ""NO"",
      ""buen_contribuyente"": ""-"",
      ""retencion"": ""SI, incorporado ..."",
      ""percepcion_vinterna"": ""-"",
      ""percepcion_cliquido"": ""-""
    }
  }
}");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("text/json"), "Ruc", "GET");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("application/json"), "Ruc", "GET");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("text/json"), "Ruc", "POST");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("application/json"), "Ruc", "POST");
            #endregion

            #region DNI
            input = new GraphSample(
 @"
query {
	persona(ruc:""XXXXXXXX"") {
		primer_nombre
		segundo_nombre
		apellido_paterno
		apellido_materno
    }
}");
            config.SetSampleRequest(input, new MediaTypeHeaderValue("text/json"), "Dni", "GET", "query");
            config.SetSampleRequest(input, new MediaTypeHeaderValue("application/x-www-form-urlencoded"), "Dni", "POST");
            config.SetSampleRequest(null, new MediaTypeHeaderValue("text/json"), "Dni", "POST");
            config.SetSampleRequest(null, new MediaTypeHeaderValue("application/json"), "Dni", "POST");

            resp = new TextSample(
@"
{
  ""data"": {
    ""persona"": {
        ""primer_nombre"" : ""JUAN"",
		""segundo_nombre"" : ""LUIS"",
		""apellido_paterno"" ""SOLIZ"",
		""apellido_materno"" : ""ROBLES""
    }
  }
}");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("text/json"), "Dni", "GET");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("application/json"), "Dni", "GET");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("text/json"), "Dni", "POST");
            config.SetSampleResponse(resp, new MediaTypeHeaderValue("application/json"), "Dni", "POST");
            #endregion
            //config.SetSampleResponse(new ImageSample("http://pngimg.com/upload/banana_PNG842.png"), new MediaTypeHeaderValue("application/json"), "Ruc", "POST");

            //// Uncomment the following to use the image on "../images/aspNetHome.png" directly as the response sample for media type "image/png"
            //// on the controller named "Values" and action named "Get" with parameter "id".
            //config.SetSampleResponse(new ImageSample("./img/imagen.png"), new MediaTypeHeaderValue("application/json"), "Values", "Get", "id");

            //// Uncomment the following to correct the sample request when the action expects an HttpRequestMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Get" were having string as the body parameter.
            //config.SetActualRequestType(typeof(string), "Values", "Get");

            //// Uncomment the following to correct the sample response when the action returns an HttpResponseMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Post" were returning a string.
            //config.SetActualResponseType(typeof(string), "Values", "Post");
        }

#if Handle_PageResultOfT
        private static object GeneratePageResult(HelpPageSampleGenerator sampleGenerator, Type type)
        {
            if (type.IsGenericType)
            {
                Type openGenericType = type.GetGenericTypeDefinition();
                if (openGenericType == typeof(PageResult<>))
                {
                    // Get the T in PageResult<T>
                    Type[] typeParameters = type.GetGenericArguments();
                    Debug.Assert(typeParameters.Length == 1);

                    // Create an enumeration to pass as the first parameter to the PageResult<T> constuctor
                    Type itemsType = typeof(List<>).MakeGenericType(typeParameters);
                    object items = sampleGenerator.GetSampleObject(itemsType);

                    // Fill in the other information needed to invoke the PageResult<T> constuctor
                    Type[] parameterTypes = new Type[] { itemsType, typeof(Uri), typeof(long?), };
                    object[] parameters = new object[] { items, null, (long)ObjectGenerator.DefaultCollectionSize, };

                    // Call PageResult(IEnumerable<T> items, Uri nextPageLink, long? count) constructor
                    ConstructorInfo constructor = type.GetConstructor(parameterTypes);
                    return constructor.Invoke(parameters);
                }
            }

            return null;
        }
#endif
    }
}