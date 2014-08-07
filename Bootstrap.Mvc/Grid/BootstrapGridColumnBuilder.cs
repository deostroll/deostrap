using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Web.Mvc.TwitterBootstrap
{
    public class BootstrapGridColumnBuilder<T>
    {
        public string Title { get; set; }
        public string PropertyName { get; set; }
        public bool IsExpression { get; set; }

        public BootstrapGridColumnBuilder<T> SetTitle(string title)
        {
            Title = title;
            return this;
        }       
        
        public Func<T, object> ColumnFunction { get; set; }
        
        public BootstrapGridColumnBuilder(Expression<Func<T, object>> expression)
        {
            IsExpression = true;
            ColumnFunction = expression.Compile();
        }
        
        public BootstrapGridColumnBuilder(string PropertyName)
        {
            IsExpression = false;
            Title = this.PropertyName = PropertyName;
            ParameterExpression exp = Expression.Parameter(typeof(T));
            var progGetExp = Expression.Property(exp, PropertyName);
            var foo = Expression.Lambda<Func<T, object>>(progGetExp, exp).Compile();
            ColumnFunction = foo;
        }

    }
}
