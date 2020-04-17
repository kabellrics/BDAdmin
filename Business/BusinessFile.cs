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
                    newfile.ImgExtension = Path.GetExtension(path);
                    newfile.Name = Path.GetFileNameWithoutExtension(path);
                    newfile.Data = File.ReadAllBytes(path);
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
            else if(Path.GetExtension(path).ToLower() == "cbz")
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

    }
}
