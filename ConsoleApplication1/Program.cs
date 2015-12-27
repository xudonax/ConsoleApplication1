using System;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong seed = 0xfa40e19fb9cbd97e;

            //var lfsr = new LinearFeedbackShiftRegister(seed);
            var lfsr = new LinearFeedbackShiftRegister();
            ulong value = 0uL;

            foreach (var val in lfsr)
            {
                value = val;
                Console.WriteLine(value);
            }

            Console.WriteLine($"For seed {lfsr.Seed} the period is {lfsr.Period}.");
            Console.ReadKey();
        }
    }
}
