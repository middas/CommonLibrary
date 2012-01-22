using System;
using System.Text;

namespace CommonLibrary.Utilities
{
    public class IntToWord
    {
        public static string ConvertIntToWords(int i)
        {
            int abs = System.Math.Abs(i);
            if (i == 0)
                return "zero";
            int count = 1;
            while ((abs /= 1000) > 0)
                count++;
            abs = System.Math.Abs(i);
            int[] groups = new int[count];
            for (int x = 0; x < groups.Length; x++)
            {
                groups[x] = abs % 1000;
                abs /= 1000;
            }
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < groups.Length - 1; x++)
                sb.Insert(0, string.Format(" {0}", ConvertToWords(groups[x], x + 1)));
            sb.Insert(0, string.Format("{0}", ConvertToWords(groups[count - 1], count)));
            if (i < 0)
                sb.Insert(0, "negative ");
            string value = sb.ToString();
            while (value.Contains("  "))
                value = value.Replace("  ", " ");
            return value.Trim();
        }

        private static string GetNumberPosition(int i)
        {
            string s = "";
            switch (i)
            {
                case 0:
                case 1:
                    break;
                case 2:
                    s = " thousand";
                    break;
                case 3:
                    s = " million";
                    break;
                case 4:
                    s = " billion";
                    break;
            }
            return s;
        }

        private static string ConvertToWords(int threeDigits, int position)
        {
            string s = "";
            if (threeDigits > 999)
                throw new Exception("Cannot convert more than 3 numbers at a time");
            int hundreds = threeDigits / 100;
            int tens = (threeDigits % 100);
            int units = (tens % 10);
            tens /= 10;
            if (hundreds == 0 && tens == 0)
                s = GetWordFromInt(units);
            else
            {
                if (hundreds > 0)
                    s = string.Format("{0} hundred", GetWordFromInt(hundreds));
                if (tens > 0)
                {
                    if (tens == 1)
                    {
                        switch (units)
                        {
                            case 0:
                                s = string.Format("{0} {1}", s, "ten");
                                break;
                            case 1:
                                s = string.Format("{0} {1}", s, "eleven");
                                break;
                            case 2:
                                s = string.Format("{0} {1}", s, "twelve");
                                break;
                            case 3:
                                s = string.Format("{0} {1}", s, "thirteen");
                                break;
                            case 4:
                                s = string.Format("{0} {1}", s, "fourteen");
                                break;
                            case 5:
                                s = string.Format("{0} {1}", s, "fifteen");
                                break;
                            case 6:
                                s = string.Format("{0} {1}", s, "sixteen");
                                break;
                            case 7:
                                s = string.Format("{0} {1}", s, "seventeen");
                                break;
                            case 8:
                                s = string.Format("{0} {1}", s, "eighteen");
                                break;
                            case 9:
                                s = string.Format("{0} {1}", s, "ninteen");
                                break;
                        }
                    }
                    else
                    {
                        switch (tens)
                        {
                            case 0:
                            case 1:
                                break;
                            case 2:
                                s = string.Format("{0} {1}", s, "twenty");
                                break;
                            case 3:
                                s = string.Format("{0} {1}", s, "thirty");
                                break;
                            case 4:
                                s = string.Format("{0} {1}", s, "forty");
                                break;
                            case 5:
                                s = string.Format("{0} {1}", s, "fifty");
                                break;
                            case 6:
                                s = string.Format("{0} {1}", s, "sixty");
                                break;
                            case 7:
                                s = string.Format("{0} {1}", s, "seventy");
                                break;
                            case 8:
                                s = string.Format("{0} {1}", s, "eighty");
                                break;
                            case 9:
                                s = string.Format("{0} {1}", s, "ninety");
                                break;
                        }
                        if (units > 0)
                            s = string.Format("{0}-", s);
                    }
                }
                else if (units > 0)
                    s = string.Format("{0} and", s);
                if (units > 0 && tens != 1)
                    s = string.Format("{0}{1}", s, GetWordFromInt(units));
            }
            if (hundreds > 0 || tens > 0 || units > 0)
                s = string.Format("{0}{1}", s, GetNumberPosition(position));
            return s;
        }

        private static string GetWordFromInt(int i)
        {
            string s = "";
            switch (i)
            {
                case 1:
                    s = "one";
                    break;
                case 2:
                    s = "two";
                    break;
                case 3:
                    s = "three";
                    break;
                case 4:
                    s = "four";
                    break;
                case 5:
                    s = "five";
                    break;
                case 6:
                    s = "six";
                    break;
                case 7:
                    s = "seven";
                    break;
                case 8:
                    s = "eight";
                    break;
                case 9:
                    s = "nine";
                    break;
            }
            return s;
        }
    }
}
