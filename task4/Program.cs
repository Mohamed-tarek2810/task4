using System;
using System.Collections.Generic;

namespace task4
{
   //MOHAMED TAREK ELSAIED 
    internal class Program
    {
        static void Main(string[] args)
        {
            // Accounts
            var accounts = new List<Account>();
            accounts.Add(new Account());
            accounts.Add(new Account(name: "Larry"));
            accounts.Add(new Account(name: "Moe", balance: 2000));
            accounts.Add(new Account(name: "Curly", balance: 5000));

            AccountUtil.Deposit(accounts, amount: 1000);
            AccountUtil.Withdraw(accounts, amount: 2000);

            // Savings
            var savAccounts = new List<SavingsAccount>();
            savAccounts.Add(new SavingsAccount());
            savAccounts.Add(new SavingsAccount(name: "Superman"));
            savAccounts.Add(new SavingsAccount(name: "Batman", balance: 2000));
            savAccounts.Add(new SavingsAccount(name: "Wonderwoman", balance: 5000, interestRate: 5.0));

            AccountUtil.Deposit(savAccounts, amount: 1000);
            AccountUtil.Withdraw(savAccounts, amount: 2000);

            // Checking
            var checAccounts = new List<CheckingAccount>();
            checAccounts.Add(new CheckingAccount());
            checAccounts.Add(new CheckingAccount(name: "Larry2"));
            checAccounts.Add(new CheckingAccount(name: "Moe2", balance: 2000));
            checAccounts.Add(new CheckingAccount(name: "Curly2", balance: 5000));

            AccountUtil.Deposit(checAccounts, amount: 1000);
            AccountUtil.Withdraw(checAccounts, amount :2000);
            AccountUtil.Withdraw(checAccounts, amount: 2000);

            // Trust
            var trustAccounts = new List<TrustAccount>();
            trustAccounts.Add(new TrustAccount());
            trustAccounts.Add(new TrustAccount(name: "Superman2"));
            trustAccounts.Add(new TrustAccount(name: "Batman2", balance: 2000));
            trustAccounts.Add(new TrustAccount(name: "Wonderwoman2", balance: 5000, interestRate: 5.8));

            AccountUtil.Deposit(trustAccounts, amount: 1000);
            AccountUtil.Deposit(trustAccounts, amount: 6000);
            AccountUtil.Withdraw(trustAccounts, amount: 2000);
            AccountUtil.Withdraw(trustAccounts, amount: 3000);
            AccountUtil.Withdraw(trustAccounts, amount: 500);

            Console.WriteLine();
        }
    }

    public class Account
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Account(string name = "Unnamed Account", double balance = 0.0)
        {
            Name = name;
            Balance = balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount < 0) return false;
            Balance += amount;
            return true;
        }

        public virtual bool Withdraw(double amount)
        {
            if (Balance - amount >= 0)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"name: {Name}, Balance = {Balance}";
        }
    }

    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(string name = "Unnamed Savings Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance)
        {
            InterestRate = interestRate;
        }

        public void ApplyInterest()
        {
            Balance += Balance * (InterestRate / 100);
        }

        public override string ToString()
        {
            return $"{base.ToString()}, InterestRate: {InterestRate}";
        }
    }

    public class CheckingAccount : Account
    {
        private double Fee = 1.50;

        public CheckingAccount(string name = "Unnamed Checking Account", double balance = 0.0)
            : base(name, balance) { }

        public override bool Withdraw(double amount)
        {
            double total = amount + Fee;
            return base.Withdraw(total);
        }
    }

    public class TrustAccount : SavingsAccount
    {
        private readonly double withdramTriesThreshold = 3;
        private int triesCounter = 0;
        private DateTime triesDate = DateTime.Now.Date;
        private const double BonusThreshold = 5000.0;
        private const double BonusAmount = 50.0;
        private const double MaxWithdrawalPercent = 0.20;

        public TrustAccount(string name = "Unnamed Trust Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance, interestRate) { }

        public override bool Deposit(double amount)
        {
            if (amount >= BonusThreshold)
                amount += BonusAmount;
            return base.Deposit(amount);
        }

        public override bool Withdraw(double amount)
        {
            if (DateTime.Now.Date > triesDate.AddYears(1))
            {
                triesDate = DateTime.Now.Date;
                triesCounter = 0;
            }

            if (triesCounter < withdramTriesThreshold && amount < (Balance * MaxWithdrawalPercent))
            {
                triesCounter++;
                return base.Withdraw(amount);
            }

            return false;
        }
    }

    public static class AccountUtil
    {
        public static void Deposit<T>(List<T> accounts, double amount) where T : Account
        {
            Console.WriteLine($"\n=== Depositing to {typeof(T).Name}s =================================");
            foreach (var acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited {amount} to {acc}");
                else
                    Console.WriteLine($"Failed Deposit of {amount} to {acc}");
            }
        }

        public static void Withdraw<T>(List<T> accounts, double amount) where T : Account
        {
            Console.WriteLine($"\n=== Withdrawing from {typeof(T).Name}s ==============================");
            foreach (var acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew {amount} from {acc}");
                else
                    Console.WriteLine($"Failed Withdrawal of {amount} from {acc}");
            }
        }
    }
}
