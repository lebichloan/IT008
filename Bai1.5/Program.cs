using System;
using System.IO;

class Program
{
    static void Main()
    {
        string folderPath = null;
        while (folderPath == null)
        {
            Console.Write("Nhap duong dan thu muc: ");
            folderPath = Console.ReadLine();
        }

        if (Directory.Exists(folderPath))
        {
            Console.WriteLine($"Cac tap tin trong thu muc '{folderPath}' la:");

            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
        else
        {
            Console.WriteLine($"Duong dan thu muc '{folderPath}' khong ton tai.");
        }
    }
}
