using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

namespace DotNetLab1
{
    public class UserMenu
    {
        private readonly Bank bank;
        private readonly Operations operations;

        public UserMenu()
        {
            bank = new Bank("Банк");
            operations = new Operations(bank);
        }

        public void ConfigureConsole()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
        }

        public void InitializeBank()
        {
            bank.Accounts.Add(new Account("123456", "Надія", 500, "1234"));
            bank.Accounts.Add(new Account("654321", "Володимир", 1000, "4321"));
        }

        public void RunMainMenu()
        {
            while (true)
            {
                DisplayMenu(new[] { "Увійти", "Зареєструвати нову картку", "Вихід" });
                switch (Console.ReadLine()?.Trim())
                {
                    case "1": HandleUserAuthentication(); break;
                    case "2": RegisterAccount(); break;
                    case "3": ExitProgram(); return;
                    default: Console.WriteLine("Невірний вибір. Спробуйте ще раз."); break;
                }
            }
        }

        private void DisplayMenu(string[] options)
        {
            for (int i = 0; i < options.Length; i++)
                Console.WriteLine($"{i + 1}. {options[i]}");
        }

        private void HandleUserAuthentication()
        {
            var account = AuthenticateUser();
            if (account != null) StartATMOperations(account);
        }

        private Account AuthenticateUser()
        {
            string cardNumber = PromptInput("Введіть номер картки: ", input => (operations.IsValidCardNumber(input), input), "Неправильний номер картки");
            string pinCode = PromptInput("Введіть PIN: ", input => (operations.IsValidPin(input), input), "Неправильний PIN-код");

            try { return operations.Authenticate(cardNumber, pinCode); }
            catch (Exception ex) { Console.WriteLine($"Помилка. {ex.Message}"); return null; }
        }

        private T PromptInput<T>(string message, Func<string, (bool, T)> validationFunc, string errorMessage)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim();
                var (isValid, result) = validationFunc(input);
                if (isValid) return result;
                Console.WriteLine(errorMessage);
            }
        }

        private void StartATMOperations(Account account)
        {
            while (true)
            {
                DisplayMenu(new[] { "Перевірити баланс", "Зняти гроші", "Поповнити рахунок", "Перерахувати гроші", "Вийти на головне меню" });
                switch (Console.ReadLine()?.Trim())
                {
                    case "1": Console.WriteLine($"Баланс: {operations.CheckBalance(account)} грн"); break;
                    case "2": ProcessTransaction(account, operations.Withdraw, "Введіть суму для зняття: "); break;
                    case "3": ProcessTransaction(account, operations.Deposit, "Введіть суму для поповнення: "); break;
                    case "4": TransferMoney(account); break;
                    case "5": Console.WriteLine("Ви вийшли з акаунта"); return;
                    default: Console.WriteLine("Немає такого варіанту"); break;
                }
            }
        }

        private void ProcessTransaction(Account account, Action<Account, decimal> transactionMethod, string prompt)
        {
            decimal amount = PromptInput(prompt, input => (decimal.TryParse(input, out decimal val) && val > 0, val), "Невірна сума, спробуйте ще раз.");
            try { transactionMethod(account, amount); Console.WriteLine("Операція пройшла успішно."); }
            catch (Exception ex) { Console.WriteLine($"Сталася помилка: {ex.Message}"); }
        }

        private void TransferMoney(Account account)
        {
            string recipientCardNumber = PromptInput("Введіть номер картки отримувача: ", input => (operations.IsValidCardNumber(input), input), "Неправильний номер картки");
            decimal amount = PromptInput("Введіть суму для переказу: ", input => (decimal.TryParse(input, out decimal val) && val > 0, val), "Невірна сума, спробуйте ще раз.");
            try { operations.Transfer(account, recipientCardNumber, amount); Console.WriteLine("Переказ пройшов успішно"); }
            catch (Exception ex) { Console.WriteLine($"Помилка. {ex.Message}"); }
        }

        private void RegisterAccount()
        {
            string ownerName = PromptInput("Введіть своє ім'я: ", input => (!string.IsNullOrWhiteSpace(input), input), "Ім'я не може бути порожнім.");
            string cardNumber = PromptInput("Введіть номер картки: ", input => {bool isValid = operations.IsValidCardNumber(input);return (isValid, input);}, "Неправильний номер картки");
            string pinCode = PromptInput("Введіть PIN-код (4 цифри): ", input => (operations.IsValidPin(input), input), "Неправильний PIN-код");
            decimal initialBalance = PromptInput("Введіть початковий баланс: ", input => (decimal.TryParse(input, out decimal val) && val >= 0, val), "Невірна сума, спробуйте ще раз.");

            try
            {
                operations.RegisterAccount(ownerName, cardNumber, pinCode, initialBalance);
                Console.WriteLine("Акаунт зареєстровано");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка. {ex.Message}");
            }

        }

        private void ExitProgram() => Console.WriteLine("До нових зустрічей!");
    }
}