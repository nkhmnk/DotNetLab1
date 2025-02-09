using ClassLibrary;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetLab1
{
    internal class Program
    {
        static Bank bank = new Bank("Банк");
        static Operations Operations = new Operations(bank);

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            bank.Accounts.Add(new Account("123456", "Надія", 500, "1234"));
            bank.Accounts.Add(new Account("654321", "Володимир", 1000, "4321"));

            while (true)
            {
                Console.WriteLine("1. Увійти");
                Console.WriteLine("2. Зареєструвати нову картку");
                Console.WriteLine("3. Вихід");
                string choice = Console.ReadLine();

                if (choice == "3")
                {
                    Console.WriteLine("До нових зустрічей! ");
                    return;
                }

                if (choice == "2")
                {
                    RegisterAccount();
                    continue;
                }

                Account account = AuthenticateUser();

                if (account == null)
                {
                    continue;
                }

                StartATMOperations(account);
            }
        }

        private static Account AuthenticateUser() //вхід
        {
            string cardNumber = ReadValidCardNumber("Введіть номер картки: ");
            string pinCode = ReadValidPinCode("Введіть PIN: ");
            
            try
            {
                return Operations.Authenticate(cardNumber, pinCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка. {ex.Message}");
                return null;
            }
        }

        private static string ReadValidCardNumber(string prompt) // зчитування та перевірка номеру картки
        {
            while (true)
            {
                Console.Write(prompt);
                string cardNumber = Console.ReadLine();

                if (IsValidCardNumber(cardNumber))
                    return cardNumber;
                
                Console.WriteLine("Неправильний номер картки");
            }
        }

        private static string ReadValidPinCode(string prompt) // зчитування та перевірка PIN-коду
        {
            while (true)
            {
                Console.Write(prompt);
                string pinCode = Console.ReadLine();

                if (IsValidPinCode(pinCode))
                    return pinCode;
                
                Console.WriteLine("Неправильний PIN-код");
            }
        }

        private static bool IsValidCardNumber(string cardNumber) // перевірка картки
        {
            return Regex.IsMatch(cardNumber, @"^\d{6}$");
        }

        private static bool IsValidPinCode(string pinCode) // перевірка пін-коду
        {
            return Regex.IsMatch(pinCode, @"^\d{4}$");
        }

        private static void RegisterAccount() //реєстрація
        {
            Console.Write("Введіть своє ім'я: ");
            string ownerName = Console.ReadLine();
            
            string cardNumber = ReadValidCardNumber("Введіть номер картки (6 цифр): ");
            string pinCode = ReadValidPinCode("Введіть PIN-код (4 цифри): ");
            
            decimal balance = ReadValidBalance();

            try
            {
                Operations.RegisterAccount(ownerName, cardNumber, pinCode, balance);
                Console.WriteLine("Акаунт зареєстровано");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка. {ex.Message}");
            }
        }

        private static decimal ReadValidBalance() // перевірка балансу
        {
            while (true)
            {
                Console.Write("Введіть початковий баланс: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal balance))
                {
                    return balance;
                }
                else
                {
                    Console.WriteLine("Невірний баланс");
                }
            }
        }

        private static void WithdrawMoney(Account account) // зняття коштів
        {
            decimal withdrawAmount = ReadValidAmount("Введіть суму для зняття: ");
            
            if (Operations.Withdraw(account, withdrawAmount))
            {
                Console.WriteLine("Зняття пройшло успішно");
            }
            else
            {
                Console.WriteLine("Недостатньо коштів");
            }
        }

        private static void DepositMoney(Account account) // поповнення
        {
            decimal depositAmount = ReadValidAmount("Введіть суму для поповнення: ");
            
            Operations.Deposit(account, depositAmount);
            Console.WriteLine("Поповнення пройшло успішно");
        }

        private static void TransferMoney(Account account) //переказ
        {
            string recipientCardNumber = ReadValidCardNumber("Введіть номер картки отримувача: ");
            decimal transferAmount = ReadValidAmount("Введіть суму для переказу: ");
            
            try
            {
                Operations.Transfer(account, recipientCardNumber, transferAmount);
                Console.WriteLine("Переказ пройшов успішно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка. {ex.Message}");
            }
        }

        private static decimal ReadValidAmount(string prompt) // перевірка суми
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    return amount;
                }
                else
                {
                    Console.WriteLine("Невірна сума");
                }
            }
        }

        private static void StartATMOperations(Account account) //меню
        {
            while (true)
            {
                Console.WriteLine("\n1. Перевірити баланс");
                Console.WriteLine("2. Зняти гроші");
                Console.WriteLine("3. Поповнити рахунок");
                Console.WriteLine("4. Перерахувати гроші");
                Console.WriteLine("5. Вийти на головне меню");
                string option = Console.ReadLine();

                try
                {
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine($"Баланс: {Operations.CheckBalance(account)} грн");
                            break;
                        case "2":
                            WithdrawMoney(account);
                            break;
                        case "3":
                            DepositMoney(account);
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Сталася помилка: {ex.Message}");
                }
            }
        }
    }
}
