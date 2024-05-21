using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test1.Cmd;

namespace Test1.Helpers {
    internal class Tokenizer {
        private string? input_string;
        private static List<Node> NodeCollection = new();


        /* NODE HANDLING */
        // this enum will tell the current tokens type, rework is decidable...
        public enum TokenType {
            TT_KEYWORD,
            TT_FLAG,
            TT_NUMERIC,
            TT_STRINGLIT,
        }

        public class Node {
            public string TokenName { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
            public dynamic Value { get; set; } = 0;
            public TokenType TokenType { get; set; }
        }

        public static List<Node> GetNodeList() { return NodeCollection; }

        public static void CreateTokens(string[] input)
        {
            for (int i = 0; i < input.Length; i++) {
                Node node = new();
                int numeric_valCount = 0, path_count = 0;

                //if it is a flag type, then trim the "--" characters
                if (i == 0) {                                                                   // the first word is always the keyword (piping not implemented!!)
                    node.TokenType = TokenType.TT_KEYWORD;
                    node.TokenName = input[i];
                }
                else if (input[i].StartsWith('-')) {                                                                   // if the string is a flag
                    string formatted_word = input[i].Trim('-');
                    node.TokenName = formatted_word;
                    node.TokenType = TokenType.TT_FLAG;
                }
                else if (int.TryParse(input[i], out int current_val)) {                                                                   // if the string is a number
                    numeric_valCount++;
                    node.TokenName = "value_" + numeric_valCount.ToString();
                    node.Value = current_val;
                    node.TokenType = TokenType.TT_NUMERIC;
                }
                else if (float.TryParse(input[i], out float current_floatval)) {                                                                   // if the string is a number
                    numeric_valCount++;
                    node.TokenName = "value_" + numeric_valCount.ToString();
                    node.Value = current_floatval;
                    node.TokenType = TokenType.TT_NUMERIC;
                }
                else                                                                // if it's a string literal (path)
                {
                    path_count++;
                    node.TokenName = "path_" + path_count.ToString();
                    node.Path = input[i].Trim('\"', '\'');
                    node.TokenType = TokenType.TT_STRINGLIT;
                }

                NodeCollection.Add(node);
            }
        }

        /* TOKENIZER */
        public Tokenizer()
        {
            NodeCollection = new();
        }

        public void Tokenize(string[] output_array)
        {
            output_array = TokenizeInput(TakeInput());

            if (output_array != null) {
                CreateTokens(output_array);
            }
            else {
                States.current_Sys_S = States.System_State.ERROR_S;
            }
        }

        private static string[] TokenizeInput(string input_stream)
        {
            return input_stream.Split(' ');
        }

        public string[] TokenizeInput(string[] args_array)
        {
            string[] output_array = (string[])args_array.Clone();
            if (args_array.Length == 0) {
                string input_stream = TakeInput();
                output_array = input_stream.Split(' ');
            }

            CreateTokens(output_array);
            return output_array;
        }

#pragma warning disable
        /* Small functions */
        private string TakeInput()
        {
            input_string = Console.ReadLine();
            return input_string;
        }
#pragma warning restore

    }
}
