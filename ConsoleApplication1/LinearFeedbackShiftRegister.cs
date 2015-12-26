using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ConsoleApplication1
{
    public class LinearFeedbackShiftRegister : IEnumerable<ulong>
    {
        private ulong shiftRegister;
        private ulong seed;
        
        public LinearFeedbackShiftRegister()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[8];
                rng.GetBytes(randomBytes);

                seed = BitConverter.ToUInt64(randomBytes, 0);
            }
            shiftRegister = seed;
        }

        public LinearFeedbackShiftRegister(ulong seed)
        {
            this.seed = seed;
            shiftRegister = seed;
        }

        public IEnumerator<ulong> GetEnumerator()
        {
            // This is safe because we are in a Enumerator here
            do
            {
                // Get the bits at position 60, 61, 63 and 64:
                var bit60 = (shiftRegister & (1uL << 60 - 1)) != 0;
                var bit61 = (shiftRegister & (1uL << 61 - 1)) != 0;
                var bit63 = (shiftRegister & (1uL << 63 - 1)) != 0;
                var bit64 = (shiftRegister & (1uL << 64 - 1)) != 0;

                // XOR 64 with 63, that with 61 and that with 60
                var immediate1 = bit64 ^ bit63;
                var immediate2 = immediate1 ^ bit61;
                var shiftMeIn = immediate2 ^ bit60 ? 1uL : 0uL;

                // Shift the bit into shiftRegister
                shiftRegister = (shiftRegister >> 1) | (shiftMeIn << 63);

                // Let's see what the period is
                Period++;

                yield return shiftRegister;
            }
            while (shiftRegister != seed);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ulong Period { get; set; }
    }
}
