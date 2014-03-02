using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bootstrap.Mvc
{
    public class ColumnFactory<T>
    {
        List<string> PropertyCache;
        public ColumnFactory()
        {
            PropertyCache = new List<string>();
        }
        public void Add(string PropertyName)
        {
            PropertyCache.Add(PropertyName);
        }

        public string[] ModelProperties { get { return PropertyCache.ToArray(); } }
    }
}
