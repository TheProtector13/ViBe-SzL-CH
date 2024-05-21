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
