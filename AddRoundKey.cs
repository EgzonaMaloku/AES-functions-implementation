using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AesFunctions
{
    class AddRoundKey
    {

        public void ApplyAddRoundKey(AESState state, byte[,] roundKey)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state.State[i, j] ^= roundKey[i, j];
                }
            }
        }
    }
}
