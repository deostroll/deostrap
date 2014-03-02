using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

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
                    return wtr.ToString();
                }
            }

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
    }
}
