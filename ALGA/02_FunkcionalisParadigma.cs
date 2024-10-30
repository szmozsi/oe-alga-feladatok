using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public Func<T, bool> BejaroFeltetel { get; set; }

        public FeltetelesFeladatTarolo(int méret) : base(méret) { }

        public void FeltetelesVegrehajtas(Func<T, bool> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tároló[i]))
                {
                    tároló[i].Vegrehajtas();
                }
            }
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return new FeltetelesFeladatTaroloBejaro<T>(this, BejaroFeltetel ?? (x => true));
        }
    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
    {
        private FeltetelesFeladatTarolo<T> tároló;
        private int aktuálisIndex = -1;
        private Func<T, bool> bejaroFeltetel;

        public FeltetelesFeladatTaroloBejaro(FeltetelesFeladatTarolo<T> tároló, Func<T, bool> feltetel)
        {
            this.tároló = tároló;
            this.bejaroFeltetel = feltetel;
        }

        public T Current
        {
            get
            {
                if (aktuálisIndex < 0 || aktuálisIndex >= tároló.n)
                    throw new InvalidOperationException();
                return tároló.tároló[aktuálisIndex];
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (++aktuálisIndex < tároló.n)
            {
                if (bejaroFeltetel == null || bejaroFeltetel(tároló.tároló[aktuálisIndex]))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            aktuálisIndex = -1;
        }

        public void Dispose() { }
    }
}
