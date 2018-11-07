using Cendyn.eConcierge.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetPropertyValue<T>(this object obj, string property)
        {
            return (T)obj.GetType().GetProperty(property).GetValue(obj, null);
        }

        public static T GetPropertyValue<T>(this object obj, Expression<Func<object, object>> property)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property);
            return (T)obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
