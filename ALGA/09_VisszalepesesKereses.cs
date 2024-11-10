using System;
using System.Collections.Generic;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        public int n;
        public int[] M;
        public T[,] R;
        public Func<int, T, bool> ft;
        public Func<int, T, T[], bool> fk;
        public Func<T[], double> josag;
        public int LepesSzam { get; set; }
        public Func<int, T[], double> fb;

        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], double> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
            LepesSzam = 0;
        }

        public virtual T[] OptimalisMegoldas()
        {
            double minValue = double.MinValue;
            T[] E = new T[n];
            T[] O = new T[n];
            Backtrack(0, E, ref O, ref minValue);
            return O;
        }

        protected virtual void Backtrack(int szint, T[] E, ref T[] O, ref double maxJ)
        {
            if (szint == n)
            {
                double j = josag(E);
                if (j > maxJ)
                {
                    maxJ = j;
                    Array.Copy(E, O, n);
                }
                return;
            }

            for (int i = 0; i < M[szint]; i++)
            {
                T r = R[szint, i];
                if (ft(szint, r) && fk(szint, r, E))
                {
                    E[szint] = r;
                    LepesSzam++;
                    Backtrack(szint + 1, E, ref O, ref maxJ);
                }
            }
        }
    }

    public class VisszalepesesHatizsakPakolas : VisszalepesesOptimalizacio<bool>
    {
        public HatizsakProblema problema;

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema) : base(problema.n, new int[problema.n], new bool[problema.n, 2], null, null, null)
        {
            this.problema = problema;
            for (int i = 0; i < problema.n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            ft = (szint, r) => true;
            fk = (szint, r, E) => {
                int suly = 0;
                for (int i = 0; i < szint; i++)
                    if (E[i]) suly += problema.w[i];
                return suly + (r ? problema.w[szint] : 0) <= problema.Wmax;
            };
            josag = (E) => {
                int ertek = 0;
                for (int i = 0; i < problema.n; i++)
                    if (E[i]) ertek += (int)problema.p[i];
                return ertek;
            };
        }

        public int OptimalisErtek() => (int)josag(OptimalisMegoldas());
    }

    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        public Func<int, T[], double> fb;

        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], double> josag, Func<int, T[], double> fb)
            : base(n, M, R, ft, fk, josag)
        {
            this.fb = fb;
        }

        public void Backtrack(int szint, T[] E, ref T[] O, ref double maxJ)
        {
            if (E == null || E.Length == 0)
            {
                throw new ArgumentNullException("E array is null or empty");
            }

            if (O == null)
            {
                O = new T[E.Length];
            }

            if (maxJ == double.MinValue)
            {

            }
        }
    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema) { }

        public override bool[] OptimalisMegoldas()
        {
            var opt = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(problema.n, M, R, ft, fk, josag, fb);
            var megoldas = opt.OptimalisMegoldas();
            LepesSzam = opt.LepesSzam;
            return megoldas;
        }
    }
}
