using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class LancElem<T>
    {
        public T Tart { get; set; }
        public LancElem<T> Köv { get; set; }

        public LancElem(T tart, LancElem<T> köv = null)
        {
            Tart = tart;
            Köv = köv;
        }
    }


    public class LancoltVerem<T> : Verem<T>
    {
        private LancElem<T> fej;

        public bool Ures => fej == null;

        public void Verembe(T érték)
        {
            fej = new LancElem<T>(érték, fej);
        }

        public T Verembol()
        {
            if (fej == null)
            {
                throw new NincsElemKivetel();
            }

            T érték = fej.Tart;
            fej = fej.Köv;

            return érték;
        }

        public T Felso()
        {
            if (Ures) throw new NincsElemKivetel();
            return fej.Tart;
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        private LancElem<T> fej;
        private LancElem<T> vég;

        public bool Ures => fej == null;

        public void Sorba(T érték)
        {
            LancElem<T> újElem = new LancElem<T>(érték);
            if (vég != null)
            {
                vég.Köv = újElem;
            }
            vég = újElem;
            if (fej == null)
            {
                fej = vég;
            }
        }

        public T Sorbol()
        {
            if (Ures) throw new NincsElemKivetel();
            T tart = fej.Tart;
            fej = fej.Köv;
            if (fej == null)
            {
                vég = null;
            }
            return tart;
        }

        public T Elso()
        {
            if (Ures) throw new NincsElemKivetel();
            return fej.Tart;
        }
    }

    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        public LancElem<T> fej;
        public int elemszám;

        public LancoltLista()
        {
            fej = null;
            elemszám = 0;
        }

        public int Elemszam => elemszám;

        public T Kiolvas(int index)
        {
            if (index < 0 || index >= elemszám) throw new HibasIndexKivetel();
            return ElemAt(index).Tart;
        }

        public void Modosit(int index, T érték)
        {
            if (index < 0 || index >= elemszám) throw new HibasIndexKivetel();
            ElemAt(index).Tart = érték;
        }

        public void Hozzafuz(T érték)
        {
            if (fej == null)
            {
                fej = new LancElem<T>(érték);
            }
            else
            {
                LancElem<T> current = fej;
                while (current.Köv != null)
                {
                    current = current.Köv;
                }
                current.Köv = new LancElem<T>(érték);
            }
            elemszám++;
        }

        public void Beszur(int index, T érték)
        {
            if (index < 0 || index > elemszám) throw new HibasIndexKivetel();

            if (index == 0)
            {
                fej = new LancElem<T>(érték, fej);
            }
            else
            {
                LancElem<T> előző = ElemAt(index - 1);
                előző.Köv = new LancElem<T>(érték, előző.Köv);
            }

            elemszám++;
        }

        public void Torol(T érték)
        {
            while (fej != null && fej.Tart.Equals(érték))
            {
                fej = fej.Köv; 
                elemszám--; 
            }


            LancElem<T> current = fej;
            while (current != null && current.Köv != null)
            {
                if (current.Köv.Tart.Equals(érték))
                {

                    current.Köv = current.Köv.Köv;
                    elemszám--;
                }
                else
                {
                    current = current.Köv;
                }
            }
        }

        public void Bejar(Action<T> művelet)
        {
            LancElem<T> current = fej;
            while (current != null)
            {
                művelet(current.Tart); 
                current = current.Köv;
            }
        }

        private LancElem<T> ElemAt(int index)
        {
            LancElem<T> current = fej;
            for (int i = 0; i < index; i++)
            {
                current = current.Köv;
            }
            return current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            LancElem<T> current = fej;
            while (current != null)
            {
                yield return current.Tart;
                current = current.Köv;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface IBejáró<T>
    {
        T Aktuális { get; }
        void Alaphelyzet();
        bool Kovetkezo();
    }

    public class LancoltListaBejaro<T> : IBejáró<T>
    {
        private LancElem<T> fej;
        private LancElem<T> aktuálisElem;

        public T Aktuális => aktuálisElem.Tart;

        public LancoltListaBejaro(LancElem<T> fej)
        {
            this.fej = fej;
            Alaphelyzet();
        }

        public void Alaphelyzet()
        {
            aktuálisElem = fej;
        }

        public bool Kovetkezo()
        {
            if (aktuálisElem == null) return false;
            aktuálisElem = aktuálisElem.Köv;
            return aktuálisElem != null;
        }
    }

    internal class _04_EgyszeruLanc
    {
    }
}
