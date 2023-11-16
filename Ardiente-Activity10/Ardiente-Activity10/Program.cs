using System;
using System.Collections.Generic;
using System.Linq;

namespace ParityAndBCC
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Determine Parity of a bit set");
                Console.WriteLine("2. Find BCC of a block of character bits");
                Console.WriteLine("3. Check for errors in Block Character Bits and its BCC");
                Console.WriteLine("4. Find BCC with Parity");
                Console.WriteLine("Type 'quit' to exit");

                string userInput = Console.ReadLine();

                if (userInput.ToLower() == "quit")
                {
                    Console.WriteLine("Exiting the program...");
                    break;
                }

                switch (userInput)
                {
                    case "1":
                        string bitSet = GetBinaryInput("Enter a set of bits:");
                        string parityOption = GetParityOption();
                        Console.WriteLine("\nChecking if the bit set has the selected parity:");
                        Console.WriteLine($"Input bit set: {bitSet}");
                        string parityResult = GetSpecificParity(bitSet, parityOption);
                        Console.WriteLine($"Parity: {parityResult}");
                        break;

                    case "2":
                        string[] blockOfBits = GetBlockOfBits();
                        Console.WriteLine("\nFinding Block Character Check (BCC):");
                        string bcc = GetBCC(blockOfBits);
                        Console.WriteLine($"Block of Character Bits:");
                        foreach (var line in blockOfBits)
                        {
                            Console.WriteLine(line);
                        }
                        Console.WriteLine($"BCC: {bcc}");
                        break;

                    case "3":
                        string[] blockToCheck = GetBlockOfBits();
                        string bccToCheck = GetBinaryInput("Enter the BCC to check:");
                        bool hasError = CheckError(blockToCheck, bccToCheck);
                        Console.WriteLine($"Does the block of character bits have an error? {hasError}");
                        break;

                    case "4":
                        string blockForBCCWithParity = GetBinaryInput("\nEnter a block of character bits to find its BCC with parity:");
                        Console.WriteLine("\nFinding BCC through LRC, EVEN-set, ODD-set parity bits, and BCC parity:");
                        string bccWithParity = GetBCCWithParity(blockForBCCWithParity);
                        Console.WriteLine($"BCC with Parity: {bccWithParity}");
                        break;

                    default:
                        Console.WriteLine("Invalid option! Please choose a valid option or type 'quit' to exit.");
                        break;
                }
            }
        }

        static string GetBinaryInput(string prompt)
        {
            string input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
                if (!IsBinary(input))
                {
                    Console.WriteLine("Invalid input! Please enter binary data (0s and 1s) only.");
                }
            } while (!IsBinary(input));
            return input;
        }

        static bool IsBinary(string input)
        {
            return input.All(c => c == '0' || c == '1');
        }

        static string GetParityOption()
        {
            string option;
            do
            {
                Console.WriteLine("Choose Parity:");
                Console.WriteLine("1. EVEN");
                Console.WriteLine("2. ODD");
                option = Console.ReadLine();
            } while (option != "1" && option != "2");

            return option;
        }

        static string GetSpecificParity(string bitSet, string parityOption)
        {
            int count = bitSet.Count(bit => bit == '1');

            if ((count % 2 == 0 && parityOption == "1") || (count % 2 != 0 && parityOption == "2"))
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        static string[] GetBlockOfBits()
        {
            Console.WriteLine("Enter the number of lines in the block:");
            if (!int.TryParse(Console.ReadLine(), out int lines))
            {
                Console.WriteLine("Invalid input! Please enter a valid number.");
                return GetBlockOfBits();
            }

            Console.WriteLine("Enter the number of bits per line:");
            if (!int.TryParse(Console.ReadLine(), out int bits))
            {
                Console.WriteLine("Invalid input! Please enter a valid number.");
                return GetBlockOfBits();
            }

            string[] block = new string[lines];

            for (int i = 0; i < lines; i++)
            {
                block[i] = GetBinaryInput($"Enter line {i + 1} of {lines} (length: {bits}):");
                if (block[i].Length != bits)
                {
                    Console.WriteLine($"Invalid input! The line must have {bits} bits.");
                    i--;
                }
            }

            return block;
        }

        static string GetBCC(string[] blockOfBits)
        {
            if (blockOfBits.Length == 0)
            {
                Console.WriteLine("Block of bits is empty!");
                return string.Empty;
            }

            string bcc = blockOfBits[0];

            for (int i = 1; i < blockOfBits.Length; i++)
            {
                bcc = CalculateXOR(bcc, blockOfBits[i]);
            }

            return bcc;
        }

        static bool CheckError(string[] blockOfBits, string bcc)
        {
            string calculatedBCC = GetBCC(blockOfBits);
            return calculatedBCC != bcc;
        }

        static string GetBCCWithParity(string blockOfBits)
        {
            List<int> bits = blockOfBits.Select(bit => int.Parse(bit.ToString())).ToList();
            int bcc = bits.Aggregate((x, y) => x ^ y);

            string lrc = Convert.ToString(bcc, 2).PadLeft(blockOfBits.Length, '0');
            string evenParity = bcc % 2 == 0 ? "0" : "1";
            string oddParity = bcc % 2 == 0 ? "1" : "0";

            return $"{blockOfBits}{evenParity}{oddParity}{lrc}";
        }

        static string CalculateXOR(string str1, string str2)
        {
            if (str1.Length != str2.Length)
            {
                throw new InvalidOperationException("Strings must have equal lengths.");
            }

            char[] result = new char[str1.Length];
            for (int i = 0; i < str1.Length; i++)
            {
                result[i] = str1[i] == str2[i] ? '0' : '1';
            }

            return new string(result);
        }
    }
}
