using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
	public sealed partial class CurrencyConverter : Page
	{

		// Variable used to fill the combo boxes on the CurrencyConverter XAML page with values to
		// avoid duplication.
		public ObservableCollection<string> CURRENCY_COLLECTION { get; set; }

		// Currency conversion rates.
		private static readonly Dictionary<string, double> CURRENCY_DICT = new Dictionary<string, double>()
		{
			{"USD-USD", 1},
			{"USD-EUR", 0.85189982},
			{"USD-GBP", 0.72872436},
			{"USD-INR", 74.257327},

			{"EUR-EUR", 1},
			{"EUR-USD", 1.1739732},
			{"EUR-GBP", 0.8556672},
			{"EUR-INR", 87.00755},

			{"GBP-GBP", 1},
			{"GBP-USD", 1.371907},
			{"GBP-EUR", 1.1686692},
			{"GBP-INR", 101.68635},

			{"INR-INR", 1},
			{"INR-USD", 0.011492628},
			{"INR-EUR", 0.013492774},
			{"INR-GBP", 0.0098339397}
		};

		/// <summary>
		/// Main method.
		/// </summary>
		public CurrencyConverter()
		{
			this.InitializeComponent();

			// Initialises the currency collection used in the From/To ComboBoxes on the XAML page
			CURRENCY_COLLECTION = new ObservableCollection<string>
			{
				"USD - US Dollar",
				"EUR - Euro",
				"GBP - British Pound",
				"INR - Indian Rupee"
			};

			// Apply the currencies to the combo boxes.
			baseComboBox.ItemsSource = CURRENCY_COLLECTION;
			targetComboBox.ItemsSource = CURRENCY_COLLECTION;

			// Select a default value.
			baseComboBox.SelectedIndex = 0;
			targetComboBox.SelectedIndex = 0;
		}

		/// <summary>
		/// Converts the currency amount entered from the base currency selected under "From" to the target currency selected under "To"
		/// and then displays the converted amount and the conversion rates below.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void convertButton_Click(object sender, RoutedEventArgs e)
		{
			string selectedFromCurrency = baseComboBox.SelectedValue.ToString();
			string selectedToCurrency = targetComboBox.SelectedValue.ToString();
			string baseCurrencyCode, baseCurrencyName, targetCurrencyCode, targetCurrencyName, currencyKey, targetCurrencySymbol;
			double baseAmount, baseToTargetConversionRate, targetToBaseConversionRate, convertedAmount;
			string plural = "";

			// Validate the contents in the amountTextBox to ensure it's a double data type.
			try
			{
				baseAmount = double.Parse(amountTextBox.Text);
			}
			catch
			{
				var dialogMessage = new MessageDialog("Error! Please enter a valid currency amount.");
				await dialogMessage.ShowAsync();
				amountTextBox.Focus(FocusState.Programmatic);
				amountTextBox.SelectAll();
				return;
			}

			// Extract the currency code (e.g. USD) and the currency name (e.g. US Dollars) from both
			// selected currency combo boxes.
			(baseCurrencyCode, baseCurrencyName) = extractCodeAndName(selectedFromCurrency);
			(targetCurrencyCode, targetCurrencyName) = extractCodeAndName(selectedToCurrency);

			// Look up From/To conversion rate.
			currencyKey = baseCurrencyCode + "-" + targetCurrencyCode;
			baseToTargetConversionRate = CURRENCY_DICT[currencyKey];

			// Look up To/From conversion rate.
			currencyKey = targetCurrencyCode + "-" + baseCurrencyCode;
			targetToBaseConversionRate = CURRENCY_DICT[currencyKey];

			// Convert.
			convertedAmount = convertCurrency(baseAmount, baseToTargetConversionRate);

			// Update form.
			if (baseAmount > 1)
			{
				plural = "s";
			}

			baseAmountTextBlock.Text = baseAmount + " " + baseCurrencyName + plural + " =";

			targetCurrencySymbol = getCurrencySymbol(targetCurrencyCode);
			targetAmountTextBlock.Text = targetCurrencySymbol + convertedAmount.ToString("N8") + " " + targetCurrencyName + "s";

			baseToTargetRateTextBlock.Text = "1 " + baseCurrencyCode + " =  " + baseToTargetConversionRate + " " + targetCurrencyName + "s";
			targetToBaseRateTextBlock.Text = "1 " + targetCurrencyCode + " = " + targetToBaseConversionRate  + " " + baseCurrencyName + "s";
		}

		/// <summary>
		/// Splits the string contents of the currency combo box into a currency code and the name of the currency.
		/// e.g. "USD - US Dollars" into "USD" and "US Dollars".
		/// </summary>
		/// <param name="selectedCurrency"></param>
		/// <returns>two strings, currencyCode which is the currency code of the desired currency, and currencyName which
		/// is the full name of the currency</returns>
		private (string, string) extractCodeAndName (string selectedCurrency)
		{
			string currencyCode, currencyName;

			string[] parts = selectedCurrency.Split(" - ");
			currencyCode = parts[0];
			currencyName = parts[1];

			return (currencyCode, currencyName);
		}

		/// <summary>
		/// Retrieves the symbol for the requested currency.
		/// </summary>
		/// <param name="currencyCode"></param>
		/// <returns>a string currencySymbol containing the symbol of the requested currency</returns>
		private string getCurrencySymbol(string currencyCode)
		{
			string currencySymbol = "";

			if (currencyCode.Equals("USD")) {
				currencySymbol = "$";
			}
			else if (currencyCode.Equals("EUR")) {
				currencySymbol = "€";
			}
			else if (currencyCode.Equals("GBP")) {
				currencySymbol = "£";
			}
			else if (currencyCode.Equals("INR")) {
				currencySymbol = "₹";
			}

			return currencySymbol;
		}

		/// <summary>
		/// Converts the amount in the base currency into the amount in the target currency.
		/// </summary>
		/// <param name="fromAmount"></param>
		/// <param name="conversionRate"></param>
		/// <returns>a double convertedAmount calculating the converted target currency amount</returns>
		private double convertCurrency(double fromAmount, double conversionRate)
		{
			double convertedAmount = fromAmount * conversionRate;

			return convertedAmount;
		}

		/// <summary>
		/// Exits back to the main menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainMenu));
		}
	}
}
