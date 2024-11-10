using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AutomatedTellerMachine
    {
        public string Id { get; private set; }
        public string Address { get; private set; }
        public decimal CashAvailable { get; private set; }

        public AutomatedTellerMachine(string id, string address, decimal cashAvailable)
        {
            Id = id;
            Address = address;
            CashAvailable = cashAvailable;
        }

        public bool WithdrawCash(decimal amount) //зняття
        {
            if (CashAvailable >= amount)
            {
                CashAvailable -= amount;
                return true;
            }
            return false;
        }

        public void DepositCash(decimal amount) //поповнення
        {
            CashAvailable += amount;
        }
    }
}
