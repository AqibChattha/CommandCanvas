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

    /// <summary>
    /// Main Form class. It is the main display point for the application.
    /// </summary>
    public partial class Form1 : Form
    {
        // The area where the shapes are drawn.
        Canvas Canvas;

        // checker for errors in the program
        bool errorInProgram = false;

        // It will contain all the command that are currently used to make the canvas
        String commandsGiven = "";

        // The width of the current form
        // the height of the current form
        int formWidth, formHeight;

        /// <summary>
        /// Constructor for the Form class. It will initialize all the GUI coponents set the default values to the attributes.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            Canvas = new Canvas(pbOutput);
            formWidth = this.Size.Width;
            formHeight = this.Size.Height;
        }

        /// <summary>
        /// The method to execute the single command from the progrom or command line.
        /// </summary>
        /// <param name="command"></param>
        private bool execute(String command)
        {
            command = command.ToLower();
            // check if the command is valid
            if (Parser.isValidSyntex(command))
            {
                try
                {
                    Parser Parser = new Parser(command);
                    // check if the command is "run", "run" runs the program in the rich text box through command line.
                    if (command.Equals("run"))
                    {
                        executerun(rtbInput.Text);
                    }
                    // check if the command is "clear". It will clear the canvas.
                    else if (command.Equals("clear"))
                    {
                        Canvas.Clear();
                    }
                    // check if the command is "reset". It will clear the canvas and also the progrom box
                    else if (command.Equals("reset"))
                    {
                        rtbInput.Text = "";
                        tbFilePath.Text = "";
                        Canvas.Clear();
                    }
                    // check if the command is "moveto". It will move the pointer on canvas.
                    else if (Parser.getCommandType().Equals("moveto"))
                    {
                        int x = Convert.ToInt32(Parser.parameters[0]);
                        int y = Convert.ToInt32(Parser.parameters[1]);
                        Canvas.MoveTo(x, y);
                    }
                    // check if the command is "drawto". It will draw with the pointer on canvas.
                    else if (Parser.getCommandType().Equals("drawto"))
                    {
                        int x = Convert.ToInt32(Parser.parameters[0]);
                        int y = Convert.ToInt32(Parser.parameters[1]);

                        // check whether the "pen" argumant was also passed to change the color of the shape
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawTo(x, y);
                        }
                        else
                        {
                            Canvas.DrawTo(x, y, Parser.parameters[2]);
                        }
                    }
                    // check if the command is "circle". It will draw a circle with pointer as center.
                    else if (Parser.getCommandType().Equals("circle"))
                    {
                        int radius = Convert.ToInt32(Parser.parameters[0]);
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawCircle(radius);
                        }
                        // check whether the "pen" argumant was also passed to change the color of the shape
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            Canvas.DrawCircle(radius, Parser.parameters[1]);
                        }
                        // check whether the "fill" argumant was also passed to fill the shape with color
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            Canvas.DrawCircle(radius, Parser.parameters[1], Parser.parameters[2]);
                        }
                    }
                    // check if the command is "triangle". It will draw a triangle with the pointer as center.
                    else if (Parser.getCommandType().Equals("triangle"))
                    {
                        int x1 = Convert.ToInt32(Parser.parameters[0]);
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawTriangle(x1);
                        }
                        // check whether the "pen" argumant was also passed to change the color of the shape
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            Canvas.DrawTriangle(x1, Parser.parameters[1]);
                        }
                        // check whether the "fill" argumant was also passed to fill the shape with color
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            Canvas.DrawTriangle(x1, Parser.parameters[1], Parser.parameters[2]);
                        }
                    }
                    // check if the command is "rectangle". It will draw a rectangle with pointer as center.
                    else if (Parser.getCommandType().Equals("rectangle"))
                    {
                        int x1 = Convert.ToInt32(Parser.parameters[0]);
                        int y1 = Convert.ToInt32(Parser.parameters[1]);
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            Canvas.DrawRectangle(x1, y1);
                        }
                        // check whether the "pen" argumant was also passed to change the color of the shape
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            Canvas.DrawRectangle(x1, y1, Parser.parameters[2]);
                        }
                        // check whether the "fill" argumant was also passed to fill the shape with color
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            Canvas.DrawRectangle(x1, y1, Parser.parameters[2], Parser.parameters[3]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorInProgram = true;
                        return false;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorInProgram = true;
                    return false;
                }

                // add the commands in the commandsGiven variable to provide input on canvas size changes
                if (!(command.Equals("clear") || command.Equals("reset") || command.Equals("run")))
                {
                    commandsGiven += command + "\n";
                }
                else
                {
                    if (!command.Equals("run"))
                        commandsGiven = "";
                }

                // The most important line. It will call the paint function to re-paint the changes in the picturebox
                pbOutput.Invalidate();

            }
            else
            {
                MessageBox.Show("Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorInProgram = true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Called when the exit button is pressed. It will exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Called when run button is pressed. It will run the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            executerun(rtbInput.Text);
        }

        /// <summary>
        /// Called when the "enter" key is pressed while typing in command line. It will run the command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // if the command executes successfully then empty the commandline.
                if (execute(textBox1.Text)) textBox1.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Called when syntex check button is clicked. It will check the program whether is sytexically correct or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSyntax_Click(object sender, EventArgs e)
        {
            if (Parser.isValidSyntex(rtbInput.Text))
            {
                MessageBox.Show("All the command are written correctly.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There is an error in the given program.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// This method will execute the program line by line.
        /// </summary>
        /// <param name="commands"></param>
        private void executerun(String input)
        {
            try
            {
                String[] commands = input.Split('\n');
                // as the program is to be run, so there are no errors
                errorInProgram = false;
                foreach (String command in commands)
                {
                    // skip if the line is empty
                    if (command.Equals("")) continue;
                    // abort if there is an error
                    if (errorInProgram) { return; }
                    // normally execute the command
                    execute(command);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid program, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Called when the form is done resizing. It is used to resize and fit the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            // If the form has same dimesions, do nothing. It means if the form is moved do nothing.
            if (this.Size.Width == formWidth && this.Size.Height == formHeight) return;
            // create a newly sized canvas
            Canvas = new Canvas(pbOutput);
            executerun(commandsGiven);
        }

        /// <summary>
        /// Called when the form is resizing. It is used to resize and fit the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            // to catch the event when form is maximized
            if (WindowState == FormWindowState.Maximized)
            {
                // create a newly sized canvas
                Canvas = new Canvas(pbOutput);
                executerun(commandsGiven);
            }
        }

        /// <summary>
        /// Called when the load button is clicked. It will load the data from the path into the program box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // check if the file exists
            if (File.Exists(tbFilePath.Text))
            {
                // load the text in the program box
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                String text = String.Join(Environment.NewLine, lines);
                rtbInput.Text = text;
                tbFilePath.Text = "";
            }
            else
            {
                MessageBox.Show("No file found at the given path.", "File Not Found", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            // free the dialog box resources
            openFileDialog.Dispose();
        }

        /// <summary>
        /// Called when the save button is clicked. It will save the data to the path provided.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // If there is no path in the tbFilePath text box
            if (tbFilePath.Text.Equals(""))
            {
                // setting the file dialog properties
                openFileDialog.Filter = "(*.txt;)| *.txt;";
                openFileDialog.AddExtension = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.DereferenceLinks = true;
                openFileDialog.Multiselect = false;

                // show the dialog
                DialogResult result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    { // if the file on the path chosen exists
                        if (File.Exists(openFileDialog.FileName))
                        { // Ask whether the user wants to overwrite the existing file.
                            if (MessageBox.Show("Do want to overwrite the selected file.", "File already exits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            { // using the stream writer (resource) write the text in that file.
                                using (StreamWriter sw = File.CreateText(openFileDialog.FileName))
                                {
                                    sw.Write(rtbInput.Text);
                                }
                                MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        else
                        { // using the stream writer (resource) write the text in that file.
                            using (StreamWriter sw = File.CreateText(openFileDialog.FileName))
                            {
                                sw.Write(rtbInput.Text);
                            }
                            MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("There was an error saving the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // free the dialog box resources
                openFileDialog.Dispose();
            }
            else
            { // if the tbFilePath has a path in text field
                try
                { // if the file on the path entered exists
                    if (File.Exists(tbFilePath.Text))
                    { // Ask whether the user wants to overwrite the existing file.
                        if (MessageBox.Show("Do want to overwrite the selected file.", "File already exits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        { // using the stream writer (resource) write the text in that file.
                            using (StreamWriter sw = File.CreateText(tbFilePath.Text))
                            {
                                sw.Write(rtbInput.Text);
                            }
                            MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    { // using the stream writer (resource) write the text in that file.
                        using (StreamWriter sw = File.CreateText(tbFilePath.Text))
                        {
                            sw.Write(rtbInput.Text);
                        }
                        MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                catch (Exception) { MessageBox.Show("There was an error saving the file. The path entered can be incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        /// <summary>
        /// Called when browse button is clicked. It will open the file dialog to select the file on local system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // only text files are allowed
            openFileDialog.Filter = "Text File(*.txt)|*.txt";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // set the path in the tbFilePath
                tbFilePath.Text = openFileDialog.FileName;
            }
            // free the dialog box resources
            openFileDialog.Dispose();
        }
    }
}