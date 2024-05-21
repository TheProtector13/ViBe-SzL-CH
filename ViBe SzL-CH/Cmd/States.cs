using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1.Cmd {
    static class States {
        public static System_State current_Sys_S = System_State.MAIN_S;

        public enum System_State {
            MAIN_S,
            EXIT_S,
            ERROR_S,
        }
    }
}
