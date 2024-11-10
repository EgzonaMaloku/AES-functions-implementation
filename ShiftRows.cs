namespace AesFunctions
{
    public class ShiftRows
    {
        public void ApplyInverseShiftRows(AESState state)
        {
            // Row 1: No shift
            //  Row 2: Shift right by 1 (as shifting left by 3)
            ShiftRow(state.State, 1, 3);

            // Row 3: Shift right by 2
            ShiftRow(state.State, 2, 2);

            // Row 4: Shift right by 3 (as shifting left by 1)
            ShiftRow(state.State, 3, 1);
        }

        public void ApplyShiftRows(AESState state)
        {
            // Row 1: No shift
            // Row 2: Shift left by 1
            ShiftRow(state.State, 1, 1);

            // Row 3: Shift left by 2
            ShiftRow(state.State, 2, 2);

            // Row 4: Shift left by 3
            ShiftRow(state.State, 3, 3);
        }

        private void ShiftRow(byte[,] state, int row, int shiftAmount)
        {
            byte[] temp = new byte[4];

            for (int i = 0; i < 4; i++)
                temp[i] = state[row, (i + shiftAmount) % 4];

            for (int i = 0; i < 4; i++)
                state[row, i] = temp[i];
        }
    }
}
