using System;
using System.Collections.Generic;

namespace UPCaToDecimalApp
{
    struct UPCa_Code
    {
        public const string LEFT_GUARD = "▍ ▍";
        public const string RIGHT_GUARD = "▍ ▍";
        public const string CENTER_GUARD = " ▍ ▍ ";
        public const int GROUP_LENGTH = 7;
        public const int NUM_GROUPS_LEFT = 6;
        public const int NUM_GROUPS_RIGHT = 6;
        // Dictionaries cant be generated at compile-time, but switch statements are.
        public static int LEFT_HAND(string group)
        {
            switch (group)
            {
                case "   ▍▍ ▍": return 0;
                case "  ▍▍  ▍": return 1;
                case "  ▍  ▍▍": return 2;
                case " ▍▍▍▍ ▍": return 3;
                case " ▍   ▍▍": return 4;
                case " ▍▍   ▍": return 5;
                case " ▍ ▍▍▍▍": return 6;
                case " ▍▍▍ ▍▍": return 7;
                case " ▍▍ ▍▍▍": return 8;
                case "   ▍ ▍▍": return 9;
                default: return -1; // Invalid group
            }
        }
        public static int RIGHT_HAND(string group)
        {
            switch (group)
            {
                case "▍▍▍  ▍ ": return 0;
                case "▍▍  ▍▍ ": return 1;
                case "▍▍ ▍▍  ": return 2;
                case "▍    ▍ ": return 3;
                case "▍ ▍▍▍  ": return 4;
                case "▍  ▍▍▍ ": return 5;
                case "▍ ▍    ": return 6;
                case "▍   ▍  ": return 7;
                case "▍  ▍   ": return 8;
                case "▍▍▍ ▍  ": return 9;
                default: return -1; // Invalid group
            }
        }
        // For this assignment separating the numberSystem and moduloCheck groups is unecessary,
        // but this is how I would build it in case I want to expand functionality in the future.
        public int numberSystem { get; private set; }
        public int[] left { get; private set; }
        public int[] right { get; private set; }
        public int moduloCheck { get; private set; }
        // Set left/right hand data from inputstring.
        // These are the intended way of setting the above values.
        public void SetLeftFromFullString(string leftString)
        {
            numberSystem = LEFT_HAND(leftString.Substring(0, GROUP_LENGTH));
            if (left == null || left.Length != 6) {
                left = new int[5];
            }
            for( int i = 1; i < NUM_GROUPS_LEFT; ++i) {
                left[i-1] = LEFT_HAND( leftString.Substring(i*GROUP_LENGTH, GROUP_LENGTH) );
            }
        }
        public void SetRightFromFullString(string rightString)
        {
            moduloCheck = RIGHT_HAND(rightString.Substring(5 * GROUP_LENGTH, GROUP_LENGTH));
            if(right == null || right.Length != 6) {
                right = new int[5];
            }
            for (int i = 0; i < NUM_GROUPS_RIGHT - 1; ++i)
            {
                right[i] = RIGHT_HAND(rightString.Substring(i*GROUP_LENGTH, GROUP_LENGTH));
            }
        }
        // ToString-like helpers
        public string LeftToString()
        {
            string res = numberSystem.ToString() + ' ';
            foreach (int i in left)
            {
                res += i.ToString();
            }
            return res;
        }
        public string RightToString()
        {
            string res = "";
            foreach (int i in right)
            {
                res += i.ToString();
            }
            res += ' ' + moduloCheck.ToString();
            return res;
        }
        // Override ToString so we can write UPCa_CodeInstance.ToString() and get the nicely formatted string we want out.
        public override string ToString()
        {
            return LeftToString() + ' ' + RightToString();
        }
        public string AsStringWithGuards()
        {
            return LEFT_GUARD + ' ' + LeftToString() + ' ' + CENTER_GUARD + ' ' + RightToString() + ' ' + RIGHT_GUARD;
        }
    }
}
