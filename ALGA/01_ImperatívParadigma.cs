using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
{
    void Vegrehajtas();
}

public interface IFuggo
{
    bool FuggosegTeljesul { get; }
}

public class TaroloMegteltKivetel : Exception
{
    public TaroloMegteltKivetel(string message) : base(message)
    {
    }
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
            throw new TaroloMegteltKivetel("A tároló megtelt, nem lehet több elemet hozzáadni!");
        }
    }

    public virtual void MindentVegrehajt()
    {
        for (int i = 0; i < n; i++)
        {
            tároló[i].Vegrehajtas();
        }
    }
    public IEnumerator<T> GetEnumerator()
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
    private int pozíció = -1;

    public FeladatTaroloBejaro(FeladatTarolo<T> tároló)
    {
        this.tároló = tároló;
    }

    public T Current
    {
        get
        {
            if (pozíció < 0 || pozíció >= tároló.n)
                throw new InvalidOperationException();
            return tároló.tároló[pozíció];
        }
    }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        pozíció++;
        return pozíció < tároló.n;
    }

    public void Reset()
    {
        pozíció = -1;
    }

    public void Dispose() { }
}

public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
{
    public FuggoFeladatTarolo(int méret) : base(méret)
    {
    }

    public override void MindentVegrehajt()
    {
        for (int i = 0; i < n; i++)
        {
            if (tároló[i].FuggosegTeljesul)
            {
                tároló[i].Vegrehajtas();
            }
            else
            {
                Console.WriteLine("A feladat függősége nem teljesült, nem hajtható végre.");
            }
        }
    }
}


}

