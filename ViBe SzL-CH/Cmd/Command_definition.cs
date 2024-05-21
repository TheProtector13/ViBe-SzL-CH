using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test1.Helpers;
using ViBe_SzL_CH;

namespace Test1.Cmd {
    internal class Command_definition {

        public static void Exec(List<Tokenizer.Node> nodes)
        {
            string command = nodes[0].TokenName;
            switch (command) {
                case "help":
                    Help(0);
                    break;
                case "exit":
                    Exit();
                    break;
                case "vibe":
                    Vibe(nodes);
                    break;
                case "test":
                    Test(nodes);
                    break;
                case "cls" or "clear":
                    Console.Clear();
                    break;
                default:
                    Help(0);
                    break;
            }
        }

        public static void Exec()
        {
            Vibe(Tokenizer.GetNodeList());
        }

        // -------------------------------------------------
        public static void VibeParams_SetAll_ToDefault()
        {
            M = 0; N = 0; Omega = 0; Min_cardinality = 0;
            Src = string.Empty; Dst = string.Empty;
        }

        public static int[] Mruler { get; } = { 19, 201 };
        public static int[] Nruler { get; } = { 4, 101 };
        public static int[] Omega_ruler { get; } = { 3, 129 };
        public static int[] Min_cardinality_ruler { get; } = { 1, 181 };
        public static float[] Reinit_treshold_ruler { get; } = { 0.19f, 0.96f };
        public static int[] Masking_ruler { get; } = { -1, 3 };

        private static int M { get; set; } = 0;
        private static int N { get; set; } = 0;
        private static int Omega { get; set; } = 0;
        private static int Min_cardinality { get; set; } = 0;
        private static float Reinit_treshold { get; set; } = 0;
        private static int Masking { get; set; } = 0;
        private static string Src { get; set; } = string.Empty;
        private static string Dst { get; set; } = string.Empty;
        // -------------------------------------------------



        // --------------------------------------- Command_definitions ---------------------------------------
        private static void Test(List<Tokenizer.Node> nodes)
        {
            int input_lngth = nodes.Count;
            Console.WriteLine("Length is: " + input_lngth);

            for (int i = 0; i < input_lngth; i++) {
                Console.WriteLine(nodes[i].TokenName + " | " + nodes[i].TokenType + " | " + nodes[i].Path);
            }
        }


        private static void Help(byte mode)
        {
            switch (mode) {
                case 0: // the global help function
                    Console.WriteLine("\nThe interpreter only knows these commands.\n" +
                                      "*Keyword          -------------------------------------          Description*\n" +
                                      "  help                             ---                     all commands and their description\n" +
                                      "  exit                             ---                     exits from the program\n" +
                                      "  gui                              ---                     opens the program gui\n" +
                                      " cls | clear                       ---                     clears the console\n" +
                                      "  vibe                             ---                     start the algorithm with the specified parameters\n" +
                                      "                                                            use suffix 'help' or flag '-h' to list all the parameters\n" +
                                      "                                                                    (no params = deafult param values)\n" +
                                      "  cd                               ---                     changes the file pointer to the specified file if it exists\n" +
                                      "                                                                    (no param = returns the current pointer value)\n");
                    break;
                case 1: // vibe help function
                    Console.WriteLine("\nThe ViBe accepts only these parameters.\n" +
                                      "*Parameter         ---------------------------------         Description*\n" +
                                      " -m | --m                               ---                    desc...\n" +
                                      " -n | --n                               ---                    desc...\n" +
                                      " -omega | --omega                       ---                    desc...\n" +
                                      " -min_cardinality | --min_cardinality   ---                    desc...\n" +
                                      " -src | --src                           ---                   sets the source (if not provided uses the camera)\n" +
                                      " -dst | --dst                           ---                   sets the destination\n");
                    break;
            }
        }

        private static void Exit()
        {
            Console.WriteLine();
            while (true) {
                Console.Write("Are you sure you want to exit? (y/Y | n/N) ");
                string? user_input = Console.ReadLine();

                if (user_input != null) { user_input = user_input.Trim(' '); }

                if (user_input == "n" || user_input == "N") {
                    Console.WriteLine();
                    return;
                }
                else if (user_input == "y" || user_input == "Y") {
                    States.current_Sys_S = States.System_State.EXIT_S;
                    return;
                }

                Console.WriteLine();
            }
        }

        private static void Vibe(List<Tokenizer.Node> parameters)
        {
            Console.WriteLine("[VIBE-DEBUG] executing vibe...");
            int nodes_length = parameters.Count, dst_count = 0;
            VibeParams_SetAll_ToDefault(); // set the parameters to default

            for (int i = 1; i < nodes_length; i++) {
                if ((parameters[i].TokenName == "help" || parameters[i].TokenName == "h") && parameters[i].TokenType == Tokenizer.TokenType.TT_FLAG) // if the current flag is the 'help' flag
                {
                    Help(1); return;                // print the help, then exit from the ViBe context
                }
                else if (parameters[i].TokenType == Tokenizer.TokenType.TT_FLAG) {
                    Tokenizer.Node current_node = parameters[i];
                    string current_flag = current_node.TokenName;

                    if (i < nodes_length - 1) {
                        i++;
                        if (parameters[i].TokenType == Tokenizer.TokenType.TT_NUMERIC)
                            switch (current_flag) {
                                case "m":
                                    M = parameters[i].Value;
                                    Console.WriteLine("M value set: " + M);
                                    break;
                                case "n":
                                    N = parameters[i].Value;
                                    Console.WriteLine("N value set: " + N);
                                    break;
                                case "omega":
                                    Omega = parameters[i].Value;
                                    Console.WriteLine("Omega value set: " + Omega);
                                    break;
                                case "min_cardinality":
                                    Min_cardinality = parameters[i].Value;
                                    Console.WriteLine("Min_card value set: " + Min_cardinality);
                                    break;
                                case "reinit_treshold":
                                    Reinit_treshold = parameters[i].Value;
                                    Console.WriteLine("Reinit_treshold value set: " + Reinit_treshold);
                                    break;
                                case "masking":
                                    Masking = parameters[i].Value;
                                    switch (Masking) {
                                        case 0:
                                            Console.WriteLine("Masking value set: Only Mask");
                                            break;
                                        case 1:
                                            Console.WriteLine("Masking value set: Only Mask Background");
                                            break;
                                        case 2:
                                            Console.WriteLine("Masking value set: Only Mask Foreground");
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    Console.WriteLine("[ERROR_ViBe] Wrong type of argument provided for '" + current_flag + "'! ~[int X string]~ collision.");
                                    return;
                            }
                        else if (parameters[i].TokenType == Tokenizer.TokenType.TT_STRINGLIT) {
                            if (current_flag == "dst") {
                                Dst = parameters[i].Path;
                                Console.WriteLine("Dst set: " + Dst);
                                dst_count++;
                            }
                            else if (current_flag == "src") {
                                if (parameters[i].Path == string.Empty)
                                    Console.WriteLine("[ViBe_DEBUG] opening camera...");
                                else
                                    Src = parameters[i].Path;
                            }
                            else {
                                Console.WriteLine("[ERROR_ViBe] Wrong type of argument provided for '" + current_flag + "'! ~[string X int]~ collision.");
                                return;
                            }
                        }
                        else {
                            Console.WriteLine("[ERROR_ViBe] No value provided for parameter '" + current_flag + "'!");
                            return;
                        }
                    }
                    else {
                        Console.WriteLine("[ERROR_ViBe] No value provided for parameter '" + current_flag + "'!");
                        return;
                    }

                }
            }

            /* Additional error handling */
            // NO Dst
            if (dst_count < 1) {
                Console.WriteLine("[ERROR_ViBe] No destination provided, try again and specify a destination!\n If you need additional help use the suffix 'help' or '-h'.\n");
                return;
            }
            // -----------------------------------------------


            /* Additional smooting */
            // search for '/' or '\' token in the source and in the dst, then update them accordingly

            if (Src != string.Empty) {
                if (!(Src.Contains('/') || Src.Contains('\\')))
                    Src = LanguageRules.__Base_Path + '\\' + Src;

                if (!LanguageRules.File_exists(Src)) // Source does not exists
                {
                    Console.WriteLine("[ERROR_ViBe] Source \"" + Src + "\" does not exists");
                    return;
                }
            }

            if (!(Dst.Contains('/') || Dst.Contains('\\')))
                Dst = LanguageRules.__Base_Path + '\\' + Dst;
            // -----------------------------------------------

            // start vibe algorithm here (call the function which starts it...)
            // Vibe_start(M, N, Omega, Min_cardinality);
            if (Src == string.Empty) {
                Camera_ViBe_Object vibe = new(Dst);
                if (M != 0 && M > Mruler[0] && M < Mruler[1]) {
                    vibe.N = M;
                }
                if (N != 0 && N > Nruler[0] && N < Nruler[1]) {
                    vibe.Radius = N;
                }
                if (Omega != 0 && Omega > Omega_ruler[0] && Omega < Omega_ruler[1]) {
                    vibe.Omega = Omega;
                }
                if (Min_cardinality != 0 && Min_cardinality > Min_cardinality_ruler[0] && Min_cardinality < M && Min_cardinality < Min_cardinality_ruler[1]) {
                    vibe.Min_Cardinality = Min_cardinality;
                }
                if (Reinit_treshold != 0 && Reinit_treshold > Reinit_treshold_ruler[0] && Reinit_treshold < Reinit_treshold_ruler[1]) {
                    vibe.Reinit_Treshold = Reinit_treshold;
                }
                if (Masking > Masking_ruler[0] && Masking < Masking_ruler[1]) {
                    vibe.Masking = (byte)Masking;
                }
                Console.WriteLine("[VIBE- DEBUG]Vibe function executing here... || M = " + vibe.N + " | N = " + vibe.Radius + " | omega = " + vibe.Omega + " | min_cardinality = " + vibe.Min_Cardinality);
                vibe.Start();
                vibe.Dispose();
                return;
            }
            else {
                ViBe_Object vibe = new(Src, Dst);
                if (M != 0 && M > Mruler[0] && M < Mruler[1]) {
                    vibe.N = M;
                }
                if (N != 0 && N > Nruler[0] && N < Nruler[1]) {
                    vibe.Radius = N;
                }
                if (Omega != 0 && Omega > Omega_ruler[0] && Omega < Omega_ruler[1]) {
                    vibe.Omega = Omega;
                }
                if (Min_cardinality != 0 && Min_cardinality > Min_cardinality_ruler[0] && Min_cardinality < M && Min_cardinality < Min_cardinality_ruler[1]) {
                    vibe.Min_Cardinality = Min_cardinality;
                }
                if (Reinit_treshold != 0 && Reinit_treshold > Reinit_treshold_ruler[0] && Reinit_treshold < Reinit_treshold_ruler[1]) {
                    vibe.Reinit_Treshold = Reinit_treshold;
                }
                if (Masking > Masking_ruler[0] && Masking < Masking_ruler[1]) {
                    vibe.Masking = (byte)Masking;
                }
                Console.WriteLine("[VIBE- DEBUG]Vibe function executing here... || M = " + vibe.N + " | N = " + vibe.Radius + " | omega = " + vibe.Omega + " | min_cardinality = " + vibe.Min_Cardinality + " | Reinit_Treshold = " + vibe.Reinit_Treshold + " | Masking = " + vibe.Masking);
                vibe.Start();
                vibe.Dispose();
            }
        }

        // --------------------------------------------------------------------------------------------------
    }
}
