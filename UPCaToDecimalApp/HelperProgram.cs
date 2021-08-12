using System;
using System.Collections.Generic;
using System.Text;

namespace UPCaToDecimalApp
{
    class HelperProgram
    {   
        // We assume left to right here, to support bidirectional reading check direction before this method is called.
        public static string Convert_UPC_A_To_Decimal_String(string code)
        {
            if ( !code.StartsWith(UPCa_Code.LEFT_GUARD) ) {
                // No left guard
                return "[MissingLeftGuard]";
            }
            int currIndex = UPCa_Code.LEFT_GUARD.Length;
            string left = code.Substring(currIndex, UPCa_Code.NUM_GROUPS_LEFT * UPCa_Code.GROUP_LENGTH);
            currIndex += left.Length;
            if ( !code.Substring(currIndex).StartsWith(UPCa_Code.CENTER_GUARD) ) {
                // No center guard
                return "[MissingCenterGuard]";
            }
            currIndex += UPCa_Code.CENTER_GUARD.Length;
            string right = code.Substring(currIndex, UPCa_Code.NUM_GROUPS_LEFT * UPCa_Code.GROUP_LENGTH);
            currIndex += right.Length;
            if ( !code.Substring(currIndex).StartsWith(UPCa_Code.RIGHT_GUARD) ) {
                // No right guard
                return "[MissingRightGuard]";
            }

            UPCa_Code _code = new();
            _code.SetLeftFromFullString(left);
            _code.SetRightFromFullString(right);

            return _code.ToString();
        }
    }
}
