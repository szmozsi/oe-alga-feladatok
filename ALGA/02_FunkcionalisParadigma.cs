using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OE.ALGA.Paradigmaks
{

    public class TaroloMegteltKivetel : Exception
    {
        public TaroloMegteltKivetel() : base("A tároló megtelt!") { }
    }
    public interface IVegrehajthato
    {
        void Vegrehajtas();
    }

    

    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        public T[] tároló;
        public int n;

        public FeladatTarolo(int méret)
        {
            tároló = new T[méret];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            if (n < tároló.Length)
            {
                tároló[n] = elem;
                n++;
            }
            else
            {
                throw new TaroloMegteltKivetel();
            }
        }

        public virtual void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tároló[i].Vegrehajtas();
            }
        }
        public virtual IEnumerator<T> GetEnumerator()
        {
            return new FeladatTaroloBejaro<T>(this);
        }

         IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
    {
        private FeladatTarolo<T> tároló;
        private int aktuálisIndex = -1;

        public FeladatTaroloBejaro(FeladatTarolo<T> tároló)
        {
            this.tároló = tároló;
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
            aktuálisIndex++;
            return aktuálisIndex < tároló.n;
        }

        public void Reset()
        {
            aktuálisIndex = -1;
        }

        public void Dispose() { }
    }

    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public Func<T, bool> BejaroFeltetel { get; set; }

        public FeltetelesFeladatTarolo(int méret) : base(méret)
        {

        }
        public void FeltetelesVegrehajtas(Func<T, bool> feltétel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltétel(tároló[i]))
                {
                    tároló[i].Vegrehajtas();
                }
            }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new FeltetelesFeladatTaroloBejaro<T>(this, BejaroFeltetel);
        }
    }


    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
    {
        private FeltetelesFeladatTarolo<T> tároló;
        private int aktuálisIndex = -1;
        private Func<T, bool> bejaroFeltetel;

        public FeltetelesFeladatTaroloBejaro(FeltetelesFeladatTarolo<T> tároló, Func<T, bool> feltétel)
        {
            this.tároló = tároló;
            bejaroFeltetel = feltétel;
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
