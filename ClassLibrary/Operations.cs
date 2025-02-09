using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Operations
    {
        public Bank bank;

        public Operations(Bank bank)
        {
            this.bank = bank;
        }

        // вхід
        public Account Authenticate(string cardNumber, string pinCode)
        {
            if (!IsValidCardNumber(cardNumber))
            {
                throw new ArgumentException("Невірний формат номера картки! Номер картки повинен складатися з 6 цифр.");
            }

            if (!IsValidPin(pinCode))
            {
                throw new ArgumentException("Невірний формат PIN-коду! PIN повинен складатися з 4 цифр.");
            }

            Account account = bank.GetAccountByCardNumber(cardNumber);
            if (account != null && account.Authenticate(pinCode))
            {
                return account;
            }
            else
            {
                throw new UnauthorizedAccessException("Невірний номер картки або PIN.");
            }
        }

        // перевірка балансу
        public decimal CheckBalance(Account account)
        {
            return account.Balance;
        }

        // зняти кошти
        public bool Withdraw(Account account, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сума для зняття повинна бути більше нуля.");
            }

            return account.Withdraw(amount);
        }

        // поповнення
        public void Deposit(Account account, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сума для поповнення повинна бути більше нуля.");
            }

            account.Deposit(amount);
        }

        // переказ
        public void Transfer(Account sender, string recipientCardNumber, decimal amount)
        {
            if (!IsValidCardNumber(recipientCardNumber))
            {
                throw new ArgumentException("Невірний формат номера картки отримувача.");
            }

            Account recipientAccount = bank.GetAccountByCardNumber(recipientCardNumber);
            if (recipientAccount == null)
            {
                throw new ArgumentException("Невірний номер картки отримувача.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Сума для переказу повинна бути більше нуля.");
            }

            sender.Transfer(recipientAccount, amount);
        }

        // реєстрація
        public void RegisterAccount(string ownerName, string cardNumber, string pinCode, decimal initialBalance)
        {
            if (!IsValidCardNumber(cardNumber))
            {
                throw new ArgumentException("Невірний формат номера картки! Номер картки повинен складатися з 6 цифр.");
            }

            if (!IsValidPin(pinCode))
            {
                throw new ArgumentException("Невірний формат PIN-коду! PIN повинен складатися з 4 цифр.");
            }

            if (initialBalance < 0)
            {
                throw new ArgumentException("Баланс не може бути від'ємним.");
            }

            Account newAccount = new Account(cardNumber, ownerName, initialBalance, pinCode);
            bank.Accounts.Add(newAccount);
        }



        // валідація номера картки
        private bool IsValidCardNumber(string cardNumber)
        {
            return cardNumber.Length == 6 && long.TryParse(cardNumber, out _);
        }

        // валідація пін коду
        private bool IsValidPin(string pinCode)
        {
            return pinCode.Length == 4 && int.TryParse(pinCode, out _);
        }


    }
}
