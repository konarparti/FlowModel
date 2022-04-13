using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlowModelDesktop.Models
{
    public class MathModel
    {
        //private readonly InputData _inputData;
        //private readonly DbData _dbData;

        public MathModel()
        {
            //_inputData = inputData;
            //_dbData = dbData;
        }

        public static decimal DecimalPow(decimal x, decimal y)
        {
            return (decimal)System.Math.Pow(decimal.ToDouble(x), decimal.ToDouble(y));
        }

        public static decimal DecimalLn(decimal x)
        {
            return (decimal)System.Math.Log(decimal.ToDouble(x));
        }

        public void Calculation(InputData inputData, DbData dbData, out decimal Q, out List<decimal> Tp_List, out List<decimal> Eta_List, out TimeSpan time, out long memory)
        {
            var stopwatch = new Stopwatch();
            var process = Process.GetCurrentProcess();
            stopwatch.Start();
            decimal F = 0.125m * DecimalPow((inputData.H / inputData.W), 2) - 0.625m * (inputData.H / inputData.W) + 1;
            decimal Qch = (inputData.H * inputData.W * inputData.Vu * F) / 2;
            decimal gamma = inputData.Vu / inputData.H;
            decimal q_gamma = inputData.H * inputData.W * dbData.Mu * DecimalPow(gamma, (dbData.n + 1));
            decimal q_alpha = inputData.W * dbData.alpha_u * (DecimalPow(dbData.b, (-1)) - inputData.Tu - dbData.Tr);
            decimal N = inputData.L / inputData.DeltaZ;
            
            Tp_List = new List<decimal>();
            Eta_List = new List<decimal>();
            decimal z, T;
            for (int i = 0; i <= N; i++)
            {
                z = i * inputData.DeltaZ;
                
                {
                    T = dbData.Tr + (1 / dbData.b) * DecimalLn(
                        ((dbData.b * q_gamma + inputData.W * dbData.alpha_u) / (dbData.b * q_alpha))
                        * (1 - DecimalPow((decimal) System.Math.E, ((-dbData.b * q_alpha * z) / (dbData.ro * dbData.c * Qch))))
                        + DecimalPow((decimal) System.Math.E, (dbData.b * (dbData.To - dbData.Tr - ((q_alpha * z) / (dbData.ro * dbData.c * Qch))))));
                    Tp_List.Add(System.Math.Round(T, 2));
                    Eta_List.Add(System.Math.Round(dbData.Mu * DecimalPow((decimal) System.Math.E, (-dbData.b * (T - dbData.Tr))) * DecimalPow(gamma, (dbData.n - 1)), 2));
                }
            }

            Q = dbData.ro * Qch;
            decimal Tp = Tp_List[Tp_List.Count-1];
            decimal Etap = Eta_List[Eta_List.Count-1];
            stopwatch.Stop();
            time = stopwatch.Elapsed;
            memory = process.PagedMemorySize64;

        }
    }
}
