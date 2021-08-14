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
            CLArgs = new string[4];
        }

        // Test to check that UPCa_Code is consistent when converting barcodes to ints and vice versa.
        // Since this test relies on the specified formatting it will also implicitly check that the output format is correct.
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
        // Test conversion from barcode to number representation of UPC-A codes.
        // Data is generated in TestData class below.
        [TestCaseSource(typeof(TestData), nameof(TestData.RandGenUPCACodes))]
        public string TestUPCaToDecimal(string UPCaCode)
        {
            return Convert_UPC_A_To_Decimal_String(UPCaCode);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.TestDataFiles))]
        public void TestUPCaToDecimalApp_ReadFromFile(string testDataFile)
        {
            string outFile = testDataFile
                .Replace("\\TestData\\InData\\", "\\TestData\\OutData\\")
                .Replace(".data", ".out");
            string expectedFile = testDataFile
                .Replace("\\TestData\\InData\\", "\\TestData\\Expected\\")
                .Replace(".data", ".expected");

            CLArgs[0] = testDataFile;
            CLArgs[1] = "-o";
            CLArgs[2] = outFile;
            CLArgs[3] = "--c"; // Must force console output to be able to hijack it.
            // Since the application outputs to console we need to hijack the output during the test.
            var oldOut = Console.Out;
            StringWriter listener = new StringWriter();
            Console.SetOut(listener);

            Program.Main(CLArgs);

            Console.SetOut(oldOut);
            
            // Note that this assertion will fail if file format includes BOM
            FileAssert.AreEqual(expectedFile, outFile);

            // Console.WriteLine adds an additional line terminator to the stream after each call.
            string actualResult = listener.ToString().TrimEnd();
            string expectedResult = File.ReadAllText(expectedFile);

            Assert.That(actualResult, Is.Not.Empty);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }

    public class TestData
    {
        private static string testDir;
        private static readonly string filter = "*.data";
        private static string[] testDataFiles;
        private static readonly UTF8Encoding utf8 = new UTF8Encoding(true, true);
        private static readonly int numGenCases = 100;

        // Randomly generated testData for TestUPCaToDecimal.
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
        // Attempts to check if the TestData folder exists in a local environment,
        // and returns the path to each file which ends in .data, in order found.
        public static IEnumerable TestDataFiles
        {
            get
            {
                // Relies on test exe being run in an environment where we can easily find TestData folder.
                //TODO: Should probably copy TestData folder to wherever we are building the test exe.
                testDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\TestData\InData\";
                testDataFiles = Directory.GetFiles(testDir, filter);
                foreach (string testDataFile in testDataFiles) {
                    yield return new TestCaseData(testDataFile);
                }
            }
        }

        // Helpers
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