using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public HatizsakProblema(int n, int Wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = Wmax;
            this.w = w;
            this.p = p;
        }

        public int OsszSuly(bool[] pakolas)
        {
            int osszSuly = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    osszSuly += w[i];
                }
            }
            return osszSuly;
        }

        public float OsszErtek(bool[] pakolas)
        {
            float osszErtek = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    osszErtek += p[i];
                }
            }
            return osszErtek;
        }

        public bool Ervenyes(bool[] pakolas)
        {
            return OsszSuly(pakolas) <= Wmax;
        }
    }

    public class NyersEro<T>
    {
        public int m { get; }
        public Func<int, T> generátor { get; }
        public Func<T, float> jóság { get; }

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generátor, Func<T, float> jóság)
        {
            this.m = m;
            this.generátor = generátor;
            this.jóság = jóság;
        }

        public T OptimalisMegoldas()
        {
            T optimalisMegoldas = generátor(1);
            float maxJóság = jóság(optimalisMegoldas);
            LepesSzam = 1;

            for (int i = 2; i <= m; i++)
            {
                T currentSolution = generátor(i);
                float currentJóság = jóság(currentSolution);
                LepesSzam++;

                if (currentJóság > maxJóság)
                {
                    maxJóság = currentJóság;
                    optimalisMegoldas = currentSolution;
                }
            }

            return optimalisMegoldas;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        public int LepesSzam { get; private set; }
        private HatizsakProblema probléma;

        public NyersEroHatizsakPakolas(HatizsakProblema probléma)
        {
            this.probléma = probléma;
        }

        public bool[] OptimalisMegoldas()
        {
            NyersEro<bool[]> opt = new NyersEro<bool[]>(
                (int)Math.Pow(2, probléma.n),
                i => Generátor(i),
                pakolas => Jóság(pakolas)
            );

            LepesSzam = opt.LepesSzam;
            return opt.OptimalisMegoldas();
        }

        public float OptimalisErtek()
        {
            bool[] optimalisPakolas = OptimalisMegoldas();
            return probléma.OsszErtek(optimalisPakolas);
        }

        private bool[] Generátor(int i)
        {
            bool[] pakolás = new bool[probléma.n];
            for (int j = 0; j < probléma.n; j++)
            {
                pakolás[j] = (i & (1 << j)) != 0;
            }
            return pakolás;
        }

        private float Jóság(bool[] pakolás)
        {
            return probléma.Ervenyes(pakolás) ? probléma.OsszErtek(pakolás) : -1;
        }
    }
}
