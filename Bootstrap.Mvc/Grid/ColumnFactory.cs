using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Web.Mvc.TwitterBootstrap;

namespace Bootstrap.Mvc
{
    public class ColumnFactory<T>
    {
        List<string> PropertyCache;
        private int index;
        public IList<BootstrapGridColumnBuilder<T>> ColumnBuilderCache;
        public ColumnFactory()
        {
            index = 0;
            PropertyCache = new List<string>();
            ColumnBuilderCache = new List<BootstrapGridColumnBuilder<T>>();
        }
        public BootstrapGridColumnBuilder<T> Add(string PropertyName)
        {
            index++;
            PropertyCache.Add(PropertyName);
            var bldr = new BootstrapGridColumnBuilder<T>(PropertyName);
            ColumnBuilderCache.Add(bldr);
            return bldr;
        }

        public BootstrapGridColumnBuilder<T> Add(Expression<Func<T, object>> ColumnBuilderExpression)
        {
            index++;
            string memberName;
            if (ColumnBuilderExpression.Body is MemberExpression)
            {
                memberName = ((MemberExpression)ColumnBuilderExpression.Body).Member.Name;
                PropertyCache.Add(memberName);
            }
            else
            {
                memberName = "Col" + index;
            }
            return new BootstrapGridColumnBuilder<T>(memberName);
        }
        public string[] ModelProperties { get { return PropertyCache.ToArray(); } }
    }
}
