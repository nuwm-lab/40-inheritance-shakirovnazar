using System;

namespace LabWork4
{
    // Базовий клас: Система рівнянь (наприклад, квадратичних)
    // Вигляд: a*x1^2 + b*x2 = c
    class SystemOfEquations
    {
        // Коефіцієнти для двох рівнянь
        // Рівняння 1: a1*... + b1*... = c1
        // Рівняння 2: a2*... + b2*... = c2
        protected double a1, b1, c1;
        protected double a2, b2, c2;

        // Метод задання коефіцієнтів
        public virtual void SetCoefficients()
        {
            Console.WriteLine("--- Введiть коефiцiєнти для Системи Рiвнянь (Base) ---");
            Console.WriteLine("Рiвняння вигляду: a*x1^2 + b*x2 = c");
            try
            {
                Console.Write("Рiвняння 1 (a1, b1, c1): ");
                a1 = Convert.ToDouble(Console.ReadLine());
                b1 = Convert.ToDouble(Console.ReadLine());
                c1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Рiвняння 2 (a2, b2, c2): ");
                a2 = Convert.ToDouble(Console.ReadLine());
                b2 = Convert.ToDouble(Console.ReadLine());
                c2 = Convert.ToDouble(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Помилка вводу. Використовуються нульовi значення.");
            }
        }

        // Виведення системи на екран
        public virtual void Show()
        {
            Console.WriteLine("\nСистема рiвнянь (Квадратична):");
            Console.WriteLine($"{a1}*x1^2 + {b1}*x2 = {c1}");
            Console.WriteLine($"{a2}*x1^2 + {b2}*x2 = {c2}");
        }

        // Перевірка, чи задовольняє вектор X(x1, x2) системі
        public virtual bool IsSatisfied(double x1, double x2)
        {
            // Перевіряємо з певною точністю (epsilon) для double
            double epsilon = 0.0001;
            
            double res1 = a1 * x1 * x1 + b1 * x2;
            double res2 = a2 * x1 * x1 + b2 * x2;

            bool eq1 = Math.Abs(res1 - c1) < epsilon;
            bool eq2 = Math.Abs(res2 - c2) < epsilon;

            return eq1 && eq2;
        }
    }

    // Похідний клас: СЛАР (Система Лінійних Алгебричних Рівнянь)
    // Вигляд: a*x1 + b*x2 = c
    class SLAE : SystemOfEquations
    {
        // Перевантаження методу задання коефіцієнтів
        public override void SetCoefficients()
        {
            Console.WriteLine("--- Введiть коефiцiєнти для СЛАР (Derived) ---");
            Console.WriteLine("Рiвняння вигляду: a*x1 + b*x2 = c (Лiнiйнi)");
            try
            {
                Console.Write("Рiвняння 1 (a1, b1, c1): ");
                a1 = Convert.ToDouble(Console.ReadLine());
                b1 = Convert.ToDouble(Console.ReadLine());
                c1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Рiвняння 2 (a2, b2, c2): ");
                a2 = Convert.ToDouble(Console.ReadLine());
                b2 = Convert.ToDouble(Console.ReadLine());
                c2 = Convert.ToDouble(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Помилка вводу.");
            }
        }

        // Перевантаження виведення
        public override void Show()
        {
            Console.WriteLine("\nСистема Лiнiйних Алгебричних Рiвнянь (СЛАР):");
            Console.WriteLine($"{a1}*x1 + {b1}*x2 = {c1}");
            Console.WriteLine($"{a2}*x1 + {b2}*x2 = {c2}");
        }

        // Перевантаження перевірки
        public override bool IsSatisfied(double x1, double x2)
        {
            double epsilon = 0.0001;

            // Лінійні рівняння: a*x1 + b*x2
            double res1 = a1 * x1 + b1 * x2;
            double res2 = a2 * x1 + b2 * x2;

            bool eq1 = Math.Abs(res1 - c1) < epsilon;
            bool eq2 = Math.Abs(res2 - c2) < epsilon;

            return eq1 && eq2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Робота з базовим класом (Загальна система)
            SystemOfEquations sys = new SystemOfEquations();
            sys.SetCoefficients();
            sys.Show();

            Console.WriteLine("\nВведiть вектор X(x1, x2) для перевiрки базової системи:");
            Console.Write("x1: ");
            double x1 = Convert.ToDouble(Console.ReadLine());
            Console.Write("x2: ");
            double x2 = Convert.ToDouble(Console.ReadLine());

            if (sys.IsSatisfied(x1, x2))
                Console.WriteLine("Вектор ЗАДОВОЛЬНЯЄ систему рiвнянь.");
            else
                Console.WriteLine("Вектор НЕ задовольняє систему рiвнянь.");

            Console.WriteLine(new string('-', 30));

            // 2. Робота з похідним класом (СЛАР)
            SLAE slae = new SLAE();
            slae.SetCoefficients(); // Викличеться перевантажений метод
            slae.Show();            // Викличеться перевантажений метод

            Console.WriteLine("\nВведiть вектор X(x1, x2) для перевiрки СЛАР:");
            Console.Write("x1: ");
            x1 = Convert.ToDouble(Console.ReadLine());
            Console.Write("x2: ");
            x2 = Convert.ToDouble(Console.ReadLine());

            if (slae.IsSatisfied(x1, x2)) // Викличеться перевантажений метод
                Console.WriteLine("Вектор ЗАДОВОЛЬНЯЄ СЛАР.");
            else
                Console.WriteLine("Вектор НЕ задовольняє СЛАР.");

            Console.ReadLine();
        }
    }
}
