using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test1.Cmd;

namespace Test1.Helpers {
    internal class LanguageRules {
        private readonly int param_count = 10, command_count = 6, white_list_lngth = 1;
        private readonly string[] param, command, white_list;
        public static int param_index = 1;

        // get the base path (the desktop folder)
        public static string __Base_Path = Environment.CurrentDirectory;

        public LanguageRules()
        {
            // kell Enviroment.CurrentDirectory, a parancs alapbol az .exe file lesz, parameterei allitsak a vibe parametereit (omega, M....)
            param = new string[param_count];
            command = new string[command_count];
            white_list = new string[white_list_lngth];

            // parameters for the 'vibe' command
            param[0] = "src";
            param[1] = "dst";
            param[2] = "m";
            param[3] = "n";
            param[4] = "omega";
            param[5] = "min_cardinality";
            param[6] = "help";
            param[7] = "h";
            param[8] = "reinit_treshold";
            param[9] = "masking";

            // list of all the commands which the system accepts
            command[0] = "help";
            command[1] = "exit";
            command[2] = "cls";
            command[3] = "clear";
            command[4] = "vibe";
            command[5] = "test";

            // white list for commands which accepts parameters
            white_list[0] = "vibe";
        }

        /// <summary>
        /// Decides if the provided file or folder exists
        /// RETURN {F_NoFile}: the input {file_path} is NULL or an empty string
        /// RETURN {F_FileOk}: the file|folder exists
        /// </summary>
        /// <param name="file_path"></param>
        /// <returns></returns>
        public static bool File_exists(string? file_path)
        {
            if (string.IsNullOrEmpty(file_path)) //if the string is null or empty
            {
                States.current_Sys_S = States.System_State.ERROR_S;
                return false;
            }

            bool exists;

            exists = File.Exists(file_path);

            if (exists) return true;

            States.current_Sys_S = States.System_State.ERROR_S;
            return false;
        }

        /// <summary>
        /// Searches the parameters and decides if they exists
        /// RETURN true: all of the provided parameter exists
        /// RETURN false: one of the provided parameter is not correct
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public bool ParamMatches(List<Tokenizer.Node> nodes)
        {
            param_index = 1; //keep track of the index of the parameter in the input for better error handling
            for (int i = 1; i < nodes.Count; i++) //current node (parameter) in the input
            {
                for (int j = 0; j < param_count; j++) //current parameter in the language rules
                {
                    //if it is a match
                    if (nodes[i].TokenName == param[j]) break;

                    //if param not exists, return false
                    if (j == (param_count - 1) && (nodes[i].TokenType is Tokenizer.TokenType.TT_FLAG)) {
                        Console.WriteLine("[PM] Invalid param: " + nodes[i].TokenName + " | At index: " + param_index);
                        return false;
                    }
                }
                param_index++;
            }

            return true;
        }

        public bool ParamMatches(string[] nodes)
        {
            for (int i = 1; i < nodes.Length; i++) //current node (parameter) in the input
            {
                for (int j = 0; j < param_count; j++) //current parameter in the language rules
                {
                    //if it is a match
                    if (nodes[i] == param[j]) break;

                    //if param not exists, return false
                    if (j == (param_count - 1)) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Searches the provided command and decide if it exists
        /// RETURN true: the provided command exists
        /// RETURN false: the provided command is not correct
        /// </summary>
        /// <param name="current_command"></param>
        /// <returns></returns>
        public bool CommandMatches(string? current_command)
        {
            for (int i = 0; i < command_count; i++)
                if (command[i] == current_command)
                    return true;

            return false;
        }

        /// <summary>
        /// Searches the provided command on the white list (the white listed commands accepts parameters)
        /// RETURN true: is on the white list
        /// RETURN false: not on the white list
        /// </summary>
        /// <param name="current_command"></param>
        /// <returns></returns>
        public bool Is_White_listed(string? current_command)
        {
            if (string.IsNullOrEmpty(current_command)) return false;

            for (int i = 0; i < white_list_lngth; i++)
                if (white_list[i] == current_command)
                    return true;

            return false;
        }

        /* Returns the index of the current parameter */
        public static int GetParamIndex() { return param_index; }
    }

}
