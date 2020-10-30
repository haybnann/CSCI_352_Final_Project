/*
 * TO DO: add a database with years and tax brackets dating back to atleast 70s - not sure how to structure it yet
 *          - most of the following funnctions are dependent on the DB so it is currently highest priority
 *          - suggestions: year is the name of a table, with the tax rate in one column and the brackets in another ...could probably make it easier somehow
 *
 */



using System;
using System.Collections.Generic;

namespace Edward
{
    class Program
    {
        static void Main(string[] args)
        {

            //test functions in main
            UserProfile myProfile = new UserProfile();
            myProfile.AddIncomeHistory(100000, "single", 10000, 6000, 2020);
            myProfile.AddIncomeHistory(112345, "single", 10425, 6435, 2019);
            myProfile.AddIncomeHistory(152352, "single", 10524, 6554, 2018);
            myProfile.printTaxHistory();

        }
    }
    class UserProfile
    {
        List<YearlyTaxProfile> userTaxHistory;//make a list to hold all of the users previous financial info
        //userSpendingTracker  = STRETCH GOAL

        //constructor
        public UserProfile()
        {
            userTaxHistory = new List<YearlyTaxProfile>();
            //spendingTracker init
        }
        
        //add a year of financial info to the user's list
        public void AddIncomeHistory(double annualSalary, string filingStatus,double pretax,double posttax, int year)
        {
            //add to list --------TO DO: Organize it by year
            userTaxHistory.Add(
                new YearlyTaxProfile(annualSalary, filingStatus, pretax, posttax, year)
            );
        }
        //used to graph data projections of future worth of assets
        public double AssetValuePrediction(int yearsOfInterest, double effectiveInterest)
        {
            //return projected value to graph
            return 0;//delete later
        }
        //mass upload years of finincial info from a spreadsheet
        public void MassAddIncomeHistory(String fileName)
        {
            //parse the csv file and use AddIncomeHistory() to add records
        }
        public void printTaxHistory()
        {
            foreach( YearlyTaxProfile p in userTaxHistory)
            {
                Console.WriteLine($"Your Income for the year of {p.getYear()} was {p.getSalary()} with " +
                    $"Pre-Tax Contributions of {p.getPreTaxContributions()} and Post-Tax Contributions of {p.getPostTaxContributions()}" +
                    $". You filed as {p.getFilingStatus()} for this year.");
            }
        }
    }

    class YearlyTaxProfile
    {
        private double annualSalary;
        private string filingStatus;
        private double pretaxContribution;
        private double posttaxContribution;
        private int year;

        //add enum for tax brackets
        //add enum for tax deductions


        //constructor
        public YearlyTaxProfile(   double salary,
                            string status,
                            double pretax,
                            double posttax,
                            int date)
        {
            annualSalary = salary;
            filingStatus = status;
            pretaxContribution = pretax;
            posttaxContribution = posttax;
            year = date;
        }

        //setters
        void setSalary(double salary)
        {
            annualSalary = salary;
        }
        void setFilingStatus(String status)
        {
            filingStatus = status;
        }
        void setPostTaxContributions(double contributions)
        {
            posttaxContribution = contributions;
        }
        void setPreTaxContributions(double contributions)
        {
            pretaxContribution = contributions;
        }
        void setYear(int date)
        {
            year = date;
        }
        //getters
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
       public int getYear()
        {
            return year;
        }

        //tax calculations: --- need to add enums with tax info by year   
        double getTakeHomePay()
        {
            //return annualSalary - getFICA_Taxes()- getFederalIncomeTax()-getLocalIncomeTax()-pretaxcontributions
            return 0; //DELETE  
        }
        double getFICA_Taxes()
        {
            //calculate taxes on adjusted gross income
            //requires: agi, filingstatus, and tax deductions
            return 0;//DELETE
        }
        double getTaxDeductions()
        {
            //use filing status & year to decide what the deduction should be
            //might could use a database or a table instead of enum for all the years
            return 0;//DELETE
        }
        double getAdjustedGrossIncome()
        {
            //return salary - pretaxcontributions - taxDeductoins();
            return 0;//DELETE
        }
        double getFederalIncomeTax()
        {
            //based on filingstatus & agi
            //return  taxes 
            return 0;//delete
        }
        double getLocalIncomeTax()
        {
            //based on agi & filingstatus & state
            //return taxes
            return 0; //delete
        }
    }
}
