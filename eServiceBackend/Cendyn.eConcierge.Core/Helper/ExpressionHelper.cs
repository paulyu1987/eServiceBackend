using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Core.Helper
{
    internal static class ExpressionHelper
    {
        public static string GetPropertyName(Expression<Func<object, object>> property)
        {
            var expr = (property.Body);
            string propertyName = string.Empty;

            if (expr is UnaryExpression)
            {
                propertyName =
                    (((MemberExpression)
                      (((UnaryExpression)
                        (property.Body)).Operand)).Member).Name;
            }
            else if (expr is MemberExpression)
            {
                propertyName = (((MemberExpression)
                           (property.Body)).Member).Name;
            }

            return propertyName;
        }

    }
}
