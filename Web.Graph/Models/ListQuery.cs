using System;
using System.Collections.Generic;
using GraphQL.Types;
using Ruc;
using Web.Graph.Utils;

namespace Web.Graph.Models
{
    /// <summary>
    /// Class for Query receive multiple ruc, and multiple responses.
    /// </summary>
    public class ListQuery : ObjectGraphType
    {
        /// <summary>
        /// New Instance of <see cref="ListQuery"/>
        /// </summary>
        public ListQuery()
        {
            Field<ListGraphType<CompanyType>>(
                "empresa",
                arguments: new QueryArguments(new QueryArgument<ListGraphType<StringGraphType>> { Name = "ruc" }),
                resolve: context =>
                {
                    var rucs = context.GetArgument<List<string>>("ruc");
                    var response = new List<Company>();
                    try
                    {
                        var cs = new RucMultipleConsult();
                        var result = cs.GetInfo(rucs.ToArray());
                        var props = typeof(Company).GetProperties();
                        foreach (var res in result)
                        {
                            var empresa = new Company();
                            byte i = 0;
                            foreach (var prop in props)
                            {
                                prop.SetValue(empresa, res[i++].TrimEnd());
                            }
                            response.Add(empresa);
                        }
                    }
                    catch (Exception e)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                    }
                    return response;
                }
            );
        }
    }
}