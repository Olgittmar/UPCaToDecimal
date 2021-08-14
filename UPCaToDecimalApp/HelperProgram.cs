using System;
using System.Collections.Generic;
using System.Text;

namespace UPCaToDecimalApp
{
    public class HelperProgram
    {
        // We assume left to right here, to support bidirectional reading check direction before this method is called.
        public static string Convert_UPC_A_To_Decimal_String(ReadOnlySpan<char> code)
        {
            // We are allowed to assume that the codes should be valid, but it's so easy to check so we might as well.
            if ( !code.StartsWith(UPCa_Code.LEFT_GUARD) ) {
                // No left guard
                // Due to the formulation of my test, entering any of the
                // missing guard statements will also fail  the test.
                return "[MissingLeftGuard]";
            }
            else if ( !code.Slice(UPCa_Code.LEFT_GUARD.Length
                           + UPCa_Code.NUM_GROUPS_LEFT*UPCa_Code.GROUP_LENGTH)
                    .StartsWith(UPCa_Code.CENTER_GUARD) ) {
                // No center guard
                return "[MissingCenterGuard]";
            }
            else if ( !code.EndsWith(UPCa_Code.RIGHT_GUARD) ) {
                // No right guard
                return "[MissingRightGuard]";
            }
            // Not to be confused with the string code input.
            UPCa_Code _code = new();
            // From end of left-guard and num_group_left code groups forward is the numbersystem and left code
            ReadOnlySpan<char> left = code.Slice(
                UPCa_Code.LEFT_GUARD.Length,
                UPCa_Code.NUM_GROUPS_LEFT * UPCa_Code.GROUP_LENGTH);
            // from end of left + centerguard and num_group_right code groups forward is the right code followed by modulocheck.
            ReadOnlySpan<char> right = code.Slice(
                UPCa_Code.LEFT_GUARD.Length + left.Length + UPCa_Code.CENTER_GUARD.Length,
                UPCa_Code.NUM_GROUPS_LEFT * UPCa_Code.GROUP_LENGTH);

            // Set the number values from extracted strings.
            _code.SetLeftFromCodeString(left);
            _code.SetRightFromCodeString(right);

            return _code.ToString();
        }
    }
}
