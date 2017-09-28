using System.Linq;

namespace CommonLibrary.Math
{
    public static class LuhnsAlgorithm
    {
        private static readonly int[] DoubleDigitCalculation = { 0, 2, 4, 6, 8, 1, 3, 5, 7, 9 };

        public static int GetLuhnsCheckDigit(this string account)
        {
            int checkValue = LuhnsCalculation(account.Select(c => c - '0').ToArray(), false);

            return checkValue == 0 ? 0 : 10 - checkValue;
        }

        public static bool LuhnsPass(this string account)
        {
            return LuhnsCalculation(account.Select(c => c - '0').ToArray(), true) == 0;
        }

        private static int LuhnsCalculation(int[] digits, bool includesCheckDigit)
        {
            int index = 0;
            int modIndex = includesCheckDigit ? digits.Length % 2 : digits.Length % 2 == 1 ? 0 : 1;

            return digits.Sum(d => index++ % 2 == modIndex ? DoubleDigitCalculation[d] : d) % 10;
        }
    }
}