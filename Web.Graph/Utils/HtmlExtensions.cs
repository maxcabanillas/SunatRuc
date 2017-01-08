using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Web.Graph.Utils
{
    /// <summary>
    /// Html Custom Extensions.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Get Name for Model
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public static MvcHtmlString DisplayNameRawFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression
        )
        {
            var metaData = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string value = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            return MvcHtmlString.Create(value);
        }
    }
}