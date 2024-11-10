using ClassLibrary;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfATMApp
{
    public partial class MainWindow : Window
    {
        private Bank bank;
        private Operations operations;

        public MainWindow()
        {
            InitializeComponent();
            bank = new Bank("Банк");
            operations = new Operations(bank);

            bank.Accounts.Add(new Account("123456", "Надія", 500, "1234"));
            bank.Accounts.Add(new Account("654321", "Володимир", 1000, "4321"));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string cardNumber = CardNumberTextBox.Text;
            string pinCode = PinCodeTextBox.Password;

            if (!IsValidCardNumber(cardNumber) || !IsValidPinCode(pinCode))
            {
                MessageBox.Show("Неправильний номер картки або пін-код", "Помилка входу", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Account account = operations.Authenticate(cardNumber, pinCode);
                if (account != null)
                {
                    ATMOperationsWindow operationsWindow = new ATMOperationsWindow(account, operations);
                    operationsWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка входу", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow(operations).Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private bool IsValidCardNumber(string cardNumber)
        {
            return Regex.IsMatch(cardNumber, @"^\d{6}$");
        }

        private bool IsValidPinCode(string pinCode)
        {
            return Regex.IsMatch(pinCode, @"^\d{4}$");
        }
    }
}
