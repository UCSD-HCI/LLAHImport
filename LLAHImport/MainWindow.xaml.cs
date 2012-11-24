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
using System.IO;
using Microsoft.Win32;


namespace LLAHImport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        LLAHDocumentRepo db;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectPDFBtn.IsEnabled = false;
            LoadCreateDBBtn.IsEnabled = false;
        }

        private void SelectDirectoryBtn_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            using (System.Windows.Forms.FolderBrowserDialog dlg = 
                new System.Windows.Forms.FolderBrowserDialog())
            {
                dlg.Description = "Select a folder";
                dlg.SelectedPath = DirectoryPathTbx.Text;
                
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DirectoryPathTbx.Text = dlg.SelectedPath;
                    if (File.Exists(System.IO.Path.Combine(dlg.SelectedPath, LLAHDocumentRepo.fileListName)))
                        enableLoadDatabase(true);
                    else
                        enableLoadDatabase(false);
                }
            }

        }

        private void enableLoadDatabase(Boolean isLoadable)
        {
            if (isLoadable)
            {
                LoadCreateDBBtn.Content = "Load Database";
            }
            else
            {
                LoadCreateDBBtn.Content = "Create Database";
            }
            LoadCreateDBBtn.IsEnabled = true;
        }

        private void ConvertFiles()
        {
            


            db.AddPDF("test.pdf");
            db.AddPDF("C:\\temp\\20120316_lecture_notes.pdf");
            db.AddPDF("C:\\temp\\20120904 - resume.pdf");
            db.AddPDF("C:\\temp\\dppdf.pdf");
            db.AddPDF("C:\\temp\\test.pdf");

        }

        private void SelectPDFBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "PDF Files|*.pdf|All Files|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog1.FileNames)
                {
                    DropRectangle.Text += "Adding " + fileName + "...";
                    db.AddPDF(fileName);
                    DropRectangle.Text += " done.\n";
                }
            }
        }

        private void DirectoryPathTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(Path.Combine(DirectoryPathTbx.Text, LLAHDocumentRepo.fileListName)))
                enableLoadDatabase(true);
            else
                enableLoadDatabase(false);
        }

        private void LoadCreateDBBtn_Click(object sender, RoutedEventArgs e)
        {
            db = new LLAHDocumentRepo(DirectoryPathTbx.Text);
            int numberOfDocs = db.GetNumberOfDocuments();
            if (numberOfDocs == 0)
            {
                DropRectangle.Text += "Database is empty.\n";
            }
            else
            {
                DropRectangle.Text += numberOfDocs + " documents loaded.\n";
            }
            SelectPDFBtn.IsEnabled = true;

        }
    }
}
