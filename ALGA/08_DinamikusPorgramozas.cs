using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas
    {
        private HatizsakProblema problema;
        public int LepesSzam { get; set; }

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            this.LepesSzam = 0;
        }

        public int[,] TablatFeltolt()
        {
            int n = problema.n;
            int Wmax = problema.Wmax;
            int[] w = problema.w;
            float[] p = problema.p;

            int[,] F = new int[n + 1, Wmax + 1];

            for (int t = 0; t <= n; t++)
            {
                for (int h = 0; h <= Wmax; h++)
                {
                    if (t == 0 || h == 0)
                    {
                        F[t, h] = 0;
                    }
                    else if (h < w[t - 1])
                    {
                        F[t, h] = F[t - 1, h];
                    }
                    else
                    {
                        F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - w[t - 1]] + (int)p[t - 1]);
                    }
                    LepesSzam++;
                }
            }

            return F;
        }

        public int OptimalisErtek()
        {
            int[,] F = TablatFeltolt();
            return F[problema.n, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            int n = problema.n;
            int Wmax = problema.Wmax;
            int[] w = problema.w;

            int[,] F = TablatFeltolt();
            bool[] megoldas = new bool[n];
            int h = Wmax;

            for (int t = n; t > 0; t--)
            {
                if (F[t, h] != F[t - 1, h])
                {
                    megoldas[t - 1] = true;
                    h -= w[t - 1];
                }
                else
                {
                    megoldas[t - 1] = false;
                }
            }

            return megoldas;
        }
    }
}