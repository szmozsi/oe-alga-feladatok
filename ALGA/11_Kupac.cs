using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobbPrioritás;

        public Kupac(T[] E, int n, Func<T, T, bool> nagyobbPrioritás)
        {
            this.E = E;
            this.n = n;
            this.nagyobbPrioritás = nagyobbPrioritás;
            KupacotEpit();
        }

        public static int Bal(int i) => 2 * i + 1;
        public static int Jobb(int i) => 2 * i + 2;
        public static int Szulo(int i) => (i - 1) / 2; 

        protected void Kupacol(int i)
        {
            int bal = Bal(i);
            int jobb = Jobb(i);
            int legjobb = i;

            if (bal < n && nagyobbPrioritás(E[bal], E[legjobb]))
                legjobb = bal;

            if (jobb < n && nagyobbPrioritás(E[jobb], E[legjobb]))
                legjobb = jobb;

            if (legjobb != i)
            {
                (E[i], E[legjobb]) = (E[legjobb], E[i]);
                Kupacol(legjobb);
            }
        }

        protected void KupacotEpit()
        {
            for (int i = n / 2 - 1; i >= 0; i--)
                Kupacol(i);
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
    {
        public KupacRendezes(T[] A) : base(A, A.Length, (x, y) => x.CompareTo(y) > 0) { }

        public void Rendezes()
        {
            for (int i = n; i > 1; i--)
            {
                (E[0], E[i - 1]) = (E[i - 1], E[0]);
                n--;
                Kupacol(1);
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int méret, Func<T, T, bool> nagyobbPrioritás)
            : base(new T[méret], 0, nagyobbPrioritás) { }

        private void KulcsotFelvisz(int i)
        {
            while (i > 1 && nagyobbPrioritás(E[i - 1], E[Szulo(i) - 1]))
            {
                (E[i - 1], E[Szulo(i) - 1]) = (E[Szulo(i) - 1], E[i - 1]);
                i = Szulo(i);
            }
        }

        public bool Ures => n == 0;

        public void Sorba(T ertek)
        {
            if (n >= E.Length)
                throw new NincsHelyKivetel();

            E[n] = ertek;
            n++;
            KulcsotFelvisz(n);
        }

        public T Sorbol()
        {
            if (Ures)
                throw new NincsElemKivetel();

            T legjobb = E[0];
            E[0] = E[n - 1];
            n--;
            Kupacol(1);

            return legjobb;
        }

        public T Elso()
        {
            if (Ures)
                throw new NincsElemKivetel();

            return E[0];
        }

        public void Frissit(T elem)
        {
            int index = Array.IndexOf(E, elem, 0, n);
            if (index == -1)
                throw new InvalidOperationException("NincsElemKivetel");

            KulcsotFelvisz(index + 1);
            Kupacol(index + 1);
        }
    }
}
