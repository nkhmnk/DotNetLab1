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
                DisplayMainMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        HandleUserAuthentication();
                        break;
                    case "2":
                        RegisterAccount();
                        break;
                    case "3":
                        ExitProgram();
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine("1. Увійти");
            Console.WriteLine("2. Зареєструвати нову картку");
            Console.WriteLine("3. Вихід");
        }

        private void HandleUserAuthentication()
        {
            var account = AuthenticateUser();
            if (account != null)
            {
                StartATMOperations(account);
            }
        }

        private Account AuthenticateUser()
        {
            string cardNumber = PromptInput("Введіть номер картки: ", operations.IsValidCardNumber, "Неправильний номер картки");
            string pinCode = PromptInput("Введіть PIN: ", operations.IsValidPin, "Неправильний PIN-код");

            try
            {
                return operations.Authenticate(cardNumber, pinCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка. {ex.Message}");
                return null;
            }
        }

        private string PromptInput(string message, Func<string, bool> validationFunc, string errorMessage)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim();
                if (validationFunc(input))
                {
                    return input;
                }
                Console.WriteLine(errorMessage);
            }
        }

        private void StartATMOperations(Account account)
        {
            while (true)
            {
                DisplayATMMenu();
                string option = Console.ReadLine()?.Trim();

                switch (option)
                {
                    case "1":
                        Console.WriteLine($"Баланс: {operations.CheckBalance(account)} грн");
                        break;
                    case "2":
                        ProcessTransaction(account, operations.Withdraw, "Введіть суму для зняття: ");
                        break;
                    case "3":
                        ProcessTransaction(account, operations.Deposit, "Введіть суму для поповнення: ");
                        break;
                    case "4":
                        TransferMoney(account);
                        break;
                    case "5":
                        Console.WriteLine("Ви вийшли з акаунта");
                        return;
                    default:
                        Console.WriteLine("Немає такого варіанту");
                        break;
                }
            }
        }

        private void DisplayATMMenu()
        {
            Console.WriteLine("\n1. Перевірити баланс");
            Console.WriteLine("2. Зняти гроші");
            Console.WriteLine("3. Поповнити рахунок");
            Console.WriteLine("4. Перерахувати гроші");
            Console.WriteLine("5. Вийти на головне меню");
        }

        private void ProcessTransaction(Account account, Action<Account, decimal> transactionMethod, string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim();
            bool isValidAmount = decimal.TryParse(input, out decimal amount) && amount > 0;

            if (!isValidAmount)
            {
                Console.WriteLine("Невірна сума");
                return;
            }

            try
            {
                transactionMethod(account, amount);
                Console.WriteLine("Операція пройшла успішно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сталася помилка: {ex.Message}");
            }
        }


        private void TransferMoney(Account account)
        {
            string recipientCardNumber = PromptInput(
                "Введіть номер картки отримувача: ",
                operations.IsValidCardNumber,
                "Неправильний номер картки"
            );

            Console.Write("Введіть суму для переказу: ");
            string input = Console.ReadLine()?.Trim();

            if (!decimal.TryParse(input, out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Невірна сума");
                return;
            }

            try
            {
                operations.Transfer(account, recipientCardNumber, amount);
                Console.WriteLine("Переказ пройшов успішно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка. {ex.Message}");
            }
        }


        private void RegisterAccount()
        {
            string ownerName = PromptInput("Введіть своє ім'я: ", input => !string.IsNullOrWhiteSpace(input), "Ім'я не може бути порожнім.");
            string cardNumber = PromptInput("Введіть номер картки (6 цифр): ", operations.IsValidCardNumber, "Неправильний номер картки");
            string pinCode = PromptInput("Введіть PIN-код (4 цифри): ", operations.IsValidPin, "Неправильний PIN-код");
            decimal initialBalance = GetInitialBalance();

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

        private decimal GetInitialBalance()
        {
            Console.Write("Введіть початковий баланс: ");
            return decimal.TryParse(Console.ReadLine()?.Trim(), out decimal balance) && balance >= 0 ? balance : 0;
        }

        private void ExitProgram()
        {
            Console.WriteLine("До нових зустрічей!");
        }
    }
}