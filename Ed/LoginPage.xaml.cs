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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    /// 
    
    public partial class LoginPage : Window
    {
        Boolean DEBUG = true;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            if(DEBUG == true)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                mainWindow.userName.Content = "user2";
                mainWindow.uid.Content = "2";
                this.Close();
            }
            else {
                OleDbConnection db_connect = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source =  |DataDirectory|UserDatabase.accdb");
                try
                {
                    if(db_connect.State == System.Data.ConnectionState.Closed)
                    {
                        db_connect.Open();
                    }
                    //query the logindata table for username and matching password
                    String query = "SELECT COUNT(1) FROM LoginData WHERE USERNAME=@Username AND PASSWORD=@Password";
                    OleDbCommand db_cmd = new OleDbCommand(query, db_connect);
                    db_cmd.CommandType = System.Data.CommandType.Text;
                    db_cmd.Parameters.AddWithValue("@Username", Username.Text);
                    db_cmd.Parameters.AddWithValue("@Password", Password.Password);
                    int count = Convert.ToInt32(db_cmd.ExecuteScalar());
                    if(count == 1)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        //pass username to be displayed in mainwindow
                        mainWindow.userName.Content = Username.Text;

                        
                        
                        //EXTRA query because ---FIX this mess
                        query = "SELECT * FROM LoginData WHERE USERNAME=@Username";
                        OleDbConnection c = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source =  |DataDirectory|UserDatabase.accdb");
                        OleDbCommand cmd = new OleDbCommand(query, c);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@Username", Username.Text);
                        c.Open();
                        OleDbDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            //pull out the uid
                            mainWindow.uid.Content = reader[1].ToString();
                        }   

                        //close all the things
                        db_connect.Close();
                        c.Close();


                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("username or password incorrect");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    db_connect.Close();
                }
            }
        }
    }
}
