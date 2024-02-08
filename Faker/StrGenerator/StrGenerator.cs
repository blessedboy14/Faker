using Generators;

namespace StrGenerator
{
    public class StrGenerator : BaseGenerator<string>
    {
        public override string generate(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            int length = random.Next(7, 15);
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
