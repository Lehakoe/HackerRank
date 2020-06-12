using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace _2D_Array___DS___Core
{
    [MemoryDiagnoser]
    public class AlgorithmExecutor
    {
        #region DataMembers
        Program _prog = new Program();
        #endregion

        #region OneDArrayAlgorithm
        [Benchmark]
        public void HourglassMaxSum1DArrayCaller()
        {
            HourglassMaxSum1DArray(_prog.Create1DArray());
        }

        public int HourglassMaxSum1DArray(int[] arr)
        {
            int oneDArrayBoundary = 22, topRowSum = default, bottomRowSum = default;
            int hourGlassSum = default, maxHourGlassSum = int.MinValue;

            for (int i = 0; i < oneDArrayBoundary; i++)
            {
                topRowSum = arr[i] + arr[i + 1] + arr[i + 2];  // Sum the top part of the hourglass.
                bottomRowSum = arr[i + 12] + arr[i + 13] + arr[i + 14];  // Sum the bottom part of the hourglass.
                hourGlassSum = (topRowSum + bottomRowSum + arr[i + 7]);  // Sum the hourglass, including it's spine.
                if (hourGlassSum > maxHourGlassSum)  // Get the largest hourglass sum.
                {
                    maxHourGlassSum = hourGlassSum;
                }
            }
            return maxHourGlassSum;
        }
        #endregion

        #region TwoDArrayAlgorithm
        [Benchmark]
        public void HourglassMaxSum2DArrayCaller()
        {
            HourglassMaxSum2DArray(_prog.Create2DArray());
        }

        public int HourglassMaxSum2DArray(int[][] arr)
        {
            int arrayBoundary = 4, topRowSum = default, bottomRowSum = default;
            int hourglassSpine = default, hourglassSum = 0, hourglassMax = int.MinValue;

            for (int row = 0; row < arrayBoundary; row++)
            {
                for (int col = 0; col < arrayBoundary; col++)
                {
                    topRowSum = arr[row][col] + arr[row][col + 1] + arr[row][col + 2];
                    bottomRowSum = arr[row + 2][col] + arr[row + 2][col + 1] + arr[row + 2][col + 2];
                    hourglassSpine = arr[row + 1][col + 1];
                    hourglassSum = topRowSum + bottomRowSum + hourglassSpine;
                    if (hourglassSum > hourglassMax)
                    {
                        hourglassMax = hourglassSum;
                    }
                }
            }
            return hourglassMax;
        }
        #endregion
    }

    [MemoryDiagnoser]
    public class Program
    {
        #region DataMembers
        const int TWO_D_ARRAY_INDEX_MAX = 6, ARR_ELEM_MIN = -9, ARR_ELEM_MAX = 10, ONE_D_ARRAY_SIZE = 36;
        static readonly Random _random = new Random();
        #endregion

        #region Create1DArray
        public int[] Create1DArray()
            => GenerateArray(ONE_D_ARRAY_SIZE);
        #endregion

        #region Print1DArray
        static internal void Print1DArray(int[] arr)
        {
            for (int row = 0; row < arr.Length; row += TWO_D_ARRAY_INDEX_MAX)
            {
                for (int col = 0; col < TWO_D_ARRAY_INDEX_MAX; col++)
                {
                    Console.Write($"{arr[row + col]}\t");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region Create2DArray
        public int[][] Create2DArray()
        {
            int[][] twoDArr = new int[TWO_D_ARRAY_INDEX_MAX][];
            for (int i = 0; i < TWO_D_ARRAY_INDEX_MAX; i++)
            {
                twoDArr[i] = GenerateArray(TWO_D_ARRAY_INDEX_MAX);
            }
            return twoDArr;
        }
        #endregion

        #region ArrayCreationHelper
        int[] GenerateArray(int arraySize)
        {
            int[] arr = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                arr[i] = _random.Next(ARR_ELEM_MIN, ARR_ELEM_MAX);
            }
            return arr;
        }
        #endregion

        #region Print2DArray
        static internal void Print2DArray(int[][] arr)
        {
            for (int row = 0; row < TWO_D_ARRAY_INDEX_MAX; row++)
            {
                for (int col = 0; col < TWO_D_ARRAY_INDEX_MAX; col++)
                {
                    Console.Write($"{arr[row][col]}\t");
                }
                Console.WriteLine();
            }
        }
        #endregion

        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AlgorithmExecutor>();
            BenchmarkRunner.Run<Program>();

            AlgorithmExecutor ae = new AlgorithmExecutor();
            var prog = new Program();

            int[] oneDArr = prog.Create1DArray();
            Print1DArray(oneDArr);
            Console.WriteLine($"Max = {ae.HourglassMaxSum1DArray(oneDArr)}.");

            int[][] twoDArr = prog.Create2DArray();
            Print2DArray(twoDArr);
            Console.WriteLine($"Max = {ae.HourglassMaxSum2DArray(twoDArr)}.");
        }
    }
}
