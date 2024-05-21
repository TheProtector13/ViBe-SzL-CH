using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Windows.Graphics.Printing.PrintSupport;

namespace ViBe_SzL_CH {
    internal class ViBe_Object : IDisposable, IVibeObject {
        private readonly ParallelOptions max_Thread_Count = new();
        private readonly System.Timers.Timer timer = new(5000);
        private readonly string? framenumCharacterFormat;
        private readonly byte vectordivider = (byte)Vector<short>.Count;
        protected VideoCapture capture;
        protected VideoWriter videoWriter;
        protected Stopwatch stopwatch = new();
        protected UInt64 whitepixel_count, allpixel_count;
        public bool Interrupt { get; protected set; } = false;
        public int Radius { get; set; } = 20;
        public int N { get; set; } = 20;
        public int Min_Cardinality { get; set; } = 3;
        public UInt64 Completed_frames { get; protected set; } = 0;
        public int Omega { get; set; } = 32;
        public float Reinit_Treshold { get; set; } = 0.8f;
        public byte Masking { get; set; } = 0; //0 only mask, 1 dont mask white, 2 dont mask black
        public UInt64 Capture_frame_count { get; protected set; }

        public ViBe_Object(string filepath, string savepath)
        {
            this.capture = new VideoCapture(filepath);
            this.videoWriter = new VideoWriter(savepath, VideoWriter.Fourcc('H', '2', '6', '4'), this.capture.Get(Emgu.CV.CvEnum.CapProp.Fps), new Size(this.capture.Width, this.capture.Height), true);
            this.max_Thread_Count.MaxDegreeOfParallelism = Environment.ProcessorCount;
            this.timer.Elapsed += Report;
            this.Capture_frame_count = (UInt64)capture.Get(Emgu.CV.CvEnum.CapProp.FrameCount);
            Console.CancelKeyPress += new(Interrupt_Handler);
            this.allpixel_count = (UInt64)capture.Width * (UInt64)capture.Height;
            this.framenumCharacterFormat = new String('0', this.Capture_frame_count == 0 ? 1 : (this.Capture_frame_count > 0 ? 1 : 2) + (int)Math.Log10(Math.Abs((double)this.Capture_frame_count)));
        }

        protected ViBe_Object(string savepath)
        {
            this.capture = new VideoCapture(0);
            this.videoWriter = new VideoWriter(savepath, VideoWriter.Fourcc('H', '2', '6', '4'), this.capture.Get(Emgu.CV.CvEnum.CapProp.Fps), new Size(this.capture.Width, this.capture.Height), true);
            this.max_Thread_Count.MaxDegreeOfParallelism = Environment.ProcessorCount;
            Console.CancelKeyPress += new(Interrupt_Handler);
            this.allpixel_count = (UInt64)capture.Width * (UInt64)capture.Height;
        }

        public virtual void Start()
        {
            List<Vector<short>[,,]> M;
            Image<Bgr, byte> frame = new(capture.Width, capture.Height);
            Image<Bgr, byte> newframe = new(capture.Width, capture.Height);
            capture.Set(Emgu.CV.CvEnum.CapProp.PosFrames, -1);
            capture.Read(frame);
            capture.Retrieve(newframe);
            List<Vector<short>[,,]> frame_buffer = new() {
                ConvertArray(frame.Data),
                ConvertArray(frame.Data)
            };
            byte[,,] frame_data;
            M = Init_VB(frame);
            double theta = Initialize_Theta(frame_buffer[0]);
            this.timer.Start();
            this.stopwatch.Start();
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            for (UInt64 FNO = 0; FNO < this.Capture_frame_count; FNO++) {
                whitepixel_count = 0;
                if (FNO == this.Capture_frame_count - 1) {
                    capture.Retrieve(newframe);
                }
                else {
                    capture.Read(newframe);
                }
                frame_buffer.Add(ConvertArray(newframe.Data));
                frame_data = frame.Data;
                ClassifyFrame(M, frame_buffer[1], frame_data);
                theta = ModelUpdate(M, frame_buffer[0], frame_buffer[1], frame_buffer[2], theta);
                if (theta == 0 || theta == 765) {
                    theta = 380;
                }
                frame_buffer.RemoveAt(0);
                frame = newframe;
                videoWriter.Write(frame);
                this.Completed_frames++;
                if (Interrupt) {
                    break;
                }
                if (whitepixel_count > allpixel_count * Reinit_Treshold) {
                    capture.Retrieve(newframe);
                    M = Init_VB(newframe);
                }
            }
            GCSettings.LatencyMode = GCLatencyMode.Interactive;
            frame.Dispose();
            this.stopwatch.Stop();
            this.timer.Stop();
            Console.WriteLine("Done!");
        }

        //M3 - a hattérmodellek frissítése
        protected double ModelUpdate(List<Vector<short>[,,]> ioM, Vector<short>[,,] frame_past, Vector<short>[,,] frame_current, Vector<short>[,,] frame_future, double theta)
        {
            int rows = frame_current.GetLength(0);
            int cols = frame_current.GetLength(1);
            Int64 foreground_sum = 0, background_sum = 0, foreground_count = 0, background_count = 0;
            object mutex = new();
            Parallel.For(0, rows, this.max_Thread_Count, (i) => {
                Span<short> tmp = stackalloc short[vectordivider];
                Int64 localforeground_sum = 0, localbackground_sum = 0, localforeground_count = 0, localbackground_count = 0;
                Span<Vector<short>[,,]> ioM_span = CollectionsMarshal.AsSpan(ioM);
                ref var ioM_search = ref MemoryMarshal.GetReference(ioM_span);
                for (int j = 0; j < cols; j++) {
                    // |future r+g+b - current r+g+b| * |current r+g+b - past r+g+b|
                    Vector<short> framedifferences = Vector.Abs(Vector.Subtract(frame_future[i, j, 0] + frame_future[i, j, 1] + frame_future[i, j, 2], frame_current[i, j, 0] + frame_current[i, j, 1] + frame_current[i, j, 2])) * Vector.Abs(Vector.Subtract(frame_current[i, j, 0] + frame_current[i, j, 1] + frame_current[i, j, 2], frame_past[i, j, 0] + frame_past[i, j, 1] + frame_past[i, j, 2]));
                    for (int x = 0; x < vectordivider; x++) {
                        if (framedifferences[x] >= theta) {
                            //van valtozas (foreground)
                            localforeground_count++;
                            localforeground_sum += frame_current[i, j, 0][x] + frame_current[i, j, 1][x] + frame_current[i, j, 2][x];
                        }
                        else {
                            //nincs valtozas (background)
                            if (Random.Shared.Next(1, this.Omega + 1) == 1) {
                                int randomsample = Random.Shared.Next(this.N);
                                for (int k = 0; k < 3; k++) {
                                    Unsafe.Add(ref ioM_search, randomsample)[i, j, k].CopyTo(tmp);
                                    tmp[x] = frame_current[i, j, k][x];
                                    Unsafe.Add(ref ioM_search, randomsample)[i, j, k] = new(tmp);
                                }
                            }
                            localbackground_count++;
                            localbackground_sum += frame_current[i, j, 0][x] + frame_current[i, j, 1][x] + frame_current[i, j, 2][x];
                        }
                    }
                }
                lock (mutex) {
                    foreground_sum += localforeground_sum;
                    background_sum += localbackground_sum;
                    foreground_count += localforeground_count;
                    background_count += localbackground_count;
                }
            });
            if (foreground_count == 0) return theta;
            if (background_count == 0) return theta;
            return ((foreground_sum / (double)foreground_count) + (background_sum / (double)background_count)) / 2.0;
        }

        protected void ClassifyFrame(List<Vector<short>[,,]> ioM_original, Vector<short>[,,] frame, byte[,,] output_reference_frame)
        {
            int rows = frame.GetLength(0);
            int cols = frame.GetLength(1);
            int lastcol = output_reference_frame.GetLength(1) - (cols - 1) * vectordivider;
            List<Vector<short>[,,]> ioM_clone = new(ioM_original);
            int randlin = Random.Shared.Next(rows - 1);
            int localradius = (int)(this.Radius * 4.5);
            object mutex = new();
            Parallel.For(0, rows, this.max_Thread_Count, (i) => {
                ulong localwhitepixel_count = 0;
                int[] current_matches = new int[vectordivider];
                List<Vector<short>[,,]> ioM;
                if (i != randlin) {
                    ioM = new(ioM_clone);
                }
                else {
                    ioM = ioM_original;
                }
                for (int j = 0; j < cols; j++) {
                    Array.Fill(current_matches, 0);
                    for (int k = 0; k < this.N - 1; k++) {
                        // |buffer k r+g+b - frame r+g+b| / 4.5
                        Vector<short> distances = Vector.Abs(Vector.Subtract(ioM[k][i, j, 0] + ioM[k][i, j, 1] + ioM[k][i, j, 2], frame[i, j, 0] + frame[i, j, 1] + frame[i, j, 2]));
                        for (int x = 0; x < vectordivider; x++) {
                            if (distances[x] <= localradius) {
                                current_matches[x]++;
                            }
                        }
                        if (k >= this.Min_Cardinality && FastCardMin(current_matches) >= this.Min_Cardinality) {
                            // akkor hattér, és befestem feketére
                            //M1
                            if (Masking != 2) {
                                for (int x = 0; x < ((j == cols - 1) ? lastcol : vectordivider); x++) {
                                    output_reference_frame[i, j * vectordivider + x, 0] = 0;
                                    output_reference_frame[i, j * vectordivider + x, 1] = 0;
                                    output_reference_frame[i, j * vectordivider + x, 2] = 0;
                                }
                            }
                            Vector<short>[,,] temp = ioM[k];
                            ioM.RemoveAt(k);
                            ioM.Insert(0, temp);
                            break;
                        }
                    }
                    if (FastCardMin(current_matches) < this.Min_Cardinality) {
                        //egyébként előtér, és fehér
                        //M2
                        for (int x = 0; x < ((j == cols - 1) ? lastcol : vectordivider); x++) {
                            if (current_matches[x] < this.Min_Cardinality) {
                                if (Masking != 1) {
                                    output_reference_frame[i, j * vectordivider + x, 0] = 255;
                                    output_reference_frame[i, j * vectordivider + x, 1] = 255;
                                    output_reference_frame[i, j * vectordivider + x, 2] = 255;
                                }
                                localwhitepixel_count++;
                            }
                            else {
                                if (Masking != 2) {
                                    output_reference_frame[i, j * vectordivider + x, 0] = 0;
                                    output_reference_frame[i, j * vectordivider + x, 1] = 0;
                                    output_reference_frame[i, j * vectordivider + x, 2] = 0;
                                }
                            }
                        }
                    }
                }
                lock (mutex) {
                    this.whitepixel_count += localwhitepixel_count;
                }
            });
        }

        //M0 - első frame inicializálása
        protected List<Vector<short>[,,]> Init_VB(Image<Bgr, byte> frame)
        {
            byte[][,,] ioM_tmp = new byte[this.N][,,];
            Parallel.For(0, this.N, this.max_Thread_Count, (i) => {
                ioM_tmp[i] = (byte[,,])frame.Data.Clone();
            });
            Vector<short>[][,,] tmp = new Vector<short>[this.N][,,];
            Parallel.For(0, this.N, this.max_Thread_Count, (m) => {
                for (int i = 0; i < frame.Rows; i++) {
                    for (int j = 0; j < frame.Cols; j++) {
                        for (int k = 0; k < 3; k++) {
                            int randomnum = Random.Shared.Next(this.Radius * 2) - this.Radius;
                            if (ioM_tmp[m][i, j, k] + randomnum >= 255) {
                                ioM_tmp[m][i, j, k] = 255;
                            }
                            else if (ioM_tmp[m][i, j, k] + randomnum <= 0) {
                                ioM_tmp[m][i, j, k] = 0;
                            }
                            else {
                                ioM_tmp[m][i, j, k] = Convert.ToByte(ioM_tmp[m][i, j, k] + randomnum);
                            }
                        }
                    }
                }
                tmp[m] = ConvertArray(ioM_tmp[m]);
            });
            return tmp.ToList<Vector<short>[,,]>();
        }

        protected double Initialize_Theta(Vector<short>[,,] frame)
        {
            int rows = frame.GetLength(0);
            int cols = frame.GetLength(1);
            int[] maxvals = new int[rows];
            int[] minvals = new int[rows];
            Parallel.For(0, rows, this.max_Thread_Count, (i) => {
                int min = 765;
                int max = 0;
                short[] values = new short[vectordivider];
                for (int j = 0; j < cols - 1; j++) {
                    Vector<short> tmp = frame[i, j, 0] + frame[i, j, 1] + frame[i, j, 2];
                    tmp.CopyTo(values);
                    if (values.Min() < min) {
                        min = values.Min();
                    }
                    if (values.Max() > max) {
                        max = values.Max();
                    }
                }
                maxvals[i] = max;
                minvals[i] = min;
            });
            return (maxvals.Max() + minvals.Min()) / 2.0;
        }

        protected void Report(object? sender, System.EventArgs e)
        {
            decimal fps = Math.Round((this.Completed_frames / (decimal)this.stopwatch.ElapsedMilliseconds * 1000), 2);
            int rtime, rsec;
            try {
                rtime = (int)((this.Capture_frame_count - this.Completed_frames) / fps);
                rsec = rtime - (rtime / 60) * 60;
                rtime /= 60;
            }
            catch (DivideByZeroException) {
                rtime = int.MaxValue;
                rsec = int.MaxValue;
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Frames processed: " + this.Completed_frames.ToString(framenumCharacterFormat) + "/" + this.Capture_frame_count + " | Avarage processing speed: " + fps + " fps | Remaining processing time: " + rtime + " minutes " + rsec.ToString("00") + " seconds");
            Console.ResetColor();
        }

        protected Vector<short>[,,] ConvertArray(byte[,,] frame)
        {
            int rows = frame.GetLength(0);
            int cols = frame.GetLength(1);
            int vectorcols = (int)Math.Ceiling(cols / (double)vectordivider);
            Vector<short>[,,] output = new Vector<short>[rows, vectorcols, 3];
            Parallel.For(0, rows, max_Thread_Count, (i) => {
                int y = 0;
                short[] bshort = new short[vectordivider], gshort = new short[vectordivider], rshort = new short[vectordivider];
                byte index;
                for (int j = 0; j < cols - vectordivider; j += vectordivider) {
                    Array.Fill<short>(bshort, 0);
                    Array.Fill<short>(gshort, 0);
                    Array.Fill<short>(rshort, 0);
                    index = 0;
                    for (int x = j; x < j + vectordivider; x++) {
                        bshort[index] = frame[i, x, 0];
                        gshort[index] = frame[i, x, 1];
                        rshort[index] = frame[i, x, 2];
                        index++;
                    }
                    output[i, y, 0] = new(bshort);
                    output[i, y, 1] = new(gshort);
                    output[i, y, 2] = new(rshort);
                    y++;
                }
                Array.Fill<short>(bshort, 0);
                Array.Fill<short>(gshort, 0);
                Array.Fill<short>(rshort, 0);
                index = 0;
                for (int x = cols / vectordivider * vectordivider; x < cols; x++) {
                    bshort[index] = frame[i, x, 0];
                    gshort[index] = frame[i, x, 1];
                    rshort[index] = frame[i, x, 2];
                    index++;
                }
                while (index < vectordivider) {
                    bshort[index] = 0;
                    gshort[index] = 0;
                    rshort[index] = 0;
                    index++;
                }
                output[i, y, 0] = new(bshort);
                output[i, y, 1] = new(gshort);
                output[i, y, 2] = new(rshort);
            });
            return output;
        }

        private int FastCardMin(int[] input)
        {
            int min = input[0];
            for (int i = 1; i < input.Length; i++) {
                if (min > input[i])
                    min = input[i];
                if (min < this.Min_Cardinality)
                    break;
            }
            return min;
        }

        private void Interrupt_Handler(object? sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            Console.WriteLine("The task was interrupted by the user!\nSaving the current progress, then exiting!!!");
            this.Interrupt = true;
        }

        public void ForceInterrupt()
        {
            this.Interrupt = true;
        }

        public void Dispose()
        {
            this.videoWriter.Dispose();
            this.capture.Dispose();
            this.timer.Close();
            this.timer.Dispose();
            this.stopwatch.Stop();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            GC.WaitForPendingFinalizers();
        }
    }
}
