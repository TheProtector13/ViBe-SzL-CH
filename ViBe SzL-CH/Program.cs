using System.Runtime;
using Test1.Cmd;

internal class Program {
    [STAThread]
    private static void Main()
    {
        Console.WriteLine("Hello, World!");

        //https://www.ipol.im/pub/art/2022/434/article_lr.pdf
        //https://stackoverflow.com/questions/50501176/emgucv-video-reading-in-c-sharp
        //https://stackoverflow.com/questions/20902323/get-specific-frames-using-emgucv
        //https://stackoverflow.com/questions/5101986/iterate-over-pixels-of-an-image-with-emgu-cv
        //https://emgu.com/wiki/files/3.0.0/document/html/3fb90645-ecc4-0c4e-b238-6d0ca38f4ebc.htm
        //https://stackoverflow.com/questions/23717880/c-sharp-multidimensional-bitarray
        //vector3 (microsoft oldalarol)
        //timernek a eventhandler (microsoft oldalarol)
        //interrupteventhandler (microsoft oldalarol)
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        ApplicationConfiguration.Initialize();
        List<string> tmp = Environment.GetCommandLineArgs().ToList();
        tmp.RemoveAt(0);
        StartCmd _ = new(tmp.ToArray());
        if (StartCmd.IsGui) {
            Application.Run(new Form1());
        }
    }
}