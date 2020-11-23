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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

/*TO DO: 
 *  - Add queries for :
 *          + Adding queries to add data to database
 *  - Add Signup page
 *  - Graph data 
 *          + list of retirement growth
 */


namespace Ed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //add profile
            UserProfile user = new UserProfile();
            String db = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =  |DataDirectory|UserDatabase.accdb";

            //change query to match the user id num for who logs in
            
            Load_Profile_Query(db, uid.Content.ToString(),user);
            String u_ID = uid.Content.ToString();

            //display the data you know now
            //data.Text = user.userTaxHistory[0].getSalary().ToString();

            //Update_IncomeTax_Query(db,user,user.userTaxHistory[0].getFilingStatus());
            //data.Text = user.incomeTaxList[0].taxedAmount.ToString();

            String query = "SELECT * FROM StandardDeduction2020 WHERE FILING_STATUS = 'Single'";
            Update_Deductions_Query(db, query, user);
            //data.Text = user.taxdeduct.taxDeduction.ToString();
            //data.Text = user.getAdjustedGrossIncome(user.userTaxHistory[0]).ToString();

            //load financial data
            Values1 = new ChartValues<double> { 3, 4, 6, 3, 2, 6 };
            Values2 = new ChartValues<double> { 5, 3, 5, 7, 3, 9 };
            DataContext = this;
            //interact with user now:
        }



        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }

        //change this function into constructor for userprofile
        //function loads all database info on a user into program
        void Load_Profile_Query(String connect, String u_id, UserProfile user)
        {
            String query = "SELECT * FROM UserData WHERE ID_NUM = @uid";
            int col_num = 7;//num columns of profile data
            //var result = new System.Text.StringBuilder();
            OleDbConnection c = new OleDbConnection(connect);
            OleDbCommand cmd = new OleDbCommand(query, c);
            c.Open();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@uid", u_id);
            OleDbDataReader reader = cmd.ExecuteReader();
            
            //read every row
            while (reader.Read())
            {
                //create new tax profile
                YearlyTaxProfile ytp = new YearlyTaxProfile();

                //loop all column data
                for (int i = 0; i < col_num; i++)
                {
                    //if i = certain col insert data into tax profile
                    switch (i)
                    {
                        case 0:
                            ytp.setUID(Convert.ToDouble(reader[i].ToString()));
                            break;
                        case 1:
                            ytp.setIDnum(Convert.ToDouble(reader[i].ToString()));
                            break;
                        case 2:
                            ytp.setSalary(Convert.ToDouble(reader[i].ToString()));
                            break;
                        case 3:
                            ytp.setFilingStatus(reader[i].ToString());
                            break;
                        case 4:
                            ytp.setPreTaxContributions(Convert.ToDouble(reader[i].ToString()));
                            break;
                        case 5:
                            ytp.setPostTaxContributions(Convert.ToDouble(reader[i].ToString()));
                            break;
                        case 6:
                            ytp.setYear(Convert.ToDouble(reader[i].ToString()));
                            break;
                        default:
                            break;
                    }
                }
                //add tax profile to list in user profile
                user.userTaxHistory.Add(ytp);
            }
            c.Close();
        }

        void Update_IncomeTax_Query(String connect,UserProfile user, String filingStatus)
        {
            String q = "SELECT * FROM FedIncomeTax2020 WHERE FILINGSTATUS = @filingStatus";
            int col_num = 4;//num columns of tax
            OleDbConnection c = new OleDbConnection(connect);
            c.Open();
            OleDbCommand cmd = new OleDbCommand(q,c);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@filingStatus", filingStatus);
            OleDbDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            
            //clear out old tax data
            user.incomeTaxList.Clear();

            //read each row of query
            while (reader.Read())
            {
                //create new federal income tax
                FederalIncomeTaxBracket fitb = new FederalIncomeTaxBracket();

                //loop through ach column of data
                for (int i = 0; i < col_num; i++)
                {
                    //if i = certain col insert data into tax profile
                    switch (i)
                    {
                        case 0:
                            fitb.id = Convert.ToDouble(reader[i].ToString());
                            break;
                        case 1:
                            fitb.filingStatus = reader[i].ToString();
                            break;
                        case 2:
                            fitb.taxedAmount = Convert.ToDouble(reader[i].ToString());
                            break;
                        case 3:
                            fitb.taxRate = Convert.ToDouble(reader[i].ToString());
                            break;
                        default:
                            break;
                    }
                }
                //add bracket
                user.incomeTaxList.Add(fitb);
            }
            c.Close();
            //SORT the tax brackets before leaving
        }

        void Update_Deductions_Query(String connect, String query, UserProfile user)
        {
            int col_num = 3;//num columns of tax
            OleDbConnection c = new OleDbConnection(connect);
            OleDbCommand cmd = new OleDbCommand(query, c);
            c.Open();
            OleDbDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {

                //loop through ach column of data
                for (int i = 0; i < col_num; i++)
                {
                    //if i = certain col insert data into tax profile
                    switch (i)
                    {
                        case 0:
                            user.taxdeduct.id = Convert.ToDouble(reader[i].ToString());
                            break;
                        case 1:
                           user.taxdeduct.filingStatus = reader[i].ToString();
                            break;
                        case 2:
                            user.taxdeduct.taxDeduction = Convert.ToDouble(reader[i].ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
            c.Close();
        }

        //Add a record to the user's tax profile in a new window -- pass info back to main
        private void Add_Record_Click(object sender, RoutedEventArgs e)
        {
            AddRecordWindow w = new AddRecordWindow();
            w.Show();
        }


        //view all of the users tex records
        private void View_Records_Click(object sender, RoutedEventArgs e)
        {
            ViewRecordsWindow vrw = new ViewRecordsWindow(uid.Content.ToString());
            //vrw.u_ID.Content = ;
            vrw.Show();
            //
        }
    }
    class UserProfile
    {
        public List<YearlyTaxProfile> userTaxHistory;//make a list to hold all of the users previous financial info

        //list of tax brackets for whatever year you're currently using
        public List<FederalIncomeTaxBracket> incomeTaxList;

        public Deductions taxdeduct;

        //constructor
        public UserProfile()
        {
            userTaxHistory = new List<YearlyTaxProfile>();
            incomeTaxList = new List<FederalIncomeTaxBracket>();
            taxdeduct = new Deductions();
        }



        //add a year of financial info to the user's list
        public void AddIncomeHistory(double annualSalary, string filingStatus, double pretax, double posttax, int year)
        {
            //add to list --------TO DO: Organize it by year
            /*userTaxHistory.Add(
                new YearlyTaxProfile(annualSalary, filingStatus, pretax, posttax, year)
            );*/
        }
        //used to graph data projections of future worth of assets
        public double AssetValuePrediction(int yearsOfInterest, double effectiveInterest)
        {
            //return projected value to graph
            return 0;//delete later
        }


        public double getTakeHomePay(YearlyTaxProfile ytp)
        {
            //return annualSalary - getFICA_Taxes()- getFederalIncomeTax()-getLocalIncomeTax()-pretaxcontributions
            return ytp.getSalary() - this.getFICA_Taxes(ytp) - this.getFederalIncomeTax(ytp) - ytp.getPreTaxContributions();
        }
        public double getFICA_Taxes(YearlyTaxProfile ytp)//FIX FICA TAXES
        {
            double taxrate = 0.07;//FIX HARDCODED
            return taxrate * (ytp.getSalary() - ytp.getPreTaxContributions());
        }
        public double getTaxDeductions()
        {
            //use filing status & year to decide what the deduction should be
            return this.taxdeduct.taxDeduction;
        }
        public double getAdjustedGrossIncome(YearlyTaxProfile ytp)
        {
            return ytp.getSalary() - ytp.getPreTaxContributions() - this.getTaxDeductions();
        }

        //calculate taxes for user in a given year
        public double getFederalIncomeTax(YearlyTaxProfile ytp)
        {
            //based on taxable income (agi)
            double money = getAdjustedGrossIncome(ytp);
            double taxes = 0;
            for (int i = 0; i < incomeTaxList.Count();i++){
                if (money > incomeTaxList[i].taxedAmount)
                {
                    //add to taxes
                    taxes += incomeTaxList[i].taxedAmount * incomeTaxList[i].taxRate;
                    //subtract what has already been taxed
                    money -= incomeTaxList[i].taxedAmount;
                }
                else if(money > 0)
                {
                    //tax remaining money
                    taxes += money * incomeTaxList[i].taxRate;
                    //subtract remainder
                    money -= money * incomeTaxList[i].taxRate;
                }
            }

            return taxes;
        }

    }
    class FederalIncomeTaxBracket
    {
        public double id;
        public String filingStatus;
        public double taxedAmount;
        public double taxRate;

        //TO DO: Add set & get functions later

        //used to update the income tax bracket list

    }
    class Deductions
    {
        public double id;
        public String filingStatus;
        public double taxDeduction;
    }

    //add interest rate to ytp
    class YearlyTaxProfile
    {
        private double uid;
        private double id_num;
        private double annualSalary;
        private string filingStatus;
        private double pretaxContribution;
        private double posttaxContribution;
        private double year;

        //add enum for tax brackets
        //add enum for tax deductions

        //constructor for reading from db
        public YearlyTaxProfile() { }
        //programmatic constructor
        public YearlyTaxProfile(double u,
                            double id,
                            double salary,
                            string status,
                            double pretax,
                            double posttax,
                            double date)
        {
            uid = u;
            id_num = id;
            annualSalary = salary;
            filingStatus = status;
            pretaxContribution = pretax;
            posttaxContribution = posttax;
            year = date;
        }

        //setters
        public void setUID(double u)
        {
            uid = u;
        }
        public void setIDnum(double id)
        {
            id_num = id;
        }
        public void setSalary(double salary)
        {
            annualSalary = salary;
        }
        public void setFilingStatus(String status)
        {
            filingStatus = status;
        }
        public void setPostTaxContributions(double contributions)
        {
            posttaxContribution = contributions;
        }
        public void setPreTaxContributions(double contributions)
        {
            pretaxContribution = contributions;
        }
        public void setYear(double date)
        {
            year = date;
        }
        //getters
        double getUID(double u)
        {
            return uid;
        }
        double getIDnum(double id)
        {
            return id_num;
        }
        public double getSalary()
        {
            return annualSalary;
        }
        public String getFilingStatus()
        {
            return filingStatus;
        }
        public double getPostTaxContributions()
        {
            return posttaxContribution;
        }
        public double getPreTaxContributions()
        {
            return pretaxContribution;
        }
        public double getYear()
        {
            return year;
        }
  
    }
}
