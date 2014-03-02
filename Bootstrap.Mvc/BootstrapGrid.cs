using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using Bootstrap.Mvc;
using System.Linq.Expressions;

namespace Web.Mvc.TwitterBootstrap
{
    public enum GridType
    {
        normal = 0x00,
        condensed = 0x01,
        hoverEffect = 0x02,
        bordered = 0x04,
        striped = 0x08
    }

    public class BootstrapGrid<T> : IHtmlString
    {
        bool _isColumnsSet;

        public BootstrapGrid()
        {
            TableType = GridType.normal;
        }

        IEnumerable<T> _data;

        public BootstrapGrid<T> SetData(IEnumerable<T> data)
        {
            _data = data;
            return this;
        }

        public GridType TableType { get; set; }

        public BootstrapGrid<T> SetTableType(GridType type)
        {
            TableType = type;
            return this;
        }

        public string ToHtmlString()
        {
            using (StringWriter wtr = new StringWriter())
            {
                using (HtmlTextWriter hwtr = new HtmlTextWriter(wtr))
                {
                    var typ = typeof(T);
                    var properties = typ.GetProperties();

                    List<string> tblCssClasses = new List<string>();
                    tblCssClasses.Add("table");

                    if ((TableType & GridType.hoverEffect) != 0) tblCssClasses.Add("table-hover");
                    if ((TableType & GridType.condensed) != 0) tblCssClasses.Add("table-condensed");
                    if ((TableType & GridType.striped) != 0) tblCssClasses.Add("table-striped");
                    if ((TableType & GridType.bordered) != 0) tblCssClasses.Add("table-bordered");

                    hwtr.AddAttribute(HtmlTextWriterAttribute.Class, string.Join(" ", tblCssClasses));
                    if (!string.IsNullOrEmpty(Id))
                        hwtr.AddAttribute(HtmlTextWriterAttribute.Id, Id);
                    hwtr.RenderBeginTag(HtmlTextWriterTag.Table); //table begin
                    hwtr.RenderBeginTag(HtmlTextWriterTag.Thead); //thead begin

                    //Determine if the columns have been set in client
                    if (_isColumnsSet)
                    {
                        string[] cols = GridColumnsCache.ModelProperties;
                        RenderGridWithColumns(hwtr, cols);
                    }
                    else
                    {
                        RenderNormalGrid(hwtr, properties);
                    }                    
                    return wtr.ToString();
                }
            }

        }

        private void RenderNormalGrid(HtmlTextWriter hwtr, PropertyInfo[] properties)
        {
            foreach (PropertyInfo pi in properties)
            {
                hwtr.RenderBeginTag(HtmlTextWriterTag.Th);
                hwtr.Write(pi.Name);
                hwtr.RenderEndTag();
            }

            hwtr.RenderEndTag(); //thead end

            hwtr.RenderBeginTag(HtmlTextWriterTag.Tbody); //tbody begin
            foreach (object item in _data)
            {
                hwtr.RenderBeginTag(HtmlTextWriterTag.Tr); //tr begin

                //properties = item.GetType().GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    hwtr.RenderBeginTag(HtmlTextWriterTag.Td);
                    hwtr.Write(pi.GetValue(item));
                    hwtr.RenderEndTag();
                }

                hwtr.RenderEndTag();// tr end
            }
            hwtr.RenderEndTag(); //tbody end
            hwtr.RenderEndTag(); //table end
        }
        private void RenderGridWithColumns(HtmlTextWriter hwtr, string[] properties)
        {
            foreach (string pi in properties)
            {
                hwtr.RenderBeginTag(HtmlTextWriterTag.Th);
                hwtr.Write(pi);
                hwtr.RenderEndTag();
            }

            hwtr.RenderEndTag(); //thead end

            hwtr.RenderBeginTag(HtmlTextWriterTag.Tbody); //tbody begin
            foreach (object item in _data)
            {
                hwtr.RenderBeginTag(HtmlTextWriterTag.Tr); //tr begin
                Dictionary<string, Func<T, object>> dlgtCache = (from x in properties
                                                                 select new { PropertyName = x, Dlgt = MakePropertyDelegate(x) })
                                                                 .ToDictionary(d => d.PropertyName, d => d.Dlgt);
                //properties = item.GetType().GetProperties();
                foreach (string pi in properties)
                {
                    hwtr.RenderBeginTag(HtmlTextWriterTag.Td);
                    hwtr.Write(dlgtCache[pi]((T)item));
                    hwtr.RenderEndTag();
                }

                hwtr.RenderEndTag();// tr end
            }
            hwtr.RenderEndTag(); //tbody end
            hwtr.RenderEndTag(); //table end

        }

        private Func<T, object> MakePropertyDelegate(string PropertyName)
        {
            ParameterExpression pexp = Expression.Parameter(typeof(T));
            Expression propgetterexp = Expression.Property(pexp, PropertyName);
            Func<T, object> foo = Expression.Lambda<Func<T, object>>(propgetterexp, pexp).Compile();
            return foo;
        }
        public BootstrapGrid<T> SetId(string ID)
        {
            this.Id = ID;
            return this;
        }

        string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public BootstrapGrid<T> Columns(Action<ColumnFactory<T>> colBuilder)
        {
            GridColumnsCache = new ColumnFactory<T>();
            colBuilder(GridColumnsCache);
            _isColumnsSet = true;
            return this;
        }

        public ColumnFactory<T> GridColumnsCache { get; set; }
    }
}
