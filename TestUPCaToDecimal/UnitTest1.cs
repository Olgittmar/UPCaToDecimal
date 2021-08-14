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

        [Test]
        public void TestUPCaCodeToString()
        {
            UPCa_Code code = new();
            string left = "";
            string right = "";
            int[] leftNums = { 2, 4, 3, 6, 5, 8 };
            int[] rightNums = { 1, 3, 2, 5, 7, 9 };
            foreach ( int i in leftNums ) {
                left += UPCa_Code.LeftHandToGroup(i);
            }
            foreach (int i in rightNums) {
                right += UPCa_Code.RightHandToGroup(i);
            }
            // output should be "2 43658 13257 9"
            string expected = String.Join("", leftNums).Insert(1, " ");
            expected += " " + String.Join("", rightNums);
            expected = expected.Insert(expected.Length - 1, " ");

            code.SetLeftFromCodeString(left);
            code.SetRightFromCodeString(right);

            Assert.That(code.ToString(), Is.EqualTo(expected));
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
        private static UTF8Encoding utf8 = new UTF8Encoding(true, true);
        public static IEnumerable RandGenUPCACodes
        {
            get
            {
                int[] left, right;
                for (int i = 0; i < numGenCases; ++i)
                {
                    Random rng = new Random(i);
                    left = GenerateIntArray(rng, 6);
                    right = GenerateIntArray(rng, 6);
                    // Create input and expected output from rngs.
                    yield return new TestCaseData
                       (UPCa_Code.LEFT_GUARD +
                        IntArrayToLeftUPCBlock(left) +
                        UPCa_Code.CENTER_GUARD +
                        IntArrayToRightUPCBlock(right) +
                        UPCa_Code.RIGHT_GUARD)
                     .Returns
                       (IntArrayToFormattedLeftString( left ) + ' ' +
                        IntArrayToFormattedRightString( right ));
                }
            }
        }
        private static int[] GenerateIntArray(Random rng, int length)
        {
            int[] ret = new int[length];
            for(int i = 0; i < length; ++i) {
                ret[i] = rng.Next(0, 10);
            }
            return ret;
        }
        private static string IntArrayToLeftUPCBlock(int[] array)
        {
            string[] res = new string[array.Length];
            for(int i = 0; i < array.Length; ++i) {
                res[i] = UPCa_Code.LeftHandToGroup(array[i]);
            }
            return string.Join("", res);
        }
        private static string IntArrayToRightUPCBlock(int[] array)
        {
            string[] res = new string[array.Length];
            for(int i = 0; i < array.Length; ++i) {
                res[i] = UPCa_Code.RightHandToGroup(array[i]);
            }
            return string.Join("", res);
        }
        private static string IntArrayToFormattedLeftString(int[] array)
        {
            return String.Join("", array).Insert(1, " ");
        }
        private static string IntArrayToFormattedRightString(int[] array)
        {
            return String.Join("", array).Insert(array.Length - 1, " ");
        }
    }
}