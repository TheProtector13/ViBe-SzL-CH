using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test1.Cmd;

namespace Test1.Helpers {
    internal class Analizer {
        private readonly LanguageRules language_rules;

        public Analizer()
        {
            language_rules = new LanguageRules();
        }

        public void Analize_Then_Parse(List<Tokenizer.Node> nodes)
        {
            if (nodes.Count < 1) {
                return;
            }

            CorrectCommand(nodes);
            if (StatusFlag.command_status_ == StatusFlag.Command_Status.C_InvalidCmd)   // command is not valid
                States.current_Sys_S = States.System_State.ERROR_S;
            else if (StatusFlag.command_status_ == StatusFlag.Command_Status.C_CmdOK)   // command is valid
            {
                if (!(language_rules.Is_White_listed(nodes[0].TokenName))) {// if the current command doesn't need any parameter
                    Command_definition.Exec(nodes);
                }
                else                                                       // else it needs parameter
                {
                    CorrectParam(nodes);
                    // correct parameter | no parameter
                    if (StatusFlag.param_status_ == StatusFlag.Param_Status.P_ParamOk || StatusFlag.param_status_ == StatusFlag.Param_Status.P_NoParam) {
                        Command_definition.Exec(nodes);
                    }
                    // invalid parameter
                    else {
                        States.current_Sys_S = States.System_State.ERROR_S;
                    }
                }
            }
            else
                States.current_Sys_S = States.System_State.ERROR_S;
        }

        private void CorrectParam(List<Tokenizer.Node> nodes)
        {
            if (nodes.Count < 2) { //no parameters provided
                StatusFlag.param_status_ = StatusFlag.Param_Status.P_NoParam;
                Console.WriteLine("No param provided.");
                return;
            }

            bool param_matches = language_rules.ParamMatches(nodes);
            if (!param_matches) {
                StatusFlag.param_status_ = StatusFlag.Param_Status.P_InvalidParam;
                Console.WriteLine("Invalid param.");
                return;
            }

            StatusFlag.param_status_ = StatusFlag.Param_Status.P_ParamOk;
        }

        private void CorrectCommand(List<Tokenizer.Node> nodes)
        {
            if (nodes.Count < 1) //if no command provided, then set flag and exit
            {
                StatusFlag.command_status_ = StatusFlag.Command_Status.C_NoCmd;
                return;
            }

            bool command_matches = language_rules.CommandMatches(nodes[0].TokenName);
            if (!command_matches) //if the command is invalid
            {
                StatusFlag.command_status_ = StatusFlag.Command_Status.C_InvalidCmd;
                Console.WriteLine("Invalid command: " + nodes[0].TokenName);
                return;
            }
            StatusFlag.command_status_ = StatusFlag.Command_Status.C_CmdOK;
        }
    }
}
