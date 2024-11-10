using System;

namespace AesFunctions
{
    public class AESState
    {
        public byte[,] State { get; set; }

        public AESState(byte[,] initialState)
        {
            State = initialState;
        }

        public void PrintState(string title)
        {
            Console.WriteLine(title);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write($"{State[i, j]:X2} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
