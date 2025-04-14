using System;

public class Money
{
    private int _hryvnia;
    private int _kopiyka;

    public int Hryvnia
    {
        get => _hryvnia;
        private set
        {
            if (value < 0)
                throw new BankruptException("Банкрот");
            _hryvnia = value;
        }
    }

    public int Kopiyka
    {
        get => _kopiyka;
        private set
        {
            if (value < 0 || value >= 100)
            {
                Hryvnia += value / 100;
                value %= 100;
            }
            _kopiyka = value;
        }
    }

    public Money(int hryvnia, int kopiyka)
    {
        Hryvnia = hryvnia;
        Kopiyka = kopiyka;
        Normalize();
    }

    private void Normalize()
    {
        Hryvnia += Kopiyka / 100;
        Kopiyka %= 100;

        if (Hryvnia < 0 || (Hryvnia == 0 && Kopiyka < 0))
            throw new BankruptException("Банкрот");
    }

    // Перегрузка оператора +
    public static Money operator +(Money a, Money b)
    {
        int totalKopiykas = a.Kopiyka + b.Kopiyka;
        int totalHryvnias = a.Hryvnia + b.Hryvnia;

        return new Money(totalHryvnias, totalKopiykas);
    }

    // Перегрузка оператора -
    public static Money operator -(Money a, Money b)
    {
        int totalKopiykas = a.Kopiyka - b.Kopiyka;
        int totalHryvnias = a.Hryvnia - b.Hryvnia;

        return new Money(totalHryvnias, totalKopiykas);
    }

    // Перегрузка оператора /
    public static Money operator /(Money a, int divisor)
    {
        if (divisor <= 0)
            throw new ArgumentException("Делитель должен быть положительным числом");

        int totalKopiykas = (a.Hryvnia * 100 + a.Kopiyka) / divisor;
        return new Money(0, totalKopiykas);
    }

    // Перегрузка оператора *
    public static Money operator *(Money a, int multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentException("Множитель не может быть отрицательным");

        int totalKopiykas = (a.Hryvnia * 100 + a.Kopiyka) * multiplier;
        return new Money(0, totalKopiykas);
    }

    // Перегрузка оператора ++ (префиксный)
    public static Money operator ++(Money a)
    {
        return new Money(a.Hryvnia, a.Kopiyka + 1);
    }

    // Перегрузка оператора -- (префиксный)
    public static Money operator --(Money a)
    {
        return new Money(a.Hryvnia, a.Kopiyka - 1);
    }

    // Перегрузка оператора <
    public static bool operator <(Money a, Money b)
    {
        return a.Hryvnia < b.Hryvnia || (a.Hryvnia == b.Hryvnia && a.Kopiyka < b.Kopiyka);
    }

    // Перегрузка оператора >
    public static bool operator >(Money a, Money b)
    {
        return a.Hryvnia > b.Hryvnia || (a.Hryvnia == b.Hryvnia && a.Kopiyka > b.Kopiyka);
    }

    // Перегрузка оператора ==
    public static bool operator ==(Money a, Money b)
    {
        return a.Hryvnia == b.Hryvnia && a.Kopiyka == b.Kopiyka;
    }

    // Перегрузка оператора !=
    public static bool operator !=(Money a, Money b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Money other)
            return this == other;
        return false;
    }

    public override int GetHashCode()
    {
        return Hryvnia * 100 + Kopiyka;
    }

    public override string ToString()
    {
        return $"{Hryvnia} грн {Kopiyka} коп";
    }
}

public class BankruptException : Exception
{
    public BankruptException(string message) : base(message) { }
}

public class Program
{
    public static void Main()
    {
        Money money1 = null;
        Money money2 = null;

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Создать первую денежную сумму");
            Console.WriteLine("2. Создать вторую денежную сумму");
            Console.WriteLine("3. Сложить суммы");
            Console.WriteLine("4. Вычесть суммы");
            Console.WriteLine("5. Разделить сумму на число");

            Console.WriteLine("6. Умножить сумму на число");
            Console.WriteLine("7. Увеличить сумму на 1 копейку (++)");
            Console.WriteLine("8. Уменьшить сумму на 1 копейку (--)");
            Console.WriteLine("9. Сравнить суммы");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        money1 = CreateMoney();
                        Console.WriteLine($"Создана сумма: {money1}");
                        break;

                    case "2":
                        money2 = CreateMoney();
                        Console.WriteLine($"Создана сумма: {money2}");
                        break;

                    case "3":
                        if (CheckMoney(money1, money2))
                            Console.WriteLine($"Результат сложения: {money1 + money2}");
                        break;

                    case "4":
                        if (CheckMoney(money1, money2))
                            Console.WriteLine($"Результат вычитания: {money1 - money2}");
                        break;

                    case "5":
                        if (CheckMoney(money1))
                        {
                            Console.Write("Введите целое число для деления: ");
                            int divisor = int.Parse(Console.ReadLine());
                            Console.WriteLine($"Результат деления: {money1 / divisor}");
                        }
                        break;

                    case "6":
                        if (CheckMoney(money1))
                        {
                            Console.Write("Введите целое число для умножения: ");
                            int multiplier = int.Parse(Console.ReadLine());
                            Console.WriteLine($"Результат умножения: {money1 * multiplier}");
                        }
                        break;

                    case "7":
                        if (CheckMoney(money1))
                        {
                            money1++;
                            Console.WriteLine($"Сумма после увеличения: {money1}");
                        }
                        break;

                    case "8":
                        if (CheckMoney(money1))
                        {
                            money1--;
                            Console.WriteLine($"Сумма после уменьшения: {money1}");
                        }
                        break;

                    case "9":
                        if (CheckMoney(money1, money2))
                        {
                            Console.WriteLine($"Сумма 1: {money1}");
                            Console.WriteLine($"Сумма 2: {money2}");
                            Console.WriteLine($"Сумма 1 > Сумма 2: {money1 > money2}");
                            Console.WriteLine($"Сумма 1 < Сумма 2: {money1 < money2}");
                            Console.WriteLine($"Сумма 1 == Сумма 2: {money1 == money2}");
                            Console.WriteLine($"Сумма 1 != Сумма 2: {money1 != money2}");
                        }
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
            catch (BankruptException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    private static Money CreateMoney()
    {
        Console.Write("Введите гривны: ");
        int hryvnia = int.Parse(Console.ReadLine());
        Console.Write("Введите копейки: ");
        int kopiyka = int.Parse(Console.ReadLine());
        return new Money(hryvnia, kopiyka);
    }

    private static bool CheckMoney(params Money[] moneys)
    {
        foreach (var money in moneys)
        {
            if (money == null)
            {
                Console.WriteLine("Денежная сумма не создана. Сначала создайте сумму.");
                return false;
            }
        }
        return true;
    }
}