using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1.Helpers {
    public static class StatusFlag {
        public static Param_Status param_status_ = Param_Status.P_NoParam;
        public static Tokenizer_Status tokenizer_status_ = Tokenizer_Status.A_NotSliced;
        public static Command_Status command_status_ = Command_Status.C_NoCmd;

        public static void SetAll_ToDefault()
        {
            param_status_ = Param_Status.P_NoParam;
            tokenizer_status_ = Tokenizer_Status.A_NotSliced;
            command_status_ = Command_Status.C_NoCmd;
        }

        [Flags]
        public enum Tokenizer_Status {
            A_NotSliced,
            A_Sliced,
            A_OnlyWhiteSpc,
        }

        [Flags]
        public enum Command_Status {
            C_NoCmd,
            C_CmdOK,
            C_InvalidCmd,
        }

        [Flags]
        public enum Param_Status {
            P_NoParam,
            P_ParamOk,
            P_InvalidParam,
        }
    }
}
