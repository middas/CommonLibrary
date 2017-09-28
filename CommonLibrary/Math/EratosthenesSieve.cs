using System.Collections;
using System.Collections.Generic;

namespace CommonLibrary.Math
{
    public class EratosthenesSieve
    {
        public static IEnumerable<long> PrimeNumbers(int limit)
        {
            // add 1 to the limit to determine the limit number itself as well
            BitArray bitArray = new BitArray(limit + 1, true);

            bitArray[0] = false;
            bitArray[1] = false;
            bitArray[2] = true;

            yield return 2;

            for (int i = 4; i < bitArray.Length; i += 2)
            {
                bitArray[i] = false;
            }

            for (int i = 3; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    yield return i;

                    for (int x = i * 2; x < bitArray.Length; x += i)
                    {
                        bitArray[x] = false;
                    }
                }
            }
        }
    }
}