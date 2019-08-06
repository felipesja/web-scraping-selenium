﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebScrapingSelenium {
    public partial class Form : System.Windows.Forms.Form {
        public Form() {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e) {

        }

        private void BtnStart_Click(object sender, EventArgs e) {

            pbForm.Maximum = 50;
            pbForm.Step = 1;
            int iteste = 0;

            while (iteste < 50) {
                textBox.AppendText("HelloWorld!\n");
                textBox.ScrollToCaret();
                pbForm.PerformStep();
                iteste++;
            }
        }

        private void BtnFileOpen_Click(object sender, EventArgs e) {

            string fileContent = string.Empty;
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream)) {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }
    }
}
