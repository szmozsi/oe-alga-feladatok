using System;
using System.Collections.Generic;
using OE.ALGA.Adatszerkezetek;

namespace OE.ALGA.Adatszerkezetek
{

    public class FaSzotar<K, T> : Szotar<K, T> where K : IComparable<K>
    {
        private class FaElem
        {
            public K Kulcs { get; set; }
            public T Ertek { get; set; }
            public FaElem Bal { get; set; }
            public FaElem Jobb { get; set; }

            public FaElem(K kulcs, T ertek)
            {
                Kulcs = kulcs;
                Ertek = ertek;
                Bal = null;
                Jobb = null;
            }
        }

        private FaElem gyoker;

        public FaSzotar()
        {
            gyoker = null;
        }

        public void Beir(K kulcs, T ertek)
        {
            gyoker = Beir(gyoker, kulcs, ertek);
        }

        private FaElem Beir(FaElem node, K kulcs, T ertek)
        {
            if (node == null)
                return new FaElem(kulcs, ertek);

            int cmp = kulcs.CompareTo(node.Kulcs);
            if (cmp < 0)
                node.Bal = Beir(node.Bal, kulcs, ertek);
            else if (cmp > 0)
                node.Jobb = Beir(node.Jobb, kulcs, ertek);
            else
                node.Ertek = ertek;

            return node;
        }

        public T Kiolvas(K kulcs)
        {
            FaElem node = Kiolvas(gyoker, kulcs);
            if (node == null)
                throw new HibasKulcsKivetel();
            return node.Ertek;
        }

        private FaElem Kiolvas(FaElem node, K kulcs)
        {
            if (node == null)
                return null;

            int cmp = kulcs.CompareTo(node.Kulcs);
            if (cmp < 0)
                return Kiolvas(node.Bal, kulcs);
            else if (cmp > 0)
                return Kiolvas(node.Jobb, kulcs);
            else
                return node;
        }

        public void Torol(K kulcs)
        {
            gyoker = Torol(gyoker, kulcs);
        }

        private FaElem Torol(FaElem node, K kulcs)
        {
            if (node == null)
                return null;

            int cmp = kulcs.CompareTo(node.Kulcs);
            if (cmp < 0)
                node.Bal = Torol(node.Bal, kulcs);
            else if (cmp > 0)
                node.Jobb = Torol(node.Jobb, kulcs);
            else
            {
                if (node.Bal == null)
                    return node.Jobb;
                if (node.Jobb == null)
                    return node.Bal;

                FaElem min = Min(node.Jobb);
                node.Kulcs = min.Kulcs;
                node.Ertek = min.Ertek;
                node.Jobb = Torol(node.Jobb, min.Kulcs);
            }

            return node;
        }

        private FaElem Min(FaElem node)
        {
            while (node.Bal != null)
                node = node.Bal;
            return node;
        }
    }
    public class SulyozottEgeszGrafEl : SulyozottGrafEl<int>, IComparable<SulyozottEgeszGrafEl>
    {
        public int Honnan { get; }
        public int Hova { get; }
        public float Suly { get; }

        public SulyozottEgeszGrafEl(int honnan, int hova, float suly)
        {
            Honnan = honnan;
            Hova = hova;
            Suly = suly;
        }

        public override bool Equals(object obj)
        {
            return obj is SulyozottEgeszGrafEl el &&
                   Honnan == el.Honnan &&
                   Hova == el.Hova &&
                   Suly == el.Suly;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Honnan, Hova, Suly);
        }

        public int CompareTo(SulyozottEgeszGrafEl other)
        {
            if (other == null) return 1;
            int result = Honnan.CompareTo(other.Honnan);
            if (result == 0)
            {
                result = Hova.CompareTo(other.Hova);
                if (result == 0)
                {
                    result = Suly.CompareTo(other.Suly);
                }
            }
            return result;
        }
    }

    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        private readonly int n;
        private readonly float[,] M;

        public CsucsmatrixSulyozottEgeszGraf(int csucsokSzama)
        {
            n = csucsokSzama;
            M = new float[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    M[i, j] = float.NaN;
        }

        public int CsucsokSzama => n;

        public int ElekSzama
        {
            get
            {
                int count = 0;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (!float.IsNaN(M[i, j]))
                            count++;
                return count;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                var halmaz = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                    halmaz.Beszur(i);
                return halmaz;
            }
        }

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                var halmaz = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (!float.IsNaN(M[i, j]))
                            halmaz.Beszur(new SulyozottEgeszGrafEl(i, j, M[i, j]));
                return halmaz;
            }
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            if (honnan < 0 || honnan >= n || hova < 0 || hova >= n)
                throw new ArgumentOutOfRangeException();

            M[honnan, hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            if (honnan < 0 || honnan >= n || hova < 0 || hova >= n)
                throw new ArgumentOutOfRangeException();

            return !float.IsNaN(M[honnan, hova]);
        }

        public float Suly(int honnan, int hova)
        {
            if (honnan < 0 || honnan >= n || hova < 0 || hova >= n)
                throw new ArgumentOutOfRangeException();

            if (float.IsNaN(M[honnan, hova]))
                throw new NincsElKivetel();

            return M[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            if (csucs < 0 || csucs >= n)
                throw new ArgumentOutOfRangeException();

            var halmaz = new FaHalmaz<int>();
            for (int j = 0; j < n; j++)
                if (!float.IsNaN(M[csucs, j]))
                    halmaz.Beszur(j);
            return halmaz;
        }
    }

    public static class Utkereses
    {
        public static Szotar<int, float> Dijkstra(SulyozottGraf<int, SulyozottEgeszGrafEl> g, int start)
        {
            var tavolsag = new FaSzotar<int, float>();
            var feldolgozott = new FaHalmaz<int>();
            var prioritasSor = new KupacPrioritasosSor<int>(g.CsucsokSzama, (x, y) => tavolsag.Kiolvas(x) < tavolsag.Kiolvas(y));

            // Alapértelmezett távolságok beállítása
            g.Csucsok.Bejar(csucs => tavolsag.Beir(csucs, float.PositiveInfinity));
            tavolsag.Beir(start, 0);
            prioritasSor.Sorba(start);

            // Fő algoritmus
            while (!prioritasSor.Ures)
            {
                var aktualis = prioritasSor.Sorbol();
                feldolgozott.Beszur(aktualis);

                g.Szomszedai(aktualis).Bejar(szomszed =>
                {
                    if (feldolgozott.Eleme(szomszed))
                        return;

                    float ujTavolsag = tavolsag.Kiolvas(aktualis) + g.Suly(aktualis, szomszed);
                    if (ujTavolsag < tavolsag.Kiolvas(szomszed))
                    {
                        tavolsag.Beir(szomszed, ujTavolsag);
                        prioritasSor.Sorba(szomszed);
                    }
                });
            }

            return tavolsag;
        }
    }

    public static class FeszitofaKereses
    {
        public static Szotar<int, int> Prim(SulyozottGraf<int, SulyozottEgeszGrafEl> g, int start)
        {
            var szulo = new FaSzotar<int, int>();
            var tavolsag = new FaSzotar<int, float>();
            var feldolgozott = new FaHalmaz<int>();
            var prioritasSor = new KupacPrioritasosSor<int>(g.CsucsokSzama, (x, y) => tavolsag.Kiolvas(x) < tavolsag.Kiolvas(y));

            // Alapértelmezett távolságok beállítása
            g.Csucsok.Bejar(csucs =>
            {
                tavolsag.Beir(csucs, float.PositiveInfinity);
                szulo.Beir(csucs, -1);
            });
            tavolsag.Beir(start, 0);
            prioritasSor.Sorba(start);

            // Fő algoritmus
            while (!prioritasSor.Ures)
            {
                var aktualis = prioritasSor.Sorbol();
                feldolgozott.Beszur(aktualis);

                g.Szomszedai(aktualis).Bejar(szomszed =>
                {
                    if (feldolgozott.Eleme(szomszed))
                        return;

                    float suly = g.Suly(aktualis, szomszed);
                    if (suly < tavolsag.Kiolvas(szomszed))
                    {
                        tavolsag.Beir(szomszed, suly);
                        szulo.Beir(szomszed, aktualis);
                        prioritasSor.Sorba(szomszed);
                    }
                });
            }

            return szulo;
        }

        public static Halmaz<SulyozottEgeszGrafEl> Kruskal(SulyozottGraf<int, SulyozottEgeszGrafEl> g)
        {
            var halmaz = new FaHalmaz<SulyozottEgeszGrafEl>();
            var szulo = new FaSzotar<int, int>();
            var rank = new FaSzotar<int, int>();

            // Kezdeti beállítások
            g.Csucsok.Bejar(csucs =>
            {
                szulo.Beir(csucs, csucs);
                rank.Beir(csucs, 0);
            });

            // Élek rendezése súly szerint
            var elek = new List<SulyozottEgeszGrafEl>();
            g.Elek.Bejar(el => elek.Add(el));
            elek.Sort((el1, el2) => el1.Suly.CompareTo(el2.Suly));

            // Kruskal algoritmus
            foreach (var el in elek)
            {
                int gyoker1 = Find(szulo, el.Honnan);
                int gyoker2 = Find(szulo, el.Hova);

                if (gyoker1 != gyoker2)
                {
                    halmaz.Beszur(el);
                    Union(szulo, rank, gyoker1, gyoker2);
                }
            }

            return halmaz;
        }

        private static int Find(Szotar<int, int> szulo, int csucs)
        {
            if (szulo.Kiolvas(csucs) != csucs)
            {
                szulo.Beir(csucs, Find(szulo, szulo.Kiolvas(csucs)));
            }
            return szulo.Kiolvas(csucs);
        }

        private static void Union(Szotar<int, int> szulo, Szotar<int, int> rank, int gyoker1, int gyoker2)
        {
            int rang1 = rank.Kiolvas(gyoker1);
            int rang2 = rank.Kiolvas(gyoker2);

            if (rang1 < rang2)
            {
                szulo.Beir(gyoker1, gyoker2);
            }
            else if (rang1 > rang2)
            {
                szulo.Beir(gyoker2, gyoker1);
            }
            else
            {
                szulo.Beir(gyoker2, gyoker1);
                rank.Beir(gyoker1, rang1 + 1);
            }
        }
    }
}