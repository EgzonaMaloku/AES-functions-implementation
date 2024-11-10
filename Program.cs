using System;

namespace AesFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello AES");

            byte[,] initialState = {
                { 0x19, 0xA0, 0x9A, 0xE9 },
                { 0x3D, 0xF4, 0xC6, 0xF8 },
                { 0xE3, 0xE2, 0x8D, 0x48 },
                { 0xBE, 0x2B, 0x2A, 0x08 }
            };

            byte[,] roundKey = {
                { 0xA0, 0x88, 0x23, 0x2A },
                { 0xFA, 0x54, 0xA3, 0x6F },
                { 0xC3, 0x1B, 0x24, 0xF2 },
                { 0x6B, 0x6C, 0x75, 0x3E }
            };


            AESState state = new AESState(initialState);

            TestByteSub(state);
            TestShiftRows(state);
        }

        static void TestByteSub(AESState state)
        {
            ByteSub byteSub = new ByteSub();

            PrintMatrix(state.State, "Initial state: ");

            PrintMatrix(byteSub.GetSBox(), "SBox: ");

            byteSub.ApplyByteSub(state);

            PrintMatrix(state.State, "State after ByteSub: ");

            PrintMatrix(byteSub.GetInverseSBox(), "Inverse SBox: ");

            byteSub.ApplyInverseByteSub(state);

            PrintMatrix(state.State, "State after InverseByteSub: ");

            Console.ReadKey();
        }

        static void TestShiftRows(AESState state)
        {
            ShiftRows shiftRows = new ShiftRows();

            PrintMatrix(state.State, "Initial state: ");

            shiftRows.ApplyShiftRows(state);

            PrintMatrix(state.State, "State after ShiftRows: ");

            shiftRows.ApplyInverseShiftRows(state);

            PrintMatrix(state.State, "State after InverseShiftRows: ");

            Console.ReadKey();
        }

        static void PrintMatrix(byte[,] matrix, string title)
        {
            Console.WriteLine(title);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]:X2} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
