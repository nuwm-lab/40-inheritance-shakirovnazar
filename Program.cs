using System;
using System.Globalization; // Для використання CultureInfo

// Додано простір імен для кращої організації коду
namespace MatrixApp
{
    // Клас для єдиного, глобального екземпляра Random
    public static class GlobalRandom
    {
        // Перейменовано Random на Instance згідно з конвенціями
        public static readonly Random Instance = new Random();
    }

    // ====================================================================
    // 1. АБСТРАКТНИЙ БАЗОВИЙ КЛАС
    // ====================================================================
    public abstract class MatrixBase
    {
        // На відміну від FindMin, цей метод має бути реалізований у базовому класі,
        // оскільки логіка безпечного вводу однакова для всіх матриць.
        protected double ReadDouble(string prompt)
        {
            Console.Write(prompt);
            double value;
            
            // Використання InvariantCulture для забезпечення парсингу числа з крапкою
            while (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine("Помилка вводу. Будь ласка, введіть дійсне число (використовуйте крапку як роздільник).");
                Console.Write(prompt);
            }
            return value;
        }

        public abstract void SetFromKeyboard();
        public abstract void SetRandom();
        public abstract double FindMin();
        public abstract string GetDimensions();
        public abstract void Display();
    }

    // ====================================================================
    // 2. ПОХІДНИЙ КЛАС: ДВОВИМІРНА МАТРИЦЯ (Matrix2D)
    // ====================================================================
    public class Matrix2D : MatrixBase
    {
        private readonly double[,] _data;
        // Перейменовано константу N на Size (PascalCase)
        private const int Size = 3; 

        public Matrix2D()
        {
            _data = new double[Size, Size];
        }

        public override void SetFromKeyboard()
        {
            Console.WriteLine($"Введення елементів матриці {Size}x{Size}:");
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _data[i, j] = ReadDouble($"[{i}, {j}]: ");
                }
            }
        }

        public override void SetRandom()
        {
            Console.WriteLine("Заповнення випадковими числами (від 0.00 до 100.00)...");
            Random random = GlobalRandom.Instance;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _data[i, j] = random.NextDouble() * 100;
                }
            }
        }

        public override double FindMin()
        {
            // Ініціалізація мінімуму максимально можливим значенням (Robustness)
            double min = double.MaxValue; 
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (_data[i, j] < min)
                    {
                        min = _data[i, j];
                    }
                }
            }
            return min;
        }

        public override string GetDimensions()
        {
            return $"Двовимірна матриця ({Size}x{Size})";
        }

        public override void Display()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write($"{_data[i, j],8:F2} ");
                }
                Console.WriteLine();
            }
        }
    }

    // ====================================================================
    // 3. ПОХІДНИЙ КЛАС: ТРИВИМІРНА МАТРИЦЯ (Matrix3D)
    // (Успадкування від Matrix2D семантично некоректне, тому успадковуємо від MatrixBase)
    // ====================================================================
    public class Matrix3D : MatrixBase
    {
        private readonly double[,,] _data;
        private const int Size = 3; 

        public Matrix3D()
        {
            _data = new double[Size, Size, Size];
        }

        public override void SetFromKeyboard()
        {
            Console.WriteLine($"Введення елементів матриці {Size}x{Size}x{Size}:");
            for (int k = 0; k < Size; k++)
            {
                Console.WriteLine($"--- Слой [*, *, {k}] ---");
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        _data[i, j, k] = ReadDouble($"[{i}, {j}, {k}]: ");
                    }
                }
            }
        }

        public override void SetRandom()
        {
            Console.WriteLine("Заповнення випадковими числами (від 0.00 до 100.00)...");
            Random random = GlobalRandom.Instance;
            for (int k = 0; k < Size; k++)
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        _data[i, j, k] = random.NextDouble() * 100;
                    }
                }
            }
        }

        public override double FindMin()
        {
            // Ініціалізація мінімуму максимально можливим значенням
            double min = double.MaxValue;
            for (int k = 0; k < Size; k++)
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
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

        public override string GetDimensions()
        {
            return $"Тривимірна матриця ({Size}x{Size}x{Size})";
        }

        public override void Display()
        {
            for (int k = 0; k < Size; k++)
            {
                Console.WriteLine($"\nСлой {k}:");
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        Console.Write($"{_data[i, j],8:F2} ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }

    // ====================================================================
    // 4. ОСНОВНА ПРОГРАМА (ТОЧКА ВХОДУ)
    // ====================================================================
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- Лабораторна робота №3: Наслідування та Поліморфізм (Матриці) ---");
            
            // Демонстрація двовимірної матриці (2D)
            MatrixBase matrix2D = new Matrix2D();
            Console.WriteLine("\n=======================================================");
            Console.WriteLine($"1. Робота з {matrix2D.GetDimensions()}");
            Console.WriteLine("=======================================================");

            // Заповнення випадковими числами
            matrix2D.SetRandom();
            Console.WriteLine("\nМатриця після випадкового заповнення:");
            matrix2D.Display();
            Console.WriteLine($"Мінімальний елемент: {matrix2D.FindMin():F2}");

            // Демонстрація тривимірної матриці (3D)
            MatrixBase matrix3D = new Matrix3D();
            Console.WriteLine("\n=======================================================");
            Console.WriteLine($"2. Робота з {matrix3D.GetDimensions()}");
            Console.WriteLine("=======================================================");

            // Заповнення випадковими числами
            matrix3D.SetRandom();
            Console.WriteLine("\nМатриця після випадкового заповнення:");
            matrix3D.Display();
            Console.WriteLine($"\nМінімальний елемент: {matrix3D.FindMin():F2}");
        }
    }
}
