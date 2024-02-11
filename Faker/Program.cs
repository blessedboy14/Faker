using Faker;
public class Some
{
    public static void teain(String[] args)
    {
        IFaker f = new Faker.Faker();
        Foo example = f.Create<Foo>();
        int i = 0;
    }
}

public class Foo
{
    public Bot bot { get; set; }
    public byte t;
    public int resticted { get; }
    public string some_str;
    public int ewkere;
    public bool serega_pirat;
    public double qeqoqeq;
    
    public Foo(string some_str, int ewkere, bool serega_pirat, double qeqoqeq) { 
        this.qeqoqeq = qeqoqeq;
        this.some_str = some_str;
        this.ewkere = ewkere;   
        this.serega_pirat = serega_pirat;
    }
}

public class Bot
{
    public long some_val;
    public char eueu;
    public List<int> ints;
}
