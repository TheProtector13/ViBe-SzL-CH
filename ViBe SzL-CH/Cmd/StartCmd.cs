using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Test1.Helpers;

namespace Test1.Cmd {
    internal class StartCmd {
        private readonly string[] arg = Array.Empty<string>();
        private List<Tokenizer.Node> token_collection = new();
        public static bool IsGui { get; private set; } = false;
        private readonly Tokenizer tokenizer;
        private readonly Analizer analizer;

        public StartCmd(string[] arguments)
        {
            tokenizer = new();
            analizer = new();
            if (arguments.Length == 0) {
                Start_Gui();
            }
            else if (arguments[0] is "--console" or "-console") {
                List<string> tmp = arguments.ToList();
                tmp.RemoveAt(0);
                this.arg = tmp.ToArray();
                Start_Cmd();
            }
            else {
                List<string> tmp = arguments.ToList();
                tmp.Insert(0, "vibe");
                this.arg = tmp.ToArray();

                Run_Once();
            }


        }

        private void Run_Once()
        {
            if (token_collection.Count > 1) token_collection.Clear();

            Tokenizer.CreateTokens(tokenizer.TokenizeInput(this.arg));
            token_collection = Tokenizer.GetNodeList();
            TrimList(token_collection);
            analizer.Analize_Then_Parse(token_collection);
        }

        private static void TrimList(List<Tokenizer.Node> node_list)
        {
            int node_count = node_list.Count;
            for (int i = node_count - 1; i > (node_count / 2) - 1; i--) {
                node_list.RemoveAt(i);
            }
        }

        private static void Start_Gui()
        {
            Console.WriteLine("Starting gui++++");
            IsGui = true;
        }

        private void Start_Cmd()
        {

            bool run = true;
            while (run) {
                switch (States.current_Sys_S) {
                    case States.System_State.MAIN_S:
                        StatusFlag.SetAll_ToDefault();
                        Console.Write(LanguageRules.__Base_Path + " >> ");

                        if (token_collection.Count > 0) token_collection.Clear();

                        tokenizer.Tokenize(arg);

                        if (States.current_Sys_S == States.System_State.ERROR_S)
                            break;

                        token_collection = Tokenizer.GetNodeList();
                        analizer.Analize_Then_Parse(token_collection);

                        break;
                    case States.System_State.EXIT_S:
                        if (token_collection.Count > 0) token_collection.Clear();

                        run = false;
                        break;

                    case States.System_State.ERROR_S:
                        if (StatusFlag.command_status_ == StatusFlag.Command_Status.C_InvalidCmd)
                            Console.WriteLine("[ERROR_] Invalid command '" + token_collection[0].TokenName + "'!");

                        if (StatusFlag.param_status_ == StatusFlag.Param_Status.P_InvalidParam)
                            Console.WriteLine("[ERROR_] Invalid param found at index [" + LanguageRules.GetParamIndex() + "] for command '" + token_collection[0].TokenName + "'!");

                        if (StatusFlag.command_status_ == StatusFlag.Command_Status.C_NoCmd)
                            Console.WriteLine();

                        if (token_collection.Count > 0) token_collection.Clear();

                        States.current_Sys_S = States.System_State.MAIN_S;
                        break;
                }
            }
        }
    }
}
