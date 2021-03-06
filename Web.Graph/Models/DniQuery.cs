﻿using System;
using System.Linq;
using GraphQL.Types;
using Ruc;
using Web.Graph.Utils;

namespace Web.Graph.Models
{
    /// <summary>
    /// Class Dni Query.
    /// </summary>
    public class DniQuery : ObjectGraphType
    {
        /// <summary>
        /// New Instance for <see cref="DniQuery"/>
        /// </summary>
        public DniQuery()
        {
            Field<PersonType>(
            "persona",
            arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "dni" }),
            resolve: context =>
            {
                var dni = context.GetArgument<string>("dni");
                byte intents = 3;
                start:
                try
                {
                    //Validar DNI.
                    if (dni == null || dni.Length != 8) return null;
                    var cs = new DniConsult();
                    var result = cs.Get(dni);
                    var persona = new Person
                    {
                        ApellidoPaterno = result[1],
                        ApellidoMaterno = result[2]
                    };
                    var names = result[0].Split(' ');
                    persona.PrimerNombre = names[0];
                    persona.SegundoNombre = names.Length > 1 ? string.Join(" ", names.Skip(1)) : string.Empty;
                    return persona;
                }
                catch (Exception e)
                {
                    if(e is CaptchaException && intents-- > 0)
                        goto start;
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                }
                return null;
            }
        );
        }
    }
}