﻿using System;
using System.Text;

namespace AesFunctions
{
    class Program
    {
        private static ByteSub byteSub = new ByteSub();
        private static ShiftRows shiftRows = new ShiftRows();
        private static MixColumn mixColumn = new MixColumn();
        private static AddRoundKey addRoundKey = new AddRoundKey();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter a string to encrypt :");

            string input = Console.ReadLine();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[,] initialState = new byte[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int index = i * 4 + j;
                    // Pad with zeros if input is less than 16 bytes
                    initialState[i, j] = index < inputBytes.Length ? inputBytes[index] : (byte)0x00;
                }
            }

            byte[,] roundKey = {
                        { 0xA0, 0x88, 0x23, 0x2A },
                        { 0xFA, 0x54, 0xA3, 0x6F },
                        { 0xC3, 0x1B, 0x24, 0xF2 },
                        { 0x6B, 0x6C, 0x75, 0x3E }
     };

            int rounds = 10;

            AESState state = new AESState(initialState);

            PrintMatrix(state.State, "Initial state: ");

            AESState encrypted = Encrypt(state, roundKey, rounds);

            PrintMatrix(encrypted.State, "Encrypted state: ");

            AESState decrypted = Decrypt(encrypted, roundKey, rounds);

            PrintMatrix(decrypted.State, "Decrypted state: ");

            byte[] decryptedBytes = new byte[16];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    decryptedBytes[i * 4 + j] = decrypted.State[i, j];
                }
            }

            string decryptedString = Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\0');
            Console.WriteLine("Decrypted string: " + decryptedString);

            Console.ReadKey();
        }


        static AESState Encrypt(AESState state, byte[,] roundKey, int rounds)
        {
            addRoundKey.ApplyAddRoundKey(state, roundKey);

            for (var i = 0; i < rounds; i++)
            {
                byteSub.ApplyByteSub(state);
                shiftRows.ApplyShiftRows(state);
                if (i < rounds - 1)
                {
                    // ommit the last round
                    mixColumn.ApplyMixColumns(state);
                }

                addRoundKey.ApplyAddRoundKey(state, roundKey);
            }

            return state;
        }

        static AESState Decrypt(AESState state, byte[,] roundKey, int rounds)
        {
            for (var i = 0; i < rounds; i++)
            {
                addRoundKey.ApplyAddRoundKey(state, roundKey);
                if (i > 0)
                {
                    // ommit the first round
                    mixColumn.ApplyInverseMixColumns(state);
                }
                shiftRows.ApplyInverseShiftRows(state);
                byteSub.ApplyInverseByteSub(state);
            }

            addRoundKey.ApplyAddRoundKey(state, roundKey);

            return state;
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

        static void TestMixColumns(AESState state)
        {
            MixColumn mixColumn = new MixColumn();

            PrintMatrix(state.State, "Initial state: ");

            mixColumn.ApplyMixColumns(state);

            PrintMatrix(state.State, "State after MixColumns: ");

            mixColumn.ApplyInverseMixColumns(state);

            PrintMatrix(state.State, "State after InverseMixColumns");

            Console.ReadKey();
        }

        static void TestRoundKey(AESState state, byte[,] roundKey)
        {
            AddRoundKey addRoundKey = new AddRoundKey();

            PrintMatrix(state.State, "Initial state: ");

            addRoundKey.ApplyAddRoundKey(state, roundKey);

            PrintMatrix(state.State, "State after AddRoundKey: ");

            addRoundKey.ApplyAddRoundKey(state, roundKey);

            PrintMatrix(state.State, "State after Inverse AddRoundKey: ");

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
