using ClassLibrary;
using System;
using System.Windows;

namespace WpfATMApp
{
    public partial class ATMOperationsWindow : Window
    {
        private Account account;
        private Operations operations;

        public ATMOperationsWindow(Account account, Operations operations)
        {
            InitializeComponent();
            this.account = account;
            this.operations = operations;
        }

        // Перевірка балансу
        private void CheckBalanceButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Ваш баланс: {account.Balance} грн");
        }

        // Зняття грошей
        private void WithdrawMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(WithdrawAmountTextBox.Text, out decimal withdrawAmount))
            {
                if (operations.Withdraw(account, withdrawAmount))
                {
                    MessageBox.Show($"Знято {withdrawAmount} грн\nБаланс: {account.Balance}");
                }
                else
                {
                    MessageBox.Show("Недостатньо коштів");
                }
            }
            else
            {
                MessageBox.Show("Невірна сума для зняття");
            }
        }

        // Поповнення рахунку
        private void DepositMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(DepositAmountTextBox.Text, out decimal depositAmount))
            {
                operations.Deposit(account, depositAmount);
                MessageBox.Show($"Поповнено на {depositAmount} грн\nБаланс: {account.Balance}");
            }
            else
            {
                MessageBox.Show("Невірна сума для поповнення");
            }
        }

        // Переказ грошей
        private void TransferMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            string recipientCardNumber = RecipientCardNumberTextBox.Text;
            if (decimal.TryParse(TransferAmountTextBox.Text, out decimal transferAmount))
            {
                try
                {
                    // Пытаемся выполнить перевод
                    operations.Transfer(account, recipientCardNumber, transferAmount);
                    MessageBox.Show($"Перераховано {transferAmount} грн на картку {recipientCardNumber}\nБаланс: {account.Balance}");
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка, выводим сообщение
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Невірна сума для переказу");
            }
        }

        // Вихід
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
