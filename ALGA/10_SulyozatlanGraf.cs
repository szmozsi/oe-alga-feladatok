using System;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    // EgeszGrafEl class implementing GrafEl<int> interface
    public class EgeszGrafEl : GrafEl<int>
    {
        public int Honnan { get; }
        public int Hova { get; }

        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        public override bool Equals(object obj)
        {
            if (obj is EgeszGrafEl other)
            {
                return Honnan == other.Honnan && Hova == other.Hova;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Honnan, Hova);
        }
    }

    // CsucsmatrixSulyozatlanEgeszGraf class implementing SulyozatlanGraf<int, EgeszGrafEl>
    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        private bool[,] adjacencyMatrix;
        private int numVertices;
        private HalmazImpl<int> csucsok;
        private HalmazImpl<EgeszGrafEl> elek;

        public CsucsmatrixSulyozatlanEgeszGraf(int vertexCount)
        {
            numVertices = vertexCount;
            adjacencyMatrix = new bool[vertexCount, vertexCount];
            csucsok = new HalmazImpl<int>();
            elek = new HalmazImpl<EgeszGrafEl>();

            for (int i = 0; i < vertexCount; i++)
            {
                csucsok.Beszur(i);
            }
        }

        public int CsucsokSzama => numVertices;
        public int ElekSzama => elek.Elemszam;
        public Halmaz<int> Csucsok => csucsok;
        public Halmaz<EgeszGrafEl> Elek => elek;

        public void UjEl(int honnan, int hova)
        {
            if (!csucsok.Eleme(honnan) || !csucsok.Eleme(hova))
                throw new ArgumentException("Invalid vertices.");

            adjacencyMatrix[honnan, hova] = true;
            elek.Beszur(new EgeszGrafEl(honnan, hova));
        }

        public bool VezetEl(int honnan, int hova)
        {
            return adjacencyMatrix[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            if (!csucsok.Eleme(csucs))
                throw new ArgumentException("Invalid vertex.");

            var neighbors = new HalmazImpl<int>();
            for (int i = 0; i < numVertices; i++)
            {
                if (adjacencyMatrix[csucs, i])
                {
                    neighbors.Beszur(i);
                }
            }
            return neighbors;
        }
    }

    // Helper class implementing the Halmaz<T> interface
    public class HalmazImpl<T> : Halmaz<T>
    {
        private HashSet<T> elements;

        public HalmazImpl()
        {
            elements = new HashSet<T>();
        }

        public void Beszur(T ertek)
        {
            elements.Add(ertek);
        }

        public bool Eleme(T ertek)
        {
            return elements.Contains(ertek);
        }

        public void Torol(T ertek)
        {
            elements.Remove(ertek);
        }

        public void Bejar(Action<T> muvelet)
        {
            foreach (var elem in elements)
            {
                muvelet(elem);
            }
        }

        public int Elemszam => elements.Count;
    }

    // Graph traversal algorithms
    public static class GrafBejarasok
    {
        public static Halmaz<int> SzelessegiBejaras(CsucsmatrixSulyozatlanEgeszGraf graf, int start, Action<int> action)
        {
            var visited = new HalmazImpl<int>();
            var queue = new Queue<int>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                if (visited.Eleme(current)) continue;

                visited.Beszur(current);
                action(current);

                graf.Szomszedai(current).Bejar(neighbor =>
                {
                    if (!visited.Eleme(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                });
            }

            return visited;
        }

        public static Halmaz<int> MelysegiBejaras(CsucsmatrixSulyozatlanEgeszGraf graf, int start, Action<int> action)
        {
            var visited = new HalmazImpl<int>();
            var stack = new Stack<int>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                int current = stack.Pop();
                if (visited.Eleme(current)) continue;

                visited.Beszur(current);
                action(current);

                graf.Szomszedai(current).Bejar(neighbor =>
                {
                    if (!visited.Eleme(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                });
            }

            return visited;
        }
    }
}
