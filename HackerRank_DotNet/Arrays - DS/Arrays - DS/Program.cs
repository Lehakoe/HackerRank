using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace Arrays___DS
{
    [MemoryDiagnoser]
    public class Program
    {
        int[] _arr;
        const int MIN = 1;
        const int MAX = 10_001; //Upper boundary is excluded in Random.Next()
        readonly Random _random = new Random();

        [Params(1_000, 1_000_000)]
        public int _max_array_size;

        [GlobalSetup]
        public void Setup()
        {
            _arr = new int[_max_array_size];
        }

        [Benchmark]
        public void ArrayGenerator()
        {
            for (int i = 0; i < _max_array_size; i++)
            {
                _arr[i] = _random.Next(MIN, MAX);
            }
        }

        [Benchmark]
        public int[] BuiltInCaller() 
            => ReverseArrayBuiltInMethod(_arr);

        [Benchmark]
        public int[] LoopHalfCaller() 
            => ReverseArrayLoopHalfArraySize(_arr);

        [Benchmark]
        public int[] NewArrayCaller() 
            => ReverseArrayNewArray(_arr);

        int[] ReverseArrayBuiltInMethod(int[] arr)
        {
            Array.Reverse(arr);
            return arr;
        }

        //Unsafe context offers better performance
        unsafe int[] ReverseArrayLoopHalfArraySize(int[] arr)
        {
            int loopCounter = arr.Length / 2;
            int temp;
            int oppositeI;
            fixed(int* iPtr = arr)
            {
                for (int i = 0; i < loopCounter; i++)
                {
                    //Swop(arr, i, arr.Length - (i + 1));

                    //temp = arr[i];
                    //oppositeI = arr.Length - (i + 1);
                    //arr[i] = arr[oppositeI];
                    //arr[oppositeI] = temp;

                    temp = *(iPtr + i);
                    oppositeI = arr.Length - (i + 1);
                    *(iPtr + i) = *(iPtr + oppositeI);
                    *(iPtr + oppositeI) = temp;
                }
            }
            return arr;
        }

        // More efficient to not call the other method in a loop - above
        // Small functionality
        void Swop(int[] arr, int firstIndex, int secondIndex)
        {
            int temp = arr[firstIndex];
            arr[firstIndex] = arr[secondIndex];
            arr[secondIndex] = temp;

            // Clever, but way too inefficient
            //arr[firstIndex] = arr[firstIndex] + arr[secondIndex];
            //arr[secondIndex] = arr[firstIndex] - arr[secondIndex];
            //arr[firstIndex] = arr[firstIndex] - arr[secondIndex];
        }

        int[] ReverseArrayNewArray(int[] arr)
        {
            int length = arr.Length;
            int[] retVal = new int[length];

            for (int i = 0; i < length; i++)
            {
                retVal[i] = arr[length - i - 1];
            }

            return retVal;
        }

        static void Main()
        {
            BenchmarkRunner.Run<Program>();
        }
    }
}