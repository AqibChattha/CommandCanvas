using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using static System.Windows.Forms.LinkLabel;
using System.Security.Cryptography;
using System.IO;

namespace aseassignment
{
    public partial class Form1 : Form
    {

        //const bool EXECUTE = true;
        //const bool DONT_EXECUTE = false;
        Canvas Canvas;
        Parser Parser;

        bool errorInProgram = false;
        bool firstProgramExecuted = false;

        Thread canvaThread;

        //Bitmap Output;
        //Pen DataPen = new Pen(Brushes.Red, 2.0f);
        //Graphics g;

        //int ctx, cty;

        //int xPos, yPos;
        String[] inputProgram;
        String commandLineInput;
        //private String Parameter;
        //int i = 0;

        //private Pen penb = new Pen(Color.Black, 2);
        //private Pen penw = new Pen(Color.Red, 2);

        public Form1()
        {
            InitializeComponent();
            //Output = new Bitmap(pbOutput.Size.Width, pbOutput.Size.Height);
            //pbOutput.Image = Output;
            //g = Graphics.FromImage(Output);
            Canvas = new Canvas(pbOutput);

        }

        public void execute(String command)
        {
            if (Parser.isValidSyntex(command))
            {
                try
                {
                    Parser = new Parser(command);
                    if (Parser.getCommandType().Equals("moveto"))
                    {
                        int x = Convert.ToInt32(Parser.parameters[0]);
                        int y = Convert.ToInt32(Parser.parameters[1]);
                        Canvas.MoveTo(x, y);
                    }
                    else if (Parser.getCommandType().Equals("drawto"))
                    {
                        int x = Convert.ToInt32(Parser.parameters[0]);
                        int y = Convert.ToInt32(Parser.parameters[1]);
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawTo(x, y);
                        }
                        else
                        {
                            Canvas.DrawTo(x, y, Parser.parameters[2]);
                        }
                    }
                    else if (Parser.getCommandType().Equals("circle"))
                    {
                        int radius = Convert.ToInt32(Parser.parameters[0]);
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawCircle(radius);
                        }
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            Canvas.DrawCircle(radius, Parser.parameters[1]);
                        }
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            Canvas.DrawCircle(radius, Parser.parameters[1], Parser.parameters[2]);
                        }
                    }
                    else if (Parser.getCommandType().Equals("triangle"))
                    {
                        int x1 = Convert.ToInt32(Parser.parameters[0]);
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawTriangle(x1);
                        }
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            Canvas.DrawTriangle(x1, Parser.parameters[1]);
                        }
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            Canvas.DrawTriangle(x1, Parser.parameters[1], Parser.parameters[2]);
                        }
                    }
                    else if (Parser.getCommandType().Equals("rectangle"))
                    {
                        int x1 = Convert.ToInt32(Parser.parameters[0]);
                        int y1 = Convert.ToInt32(Parser.parameters[1]);
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawRectangle(x1, y1);
                        }
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            Canvas.DrawRectangle(x1, y1, Parser.parameters[2]);
                        }
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            Canvas.DrawRectangle(x1, y1, Parser.parameters[2], Parser.parameters[3]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorInProgram = true;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorInProgram = true;
                }
                pbOutput.Invalidate();
            }
            else
            {
                MessageBox.Show("Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorInProgram = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            commandLineInput = textBox1.Text;
            commandLineInput.Trim().ToLower();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void rtbInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            String[] lines = rtbInput.Text.Split('\n');
            //execute(textBox1.Text);
            executerun(lines);
            //aTimer.Stop();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                execute(commandLineInput);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }


        private void pbOutput_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //g.DrawLine(penb, 31, 100);

            // draw a red point on the pbOutput

            //using (Pen pen = new Pen(Color.DarkGoldenrod, 2.5f))
            //    for (int i = 1; i < point; i++) e.Graphics.DrawLine(pen, way[i - 1], way[i]);
        }

        private void btnSyntax_Click(object sender, EventArgs e)
        {
            //pbOutput.Invalidate();
            //way = SlowWay(way);
            //aTimer.Start();
            if (Parser.isValidSyntex(rtbInput.Text))
            {
                MessageBox.Show("All the command are written correctly.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There is an error in the given program.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void aTimer_Tick(object sender, EventArgs e)
        {
            //point = Math.Min(point + 1, way.Count);
            //pbOutput.Invalidate();
            //if (point >= way.Count) aTimer.Stop();
        }

        public void executerun(String[] commands)
        {
            try
            {
                errorInProgram = false;
                foreach (String command in commands)
                {
                    if (command.Equals("")) continue;
                    if (errorInProgram) { return; }
                    execute(command);
                }
                inputProgram = commands;
                firstProgramExecuted = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid program, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sizeChangeThreadFunction()
        {
            Canvas = new Canvas(pbOutput);
            executerun(inputProgram);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (firstProgramExecuted)
            {
                if (canvaThread != null) try { canvaThread.Join(); } catch (Exception) { }
                canvaThread = new Thread(sizeChangeThreadFunction);
                canvaThread.Start();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                if (firstProgramExecuted)
                {
                    if (canvaThread != null) try { canvaThread.Join(); } catch (Exception) { }
                    canvaThread = new Thread(sizeChangeThreadFunction);
                    canvaThread.Start();
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            if (File.Exists(tbFilePath.Text))
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                String text = String.Join(Environment.NewLine, lines);
                rtbInput.Text = text;
            }
            else
            {
                MessageBox.Show("No file found at the given path.", "File Not Found", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            openFileDialog.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Folders; Text Files|*.txt; \n";
            openFileDialog.AddExtension = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.DereferenceLinks = true;
            openFileDialog.Multiselect = false;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    MessageBox.Show("1");
                    if (File.Exists(openFileDialog.FileName))
                    {
                        MessageBox.Show("2");
                        if (MessageBox.Show("Do want to overwrite the selected file.", "File already exits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            MessageBox.Show("3");
                            using (StreamWriter sw = File.AppendText(openFileDialog.FileName))
                            {
                                sw.Write(rtbInput.Text);
                                sw.Close();
                            }
                            MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        if (Path.GetFileName(openFileDialog.FileName).Equals(""))
                        {
                            MessageBox.Show("s");
                            String path = openFileDialog.FileName + "program.txt";
                            String path2 = path;
                            if (File.Exists(path))
                            {
                                int i = 1;
                                path2 = path + i;
                                while (!File.Exists(path))
                                {
                                    i++;
                                    path2 = path + i;
                                }
                            }
                            File.Create(path2);
                            TextWriter sw = new StreamWriter(path2);
                            sw.Write(rtbInput.Text);
                            sw.Close();
                        }
                        else
                        {
                            MessageBox.Show("s");
                            if (!File.Exists(openFileDialog.FileName)) { File.Create(openFileDialog.FileName); }
                            TextWriter sw = new StreamWriter(openFileDialog.FileName);
                            sw.Write(rtbInput.Text);
                            sw.Close();
                        }
                    }
                }
                catch (IOException)
                {
                }
            }
            openFileDialog.Dispose();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Text File(*.txt)|*.txt";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbFilePath.Text = openFileDialog.FileName;
            }
            openFileDialog.Dispose();
        }
    }
}

//moveto 700,400
//circle 100
//circle 200
//circle 300
//circle 400
//circle 500
//circle 600
//circle 700
//circle 800
//circle 900