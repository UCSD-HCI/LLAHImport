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
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog.Filter = "PDF Files|*.pdf|All Files|*.*";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = true;

            // Process input if the user clicked OK.
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    DropTbx.Text += "Adding " + fileName + "...";
                    db.AddPDF(fileName);
                    DropTbx.Text += " done.\n";
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
                DropTbx.Text += "Database is empty.\n";
            }
            else
            {
                DropTbx.Text += numberOfDocs + " documents loaded.\n";
            }
            SelectPDFBtn.IsEnabled = true;
            DropTbx_EnableDrop();

        }


        // Form load event or a similar place
        private void DropTbx_EnableDrop()
        {
            // Enable drag and drop for this form
            // (this can also be applied to any controls)
            DropTbx.AllowDrop = true;

            // Add event handlers for the drag & drop functionality   
            DropTbx.PreviewDragEnter += new DragEventHandler(DropTbx_OnDragOver);
            DropTbx.PreviewDragOver += new DragEventHandler(DropTbx_OnDragOver);
            DropTbx.PreviewDrop += new DragEventHandler(DropTbx_Drop);
        }

        public void DropTbx_OnDragOver(object sender, DragEventArgs e)
        {

            e.Effects = DragDropEffects.All;

            e.Handled = true;

        }

        // This event occurs when the user drags over the form with 
        // the mouse during a drag drop operation 
        void DropTbx_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effects = DragDropEffects.All; // Okay
            else
                e.Effects = DragDropEffects.None; // Unknown data, ignore it

        }

        // Occurs when the user releases the mouse over the drop target 
        void DropTbx_Drop(object sender, DragEventArgs e)
        {
            // Extract the data from the DataObject-Container into a string list
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            // Do something with the data...
            int numFiles = 0;
            foreach (string fileName in FileList)
            {
                if ((File.Exists(fileName)) &&
                    (Path.GetExtension(fileName).ToLower() == ".pdf"))
                {
                    DropTbx.Text += "Adding " + fileName + "...";
                    db.AddPDF(fileName);
                    DropTbx.Text += " done.\n";
                    numFiles++;
                }
                else if (!File.Exists(fileName))
                {
                    DropTbx.Text += "Skipping " + fileName + ". File not found\n";
                }
                else
                {
                    DropTbx.Text += "Skipping " + fileName + ". Only PDF files accepted.\n";
                }

            }
            DropTbx.Text += numFiles + " files added!\n";

        }



   }
}
