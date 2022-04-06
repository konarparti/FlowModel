using System.Collections.Generic;

namespace FlowModelDesktop.Models
{
    public class Math
    {
        private readonly InputData _inputData;
        private readonly DbData _dbData;

        public Math(InputData inputData, DbData dbData)
        {
            _inputData = inputData;
            _dbData = dbData;
        }

        public static decimal DecimalPow(decimal x, decimal y)
        {
            return (decimal)System.Math.Pow(decimal.ToDouble(x), decimal.ToDouble(y));
        }

        public static decimal DecimalLn(decimal x)
        {
            return (decimal)System.Math.Log10(decimal.ToDouble(x));
        }

        public void Calculation()
        {
            decimal F = 0.125m * DecimalPow((_inputData.H / _inputData.W), 2) - 0.625m * (_inputData.H / _inputData.W) + 1;
            decimal Qch = (_inputData.H * _inputData.W * _inputData.Vu * F) / 2;
            decimal gamma = _inputData.Vu / _inputData.H;
            decimal q_gamma = _inputData.H * _inputData.W * _dbData.Mu * DecimalPow(gamma, (_dbData.n + 1));
            decimal q_alpha = _inputData.W * _dbData.alpha_u * (DecimalPow(_dbData.b, (-1)) - _inputData.Tu - _dbData.Tr);
            decimal N = _inputData.L / _inputData.DeltaZ;
            
            List<decimal> T_List = new List<decimal>();
            List<decimal> eta_List = new List<decimal>();
            decimal z, T;
            for (int i = 0; i <= N; i++)
            {
                z = i * _inputData.DeltaZ;
                if (i <= _inputData.L)
                {
                    T = _dbData.Tr + (1 / _dbData.b) * DecimalLn(
                        ((_dbData.b * q_gamma + _inputData.W * _dbData.alpha_u) / (_dbData.b * q_alpha))
                        * (1 - DecimalPow((decimal) System.Math.E, ((-_dbData.b * q_alpha * z) / (_dbData.ro * _dbData.c * Qch))))
                        + DecimalPow((decimal) System.Math.E, (_dbData.b * (_dbData.To - _dbData.Tr - ((q_alpha * z) / (_dbData.ro * _dbData.c * Qch))))));
                    T_List.Add(T);
                    eta_List.Add(_dbData.Mu * DecimalPow((decimal) System.Math.E, (-_dbData.b * (T - _dbData.Tr))) * DecimalPow(gamma, (_dbData.n - 1)));
                }
            }

            decimal Q = _dbData.ro * Qch;
            decimal Tp = T_List[T_List.Count];
            decimal Etap = eta_List[eta_List.Count];
        }
    }
}
