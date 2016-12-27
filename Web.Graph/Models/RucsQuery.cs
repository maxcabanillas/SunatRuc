using System;
using System.Linq;
using GraphQL.Types;
using Ruc;
using Web.Graph.Utils;

namespace Web.Graph.Models
{
    /// <summary>
    /// Query Ruc
    /// </summary>
    public class RucsQuery : ObjectGraphType
    {
        /// <summary>
        /// new Instance of <see cref="RucsQuery"/>
        /// </summary>
        public RucsQuery()
        {
            Field<CompanyType>(
                "empresa",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "ruc" }),
                resolve: context =>
                {
                    var ruc = context.GetArgument<string>("ruc");
                    try
                    {
                        //Validar ruc.
                        var empresa = new Company();
                        if (ruc == null || ruc.Length != 11) return empresa;
                        var cs = new RucMultipleConsult();
                        var result = cs.GetInfo(ruc);
                        var type = empresa.GetType();
                        var comp = result.First();
                        byte i = 0;
                        foreach (var prop in type.GetProperties())
                        {
                            prop.SetValue(empresa, comp[i++].TrimEnd());
                        }
                        return empresa;
                    }
                    catch (Exception e)
                    {
                        ExceptionUtility.LogException(e, "Consultando :" + ruc);
                    }
                    return null;
                }
            );
        }
    }
}