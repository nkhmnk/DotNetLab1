using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Bank
    {
        public string Name { get; private set; }
        public List<AutomatedTellerMachine> ATMs { get; private set; }
        public List<Account> Accounts { get; private set; }

        public Bank(string name)
        {
            Name = name;
            ATMs = new List<AutomatedTellerMachine>();
            Accounts = new List<Account>();
        }

        public Account GetAccountByCardNumber(string cardNumber)
        {
            return Accounts.FirstOrDefault(account => account.CardNumber == cardNumber);
        }
    }
}
