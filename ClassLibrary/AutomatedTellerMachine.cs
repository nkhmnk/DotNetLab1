using System;

namespace ClassLibrary
{
    public class AutomatedTellerMachine
    {
        public string Id { get; private set; }
        public string Address { get; private set; }
        public decimal CashAvailable { get; private set; }

        public AutomatedTellerMachine(string id, string address, decimal cashAvailable)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID банкомата не може бути порожнім.");

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Адреса банкомата не може бути порожньою.");

            if (cashAvailable < 0)
                throw new ArgumentException("Кількість доступних коштів не може бути від'ємною.");

            Id = id;
            Address = address;
            CashAvailable = cashAvailable;
        }

        public void WithdrawCash(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума для зняття повинна бути більше нуля.");

            if (CashAvailable < amount)
                throw new InvalidOperationException("Недостатньо коштів у банкоматі.");

            CashAvailable -= amount;
        }

        public void DepositCash(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума для поповнення повинна бути більше нуля.");

            CashAvailable += amount;
        }
    }
}