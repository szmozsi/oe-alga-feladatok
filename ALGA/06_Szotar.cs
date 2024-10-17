using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class SzotarElem<K, T>
    {
        public K kulcs { get; set; }
        public T tart { get; set; }

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        private SzotarElem<K, T>[] E;
        private Func<K, int> h;
        private List<SzotarElem<K, T>> U;

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            U = new List<SzotarElem<K, T>>();
            h = (kulcs) => hasitoFuggveny(kulcs) % E.Length;
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, kulcs => Math.Abs(kulcs.GetHashCode())) { }

        private SzotarElem<K, T> KulcsKeres(K kulcs)
        {
            int index = h(kulcs);
            if (E[index] != null && EqualityComparer<K>.Default.Equals(E[index].kulcs, kulcs))
            {
                return E[index];
            }

            foreach (var elem in U)
            {
                if (EqualityComparer<K>.Default.Equals(elem.kulcs, kulcs))
                {
                    return elem;
                }
            }

            return null;
        }

        public void Beir(K kulcs, T tartalom)
        {
            var letezoElem = KulcsKeres(kulcs);
            if (letezoElem != null)
            {
                letezoElem.tart = tartalom;
            }
            else
            {
                var ujElem = new SzotarElem<K, T>(kulcs, tartalom);
                int index = h(kulcs);
                if (E[index] == null)
                {
                    E[index] = ujElem;
                }
                else
                {
                    U.Add(ujElem);
                }
            }
        }

        public T Kiolvas(K kulcs)
        {
            var elem = KulcsKeres(kulcs);
            if (elem != null)
            {
                return elem.tart;
            }

            throw new HibasKulcsKivetel();
        }

        public void Torol(K kulcs)
        {
            int index = h(kulcs);
            if (E[index] != null && EqualityComparer<K>.Default.Equals(E[index].kulcs, kulcs))
            {
                E[index] = null;
            }
            else
            {
                var elem = KulcsKeres(kulcs);
                if (elem != null)
                {
                    U.Remove(elem);
                }
                else
                {
                    throw new HibasKulcsKivetel();
                }
            }
        }
    }
}
