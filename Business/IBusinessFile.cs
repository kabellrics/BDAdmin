using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Business
{
    public interface IBusinessFile
    {
        Task AnalyseFile(string path);
        Task AnalyseFolder(string path);
        byte[] ResizeFileImage(string filename,int ind=0);
        byte[] ResizeFileImageFromWeb(byte[] data, string tmpfile);
        IEnumerable<Page> ExtractPageInComics(String filepath);
    }
}