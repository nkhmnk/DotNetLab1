using System;

namespace ClassLibrary
{
    public class Account
    {
        public string CardNumber { get; private set; }
        public string OwnerName { get; private set; }
        public decimal Balance { get; private set; }
        public string PinCode { get; private set; }

        public Account(string cardNumber, string ownerName, decimal balance, string pinCode)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length != 6)
                throw new ArgumentException("Номер картки повинен містити 6 цифр.");

            if (string.IsNullOrWhiteSpace(pinCode) || pinCode.Length != 4)
                throw new ArgumentException("PIN-код повинен містити 4 цифри.");

            if (balance < 0)
                throw new ArgumentException("Баланс не може бути від'ємним.");

            CardNumber = cardNumber;
            OwnerName = ownerName;
            Balance = balance;
            PinCode = pinCode;
        }

        public bool Authenticate(string pinCode)
        {
            return PinCode == pinCode;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума для поповнення повинна бути більше нуля.");

            Balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума для зняття повинна бути більше нуля.");

            if (Balance >= amount)
            {
                Balance -= amount;
                return true;
            }
            throw new InvalidOperationException("Недостатньо коштів для зняття.");
        }

        public void Transfer(Account toAccount, decimal amount)
        {
            if (toAccount == null)
                throw new ArgumentNullException(nameof(toAccount), "Отримувач не може бути null.");

            Withdraw(amount);
            toAccount.Deposit(amount);
        }
    }
}