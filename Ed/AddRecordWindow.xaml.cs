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

namespace Ed
{
    /// <summary>
    /// Interaction logic for AddRecordWindow.xaml
    /// </summary>
    public partial class AddRecordWindow : Window
    {
        public AddRecordWindow()
        {
            InitializeComponent();
            SetupDropdowns();
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
            fdata.Add("Married Filing Jointly");
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

        private void Add_Record_Click(object sender, RoutedEventArgs e)
        {
            
        }


        //to do: handle what happens when you enter submit button
        //add data to user profile and send addition to database

    }
}
