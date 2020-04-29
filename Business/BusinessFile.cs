using Common;
using DBConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpCompress;
using System.Threading.Tasks;
using SharpCompress.Archives.Zip;
using System.Linq;
using SharpCompress.Archives.Rar;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SharpCompress.Common;
using SharpCompress.Archives;
using GalaSoft.MvvmLight.Messaging;

namespace Business
{
    public class BusinessFile : IBusinessFile
    {
        private BusinessFichier businessFichier;
        public BusinessFile()
        {
            businessFichier = new BusinessFichier();
        }
        public async Task AnalyseFile(string path)
        {
            try
            {
                if (Path.GetExtension(path).ToLower() == "cbr" || Path.GetExtension(path).ToLower() == "cbz")
                {
                    Fichier newfile = new Fichier();
                    newfile.Name = Path.GetFileNameWithoutExtension(path);
                    newfile.Image = GetCoverStream(path);
                    await businessFichier.Create(newfile);
                }
                //else
                //{
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private byte[] GetCoverStream(string path)
        {
            if (Path.GetExtension(path).ToLower() == "cbr")
            {
                return GetCoverFromCBR(path);
            }
            else if (Path.GetExtension(path).ToLower() == "cbz")
            {
                return GetCoverFromCBZ(path);
            }
            else
            {
                return null;
            }
        }
        private byte[] GetCoverFromCBZ(string path)
        {
            try
            {
                ZipArchive zipArchive = ZipArchive.Open(path);
                var entry = zipArchive.Entries.First(ent => !ent.IsDirectory);
                using (MemoryStream ms = new MemoryStream())
                {
                    entry.OpenEntryStream().CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private byte[] GetCoverFromCBR(string path)
        {
            try
            {
                RarArchive rarArchive = RarArchive.Open(path);
                var entry = rarArchive.Entries.First(ent => !ent.IsDirectory);

                using (MemoryStream ms = new MemoryStream())
                {
                    entry.OpenEntryStream().CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AnalyseFolder(string path)
        {

            try
            {
                foreach (string p in Directory.GetDirectories(path))
                {
                    if (File.Exists(p))
                    {
                        await AnalyseFile(p);
                    }
                    else if (Directory.Exists(p))
                    {
                        await AnalyseFolder(p);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public byte[] ResizeFileImageFromWeb(byte[] data,string tmpfile)
        {

            var tempfile = Path.GetDirectoryName(tmpfile);
            tempfile = Path.Combine(tempfile, "tmpcover.jpg");
            using (MemoryStream memory = new MemoryStream(data))
            {
                System.Drawing.Image Image = System.Drawing.Image.FromStream(memory);
                Image.Save(tempfile,ImageFormat.Jpeg);
            }
            return ResizeFileImage(tempfile);
        }
        public byte[] ResizeFileImage(string filename,int ind=0)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(filename);
            int width = (int)(img.Width * 0.90);
            int height = (int)(img.Height * 0.90);
            img.Dispose();
            FileInfo info = new FileInfo(filename);
            long length = info.Length;
            while (length > 800000)
            {
                ResizingImage(filename, width, height);
                info = new FileInfo(filename);
                length = info.Length;
                width = (int)(width * 0.90);
                height = (int)(height * 0.90);
            }
            var byteImg = File.ReadAllBytes(filename);
            Messenger.Default.Send(new ExtractFileNotificationMessage($"Redimensionnage de la page {ind}", byteImg));
            return byteImg;
        }
        public void ResizingImage(string fileName, int width, int height)
        {
            try
            {
                System.Drawing.Image Image = System.Drawing.Image.FromFile(fileName);
                using (System.Drawing.Image newImage = new Bitmap(Image, width, height))
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        var tempfile = Path.GetDirectoryName(fileName);
                        tempfile = Path.Combine(tempfile, "temp.jpg");
                        using (FileStream fs = new FileStream(tempfile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            newImage.Save(memory, Image.RawFormat);
                            byte[] bytes = memory.ToArray();
                            fs.Write(bytes, 0, bytes.Length);
                        }
                        Image.Dispose();
                        File.Delete(fileName);
                        File.Move(tempfile, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<Page> ExtractPageInComics(String filepath)
        {
            var extension = Path.GetExtension(filepath);
            var destFolder = Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath));
            try
            {
                Directory.CreateDirectory(destFolder);
                var destFile = Path.Combine(destFolder, Path.GetFileName(filepath));
                if (extension == ".cbz")
                {
                    File.Copy(filepath, destFile, true);
                    Messenger.Default.Send(new ExtractFileNotificationMessage($"Copy de {destFile}", null));
                    File.Move(destFile, Path.ChangeExtension(destFile, ".zip"));
                    var path = destFile.Replace("cbz", "zip");
                    //result = ExtractPageInCBZ(path);
                    foreach (var page in ExtractPageInCBZ(path))
                        yield return page;
                }
                else if (extension == ".cbr")
                {
                    File.Copy(filepath, destFile, true);
                    Messenger.Default.Send(new ExtractFileNotificationMessage($"Copy de {destFile}", null));
                    File.Move(destFile, Path.ChangeExtension(destFile, ".rar"));
                    var path = destFile.Replace("cbr", "rar");
                    //result = ExtractPageInCBR(filepath);
                    foreach (var page in ExtractPageInCBR(path))
                        yield return page;
                }
            }
            finally
            {
                Directory.Delete(destFolder, true);
                Messenger.Default.Send(new ExtractFileNotificationMessage($"Quit", null));
            }
        }
        private IEnumerable<Page> ExtractPageInCBZ(String filepath)
        {
            ZipArchive zipArchive = ZipArchive.Open(filepath);
            try
            {
                List<Page> result = new List<Page>();
                foreach (var entry in zipArchive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(Path.GetDirectoryName(filepath), new ExtractionOptions()
                    {
                        Overwrite = true
                    });
                }
                Messenger.Default.Send(new ExtractFileNotificationMessage($"Extraction dans {filepath}", null));
                var files = Directory.GetFiles(Path.GetDirectoryName(filepath)).ToList();
                int ind = 1;
                foreach (string file in files)
                {
                    if (Path.GetExtension(file) == ".jpg"
                        ||
                        Path.GetExtension(file) == ".jpeg"
                        ||
                        Path.GetExtension(file) == ".png")
                    {
                        Page page = new Page();
                        page.Ordre = ind++;
                        page.Element = ResizeFileImage(file,ind);
                        yield return page;
                    }
                }
            }
            finally
            {
                zipArchive.Dispose();
            }
        }
        private IEnumerable<Page> ExtractPageInCBR(String filepath)
        {
            RarArchive rarArchive = RarArchive.Open(filepath);
            try
            {
                List<Page> result = new List<Page>();

                foreach (var entry in rarArchive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(Path.GetDirectoryName(filepath), new ExtractionOptions()
                    {
                        Overwrite = true
                    });
                }
                Messenger.Default.Send(new ExtractFileNotificationMessage($"Extraction dans {filepath}", null));
                var files = Directory.GetFiles(Path.GetDirectoryName(filepath)).ToList();
                int ind = 1;
                foreach (string file in files)
                {
                    if (Path.GetExtension(file) == ".jpg"
                        ||
                        Path.GetExtension(file) == ".jpeg"
                        ||
                        Path.GetExtension(file) == ".png")
                    {
                        Page page = new Page();
                        page.Ordre = ind++;
                        page.Element = ResizeFileImage(file,ind);
                        yield return page;
                    }
                }
            }
            finally
            {
                rarArchive.Dispose();
            }
        }
    }
}
