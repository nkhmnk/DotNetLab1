using System;
using System.Linq;

namespace ClassLibrary
{
    public class Operations
    {
        private readonly Bank _bank;

        public Operations(Bank bank)
        {
            _bank = bank ?? throw new ArgumentNullException(nameof(bank), "Банк не може бути null.");
        }

        public Account Authenticate(string cardNumber, string pinCode)
        {
            ValidateCardNumber(cardNumber);
            ValidatePinCode(pinCode);

            Account account = _bank.GetAccountByCardNumber(cardNumber);
            if (account == null || !account.Authenticate(pinCode))
                throw new UnauthorizedAccessException("Невірний номер картки або PIN-код.");

            return account;
        }

        public bool IsValidCardNumber(string cardNumber)
        {
            return !string.IsNullOrWhiteSpace(cardNumber) && cardNumber.Length == 6 && cardNumber.All(char.IsDigit);
        }

        public bool IsValidPin(string pinCode)
        {
            return !string.IsNullOrWhiteSpace(pinCode) && pinCode.Length == 4 && pinCode.All(char.IsDigit);
        }


        public decimal CheckBalance(Account account)
        {
            EnsureAccountNotNull(account);
            return account.Balance;
        }

        public void Withdraw(Account account, decimal amount)
        {
            EnsureAccountNotNull(account);
            account.Withdraw(amount);
        }

        public void Deposit(Account account, decimal amount)
        {
            EnsureAccountNotNull(account);
            account.Deposit(amount);
        }

        public void Transfer(Account sender, string recipientCardNumber, decimal amount)
        {
            EnsureAccountNotNull(sender);
            ValidateCardNumber(recipientCardNumber);

            Account recipient = _bank.GetAccountByCardNumber(recipientCardNumber);
            if (recipient == null)
                throw new ArgumentException("Отримувач із таким номером картки не знайдений.");

            sender.Transfer(recipient, amount);
        }

        public void RegisterAccount(string ownerName, string cardNumber, string pinCode, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
                throw new ArgumentException("Ім'я власника не може бути порожнім.");

            ValidateCardNumber(cardNumber);
            ValidatePinCode(pinCode);

            if (initialBalance < 0)
                throw new ArgumentException("Баланс не може бути від'ємним.");

            Account newAccount = new Account(cardNumber, ownerName, initialBalance, pinCode);
            _bank.AddAccount(newAccount);
        }

        private void ValidateCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length != 6)
                throw new ArgumentException("Невірний формат номера картки.");
        }

        private void ValidatePinCode(string pinCode)
        {
            if (string.IsNullOrWhiteSpace(pinCode) || pinCode.Length != 4)
                throw new ArgumentException("Невірний формат PIN-коду.");
        }

        private void EnsureAccountNotNull(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Акаунт не може бути null.");
        }
    }
}