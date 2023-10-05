using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public static T FindMax<T>(List<T> list)
    {
        if (list.Count == 0)
            throw new ArgumentException("Empty list!");
        T max = list[0];
        if (typeof(T) == typeof(string))
        {
            foreach (T item in list)
                if (item.ToString().Length > max.ToString().Length)
                    max = item;
            return max;
        }

        foreach (T item in list)
            if (Comparer<T>.Default.Compare(item, max) > 0)
                max = item;
        return max;
    }

    public static T FindMin<T>(List<T> list)
    {
        if (list.Count == 0)
            throw new ArgumentException("Empty list!");
        T min = list[0];
        if (typeof(T) == typeof(string))
        {
            foreach (T item in list)
                if (item.ToString().Length < min.ToString().Length)
                    min = item;
            return min;
        }

        foreach (T item in list)
            if (Comparer<T>.Default.Compare(item, min) < 0)
                min = item;
        return min;

    }

    public static void Main(string[] args)
    {
        List<int> intList = new List<int> { 11, 7, 2004, 12234, -123 };
        List<double> doubleList = new List<double> { 3.14, 33.9, 7.334, 5.12 };
        List<string> stringList = new List<string> { "Welcome", "To", "UIT" };

        int intMax = FindMax(intList);
        double doubleMax = FindMax(doubleList);
        string stringMax = FindMax(stringList);

        int intMin = FindMin(intList);
        double doubleMin = FindMin(doubleList);
        string stringMin = FindMin(stringList);

        Console.WriteLine("Greatest integer in the list: {0}", intMax);
        Console.WriteLine("Greatest double in the list: {0}", doubleMax);
        Console.WriteLine("Longest string in the list: {0}\n", stringMax);
        Console.WriteLine("Least integer in the list: {0}", intMin);
        Console.WriteLine("Least double in the list: {0}", doubleMin);
        Console.WriteLine("Shortest string in the list: {0}", stringMin);
    }

}