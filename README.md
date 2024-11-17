# AES Implementation in C#

This project demonstrates modular implementation of the Advanced Encryption Standard (AES) encryption and decryption process in C#. It demonstrates how AES performs encryption and decryption on a 4x4 byte matrix (state) using core transformations across multiple rounds.

## Key Features

1. **Core AES Steps**:
   - **ByteSub (SubBytes)**: Performs byte substitution using the AES S-Box.
    - **ShiftRows**: Cyclically shifts the rows of the state matrix.
   - **MixColumns**: Mixes the columns of the state matrix for diffusion.
   - **AddRoundKey**: XORs the state matrix with a round key.
   - **Inverse Transformations**: Implements decryption steps for each transformation.
   - 
2. **AES Rounds**:
   - **Encryption**: 10 rounds (for 128-bit keys), with MixColumns omitted in the final round.
   - **Decryption**: Reverses the transformations in the correct order to recover the original plaintext.
   - 
3. **Custom Classes**:
   - `ByteSub`: Handles SubBytes and Inverse SubBytes transformations.
   - `ShiftRows`: Handles ShiftRows and Inverse ShiftRows transformations.
   - `MixColumn`: Handles MixColumns and Inverse MixColumns transformations.
   - `AddRoundKey`: Handles XORing the state with the round key.
   - `AESState`: Manages the 4x4 byte matrix used in the AES algorithm.
---

## How It Works

### ByteSub Class

The `ByteSub` class implements the SubBytes and Inverse SubBytes steps of the AES encryption and decryption process. It uses the S-Box for byte substitution.

- SubBytes transformation: Applies a substitution based on the AES S-Box.
- Inverse SubBytes transformation: Reverses the substitution using the inverse S-Box.

#### How to use

```csharp
// Example AES state
byte[,] stateMatrix = {
    { 0x19, 0xA0, 0x9A, 0xE9 },
    { 0x3D, 0xF4, 0xC6, 0xF8 },
    { 0xE3, 0xE2, 0x8D, 0x48 },
    { 0xBE, 0x2B, 0x2A, 0x08 }
};
AESState state = new AESState(stateMatrix);

ByteSub byteSub = new ByteSub();

// Apply SubBytes
byteSub.ApplyByteSub(state);

// Apply Inverse SubBytes
byteSub.ApplyInverseByteSub(state);

##### Example: 
- Before SubBytes

19 A0 9A E9
3D F4 C6 F8
E3 E2 8D 48
BE 2B 2A 08

- After SubBytes

D4 BF 5D 30
E0 B4 52 AE
B8 41 11 F1
1E 27 98 E5
```

### ShiftRows Class

The `ShiftRows` class implements the row-shifting step in AES encryption and its inverse for decryption. This transformation cyclically shifts the rows of the state matrix.

- ShiftRows transformation: Shifts rows of the state matrix to the left.
- Inverse ShiftRows transformation: Reverses the shifts to the right.

#### How to use

```csharp
ShiftRows shiftRows = new ShiftRows();

// Apply ShiftRows
shiftRows.ApplyShiftRows(state);

// Apply Inverse ShiftRows
shiftRows.ApplyInverseShiftRows(state);


##### Example: 
- Before ShiftRows

19 A0 9A E9
3D F4 C6 F8
E3 E2 8D 48
BE 2B 2A 08

- After ShiftRows

19 A0 9A E9
F4 C6 F8 3D
8D 48 E3 E2
08 BE 2B 2A
```

### MixColumn Class

The `MixColumn` class implements the column-mixing step in AES encryption and its inverse for decryption. It combines bytes in each column using Galois Field (GF(2^8)) arithmetic for strong diffusion.

- MixColumns transformation: Mixes each column of the state matrix using matrix multiplication in GF(2^8).
- Inverse MixColumns transformation: Reverses the mixing process.

#### How to use

```csharp
MixColumn mixColumn = new MixColumn();

// Apply MixColumns
mixColumn.ApplyMixColumns(state);

// Apply Inverse MixColumns
mixColumn.ApplyInverseMixColumns(state);

##### Example: 
- Before MixColumns

19 A0 9A E9
3D F4 C6 F8
E3 E2 8D 48
BE 2B 2A 08

- After MixColumns

D4 E0 B8 1E
BF B4 41 27
5D 52 11 98
30 AE F1 E5
```
### AddRoundKey Class

The `AddRoundKey` class implements the XOR operation between the AES state and a round key. This step integrates the encryption key into the state.
- AddRoundKey transformation: XORs the state with a round key.

#### How to use

```csharp
AddRoundKey addRoundKey = new AddRoundKey();

// Example AES state
byte[,] stateMatrix = {
    { 0x19, 0xA0, 0x9A, 0xE9 },
    { 0x3D, 0xF4, 0xC6, 0xF8 },
    { 0xE3, 0xE2, 0x8D, 0x48 },
    { 0xBE, 0x2B, 0x2A, 0x08 }
};

// Example round key
byte[,] roundKey = {
    { 0xA0, 0x88, 0x23, 0x2A },
    { 0xFA, 0x54, 0xA3, 0x6F },
    { 0xC3, 0x1B, 0x24, 0xF2 },
    { 0x6B, 0x6C, 0x75, 0x3E }
};

AESState state = new AESState(stateMatrix);

// Apply AddRoundKey
addRoundKey.ApplyAddRoundKey(state, roundKey);


##### Example: 
- Before AddRoundKey

19 A0 9A E9
3D F4 C6 F8
E3 E2 8D 48
BE 2B 2A 08

- After AddRoundKey

B9 28 B9 C3
C7 A0 65 97
20 F9 A9 BA
D5 47 5F 36
```
## Overview
This implementation breaks down the process into modular classes, providing clear and reusable components for each AES transformation.

### Example Workflow

**Input Data:**

- A 4x4 byte matrix (state).
- A round key matrix.

**Output Encryption:**

The state undergoes 10 rounds of transformations:
*AddRoundKey → SubBytes → ShiftRows → MixColumns → AddRoundKey.*
MixColumns is omitted in the last round.

**Output Decryption:**

Reverses the encryption steps in reverse order:
*AddRoundKey → InverseMixColumns → InverseShiftRows → InverseByteSub → AddRoundKey.*
MixColumns is omitted in the first round.

## Contributors

- **Rukije Morina** - *Contributor* 
- **Redon Osmanollaj** - *Contributor*  
- **Egzona Maloku** - *Contributor*  
---

**University of Prishtina "Hasan Prishtina"**  
Faculty of Electrical and Computer Engineering (FIEK)  
Course: Information Security  
**Supervisor:** Prof. Mërgim Hoti
