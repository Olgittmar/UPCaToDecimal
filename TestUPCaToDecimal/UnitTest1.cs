using NUnit.Framework;
using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Collections;
using UPCaToDecimalApp;
using static UPCaToDecimalApp.HelperProgram;

namespace TestUPCaToDecimal
{
    [TestFixture]
    public class Tests
    {
        string[] CLArgs;

        [SetUp]
        public void Setup()
        {
            CLArgs = new string[1];
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.RandGenUPCACodes))]
        public string TestUPCaToDecimal(string UPCaCode)
        {
            return HelperProgram.Convert_UPC_A_To_Decimal_String(UPCaCode);
        }
    }
    public class TestData
    {
        private static string testDir;
        private const string filter = "*.data";
        private static string[] testDataFiles;
        private static int numGenCases = 100;
        public static IEnumerable RandGenUPCACodes
        {
            get
            {
                Random rng = new Random();
                int[] left, right;
                for (int i = 0; i < numGenCases; ++i)
                {
                    left = GenerateIntArray(rng, 6);
                    right = GenerateIntArray(rng, 6);
                    // Create input and expected output from rngs.
                    yield return new TestCaseData(
                        UPCa_Code.LEFT_GUARD +
                        IntArrayToLeftUPCBlock(left) +
                        UPCa_Code.CENTER_GUARD +
                        IntArrayToRightUPCBlock(right) +
                        UPCa_Code.RIGHT_GUARD
                            ).Returns(
                        IntArrayToFormattedLeftString( left ) + ' ' +
                        IntArrayToFormattedRightString( right ));
                }
            }
        }
        private static int[] GenerateIntArray(Random rng, int length)
        {
            int[] ret = new int[length];
            for(int i = 0; i < length; ++i) {
                ret[i] = rng.Next();
            }
            return ret;
        }
        private static string IntArrayToLeftUPCBlock(int[] array)
        {
            string[] res = new string[array.Length];
            for(int i = 0; i < array.Length; ++i) {
                res[i] = UPCa_Code.LeftHandToGroup(i);
            }
            return string.Join("", res);
        }
        private static string IntArrayToRightUPCBlock(int[] array)
        {
            string[] res = new string[array.Length];
            for (int i = 0; i < array.Length; ++i) {
                res[i] = UPCa_Code.RightHandToGroup(i);
            }
            return string.Join("", res);
        }
        private static string IntArrayToFormattedLeftString(int[] array)
        {
            string res = "";
            res += array[0].ToString() + ' ';
            for(int i = 1; i < array.Length; ++i) {
                res += array[i].ToString();
            }
            return res;
        }
        private static string IntArrayToFormattedRightString(int[] array)
        {
            string res = "";
            for(int i = 0; i < array.Length - 1; ++i) {
                res += array[i].ToString();
            }
            res += ' ' + array[^1].ToString();
            return res;
        }
    }
}