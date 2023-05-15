using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aseassignment2

{
    public partial class Form1 : Form
    {
        Bitmap bitmap = new Bitmap(640, 480);
        Canvas myCanvas;
        commandParser cp;

        public Form1()
        {
            InitializeComponent();
            myCanvas = new Canvas(Graphics.FromImage(bitmap));
            cp = new commandParser(myCanvas);

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            //prevents flickering
            this.DoubleBuffered = true;

        }

        private void runButton_Click(object sender, EventArgs e)
        {
            // calls the commandparser class
            //cp.programWindow(programWindow.Text);
            string text = programWindow.Text;
            cp.programWindow(text);

            Refresh();

        }

        private void displayWindow_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(bitmap, 0, 0);
        }

        private void commandLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && commandLine.Text != "run")
            {
                try
                {
                    //calls the commandpaser class
                    string text = commandLine.Text;

                    cp.CommandLine(text, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (e.KeyCode == Keys.Enter && commandLine.Text == "run")
            {
                commandLine.Text = "";
                runButton.PerformClick();
            }


            Refresh();
        }
        // saves program to text file
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    using (StreamWriter writer = new StreamWriter(myStream))
                    {
                        writer.Write(programWindow.Text);
                        writer.Close();
                    }
                    myStream.Close();
                }
            }
            // string path = @"C:\\Users\\Rotamin\\Desktop\\myProgram.txt";
            //File.WriteAllText(path, programWindow.Text);
            //MessageBox.Show("Successfully Saved");

        }
        // load program from textfile
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    string str = openFileDialog1.FileName;
                    String filetxt = File.ReadAllText(str);
                    programWindow.Text = filetxt;
                }
            }
        }
        // close the application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f1 = new Form1();
            f1.Close();
        }

        private void exportBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                bitmap.Save(path);
            }

        }
    }
}
