using System;
using System.Collections;
using System.Collections.Immutable;

namespace Test
{
    public class Solution
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please enter array's length: ");
            int n = Int32.Parse(Console.ReadLine());
            List<Fraction> fList = new List<Fraction>();

            for (int i = 0; i < n; i++)
            {
                Fraction temp = new Fraction();
                Console.WriteLine("Please enter fraction {0}'s numerator: ", i + 1);
                temp.Numerator = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Please enter fraction {0}'s denominator: ", i + 1);
                temp.Denominator = Int32.Parse(Console.ReadLine());
                fList.Add(temp);
            }

            Console.Clear();
            Console.Write("Origin list: ");
            for (int i = 0; i < fList.Count; i++)
            {
                Console.Write("{0}/{1} ", fList[i].Numerator, fList[i].Denominator);
            }
            Console.WriteLine("\n");

            fList.Sort();
            Console.Write("Sorted list: ");
            for (int i = 0; i < fList.Count; i++)
            {
                Console.Write("{0}/{1} ", fList[i].Numerator, fList[i].Denominator);
            }
        }
    }
}