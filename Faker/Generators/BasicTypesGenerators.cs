namespace Generators
{
    public class IntGenerator : BaseGenerator<int>
    {
        public override int generate(Random random)
        {
            return random.Next();
        }
    }

    public class FloatGenerator : BaseGenerator<float>
    {
        public override float generate(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, random.Next(-126, 127));
            return (float)(mantissa * exponent);
        }
    }

    public class DoubleGenerator : BaseGenerator<double>
    {
        public override double generate(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, random.Next(-126, 127));
            return (mantissa * exponent);
        }
    }

    public class LongGenerator : BaseGenerator<long>
    { 
        public override long generate(Random random) 
        {  
            return random.NextInt64(); 
        } 
    }

    public class BoolGenerator : BaseGenerator<bool>
    {
        public override bool generate(Random random)
        {
            return random.Next() > (Int32.MaxValue / 2);
        }
    }

    public class CharGenerator : BaseGenerator<char>
    {
        public override char generate(Random random)
        {
            return (char)random.Next(char.MinValue, char.MaxValue);
        }
    }
}
