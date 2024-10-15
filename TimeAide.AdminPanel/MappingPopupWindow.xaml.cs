using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TimeAide.AdminPanel.Models;

namespace TimeAide.AdminPanel
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MappingPopupWindow : Window
    {
        public PayrollImportFieldMapping SelectedMappingFieldItem { get;set; }
        public AdminPanelImportHelper AdminConsoleHelperService { get; set; }
        public PayrollImportFieldMapping SelectedDatagridItem { get; set; }
        public MappingPopupWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          var tableList=  AdminConsoleHelperService.PayrollDBColumns.Select(s => s.TableName).Distinct();

            cboMappingTable.ItemsSource = tableList;
            this.txtFieldIndex.DataContext = SelectedMappingFieldItem;
            this.txtFieldName.DataContext = SelectedMappingFieldItem;
            this.cboFieldAsValue.DataContext = SelectedMappingFieldItem;
            this.cboFieldMapping.DataContext = SelectedMappingFieldItem;
            this.cboMappingTable.DataContext = SelectedMappingFieldItem;
            this.cboMappingColumn.DataContext = SelectedMappingFieldItem;
            this.txtDataType.DataContext = SelectedMappingFieldItem;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdateFieldsMapping_Click(object sender, RoutedEventArgs e)
        {
            var requiredFieldAsValTab= new List<string>() { "tblUserBatchCompensations", "tblUserBatchWithholdings", "tblCompanyBatchWithholdings" };

            if (SelectedMappingFieldItem.FieldMapping == "Required" && SelectedMappingFieldItem.MappingColumn==null)
            {
                MessageBox.Show("Missing column mapping.");
                return;
            }
            if (requiredFieldAsValTab.Contains(SelectedMappingFieldItem.MappingTable) && string.IsNullOrEmpty(SelectedMappingFieldItem.FieldNameAsValue))
            {
                MessageBox.Show("Item name is required. Please type the appropriate Compensation/Withholding/Contribution Name");
                cboFieldAsValue.Focus();
                return;
            }
            SelectedDatagridItem.FieldNameAsValue = string.IsNullOrEmpty(SelectedMappingFieldItem.FieldNameAsValue)?null: SelectedMappingFieldItem.FieldNameAsValue.Trim();           
            SelectedDatagridItem.FieldMapping = SelectedMappingFieldItem.FieldMapping;
            SelectedDatagridItem.MappingTable = SelectedMappingFieldItem.MappingTable;
            SelectedDatagridItem.MappingColumn = SelectedMappingFieldItem.MappingColumn;
            SelectedDatagridItem.ColumnDataType = SelectedMappingFieldItem.ColumnDataType;
            this.DialogResult = true;
            AdminConsoleHelperService.IsPayrollImportFieldMappingChanged = true;
            this.Close();
        }

        private void cboFieldMapping_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            if(cboFieldMapping.SelectedIndex == 1)
            {
                SelectedMappingFieldItem.MappingTable = null;
                SelectedMappingFieldItem.MappingColumn = null;
                SelectedMappingFieldItem.ColumnDataType = null;
                cboMappingTable.SelectedIndex = -1;
                cboMappingColumn.SelectedIndex = -1;
                txtDataType.Text = null;
            }
        }

        private void cboMappingTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show($"{cboMappingTable.SelectedValue}");
            var selectedTable = cboMappingTable.SelectedItem as string;
            var columnList = AdminConsoleHelperService.PayrollDBColumns.Where(w=>w.TableName== selectedTable).Select(s => s.ColumnName).Distinct();
            cboMappingColumn.ItemsSource = columnList;
            var itemList = AdminConsoleHelperService.GetSelectedTblItemsList(selectedTable);
            cboFieldAsValue.ItemsSource = itemList;
            if (itemList.Count == 0)
            {
                cboFieldAsValue.IsEnabled = false;
            }
            else
            {
                cboFieldAsValue.IsEnabled = true;
            }
        }


        private void cboMappingColumn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCol = cboMappingColumn.SelectedItem as string;
            var datType = AdminConsoleHelperService.PayrollDBColumns.Where(w => w.ColumnName == selectedCol).Select(s => s.DataType).FirstOrDefault();
           
            txtDataType.Text= datType;
            //txtDataType.acc
        }

        private void cboFieldAsValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
