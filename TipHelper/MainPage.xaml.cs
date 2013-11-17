using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TipHelper.Resources;

namespace TipHelper
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            updateResultIfPossible();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        /*
         * When a split button is clicked, the number of splits will be updated.
         */
        private void SplitButtonClicked(object sender, RoutedEventArgs e)
        {
            // Get the button text
            String buttonText = ((Button)sender).Content.ToString();

            // Get the current number of splits
            int newSplit = int.Parse(lblSplit.Text.Replace(" way(s)",""));

            // Figure out the new split
            if (buttonText.Equals("+1"))
                newSplit++;
            else if (buttonText.Equals("-1"))
                newSplit--;
            else if (buttonText.Equals("+5"))
                newSplit += 5;
            else if (buttonText.Equals("-5"))
                newSplit -= 5;
            else if (buttonText.Equals(" 1 "))
                newSplit = 1;

            // Ensure that the new split is at least 1
            newSplit = Math.Max(newSplit, 1);

            // Set the label text to the new split
            lblSplit.Text = newSplit.ToString() + " way(s)";
            updateResultIfPossible();
        }

        /*
         * When one of the tip buttons is clicked, the tip percentage will be updated
         */
        private void TipButtonClicked(object sender, RoutedEventArgs e)
        {
            // Get the button text
            String buttonText = ((Button)sender).Content.ToString();

            // Get the current tip amount
            int newTip = int.Parse(lblTip.Text.Replace("%", ""));

            // Figure out the new split
            if (buttonText.Equals("+1"))
                newTip++;
            else if (buttonText.Equals("-1"))
                newTip--;
            else if (buttonText.Equals("+5"))
                newTip += 5;
            else if (buttonText.Equals("-5"))
                newTip -= 5;
            else if (buttonText.Equals(" 0 "))
                newTip = 0;

            // Ensure that the new tip is at least 0
            newTip = Math.Max(newTip, 0);

            // Set the label text to the new tip
            lblTip.Text = newTip.ToString() + "%";
            updateResultIfPossible();
        }

        /*
         * Attempts to update the result label with result of the tip computation. If that's not possible, it simply puts "--" in the label
         */
        private void updateResultIfPossible()
        {

            // If we have all the required values, perform the computation and set the result as the text of the label.
            // Otherwise, just set the label to "--"
            double tip, split, bill;
            if (double.TryParse(lblTip.Text.Replace("%", ""), out tip) && double.TryParse(lblSplit.Text.Replace(" way(s)", ""), out split) && double.TryParse(txtBillAmount.Text, out bill))
                lblResult.Text = String.Format("{0:0.00}", computePayment(tip, split, bill));
            else
                lblResult.Text = "--";
        }

        /*
         * Compute the amount paid by each person given the tip, split, and bill 
         */
        private double computePayment(double tip, double split, double bill)
        {
            tip = Math.Max(tip, 0);         // ensure that the tip is at least 0%
            split = Math.Max(split, 1);     // ensure that the split is at least 1
            bill = Math.Abs(bill);          // ensure that the bill is nonnegative

            return Math.Round(bill * (1.0 + tip / 100.0) / split,2,MidpointRounding.AwayFromZero);
        }

        /*
         * Attempt to update the result every time the textbox receives new input
         */
        private void txtBillAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateResultIfPossible();
        }

        /*
         * Highlight all the text in the texbox when it is selected
         */
        private void txtBillAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}