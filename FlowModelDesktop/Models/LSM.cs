using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowModelDesktop.Models
{
    public class LSM
    {
        // Массивы значений Х и У задаются как свойства
        public double[] X { get; set; }
        public double[] Y { get; set; }

        // Искомые коэффициенты полинома в данном случае, а в общем коэфф. при функциях
        private double[] coeff;
        public double[] Coeff { get { return coeff; } }

        // Среднеквадратичное отклонение
        public double? Delta { get { return getDelta(); } }

        // Конструктор класса. Принимает 2 листа значений х и у
        public LSM(List<double> x, List<double> y)
        {
            X = new double[x.Count];
            Y = new double[y.Count];

            for (int i = 0; i < x.Count; i++)
            {
                X[i] = x[i];
                Y[i] = y[i];
            }
        }

        // Метод Наименьших Квадратов
        // В качестве базисных функций используются степенные функции y = a0 * x^0 + a1 * x^1 + ... + am * x^m
        public void Polynomial(int m)
        {
            // массив для хранения значений базисных функций
            double[,] basic = new double[X.Length, m + 1];

            // заполнение массива для базисных функций
            for (int i = 0; i < basic.GetLength(0); i++)
                for (int j = 0; j < basic.GetLength(1); j++)
                    basic[i, j] = Math.Pow(X[i], j);
            // Создание матрицы из массива значений базисных функций(МЗБФ)
            Matrix basicFuncMatr = new Matrix(basic);
            // Транспонирование МЗБФ
            Matrix transBasicFuncMatr = basicFuncMatr.Transposition();
            // Произведение транспонированного  МЗБФ на МЗБФ
            Matrix lambda = transBasicFuncMatr * basicFuncMatr;
            // Произведение транспонированого МЗБФ на следящую матрицу 
            Matrix beta = transBasicFuncMatr * new Matrix(Y);
            // Решение СЛАУ путем умножения обратной матрицы лямбда на бету
            Matrix a = lambda.InverseMatrix() * beta;
            coeff = new double[a.Row];
            for (int i = 0; i < coeff.Length; i++)
            {
                coeff[i] = a.Args[i, 0];
            }
        }

        // Функция нахождения среднеквадратичного отклонения
        private double? getDelta()
        {
            if (coeff == null) return null;
            double[] dif = new double[Y.Length];
            double[] f = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < coeff.Length; j++)
                {
                    f[i] += coeff[j] * Math.Pow(X[i], j);
                }
                dif[i] = Math.Pow((f[i] - Y[i]), 2);
            }
            return Math.Sqrt(dif.Sum() / X.Length);
        }
    }
}
