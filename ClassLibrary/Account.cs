using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CardNumber = cardNumber;
            OwnerName = ownerName;
            Balance = balance;
            PinCode = pinCode;
        }

        public bool Authenticate(string pinCode) //вхід
        {
            return PinCode == pinCode;
        }

        public void Deposit(decimal amount) //поповнення
        {
            Balance += amount;
        }

        public bool Withdraw(decimal amount) //зняття
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public void Transfer(Account toAccount, decimal amount) //переказ
        {
            if (Withdraw(amount))
            {
                toAccount.Deposit(amount);
            }
        }

    }
}
