using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TimeAide.AdminPanel.Models
{
    public class PayrollImportFieldMapping
    {
        public int TemplateId { get; set; }
        public int TemplateDetailId { get; set; }
        public int FieldIndex { get; set; }
        public string FieldName { get; set; }
        public string FieldNameAsValue { get; set; }
        public string FieldMapping { get; set; }
        public string MappingTable { get; set; }
        public string MappingColumn { get; set; }
        public string ColumnDataType { get; set; }
        public string MappingStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(MappingError))
                {
                    return MappingError;
                }
                else if (!string.IsNullOrEmpty(FieldMapping))
                {
                    if (FieldMapping == "Not Required")
                        return "Done";
                    else
                    {
                        if (!string.IsNullOrEmpty(MappingColumn))
                            return "Done";
                        else
                            return "Pending";
                    }
                }
                else
                {
                    return "Pending";
                }
            }           
        }
        public string MappingError { get; set; }
    }

    public class PayrollDBColumn
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }
}
