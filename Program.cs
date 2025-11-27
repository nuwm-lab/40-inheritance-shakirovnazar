using System;

// Використовуємо один екземпляр Random для генерації випадкових чисел
// Це запобігає отриманню однакових послідовностей.
public static class GlobalRandom
{
    public static readonly Random Random = new Random();
}

// ====================================================================
// 1. АБСТРАКТНИЙ БАЗОВИЙ КЛАС
// Визначає спільний інтерфейс для всіх типів матриць (2D, 3D і т.д.)
// ====================================================================
public abstract class MatrixBase
{
    // Забороняємо деструктор, як це прийнято у C# для класів без unmanaged ресурсів
    // Визначення абстрактних методів, які мають бути реалізовані в похідних класах

    /// <summary>Введення елементів матриці з клавіатури.</summary>
    public abstract void SetFromKeyboard();

    /// <summary>Заповнення матриці випадковими числами.</summary>
    public abstract void SetRandom();

    /// <summary>Пошук мінімального елемента в матриці.</summary>
    public abstract double FindMin();

    /// <summary>Отримання розмірності матриці (для виводу).</summary>
    public abstract string GetDimensions();

    /// <summary>Виведення матриці на консоль.</summary>
    public abstract void Display();

    /// <summary>
    /// Допоміжний метод для безпечного введення числа типу double з клавіатури.
    /// Винесено для чистоти коду, але по суті є частиною I/O логіки.
    /// </summary>
    protected double ReadDouble(string prompt)
    {
        Console.Write(prompt);
        double value;
        // Покращена валідація вводу: використовуємо double.TryParse
        while (!double.TryParse(Console.ReadLine(), out value))
        {
            Console.WriteLine("Помилка вводу. Будь ласка, введіть дійсне число.");
            Console.Write(prompt);
        }
        return value;
    }
}

// ====================================================================
// 2. ПОХІДНИЙ КЛАС: ДВОВИМІРНА МАТРИЦЯ
// ====================================================================
public class Matrix2D : MatrixBase
{
    // Приватне поле для зберігання даних. Захищено інкапсуляцією.
    private double[,] _data;
    private const int N = 3; // Розмірність 3x3

    // Конструктор: ініціалізує масив
    public Matrix2D()
    {
        _data = new double[N, N];
    }

    // Реалізація абстрактного методу для вводу з клавіатури
    public override void SetFromKeyboard()
    {
        Console.WriteLine($"Введення елементів матриці {N}x{N}:");
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                // Використовуємо допоміжний метод для безпечного вводу
                _data[i, j] = ReadDouble($"[{i}, {j}]: ");
            }
        }
    }

    // Реалізація абстрактного методу для випадкового заповнення
    public override void SetRandom()
    {
        Console.WriteLine("Заповнення випадковими числами (від 0 до 100)...");
        Random random = GlobalRandom.Random;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                // Генерація дійсного числа від 0 до 100
                _data[i, j] = random.NextDouble() * 100;
            }
        }
    }

    // Реалізація абстрактного методу для пошуку мінімального елемента
    public override double FindMin()
    {
        // Ініціалізуємо мінімум першим елементом для коректного порівняння
        double min = _data[0, 0]; 
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (_data[i, j] < min)
                {
                    min = _data[i, j];
                }
            }
        }
        return min;
    }

    // Виведення розмірності
    public override string GetDimensions()
    {
        return $"Двовимірна матриця ({N}x{N})";
    }

    // Виведення матриці
    public override void Display()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write($"{_data[i, j],8:F2} "); // Форматування для красивого виводу
            }
            Console.WriteLine();
        }
    }
}

// ====================================================================
// 3. ПОХІДНИЙ КЛАС: ТРИВИМІРНА МАТРИЦЯ
// ====================================================================
public class Matrix3D : MatrixBase
{
    private double[,,] _data;
    private const int N = 3; // Розмірність 3x3x3

    // Конструктор: ініціалізує масив
    public Matrix3D()
    {
        _data = new double[N, N, N];
    }

    // Перевизначення методу для вводу з клавіатури (3D)
    public override void SetFromKeyboard()
    {
        Console.WriteLine($"Введення елементів матриці {N}x{N}x{N}:");
        for (int k = 0; k < N; k++)
        {
            Console.WriteLine($"--- Слой [*, *, {k}] ---");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    _data[i, j, k] = ReadDouble($"[{i}, {j}, {k}]: ");
                }
            }
        }
    }

    // Перевизначення методу для випадкового заповнення (3D)
    public override void SetRandom()
    {
        Console.WriteLine("Заповнення випадковими числами (від 0 до 100)...");
        Random random = GlobalRandom.Random;
        for (int k = 0; k < N; k++)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    _data[i, j, k] = random.NextDouble() * 100;
                }
            }
        }
    }

    // Перевизначення методу для пошуку мінімального елемента (3D)
    public override double FindMin()
    {
        double min = _data[0, 0, 0];
        for (int k = 0; k < N; k++)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (_data[i, j, k] < min)
                    {
                        min = _data[i, j, k];
                    }
                }
            }
        }
        return min;
    }

    // Виведення розмірності
    public override string GetDimensions()
    {
        return $"Тривимірна матриця ({N}x{N}x{N})";
    }

    // Виведення матриці (3D)
    public override void Display()
    {
        for (int k = 0; k < N; k++)
        {
            Console.WriteLine($"\nСлой {k}:");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write($"{_data[i, j, k],8:F2} ");
                }
                Console.WriteLine();
            }
        }
    }
}

// ====================================================================
// 4. ОСНОВНА ПРОГРАМА (ТОЧКА ВХОДУ)
// Демонструє поліморфізм
// ====================================================================
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("--- Лабораторна робота: Масиви об'єктів та Наслідування (Матриці) ---");

        // Демонстрація двовимірної матриці (2D)
        Matrix2D matrix2D = new Matrix2D();
        Console.WriteLine("\n=======================================================");
        Console.WriteLine($"1. Робота з {matrix2D.GetDimensions()}");
        Console.WriteLine("=======================================================");

        // Заповнення випадковими числами
        matrix2D.SetRandom();
        Console.WriteLine("\nМатриця після випадкового заповнення:");
        matrix2D.Display();
        Console.WriteLine($"Мінімальний елемент: {matrix2D.FindMin():F2}");

        // Приклад введення з клавіатури (можна закоментувати, якщо не потрібне інтерактивне введення)
        // matrix2D.SetFromKeyboard();
        // Console.WriteLine("\nМатриця після введення з клавіатури:");
        // matrix2D.Display();
        // Console.WriteLine($"Мінімальний елемент: {matrix2D.FindMin():F2}");
        

        // Демонстрація тривимірної матриці (3D)
        Matrix3D matrix3D = new Matrix3D();
        Console.WriteLine("\n=======================================================");
        Console.WriteLine($"2. Робота з {matrix3D.GetDimensions()}");
        Console.WriteLine("=======================================================");

        // Заповнення випадковими числами
        matrix3D.SetRandom();
        Console.WriteLine("\nМатриця після випадкового заповнення:");
        matrix3D.Display();
        Console.WriteLine($"\nМінімальний елемент: {matrix3D.FindMin():F2}");
        
        Console.ReadKey();
    }
}
