using Sort;
using System.Diagnostics;
using System.Security.Cryptography;

internal class Program
{
    private static void Print(CArray Arr)
    {
        for (int i = 0; i < Arr.ColLength; ++i)
            Console.Write(string.Format("Col {0}\t", i + 1));
        Console.WriteLine();
        for (int i = 0; i < Arr.ColLength; ++i)
            Console.Write(string.Format("-------\t"));
        Console.WriteLine();

        for (int j = 0; j < Arr.RowLength; ++j)
        {
            for (int i = 0; i < Arr.ColLength; ++i)
                Console.Write(string.Format("{0}\t", Arr[i][j]));
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Array.Sort 2D!");
        Console.WriteLine();

        string[] Words = {"Apple", "Orange", "Banana", "Onion", "Garlic", "Carrot", "Sun", "Moon", "Mercury",
                "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto"};

        CArray Arr = new CArray(10, 10);

        bool b;
        do
        {
            // Fill in each cell with its row position value
            for (int i = 0; i < Arr.ColLength; ++i)
            {
                // Column contains random numbers
                for (int j = 0; j < Arr.RowLength; ++j)
                {
                    if (RandomNumberGenerator.GetInt32(0, 2) == 0)
                        Arr[i][j] = new CVariant(RandomNumberGenerator.GetInt32(0, 10));
                    else
                        Arr[i][j] = new CVariant(Words[RandomNumberGenerator.GetInt32(0, Words.Length)]);
                }
            }

            Print(Arr);

            string? str;
            int iColumn;
            do
            {
                Console.Write(string.Format("Sort on column ({0}-{1}): ", 1, Arr.ColLength));
                str = Console.ReadLine();
                b = int.TryParse(str, out iColumn);
                if (b)
                    b = iColumn > 0 && iColumn <= Arr.ColLength;
            } while (!b);

            b = false;
            do
            {
                Console.Write("Ascending or Descending (A/D): ");
                string? strSort = Console.ReadLine();
                if (string.Compare(strSort, "A", true) == 0)
                {
                    Global.g_bSortOrder = true;
                    b = true;
                }
                else if (string.Compare(strSort, "D", true) == 0)
                {
                    Global.g_bSortOrder = false;
                    b = true;
                }
            } while (!b);

            Console.WriteLine();
            Console.WriteLine(string.Format("Sorting on Column {0} {1}", iColumn, Global.g_bSortOrder ? "Ascending" : "Descending"));

            // Sort
            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            Arr.ParallelSort(iColumn - 1);
            watch.Stop();
            long Milli = watch.ElapsedMilliseconds;
            Console.WriteLine(Milli);

            Print(Arr);

            do
            {
                Console.Write("Again (Y/N): ");
                string? strAgain = Console.ReadLine();
                if (string.Compare(strAgain, "Y", true) == 0)
                {
                    b = true;
                    break;
                }
                else if (string.Compare(strAgain, "N", true) == 0)
                {
                    b = false;
                    break;
                }
            } while (true);
            Console.WriteLine();
        } while (b);
    }
}