using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
/*
 * Open this page when View Records button is clicked on mainwindow
 * View_Records_Click() opens this window
 * TO DO: FIX query to match current user
 */

namespace Ed
{
    /// <summary>
    /// Interaction logic for ViewRecordsWindow.xaml
    /// </summary>
    public partial class ViewRecordsWindow : Window
    {
        public ViewRecordsWindow(String userid)
        {
            InitializeComponent();

            OleDbConnection connection = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source =  |DataDirectory|UserDatabase.accdb");
            DataSet ds = new DataSet();

            String query = "SELECT * FROM UserData WHERE ID_NUM = " + userid;//BUG--sql injection 
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = new OleDbCommand(query, connection);
            adapter.Fill(ds, "UserData");
            dataGrid.ItemsSource = ds.Tables["UserData"].DefaultView;
        }
   
    }
}
