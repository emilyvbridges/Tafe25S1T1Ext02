using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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
	public sealed partial class MainMenu : Page
	{
		public MainMenu()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Loads the Mortgage Calculator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MortgageCalculatorButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MortgageCalculator));

		}

		/// <summary>
		/// Loads the Currency Conversion Converter.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CurrencyCalculatorButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(CurrencyConverter));
		}

		/// <summary>
		/// Exits the application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QuitButton_Click(object sender, RoutedEventArgs e)
		{
			CoreApplication.Exit();
		}

		/// <summary>
		/// Loads the maths calculator.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MathsCalculatorButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainPage));
		}
	}

}
