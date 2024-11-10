using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            string cardNumber;
            string pinCode;

            while (true)
            {
                Console.Write("Введіть номер картки: ");
                cardNumber = Console.ReadLine();

                if (!IsValidCardNumber(cardNumber))
                {
                    Console.WriteLine("Неправильний номер картки");
                    continue;
                }

                while (true)
                {
                    Console.Write("Введіть PIN: ");
                    pinCode = Console.ReadLine();

                    if (IsValidPinCode(pinCode))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неправильний PIN-код");
                    }
                }

                try
                {
                    return Operations.Authenticate(cardNumber, pinCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка. {ex.Message}");
                }
            }
        }

        private static bool IsValidCardNumber(string cardNumber) // перевірка карти
        {
            return Regex.IsMatch(cardNumber, @"^\d{6}$");
        }

        private static bool IsValidPinCode(string pinCode) // перевірка пін-коду
        {
            return Regex.IsMatch(pinCode, @"^\d{4}$");
        }

        private static void WithdrawMoney(Account account) // зняття коштів
        {
            while (true)
            {
                Console.Write("Введіть суму для зняття: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                {
                    if (Operations.Withdraw(account, withdrawAmount))
                    {
                        Console.WriteLine("Зняття пройшло успішно");
                    }
                    else
                    {
                        Console.WriteLine("Недостатньо коштів");
                    }
                    return; 
                }
                else
                {
                    Console.WriteLine("Невірна сума");
                }
            }
        }

        private static void DepositMoney(Account account) // поповнення
        {
            while (true)
            {
                Console.Write("Введіть суму для поповнення: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                {
                    Operations.Deposit(account, depositAmount);
                    Console.WriteLine("Поповнення пройшло успішно");
                    return; 
                }
                else
                {
                    Console.WriteLine("Невірна сума");
                }
            }
        }

        private static void TransferMoney(Account account) //переказ
        {
            while (true)
            {
                Console.Write("Введіть номер картки отримувача: ");
                string recipientCardNumber = Console.ReadLine();

                if (!IsValidCardNumber(recipientCardNumber))
                {
                    Console.WriteLine("Неправильний номер картки");
                    continue; 
                }

                Console.Write("Введіть суму для переказу: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount))
                {
                    try
                    {
                        Operations.Transfer(account, recipientCardNumber, transferAmount);
                        Console.WriteLine("Переказ пройшов успішно");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка. {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Невірна сума");
                }
            }
        }

        private static void RegisterAccount() //реєстрація
        {
            Console.Write("Введіть своє ім'я: ");
            string ownerName = Console.ReadLine();
            string cardNumber;

            while (true)
            {
                Console.Write("Введіть номер картки (6 цифр): ");
                cardNumber = Console.ReadLine();
                if (IsValidCardNumber(cardNumber))
                    break;

                Console.WriteLine("Неправильний номер картки");
            }

            string pinCode;
            while (true)
            {
                Console.Write("Введіть PIN-код (4 цифри): ");
                pinCode = Console.ReadLine();
                if (IsValidPinCode(pinCode))
                    break;

                Console.WriteLine("Неправильний PIN-код");
            }

            decimal balance;

            while (true)
            {
                Console.Write("Введіть початковий баланс: ");
                if (decimal.TryParse(Console.ReadLine(), out balance))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Невірний баланс");
                }
            }

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