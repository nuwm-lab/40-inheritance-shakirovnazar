using System;
using System.Globalization;
using System.Text;

namespace MatrixApp
{
    // ====================================================================
    // 1. СТРУКТУРА ДАНИХ (STRUCT)
    // Використовується для повернення значення мінімуму та його координат
    // ====================================================================
    public struct MatrixCoord
    {
        public double Value { get; }
        public int Row { get; }
        public int Col { get; }
        public int Depth { get; } // Використовується лише для 3D

        // Конструктор для 2D
        public MatrixCoord(double value, int row, int col)
        {
            Value = value;
            Row = row;
            Col = col;
            Depth = -1; // Значення за замовчуванням для 2D
        }

        // Конструктор для 3D
        public MatrixCoord(double value, int row, int col, int depth)
        {
            Value = value;
            Row = row;
            Col = col;
            Depth = depth;
        }

        public override string ToString()
        {
            if (Depth == -1)
            {
                return $"Значення: {Value:F2} (Координати: [{Row}, {Col}])";
            }
            return $"Значення: {Value:F2} (Координати: [{Row}, {Col}, {Depth}])";
        }
    }

    // Клас для єдиного, глобального екземпляра Random
    public static class GlobalRandom
    {
        public static readonly Random Instance = new Random();
    }

    // ====================================================================
    // 2. АБСТРАКТНИЙ БАЗОВИЙ КЛАС
    // Клас моделі. Не містить I/O логіки.
    // ====================================================================
    public abstract class MatrixBase
    {
        // Методи моделі (абстрактні)
        public abstract void SetRandom();
        public abstract double FindMin();
        public abstract MatrixCoord FindMinWithCoord();
        public abstract string GetDimensions();
        // Додаємо метод для отримання даних для виводу
        public abstract double GetValue(int i, int j, int k = 0);
        public abstract int GetDepth(); 
    }

    // ====================================================================
    // 3. ДВОВИМІРНА МАТРИЦЯ (Matrix2D)
    // ====================================================================
    public class Matrix2D : MatrixBase
    {
        private readonly double[,] _data;
        public int Rows { get; }
        public int Cols { get; }

        // Конструктор тепер приймає розміри
        public Matrix2D(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _data = new double[Rows, Cols];
        }

        // Метод для встановлення значення ззовні (використовується I/O логікою в Program)
        public void SetValue(int row, int col, double value)
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Cols)
            {
                _data[row, col] = value;
            }
        }
        
        // Модельна логіка: тільки заповнення масиву
        public override void SetRandom()
        {
            Random random = GlobalRandom.Instance;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    _data[i, j] = random.NextDouble() * 100;
                }
            }
        }

        public override double FindMin()
        {
            return FindMinWithCoord().Value;
        }

        // Новий метод для пошуку мінімуму з координатами
        public override MatrixCoord FindMinWithCoord()
        {
            double min = double.MaxValue;
            int minRow = -1, minCol = -1;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (_data[i, j] < min)
                    {
                        min = _data[i, j];
                        minRow = i;
                        minCol = j;
                    }
                }
            }
            return new MatrixCoord(min, minRow, minCol);
        }

        public override string GetDimensions() => $"Двовимірна матриця ({Rows}x{Cols})";
        public override double GetValue(int i, int j, int k = 0) => _data[i, j];
        public override int GetDepth() => 1; // Для 2D завжди 1 "глибина"
    }

    // ====================================================================
    // 4. ТРИВИМІРНА МАТРИЦЯ (Matrix3D)
    // ====================================================================
    public class Matrix3D : MatrixBase
    {
        private readonly double[,,] _data;
        public int Rows { get; }
        public int Cols { get; }
        public int Depth { get; }

        // Конструктор тепер приймає розміри
        public Matrix3D(int rows, int cols, int depth)
        {
            Rows = rows;
            Cols = cols;
            Depth = depth;
            _data = new double[Rows, Cols, Depth];
        }

        // Метод для встановлення значення ззовні
        public void SetValue(int row, int col, int depth, double value)
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Cols && depth >= 0 && depth < Depth)
            {
                _data[row, col, depth] = value;
            }
        }

        // Модельна логіка: тільки заповнення масиву
        public override void SetRandom()
        {
            Random random = GlobalRandom.Instance;
            for (int k = 0; k < Depth; k++)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        _data[i, j, k] = random.NextDouble() * 100;
                    }
                }
            }
        }

        public override double FindMin()
        {
            return FindMinWithCoord().Value;
        }

        // Новий метод для пошуку мінімуму з координатами
        public override MatrixCoord FindMinWithCoord()
        {
            double min = double.MaxValue;
            int minRow = -1, minCol = -1, minDepth = -1;

            for (int k = 0; k < Depth; k++)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        if (_data[i, j, k] < min)
                        {
                            min = _data[i, j, k];
                            minRow = i;
                            minCol = j;
                            minDepth = k;
                        }
                    }
                }
            }
            return new MatrixCoord(min, minRow, minCol, minDepth);
        }

        public override string GetDimensions() => $"Тривимірна матриця ({Rows}x{Cols}x{Depth})";
        public override double GetValue(int i, int j, int k = 0) => _data[i, j, k];
        public override int GetDepth() => Depth;
    }

    // ====================================================================
    // 5. ОСНОВНА ПРОГРАМА (ТОЧКА ВХОДУ) - МІСТИТЬ ВСЮ I/O ЛОГІКУ
    // ====================================================================
    class Program
    {
        // Допоміжний метод для безпечного введення числа (I/O)
        private static double ReadDouble(string prompt)
        {
            Console.Write(prompt);
            double value;
            // Використання InvariantCulture для парсингу з крапкою
            while (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine("Помилка вводу. Будь ласка, введіть дійсне число (використовуйте крапку як роздільник).");
                Console.Write(prompt);
            }
            return value;
        }

        // Метод для виводу 2D матриці (I/O)
        private static void DisplayMatrix(Matrix2D matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    Console.Write($"{matrix.GetValue(i, j),8:F2} ");
                }
                Console.WriteLine();
            }
        }

        // Метод для виводу 3D матриці (I/O)
        private static void DisplayMatrix(Matrix3D matrix)
        {
            for (int k = 0; k < matrix.Depth; k++)
            {
                Console.WriteLine($"\nСлой {k}:");
                for (int i = 0; i < matrix.Rows; i++)
                {
                    for (int j = 0; j < matrix.Cols; j++)
                    {
                        // Виправлена помилка індексування: _data[i, j, k]
                        Console.Write($"{matrix.GetValue(i, j, k),8:F2} "); 
                    }
                    Console.WriteLine();
                }
            }
        }
        
        // Метод для заповнення матриці з клавіатури (I/O)
        private static void SetFromKeyboard(Matrix2D matrix)
        {
            Console.WriteLine($"Введення елементів матриці {matrix.Rows}x{matrix.Cols}:");
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    double value = ReadDouble($"[{i}, {j}]: ");
                    matrix.SetValue(i, j, value);
                }
            }
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- Лабораторна робота №3: Наслідування та Поліморфізм (Матриці) ---");
            
            // ==================================================================
            // 1. Демонстрація двовимірної матриці (4x4)
            // ==================================================================
            Matrix2D matrix2D = new Matrix2D(4, 4); 
            Console.WriteLine("\n=======================================================");
            Console.WriteLine($"1. Робота з {matrix2D.GetDimensions()}");
            Console.WriteLine("=======================================================");

            // Заповнення та вивід (I/O)
            Console.WriteLine("Заповнення випадковими числами (від 0.00 до 100.00)...");
            matrix2D.SetRandom(); // Модельна логіка
            Console.WriteLine("\nМатриця:");
            DisplayMatrix(matrix2D); // I/O логіка

            // Пошук мінімуму
            MatrixCoord minCoord2D = matrix2D.FindMinWithCoord();
            Console.WriteLine($"\nМінімальний елемент: {minCoord2D.ToString()}");


            // ==================================================================
            // 2. Демонстрація тривимірної матриці (3x3x3)
            // ==================================================================
            Matrix3D matrix3D = new Matrix3D(3, 3, 3);
            Console.WriteLine("\n=======================================================");
            Console.WriteLine($"2. Робота з {matrix3D.GetDimensions()}");
            Console.WriteLine("=======================================================");

            // Заповнення та вивід (I/O)
            Console.WriteLine("Заповнення випадковими числами (від 0.00 до 100.00)...");
            matrix3D.SetRandom(); // Модельна логіка
            DisplayMatrix(matrix3D); // I/O логіка

            // Пошук мінімуму
            MatrixCoord minCoord3D = matrix3D.FindMinWithCoord();
            Console.WriteLine($"\nМінімальний елемент: {minCoord3D.ToString()}");
        }
    }
}
