﻿using MetadataExtractor;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using Directory = MetadataExtractor.Directory;


namespace MetadataExtractorTest
{
    /// <summary>
    /// Test for the Metadata Extractor package.
    /// Browse to a file, and it will show the metadata tag and its contents
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                this.Title = "Examining " + openFileDialog.FileName;
                this.dataGrid.ItemsSource = LoadMetadata(openFileDialog.FileName);
                this.lblFeedback.Content = openFileDialog.FileName + Environment.NewLine + "Count: " + this.dataGrid.Items.Count;
            }
        }
        private void Datagrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            this.dataGrid.Columns[0].Header = "Metadata Name";
            this.dataGrid.Columns[1].Header = "Value";
        }

        public static List<Tuple<String, String>> LoadMetadata(string filePath)
        {
            List<Tuple<String, String>> metadata = new List<Tuple<String, String>>();

            foreach (Directory metadataDirectory in ImageMetadataReader.ReadMetadata(filePath))
            {
                foreach (Tag metadataTag in metadataDirectory.Tags)
                {
                    // We could also retrieve the metadataDirectory.Name (essentially a prefix path to the Name, such as "JPEG") but it would be confusing to an end user;
                    Tuple<String, String> tuple = new Tuple<String, String>(metadataDirectory.Name + "." + metadataTag.Name, metadataTag.Description);
                    if (metadataTag.Name == "Ambient Temperature Fahrenheit")
                    {
                        System.Diagnostics.Debug.Print(metadataTag.Name + " : " + metadataTag.Description + " : " + metadataTag.Description.ToString());
                    }
                    metadata.Add(tuple);
                }
            }
            return metadata;
        }
    }
}
