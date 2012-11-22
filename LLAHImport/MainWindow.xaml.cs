using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PdfToImage;
using System.IO;


namespace LLAHImport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PDFConvert converter = new PDFConvert();
        public MainWindow()
        {
            InitializeComponent();
            ConvertSingleImage("test.pdf");
        }

        private void ConvertSingleImage(string filename)
        {
            //Setup the converter
            converter.JPEGQuality = (int)95;
            converter.OutputFormat = "jpeg";
            System.IO.FileInfo input = new FileInfo(filename);
            string extension = ".jpg";
            string output = string.Format("{0}\\{1}{2}",
                                    input.Directory,input.Name,extension);

            //If the output file exists already, be sure to add a
            //random name at the end until it is unique!
            while (File.Exists(output))
                output = output.Replace(extension,
                    string.Format("{1}{0}", extension,DateTime.Now.Ticks));

            if (converter.Convert(input.FullName, output) == true)
                ResultTbx.Text = "Converted!";
            else
                ResultTbx.Text = "Error :(";

        }
    }
}
