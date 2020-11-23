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

namespace Ed
{
    /// <summary>
    /// Interaction logic for AddRecordWindow.xaml
    /// </summary>
    public partial class AddRecordWindow : Window
    {
        String userID;
        public AddRecordWindow(String uid)
        {
            InitializeComponent();
            SetupDropdowns();
            setUid(uid);//set the global var to id 

        }
        void SetupDropdowns()
        {
            //setup income dropdown
            List<double> idata = new List<double>();
            for(int i = 0; i < 1000001; i += 1000)
            {
                idata.Add(i);
            }
            cbincome.ItemsSource = idata;
            cbincome.SelectedIndex = 0;



            //setup filingstatus dropdown
            List<string> fdata = new List<string>();
           
            fdata.Add("Single");
            fdata.Add("MarriedFilingJointly");
            fdata.Add("Married Filing Separate");
            fdata.Add("Head Of Household");
            cbfiling.ItemsSource = fdata;
            cbfiling.SelectedIndex = 0;
            


            //setup pretax dropdown
            List<double> predata = new List<double>();
            for (int i = 0; i < 1000001; i += 1000)
            {
                predata.Add(i);
            }
            cbpretax.ItemsSource = predata;
            cbpretax.SelectedIndex = 0;

            //setup posttax dropdown
            List<double> postdata = new List<double>();
            for (int i = 0; i < 1000001; i += 1000)
            {
                postdata.Add(i);
            }
            cbposttax.ItemsSource = postdata;
            cbposttax.SelectedIndex = 0;

            //setup year dropdown
            List<double> yeardata = new List<double>();
            for (int i = 2021; i > 1900; i--)
            {
                yeardata.Add(i);
            }
            cbyear.ItemsSource = yeardata;
            cbyear.SelectedIndex = 0;


        }

        //Set a global from the argument - its wrong but atleast its working
        void setUid(String uid)
        {
            userID = uid;
        }

        //add a record to the database on submission
        private void Add_Record_Click(object sender, RoutedEventArgs e)
        {
            //open database and add entry of comboboxes

            String connect = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =  |DataDirectory|UserDatabase.accdb";
            /*String query = "INSERT INTO UserData (ID_NUM,SALARY,FILING_STATUS,PRETAX,POSTTAX,YEAR) VALUES(" + userID + ","
                                                            + cbincome.SelectedItem + ","
                                                            + cbfiling.SelectedItem.ToString()+ ","
                                                            + cbpretax.SelectedItem.ToString() + ","
                                                            + cbposttax.SelectedItem.ToString() + ","
                                                            + cbyear.SelectedItem.ToString()
                                                            +")";//SQL injection bug*/

            //vulnerable to sql injection
            String query = "INSERT INTO [UserData] ([ID_NUM],[SALARY],[FILING_STATUS],[PRETAX],[POSTTAX],[YEAR]) VALUES ('" 
                            + userID +"','"
                            + cbincome.SelectedItem.ToString()+"','"
                            + cbfiling.SelectedItem.ToString()+"','" 
                            + cbpretax.SelectedItem.ToString()+"','"
                            + cbposttax.SelectedItem.ToString()+"','"
                            + cbyear.SelectedItem.ToString()
                            +"')";
            
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
            //cmd.Dispose();
            c.Close();
            this.Close();//close out the window after updating database
        }

    }
}
