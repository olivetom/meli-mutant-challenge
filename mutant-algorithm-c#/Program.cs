using System;
using System.Collections.Generic;
using System.Linq;

namespace meli
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    int boardSize = 3;
        //    int patternSize = 2;


        //    char[,] diagPattern = new char[,] { { 'a', '0', '0' },
        //                                        { '0', 'a', '0' },
        //                                        { '0', '0', 'a'} };

        //    char[,] revDiagPattern = new char[,] { { '0', '0', 'b' },
        //                                           { '0', 'b', '0' },
        //                                           { 'b', '0', '0' } };

        //    char[,] columnPattern = new char[,] { { '0', '0', 'c' },
        //                                        { '0', '0', 'c' },
        //                                        { '0', '0', 'c'} };

        //    char[,] rowPattern = new char[,] { { '0', '0', '0' },
        //                                        { '0', '0', '0' },
        //                                        { 'd', 'd', 'd'} };


        //    for (int i = 0; i < boardSize; i++)
        //    {
        //        Console.WriteLine(diagPattern[i, i]);

        //    }

        //    for (int i = 0; i < boardSize; i++)
        //    {
        //        Console.WriteLine(revDiagPattern[i, boardSize-i-1]);
        //    }

        //    for (int i = 0; i < boardSize; i++)
        //    {
        //        Console.WriteLine(columnPattern[i, i]);

        //    }
        //}
        static void Main(string[] args)
        {
            int boardSize = 4;

            var range = Enumerable.Range(1, boardSize * boardSize);
            List<List<int>> winningPaths = new List<List<int>>();

            //Horizontal Paths
            Enumerable.Range(0, boardSize).ToList().
                ForEach(k => winningPaths.Add(range.Skip(k * boardSize).Take(boardSize).ToList()));

            //Vertical Paths 
            Enumerable.Range(0, boardSize).ToList()
                .ForEach(k => winningPaths.Add(winningPaths.Take(boardSize).Select(p => p[k]).ToList()));

            //Diagonal Paths
            //Main diagonal 
            winningPaths.Add(range.Where((r, i) => i % (boardSize + 1) == 0).ToList());

            //reverse diagonal 
            winningPaths.Add(
                range.Where((r, i) => i % (boardSize - 1) == 0).Take(boardSize).ToList());
            //range.Where((r, i) => i % (boardSize - 1) == 0).Skip(1).Take(boardSize).ToList());

            //printing all the paths; one path on each line. 
            var x = winningPaths.Select(x => x.Select(z => z.ToString()).Aggregate
                ((a, b) => a.ToString() + " " + b.ToString()));
            Console.WriteLine("All winning paths for a Tic-Tac-Toe board of size 3");
            x.ToList().ForEach(i => Console.WriteLine(i));
        }
    }
}
