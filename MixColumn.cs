using System;

namespace AesFunctions
{
    public class MixColumn
    {
        // Predefined constants for MixColumns operation in AES (Galois field multiplication)
        private static readonly byte[] MultiplyBy2 = new byte[256];
        private static readonly byte[] MultiplyBy3 = new byte[256];
        private static readonly byte[] MultiplyBy9 = new byte[256];
        private static readonly byte[] MultiplyBy11 = new byte[256];
        private static readonly byte[] MultiplyBy13 = new byte[256];
        private static readonly byte[] MultiplyBy14 = new byte[256];

        static MixColumn()
        {
            // Fill multiplication lookup tables for Galois field
            for (int i = 0; i < 256; i++)
            {
                MultiplyBy2[i] = (byte)((i << 1) ^ ((i & 0x80) != 0 ? 0x1B : 0x00));
                MultiplyBy3[i] = (byte)(MultiplyBy2[i] ^ i);

                MultiplyBy9[i] = Multiply((byte)i, (byte)0x09);
                MultiplyBy11[i] = Multiply((byte)i, (byte)0x0B);
                MultiplyBy13[i] = Multiply((byte)i, (byte)0x0D);
                MultiplyBy14[i] = Multiply((byte)i, (byte)0x0E);
            }
        }

        private static byte Multiply(byte a, byte b)
        {
            byte result = 0;
            while (b > 0)
            {
                if ((b & 1) != 0) result ^= a;
                bool hiBitSet = (a & 0x80) != 0;
                a <<= 1;
                if (hiBitSet) a ^= 0x1B;
                b >>= 1;
            }
            return result;
        }

        public void ApplyMixColumns(AESState state)
        {
            for (int i = 0; i < 4; i++)
            {
                byte[] column = new byte[4];
                for (int j = 0; j < 4; j++)
                {
                    column[j] = state.State[j, i];
                }

                MixSingleColumn(column);

                for (int j = 0; j < 4; j++)
                {
                    state.State[j, i] = column[j];
                }
            }
        }

        public void ApplyInverseMixColumns(AESState state)
        {
            for (int i = 0; i < 4; i++)
            {
                byte[] column = new byte[4];
                for (int j = 0; j < 4; j++)
                {
                    column[j] = state.State[j, i];
                }

                InverseMixSingleColumn(column);

                for (int j = 0; j < 4; j++)
                {
                    state.State[j, i] = column[j];
                }
            }
        }

        private void MixSingleColumn(byte[] column)
        {
            byte a0 = column[0];
            byte a1 = column[1];
            byte a2 = column[2];
            byte a3 = column[3];

            column[0] = (byte)(MultiplyBy2[a0] ^ MultiplyBy3[a1] ^ a2 ^ a3);
            column[1] = (byte)(a0 ^ MultiplyBy2[a1] ^ MultiplyBy3[a2] ^ a3);
            column[2] = (byte)(a0 ^ a1 ^ MultiplyBy2[a2] ^ MultiplyBy3[a3]);
            column[3] = (byte)(MultiplyBy3[a0] ^ a1 ^ a2 ^ MultiplyBy2[a3]);
        }

        private void InverseMixSingleColumn(byte[] column)
        {
            byte a0 = column[0];
            byte a1 = column[1];
            byte a2 = column[2];
            byte a3 = column[3];

            column[0] = (byte)(MultiplyBy14[a0] ^ MultiplyBy11[a1] ^ MultiplyBy13[a2] ^ MultiplyBy9[a3]);
            column[1] = (byte)(MultiplyBy9[a0] ^ MultiplyBy14[a1] ^ MultiplyBy11[a2] ^ MultiplyBy13[a3]);
            column[2] = (byte)(MultiplyBy13[a0] ^ MultiplyBy9[a1] ^ MultiplyBy14[a2] ^ MultiplyBy11[a3]);
            column[3] = (byte)(MultiplyBy11[a0] ^ MultiplyBy13[a1] ^ MultiplyBy9[a2] ^ MultiplyBy14[a3]);
        }
    }
}
