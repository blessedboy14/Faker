using Faker;
public class Some
{
    public static void Main(String[] args)
    {
        IFaker f = new Faker.Faker();
        string t = "hello";
        bool m = t.GetType().IsValueType;
        Foo example = f.Create<Foo>();
        int i = 0;
    }
}

public class Foo
{
    public byte t;
    public string some_str;
    public int ewkere;
    public bool serega_pirat;
    public double qeqoqeq;
    public Bot bot { get; set; }
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
}
