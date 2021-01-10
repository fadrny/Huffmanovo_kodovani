using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Huffmanovo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("input.txt"))
            {
                string input = r.ReadToEnd();
                Console.WriteLine(input);
                int[,] pole = CountOccurrence(input);
                for (int i = 0; i < pole.GetLength(0); i++)
                {
                    Console.Write((char)pole[i,0]);
                    Console.Write("..........");
                    Console.WriteLine(pole[i,1]);
                }

                Console.WriteLine();
                
                Dictionary<char,string> final = Binary(pole);
                foreach (var d in final)
                {
                    Console.Write(d.Key);
                    Console.Write("..................");
                    Console.WriteLine(d.Value);
                }

                foreach (char c in input)
                {
                    Console.Write(final[c] + " "); 
                }
            }
        }

        static int[,] CountOccurrence(string input)
        {
            List<char> letters = new List<char>();
            List<int> occurence = new List<int>();
            
            foreach (char c in input)
            {
                if (!letters.Contains(c))
                {
                    letters.Add(c);
                    occurence.Add(0);
                }
                occurence[letters.IndexOf(c)] += 1;
            }

            int[,] unsorted = new int[letters.Count,2];
            
            for (int i = 0; i < occurence.Count; i++)
            {
                unsorted[i, 0] = (int) letters[i];
                unsorted[i, 1] = occurence[i];
            }
            int[,] output = unsorted.OrderBy(x => x[1]);
            return output;
        }

        public static string Reverse(string stringToReverse )
        {
            char[] stringArray = stringToReverse.ToCharArray();
            string reverse = String.Empty;

            Array.Reverse(stringArray);

            return new string(stringArray);
        }
        static Dictionary<char,string> Binary(int[,] pole)
        {
            var input = new Dictionary<string, int>();
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                string tmp = "" + (char) pole[i, 0];
                input[tmp] = pole[i, 1];
            }

            Dictionary<char,string> slovnik = new Dictionary<char, string>();

            for (int i = 0; i < pole.GetLength(0); i++)
            {
                slovnik.Add((char)pole[i,0],"");
            }

            while (input.Count > 1)
            {
                input = input.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);
                
                KeyValuePair<string,int> predposledni = input.ElementAtOrDefault(input.Count - 2);

                foreach (char c in predposledni.Key)
                    slovnik[c] += "1";
                foreach (char c in input.Last().Key)
                    slovnik[c] += "0";

                string prk = predposledni.Key;
                int pri = predposledni.Value;

                string pk = input.Last().Key;
                int pi = input.Last().Value;
                
                input.Remove(input.Last().Key);
                input.Remove(predposledni.Key);
                input.Add(prk+pk,pri+pi);
            }

            Dictionary<char,string> hotovka = new Dictionary<char, string>();
            for (int i = 0; i < slovnik.Count; i++)
                hotovka.Add(slovnik.ElementAt(i).Key,Reverse(slovnik.ElementAt(i).Value));

            return hotovka;
        }
    }
    
    public static class MultiDimensionalArrayExtensions
{
  /// <summary>
  ///   Orders the two dimensional array by the provided key in the key selector.
  /// </summary>
  /// <typeparam name="T">The type of the source two-dimensional array.</typeparam>
  /// <param name="source">The source two-dimensional array.</param>
  /// <param name="keySelector">The selector to retrieve the column to sort on.</param>
  /// <returns>A new two dimensional array sorted on the key.</returns>
  public static T[,] OrderBy<T>(this T[,] source, Func<T[], T> keySelector)
  {
      return source.ConvertToSingleDimension().OrderBy(keySelector).ConvertToMultiDimensional();
  }
  /// <summary>
  ///   Orders the two dimensional array by the provided key in the key selector in descending order.
  /// </summary>
  /// <typeparam name="T">The type of the source two-dimensional array.</typeparam>
  /// <param name="source">The source two-dimensional array.</param>
  /// <param name="keySelector">The selector to retrieve the column to sort on.</param>
  /// <returns>A new two dimensional array sorted on the key.</returns>
  public static T[,] OrderByDescending<T>(this T[,] source, Func<T[], T> keySelector)
  {
      return source.ConvertToSingleDimension().
      	OrderByDescending(keySelector).ConvertToMultiDimensional();
  }
  /// <summary>
  ///   Converts a two dimensional array to single dimensional array.
  /// </summary>
  /// <typeparam name="T">The type of the two dimensional array.</typeparam>
  /// <param name="source">The source two dimensional array.</param>
  /// <returns>The repackaged two dimensional array as a single dimension based on rows.</returns>
  private static IEnumerable<T[]> ConvertToSingleDimension<T>(this T[,] source)
  {
      T[] arRow;
 
      for (int row = 0; row < source.GetLength(0); ++row)
      {
          arRow = new T[source.GetLength(1)];
 
          for (int col = 0; col < source.GetLength(1); ++col)
              arRow[col] = source[row, col];
 
          yield return arRow;
      }
  }
  /// <summary>
  ///   Converts a collection of rows from a two dimensional array back into a two dimensional array.
  /// </summary>
  /// <typeparam name="T">The type of the two dimensional array.</typeparam>
  /// <param name="source">The source collection of rows to convert.</param>
  /// <returns>The two dimensional array.</returns>
  private static T[,] ConvertToMultiDimensional<T>(this IEnumerable<T[]> source)
  {
      T[,] twoDimensional;
      T[][] arrayOfArray;
      int numberofColumns;
 
      arrayOfArray = source.ToArray();
      numberofColumns = (arrayOfArray.Length > 0) ? arrayOfArray[0].Length : 0;
      twoDimensional = new T[arrayOfArray.Length, numberofColumns];
 
      for (int row = 0; row < arrayOfArray.GetLength(0); ++row)
          for (int col = 0; col < numberofColumns; ++col)
              twoDimensional[row, col] = arrayOfArray[row][col];
 
      return twoDimensional;
  }
}
}