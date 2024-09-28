using OE.ALGA.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{

    public class TombVerem<T> : Verem<T>
    {
        public T[] tömb;
        public int n;

        public TombVerem(int méret)
        {
            tömb = new T[méret];
            n = 0;
        }
        public bool Ures
        {
            get
            {
                return n == 0;
            }
        }

        public T Felso()
        {
            if (Ures) // Ellenőrizd, hogy a verem üres-e
            {
                throw new NincsElemKivetel(); // Ha üres, dobj kivételt
            }
            return tömb[n - 1];
        }

        public void Verembe(T ertek)
        {
            if (n < tömb.Length)
            {
                tömb[n] = ertek;
                n++;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Verembol()
        {
            if(Ures)
            {
                throw new NincsElemKivetel();
            }
            else
            {
                n--;
                return tömb[n];
            }
        }
    }

    public class TombSor<T> : Sor<T>
    {
        public T[] tömb;
        public int n;
        private int front;

        public TombSor(int méret)
        {
            tömb = new T[méret];
            n = 0;
            front = 0; 
        }

        public bool Ures
        {
            get { return n == 0; }
        }

        public T Elso()
        {
            if (!Ures)
            {
                return tömb[front];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public void Sorba(T ertek)
        {
            if (n < tömb.Length)
            {
                int rear = (front + n) % tömb.Length; 
                tömb[rear] = ertek;
                n++;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            else
            {
                T elem = tömb[front];
                front = (front + 1) % tömb.Length; 
                n--;
                return elem;
            }
        }
    }

    public class TombLista<T> : Lista<T>, IBejarhato<T>, IEnumerable<T>
    {
        private List<T> lista;
        private int n;

        public TombLista()
        {
            lista = new List<T>();
            n = 0;
        }

        public int Elemszam
        {
            get
            {
                return n;
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(lista[i]);
            }
        }

        public IBejaro<T> BejarotLetrehoz()
        {
            return new TombListaBejaro<T>(lista.ToArray(), n);
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > n)
            {
                throw new HibasIndexKivetel();
            }
            lista.Insert(index, ertek);
            n++;
        }

        public void Hozzafuz(T ertek)
        {
            lista.Add(ertek);
            n++;
        }

        public T Kiolvas(int index)
        {
            if (index < 0 || index >= n)
            {
                throw new HibasIndexKivetel();
            }
            return lista[index];
        }

        public void Modosit(int index, T ertek)
        {
            if (index < 0 || index >= n)
            {
                throw new HibasIndexKivetel();
            }
            lista[index] = ertek;
        }

        public void Torol(T ertek)
        {
            int index;
            while ((index = lista.IndexOf(ertek)) != -1)
            {
                lista.RemoveAt(index);
                n--;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < n; i++)
            {
                yield return lista[i]; // Használj lista[i]-t
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

public interface IBejarhato<T>
    {
        IBejaro<T> BejarotLetrehoz();
    }

    public interface IBejaro<T>
    {
        T Aktuális { get; }
        void Alaphelyzet();
        bool Kovetkezo();
    }

    public class TombListaBejaro<T> : IBejaro<T>
    {
        private T[] tömb;
        private int n;
        private int aktuálisIndex;

        public TombListaBejaro(T[] tömb, int n)
        {
            this.tömb = tömb;
            this.n = n;
            Alaphelyzet();
        }

        public T Aktuális
        {
            get
            {
                if (aktuálisIndex < 0 || aktuálisIndex >= n)
                {
                    throw new HibasIndexKivetel();
                }
                return tömb[aktuálisIndex];
            }
        }

        public void Alaphelyzet()
        {
            aktuálisIndex = -1;
        }

        public bool Kovetkezo()
        {
            if (aktuálisIndex < n - 1)
            {
                aktuálisIndex++;
                return true;
            }
            return false;
        }
    }
    internal class _03_Tomb 
    {
    }
}
