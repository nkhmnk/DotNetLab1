using ClassLibrary;
using System;
using System.Windows;

namespace WpfATMApp
{
    public partial class RegisterWindow : Window
    {
        private Operations operations;

        public RegisterWindow(Operations operations)
        {
            InitializeComponent();
            this.operations = operations;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string ownerName = OwnerNameTextBox.Text;
            string cardNumber = CardNumberTextBox.Text;
            string pinCode = PinCodeTextBox.Password;
            decimal balance = decimal.Parse(BalanceTextBox.Text);

            try
            {
                operations.RegisterAccount(ownerName, cardNumber, pinCode, balance);
                MessageBox.Show("Акаунт зареєстровано");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }
    }
}

