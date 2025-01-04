/*This file is part of ViBe-SzL-CH.

ViBe-SzL-CH is free software: you can redistribute it and/or modify it under the terms 
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

ViBe-SzL-CH is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with ViBe-SzL-CH. 
If not, see <https://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViBe_SzL_CH {
    internal interface IVibeObject {
        public int Radius { get; set; }
        public int N { get; set; }
        public int Min_Cardinality { get; set; }
        public int Omega { get; set; }
        public float Reinit_Treshold { get; set; }
        public byte Masking { get; set; }
        public UInt64 Capture_frame_count { get; }
        public UInt64 Completed_frames { get; }
        public bool Interrupt { get; }
        public virtual void Start() { }
        public void ForceInterrupt() { }
        public void Dispose() { }
    }
}
