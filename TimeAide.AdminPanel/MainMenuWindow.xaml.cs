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

namespace TimeAide.AdminPanel
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void btnShowDataMigration_Click(object sender, RoutedEventArgs e)
        {
            var dataMigWin = new DataMigrationWindow();
            dataMigWin.Owner = this;
            dataMigWin.Show();
           // dataMigWin.Topmost = true;
        }
        //btnShowExportData_Click
        private void btnShowExportData_Click(object sender, RoutedEventArgs e)
        {
            var expDataWin = new DataExportWindow();
            expDataWin.Owner = this;
            expDataWin.Show();
            // dataMigWin.Topmost = true;
        }
        private void btnCloseApplication_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnShowDocMigration_Click(object sender, RoutedEventArgs e)
        {
            var docMigWin = new DocumentMigrationWindow();
            docMigWin.Owner = this;
            docMigWin.Show();
            //docMigWin.Topmost = true;
        }

        private void btnShowImportData_Click(object sender, RoutedEventArgs e)
        {
            var impDataWin = new DataImprtWindow();
            impDataWin.Owner = this;
            impDataWin.Show();
        }
    }
}
