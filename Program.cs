using System;
using System.Globalization;
using System.Text;

namespace MatrixApp
{
    /// <summary>
    /// Структура, що зберігає значення мінімального елемента та його координати.
    /// </summary>
    public struct MatrixCoord
    {
        public double Value { get; }
        public int Row { get; }
        public int Col { get; }
        public int Depth { get; }

        /// <summary>Конструктор для 2D-матриці.</summary>
        public MatrixCoord(double value, int row, int col)
        {
            Value = value;
            Row = row;
            Col = col;
            Depth = -1; 
        }

        /// <summary>Конструктор для 3D-матриці.</summary>
        public MatrixCoord(double value, int row, int col, int depth)
        {
            Value = value;
            Row = row;
            Col = col;
            Depth = depth;
        }

        /// <summary>Повертає рядок, що представляє мінімальний елемент та його координати.</summary>
        public override string ToString()
        {
            if (Depth == -1)
            {
                return $"Значення: {Value:F2} (Координати: [{Row}, {Col}])";
            }
            return $"Значення: {Value:F2} (Координати: [{Row}, {Col}, {Depth}])";
        }
    }

    /// <summary>
    /// Статичний клас для забезпечення єдиного, безпечного для потоків екземпляра Random.
    /// </summary>
    public static class GlobalRandom
    {
        public static readonly Random Instance = new Random();
    }

    // ====================================================================
    // 1. АБСТРАКТНИЙ БАЗОВИЙ КЛАС
    // ====================================================================
    /// <summary>
    /// Базовий клас для всіх типів матриць. Визначає спільний API та I/O логіку.
    /// </summary>
    public abstract class MatrixBase
    {
        // Допоміжний метод для безпечного введення числа (I/O логіка, успадкована похідними класами)
        protected double ReadDouble(string prompt)
        {
            Console.Write(prompt);
            double value;
            // Використання InvariantCulture для парсингу числа з крапкою
            while (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine("Помилка вводу. Будь ласка, введіть дійсне число (використовуйте крапку як роздільник).");
                Console.Write(prompt);
            }
            return value;
        }

        // Абстрактні методи моделі
        /// <summary>Заповнює матрицю значеннями, введеними з клавіатури.</summary>
        public abstract void SetFromKeyboard(); 
        /// <summary>Заповнює матрицю випадковими числами.</summary>
        public abstract void SetRandom();
        /// <summary>Знаходить мінімальний елемент матриці.</summary>
        public abstract double FindMin();
        /// <summary>Знаходить мінімальний елемент матриці та його координати.</summary>
        public abstract MatrixCoord FindMinWithCoord();
        /// <summary>Повертає розмірність матриці у вигляді рядка.</summary>
        public abstract string GetDimensions();
        
        // Абстрактні методи API для доступу до даних (типобезпечні перевантаження)
        /// <summary>Отримує значення елемента матриці за двома індексами (для 2D).</summary>
        public abstract double GetValue(int i, int j);
        /// <summary>Отримує значення елемента матриці за трьома індексами (для 3D).</summary>
        public abstract double GetValue(int i, int j, int k);
    }

    // ====================================================================
    // 2. ДВОВИМІРНА МАТРИЦЯ (Matrix2D)
    // ====================================================================
    /// <summary>
    /// Реалізує логіку двовимірної матриці.
    /// </summary>
    public class Matrix2D : MatrixBase
    {
        private readonly double[,] _data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix2D(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Розміри матриці мають бути додатними.");
            Rows = rows;
            Cols = cols;
            _data = new double[Rows, Cols];
        }

        // --- Деструктор (finalizer) ---
        // Якщо викладач вимагає, додайте цей блок і поясніть, що він не потрібен
        /*
        ~Matrix2D() 
        {
            // У C# GC автоматично керує пам'яттю. Деструктори (фіналізатори)
            // використовуються лише для звільнення некерованих ресурсів.
            // Тут маємо лише керований масив, тому деструктор не потрібен.
        }
        */

        /// <summary>Встановлює значення елемента за вказаними індексами.</summary>
        public void SetValue(int row, int col, double value)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Cols)
            {
                // Обробка винятків при некоректних індексах
                throw new ArgumentOutOfRangeException("Індекси виходять за межі матриці.");
            }
            _data[row, col] = value;
        }
        
        /// <summary>Заповнює матрицю значеннями, введеними з клавіатури (поліморфізм вводу).</summary>
        public override void SetFromKeyboard()
        {
            Console.WriteLine($"\nВведення елементів матриці {Rows}x{Cols}:");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    double value = ReadDouble($"[{i}, {j}]: ");
                    _data[i, j] = value; // Встановлення значення без повторної перевірки
                }
            }
        }

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

        public override double FindMin() => FindMinWithCoord().Value;

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
        
        // Перевантаження API: GetValue(i, j) для 2D
        public override double GetValue(int i, int j)
        {
             if (i < 0 || i >= Rows || j < 0 || j >= Cols)
                throw new ArgumentOutOfRangeException("Індекси виходять за межі матриці.");
            return _data[i, j];
        }

        // Перевантаження API: GetValue(i, j, k) для 2D (повертає виняток, оскільки 2D не має глибини)
        public override double GetValue(int i, int j, int k)
        {
            throw new NotSupportedException("2D-матриця не підтримує третій індекс (глибину).");
        }
    }

    // ====================================================================
    // 3. ТРИВИМІРНА МАТРИЦЯ (Matrix3D)
    // ====================================================================
    /// <summary>
    /// Реалізує логіку тривимірної матриці.
    /// </summary>
    public class Matrix3D : MatrixBase
    {
        private readonly double[,,] _data;
        public int Rows { get; }
        public int Cols { get; }
        public int Depth { get; }

        public Matrix3D(int rows, int cols, int depth)
        {
            if (rows <= 0 || cols <= 0 || depth <= 0)
                throw new ArgumentException("Розміри матриці мають бути додатними.");
            Rows = rows;
            Cols = cols;
            Depth = depth;
            _data = new double[Rows, Cols, Depth];
        }

        /// <summary>Встановлює значення елемента за вказаними індексами.</summary>
        public void SetValue(int row, int col, int depth, double value)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Cols || depth < 0 || depth >= Depth)
            {
                throw new ArgumentOutOfRangeException("Індекси виходять за межі матриці.");
            }
            _data[row, col, depth] = value;
        }
        
        /// <summary>Заповнює матрицю значеннями, введеними з клавіатури (поліморфізм вводу).</summary>
        public override void SetFromKeyboard()
        {
            Console.WriteLine($"\nВведення елементів матриці {Rows}x{Cols}x{Depth}:");
            for (int k = 0; k < Depth; k++)
            {
                Console.WriteLine($"--- Слой [*, *, {k}] ---");
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        double value = ReadDouble($"[{i}, {j}, {k}]: ");
                        _data[i, j, k] = value;
                    }
                }
            }
        }

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

        public override double FindMin() => FindMinWithCoord().Value;

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
        
        // Перевантаження API: GetValue(i, j) для 3D (повертає виняток)
        public override double GetValue(int i, int j)
        {
            throw new NotSupportedException("3D-матриця вимагає три індекси: GetValue(i, j, k).");
        }

        // Перевантаження API: GetValue(i, j, k) для 3D
        public override double GetValue(int i, int j, int k)
        {
            if (i < 0 || i >= Rows || j < 0 || j >= Cols || k < 0 || k >= Depth)
                throw new ArgumentOutOfRangeException("Індекси виходять за межі матриці.");
            return _data[i, j, k];
        }
    }

    // ====================================================================
    // 4. ОСНОВНА ПРОГРАМА (ТОЧКА ВХОДУ) - МІСТИТЬ ВСЮ I/O ЛОГІКУ ПРЕДСТАВЛЕННЯ
    // ====================================================================
    class Program
    {
        // Універсальний метод виводу матриці (працює завдяки абстрактному API)
        private static void DisplayMatrix(MatrixBase matrix)
        {
            Console.WriteLine($"\n--- {matrix.GetDimensions()} ---");
            
            int depth = matrix is Matrix3D matrix3D ? matrix3D.Depth : 1;
            int rows = matrix is Matrix2D matrix2D ? matrix2D.Rows : ((Matrix3D)matrix).Rows;
            int cols = matrix is Matrix2D matrix2D_2 ? matrix2D_2.Cols : ((Matrix3D)matrix).Cols;

            for (int k = 0; k < depth; k++)
            {
                if (depth > 1) Console.WriteLine($"\nСлой {k}:");
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        double value = (matrix is Matrix3D) 
                            ? matrix.GetValue(i, j, k)
                            : matrix.GetValue(i, j);
                        
                        Console.Write($"{value,8:F2} ");
                    }
                    Console.WriteLine();
                }
            }
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- Лабораторна робота №3: Наслідування та Поліморфізм (Матриці) ---");
            Console.WriteLine("--------------------------------------------------------------------");

            // ==================================================================
            // 1. Демонстрація двовимірної матриці (3x3)
            // ==================================================================
            Matrix2D matrix2D = new Matrix2D(3, 3); 
            Console.WriteLine("\n[1] Демонстрація 2D-матриці (3x3)");
            
            // Демонстрація поліморфізму (виклик SetFromKeyboard)
            matrix2D.SetFromKeyboard(); 
            
            // Вивід та пошук мінімуму
            DisplayMatrix(matrix2D);
            MatrixCoord minCoord2D = matrix2D.FindMinWithCoord();
            Console.WriteLine($"Мінімальний елемент: {minCoord2D}");


            // ==================================================================
            // 2. Демонстрація тривимірної матриці (2x2x2)
            // ==================================================================
            Matrix3D matrix3D = new Matrix3D(2, 2, 2);
            Console.WriteLine("\n[2] Демонстрація 3D-матриці (2x2x2)");

            // Заповнення випадковими числами для демонстрації
            Console.WriteLine("Заповнення випадковими числами (0.00 - 100.00)...");
            matrix3D.SetRandom(); 

            // Вивід та пошук мінімуму
            DisplayMatrix(matrix3D);
            MatrixCoord minCoord3D = matrix3D.FindMinWithCoord();
            Console.WriteLine($"\nМінімальний елемент: {minCoord3D}");

            // Приклад обробки винятків
            try
            {
                // Спроба отримати значення за некоректними індексами
                Console.WriteLine("\nПриклад обробки винятків:");
                matrix2D.GetValue(5, 5); 
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
