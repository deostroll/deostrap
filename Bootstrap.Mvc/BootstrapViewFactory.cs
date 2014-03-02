using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Mvc.TwitterBootstrap
{
    public class BootstrapViewFactory
    {
        public BootstrapGrid<T> Grid<T>(IEnumerable<T> data)
        {
            var openType = typeof(BootstrapGrid<>);
            var closedType = openType.MakeGenericType(typeof(T));
            return ((BootstrapGrid<T>)Activator.CreateInstance(closedType)).SetData(data);
        }
    }
}
