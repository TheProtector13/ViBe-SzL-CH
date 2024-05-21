using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ViBe_SzL_CH {
    internal class Camera_ViBe_Object : ViBe_Object {
        private readonly string savepath;
        public Camera_ViBe_Object(string savepath) : base(savepath)
        {
            this.savepath = savepath;
        }

        public override void Start()
        {
            List<Vector<short>[,,]> M;
            Image<Bgr, byte> frame = new(capture.Width, capture.Height);
            capture.Read(frame);
            List<Vector<short>[,,]> buffer = new();
            byte[,,] frame_data;
            M = Init_VB(frame);
            double theta = Initialize_Theta(ConvertArray(frame.Data));
            for (int i = 0; i < 2; i++) {
                buffer.Add(ConvertArray(frame.Data));
            }
            this.stopwatch.Start();
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            while (capture.IsOpened) {
                whitepixel_count = 0;
                capture.Read(frame);
                CvInvoke.Imshow("video", frame);
                CvInvoke.WaitKey(1);
                buffer.Add(ConvertArray(frame.Data));
                frame_data = frame.Data;
                ClassifyFrame(M, buffer[1], frame_data);
                theta = ModelUpdate(M, buffer[0], buffer[1], buffer[2], theta);
                buffer.RemoveAt(0);
                CvInvoke.Imshow("image", frame);
                videoWriter.Write(frame);
                this.Completed_frames++;
                if (Interrupt) {
                    break;
                }
                if (whitepixel_count > allpixel_count * Reinit_Treshold) {
                    capture.Retrieve(frame);
                    M = Init_VB(frame);
                }
            }
            GCSettings.LatencyMode = GCLatencyMode.Interactive;
            decimal fps = Math.Round((this.Completed_frames / (decimal)this.stopwatch.ElapsedMilliseconds * 1000), 2);
            this.stopwatch.Stop();
            Console.WriteLine("Converting video fps to the measured value: " + fps);
            videoWriter.Dispose();
            VideoCapture tmpcapture = new(this.savepath);
            VideoWriter tmpwriter = new(Path.GetDirectoryName(savepath) + "\\tmp000.mkv", VideoWriter.Fourcc('H', '2', '6', '4'), (double)fps, new Size(this.capture.Width, this.capture.Height), true);
            UInt64 tmpframecount = (UInt64)tmpcapture.Get(Emgu.CV.CvEnum.CapProp.FrameCount);
            Image<Bgr, byte> tmpimage = new(tmpcapture.Width, tmpcapture.Height);
            for (UInt64 FNO = 0; FNO < tmpframecount; FNO++) {
                tmpcapture.Set(Emgu.CV.CvEnum.CapProp.PosFrames, FNO);
                tmpcapture.Read(tmpimage);
                tmpwriter.Write(tmpimage);
            }
            tmpimage.Dispose();
            tmpcapture.Dispose();
            tmpwriter.Dispose();
            frame.Dispose();
            File.Delete(savepath);
            File.Move(Path.GetDirectoryName(savepath) + "\\tmp000.mkv", savepath);
            Console.WriteLine("done!");
            this.Dispose();
        }
    }
}
