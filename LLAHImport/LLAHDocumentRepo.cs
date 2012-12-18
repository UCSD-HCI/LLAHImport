using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace LLAHImport
{
    class LLAHDocumentRepo
    {
        string documentImagePath;
        string documentListPath;
        static int documentIndex = 0;
        PDFConvert converter = new PDFConvert();
        Dictionary<int, string> documentList = new Dictionary<int,string>();
        public const string LLAHLibName = "llahdoc.dll";
        public const string fileListName = "fileList.csv";

        public LLAHDocumentRepo(string documentTopDir)
        {
            //Create document image dir if not available
            this.documentImagePath = Path.Combine(documentTopDir, "images");

            if (!Directory.Exists(documentImagePath))
            {
                Directory.CreateDirectory(documentImagePath);
                
            }
            documentListPath = Path.Combine(documentTopDir,fileListName);
            if (!File.Exists(documentListPath))
            {
                File.Create(documentListPath).Close();
            }
            //Reads all entries from documentListPath;
            LoadDocumentList();

            //Setup the converter
            converter.JPEGQuality = (int)95;
            converter.ResolutionX = 200;
            converter.ResolutionY = 200;

            converter.OutputToMultipleFile = true;
            converter.OutputFormat = "jpeg";
        }

        public int GetNumberOfDocuments()
        {
            return (documentList.Count);
        }

        public bool AddPDF(string filename)
        {
            System.IO.FileInfo input = new FileInfo(filename);
            string basename = Path.GetFileNameWithoutExtension(filename);
            string extension = ".jpg";
            string output = string.Format("{0}\\{1:D5}{2}{3}",
                                    documentImagePath, documentIndex, basename + "_p", extension);
            AddToDocumentList(documentIndex, basename);
            documentIndex++;

            if (converter.Convert(filename, output) == true)
                return true;
            else
                return false;

        }

        public void AddToDocumentList(int docNum, string fileBaseName)
        {
            using (StreamWriter sw = new StreamWriter(documentListPath, true))
            {
                sw.WriteLine("{0},{1}", docNum, fileBaseName);
                sw.Close();
            }
        }

        public void LoadDocumentList()
        {
            int docNum = -1;
            try
            {
                using (StreamReader r = new StreamReader(documentListPath))
                {
                    String line;
                    while ((line = r.ReadLine()) != null)
                    {
                        String[] words = line.Split(',');
                        if (words.Length != 2)
                            break;
                        else
                        {
                            docNum = Convert.ToInt32(words[0]);
                            string docBaseName = words[1];
                            documentList.Add(docNum, docBaseName);
                        }
                    }
                    documentIndex = docNum + 1;
                    r.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

    }
}
