using OE.ALGA.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{

    public class FaElem<T> where T : IComparable<T>
    {
        public T tart { get; set; }
        public FaElem<T> bal { get; set; }
        public FaElem<T> jobb { get; set; }

        public FaElem(T tart, FaElem<T> bal = null, FaElem<T> jobb = null)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        private FaElem<T> gyoker;

        public FaHalmaz()
        {
            gyoker = null;
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfaBeszur(gyoker, ertek);
        }

        private FaElem<T> ReszfaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)
                return new FaElem<T>(ertek);

            if (ertek.CompareTo(p.tart) < 0)
                p.bal = ReszfaBeszur(p.bal, ertek);
            else if (ertek.CompareTo(p.tart) > 0)
                p.jobb = ReszfaBeszur(p.jobb, ertek);

            return p;
        }

        public bool Eleme(T ertek)
        {
            return ReszfaElem(gyoker, ertek);
        }

        private bool ReszfaElem(FaElem<T> p, T ertek)
        {
            if (p == null)
                return false;

            if (ertek.CompareTo(p.tart) == 0)
                return true;
            else if (ertek.CompareTo(p.tart) < 0)
                return ReszfaElem(p.bal, ertek);
            else
                return ReszfaElem(p.jobb, ertek);
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfaTorol(gyoker, ertek);
        }

        private FaElem<T> ReszfaTorol(FaElem<T> p, T ertek)
        {
            if (p == null)
                return null;

            if (ertek.CompareTo(p.tart) < 0)
                p.bal = ReszfaTorol(p.bal, ertek);
            else if (ertek.CompareTo(p.tart) > 0)
                p.jobb = ReszfaTorol(p.jobb, ertek);
            else
            {
                if (p.bal == null)
                    return p.jobb;
                if (p.jobb == null)
                    return p.bal;

                T minErtek = MinErtek(p.jobb);
                p.tart = minErtek;
                p.jobb = ReszfaTorol(p.jobb, minErtek);
            }

            return p;
        }

        private T MinErtek(FaElem<T> p)
        {
            T min = p.tart;
            while (p.bal != null)
            {
                min = p.bal.tart;
                p = p.bal;
            }
            return min;
        }

        public void Bejar(Action<T> muvelet)
        {
            BejarasPreOrder(gyoker, muvelet);
        }

        private void BejarasPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p == null) return;

            muvelet(p.tart);
            BejarasPreOrder(p.bal, muvelet);
            BejarasPreOrder(p.jobb, muvelet);
        }
    }

}