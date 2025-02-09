using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    public class Bank
    {
        public string Name { get; private set; }
        public List<AutomatedTellerMachine> ATMs { get; private set; }
        public List<Account> Accounts { get; private set; }

        public Bank(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва банку не може бути порожньою.");

            Name = name;
            ATMs = new List<AutomatedTellerMachine>();
            Accounts = new List<Account>();
        }

        public void AddATM(AutomatedTellerMachine atm)
        {
            if (atm == null)
                throw new ArgumentNullException(nameof(atm), "Банкомат не може бути null.");

            ATMs.Add(atm);
        }

        public void AddAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Акаунт не може бути null.");

            if (Accounts.Any(a => a.CardNumber == account.CardNumber))
                throw new InvalidOperationException("Акаунт із таким номером картки вже існує.");

            Accounts.Add(account);
        }

        public Account GetAccountByCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length != 6)
                throw new ArgumentException("Невірний формат номера картки.");

            return Accounts.FirstOrDefault(account => account.CardNumber == cardNumber);
        }
    }
}

