/*This file is part of ViBe-SzL-CH.

ViBe-SzL-CH is free software: you can redistribute it and/or modify it under the terms 
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

ViBe-SzL-CH is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with ViBe-SzL-CH. 
If not, see <https://www.gnu.org/licenses/>.*/

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
