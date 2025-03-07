using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Calculator
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MortgageCalculator : Page
	{
		public MortgageCalculator()
		{
			this.InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//get values from the textbox inputs
			double principal = 0;
			double yearlyInterestRate = 0;
			int years = 0;
			int months = 0;

			//check user inputs are valid
			if (double.TryParse(principalBorrowTextBox.Text, out principal) &&
				int.TryParse(yearsTextBox.Text, out years) &&
				int.TryParse(monthsTextBox.Text, out months) &&
				double.TryParse(yearlyInterestRateTextBox.Text, out yearlyInterestRate))

			{
				// calculate monthly interest rate from yearly interest rate
				double monthlyInterestRate = yearlyInterestRate / 100 / 12;

				// call method (below) to calculate mortgage repayment
				double monthlyPayment = CalculateMonthlyRepayment(principal, monthlyInterestRate, years, months);

				if (monthlyPayment > 0)
				{
					repaymentTextBox.Text = monthlyPayment.ToString("C");
					monthlyInterestRateTextBox.Text = monthlyInterestRate.ToString("P4");

				}
				else
				{
					repaymentTextBox.Text = "Error: Input is not valid, please try again.";
					monthlyInterestRateTextBox.Text = "";
				}
			}
			else
			{
				repaymentTextBox.Text = "Error - please enter values that are valid.";
				monthlyInterestRateTextBox.Text = "";
			}
		}
		//method to calculate repayment for mortgage calculator
		private double CalculateMonthlyRepayment(double principal, double monthlyInterestRate, int years, int months)
		{
			int totalMonths = years * 12 + months;


			if (totalMonths == 0 || monthlyInterestRate == 0)
				return 0; //stop calculation from dividing by zero

			double numerator = principal * monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, totalMonths);
			double denominator = Math.Pow(1 + monthlyInterestRate, totalMonths) - 1;

			return (denominator != 0) ? numerator / denominator : 0;


		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainMenu));
        }
    }


}
	
