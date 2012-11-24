using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LLAHImport
{
    class LLAHDatabase
    {
        string documentImagePath;
        string databasePath;
        PDFConvert converter = new PDFConvert();
        static int fileId = 0;

        #region LLAHLib Import

        #endregion 

        public LLAHDatabase(string docLocation, string dbPath)
        {
            this.documentImagePath = docLocation;
            this.databasePath = dbPath;
            //Setup the converter
            converter.JPEGQuality = (int)95;
            converter.OutputToMultipleFile = true;
            converter.OutputFormat = "jpeg";
        }

        public void updateDatabase()
        {

        }
        public Boolean addPDF(string filename)
        {
            System.IO.FileInfo input = new FileInfo(filename);
            string basename = Path.GetFileNameWithoutExtension(filename);
            string directory = Path.GetDirectoryName(filename);
            if (directory == "")
                directory = ".";
            string extension = ".jpg";
            string output = string.Format("{0}\\{1:D5}{2}{3}",
                                    directory, fileId, basename + "_p", extension);
            fileId++;

            //If the output file exists already, be sure to add a
            //random name at the end until it is unique!
            if (File.Exists(output))
            {
                File.Delete(output);
            }

            if (converter.Convert(input.FullName, output) == true)
                return true;
            else
                return false;

        }

    }
}
