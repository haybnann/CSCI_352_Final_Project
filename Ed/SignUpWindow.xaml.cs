using System;
using System.Collections.Generic;
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

namespace Ed
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

        //open database and add entry of comboboxes

        String connect = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =  |DataDirectory|UserDatabase.accdb";

        //vulnerable to sql injection
        String query = "INSERT INTO [LoginData] ([USERNAME],[PASSWORD]) VALUES ('"
                + username + "','"+password+ "')";

        OleDbConnection c = new OleDbConnection(connect);
        OleDbDataAdapter adapt = new OleDbDataAdapter();
        OleDbCommand cmd = new OleDbCommand(query, c);

        try
        {
            c.Open();
            adapt.InsertCommand = cmd;
            double x = adapt.InsertCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed due to" + ex.Message);
        }
        LoginPage lgp = new LoginPage();
        lgp.Show();
        c.Close();
        this.Close();//close out the window after updating database

        }
    }
}
