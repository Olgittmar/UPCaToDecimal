using System;
using System.Collections.Generic;

namespace UPCaToDecimalApp
{
    // Helper struct that moves a few static codes used to compile-time
    // and takes care of formatting the codes for us.
    public struct UPCa_Code
    {
        public const string LEFT_GUARD = "▍ ▍";
        public const string RIGHT_GUARD = "▍ ▍";
        public const string CENTER_GUARD = " ▍ ▍ ";
        public const int GROUP_LENGTH = 7;
        public const int NUM_GROUPS_LEFT = 6;
        public const int NUM_GROUPS_RIGHT = 6;
        // Dictionaries cant be generated at compile-time, but switch statements are.
        public static int LEFT_HAND(ReadOnlySpan<char> group)
        {
            switch (group.ToString())
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
        public static int RIGHT_HAND(ReadOnlySpan<char> group)
        {
            switch (group.ToString())
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
        public static string LeftHandToGroup(int num)
        {
            switch (num)
            {
                case 0 : return "   ▍▍ ▍";
                case 1 : return "  ▍▍  ▍";
                case 2 : return "  ▍  ▍▍";
                case 3 : return " ▍▍▍▍ ▍";
                case 4 : return " ▍   ▍▍";
                case 5 : return " ▍▍   ▍";
                case 6 : return " ▍ ▍▍▍▍";
                case 7 : return " ▍▍▍ ▍▍";
                case 8 : return " ▍▍ ▍▍▍";
                case 9 : return "   ▍ ▍▍";
                default: return ""; // Invalid num
            }
        }
        public static string RightHandToGroup(int num)
        {
            switch (num)
            {
                case 0 : return "▍▍▍  ▍ ";
                case 1 : return "▍▍  ▍▍ ";
                case 2 : return "▍▍ ▍▍  ";
                case 3 : return "▍    ▍ ";
                case 4 : return "▍ ▍▍▍  ";
                case 5 : return "▍  ▍▍▍ ";
                case 6 : return "▍ ▍    ";
                case 7 : return "▍   ▍  ";
                case 8 : return "▍  ▍   ";
                case 9 : return "▍▍▍ ▍  ";
                default: return "";
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
        public void SetLeftFromCodeString(ReadOnlySpan<char> leftString)
        {
            numberSystem = LEFT_HAND(leftString.Slice(0, GROUP_LENGTH));
            if (left == null || left.Length != NUM_GROUPS_LEFT - 1) {
                left = new int[NUM_GROUPS_LEFT - 1];
            }
            for( int i = 1; i < NUM_GROUPS_LEFT; ++i) {
                left[i-1] = LEFT_HAND(leftString.Slice(i*GROUP_LENGTH, GROUP_LENGTH) );
            }
        }
        public void SetRightFromCodeString(ReadOnlySpan<char> rightString)
        {
            moduloCheck = RIGHT_HAND(rightString.Slice((NUM_GROUPS_RIGHT-1)*GROUP_LENGTH, GROUP_LENGTH));
            if(right == null || right.Length != NUM_GROUPS_LEFT - 1) {
                right = new int[NUM_GROUPS_RIGHT - 1];
            }
            for (int i = 0; i < NUM_GROUPS_RIGHT - 1; ++i) {
                right[i] = RIGHT_HAND(rightString.Slice(i*GROUP_LENGTH, GROUP_LENGTH));
            }
        }
        // ToString-like helpers
        public string LeftToString()
        {
            return numberSystem.ToString() + " " + String.Join("", left);
        }
        public string RightToString()
        {
            return String.Join("", right) + " " + moduloCheck.ToString();
        }
        // Override ToString so we can write UPCa_CodeInstance.ToString() and get the nicely formatted string we want out.
        public override string ToString()
        {
            return LeftToString() + " " + RightToString();
        }
    }
}
