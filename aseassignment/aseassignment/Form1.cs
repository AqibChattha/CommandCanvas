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
    /// Main Form class. It is the main display point for the application. It will display the canvas and the buttons.
    /// It will also be used to take commands from the user and pass them to the canvas. You can write a program,
    /// Save it or load it from other text files.
    /// </summary>
    public partial class Form1 : Form
    {
        /* The canvas object. It will be used to draw the shapes on the the object it's been passed.
         * Here we will be using the canvas object to draw the shapes on a picturebox.
         */
        ShapeFactory Canvas;

        // It will contain all the commands that are used to make the current canvas (picturebox).
        String commandsGiven = "";

        // The width and height of the current form
        int formWidth, formHeight;

        public List<Tuple<string, int>> Variables = new List<Tuple<string, int>>();

        public List<Tuple<string, List<string>>> loops = new List<Tuple<string, List<string>>>();

        public List<Tuple<string, List<string>, List<string>>> methods = new List<Tuple<string, List<string>, List<string>>>();

        public bool runButtonClicked = false;

        CommandParser cb;

        private static Form1? instance;

        public static Form1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Form1();
                }
                return instance;
            }
        }


        /// <summary>
        /// Constructor for the Form class. It will initialize all the GUI coponents set the default values to the attributes.
        /// It will also set the canvas object to the picturebox. and store the form dimensions in the attributes.
        /// </summary>
        private Form1()
        {
            // Initialize the GUI components
            InitializeComponent();

            // Set the canvas object to the picturebox. It will be used to draw the shapes on the picturebox.
            Canvas = new ShapeFactory(pbOutput);
            cb = new CommandParser(Canvas);

            // Set the form dimensions to the attributes.
            formWidth = this.Size.Width;
            formHeight = this.Size.Height;
            tColorTransition.Start();
        }

        private bool getVariable(string command)
        {
            string[] commandArgs = command.Split('=');
            if (commandArgs.Length == 2)
            {
                string variableName = commandArgs[0].Trim();
                string variableValue = commandArgs[1].Trim();
                if (Variables.Any(x => x.Item1.Equals(variableName)))
                {
                    if (variableValue.Contains("+"))
                    {
                        string[] variableValueArgs = variableValue.Split('+');
                        if (variableValueArgs.Length == 2)
                        {
                            string variableValue1 = variableValueArgs[0].Trim();
                            string variableValue2 = variableValueArgs[1].Trim();

                            Variables[Variables.FindIndex(x => x.Item1.Equals(variableName))] = new Tuple<string, int>(variableName, getValue(variableValue1) + getValue(variableValue2));
                            return true;
                        }
                    }
                    else if (variableValue.Contains("-"))
                    {
                        string[] variableValueArgs = variableValue.Split('-');
                        if (variableValueArgs.Length == 2)
                        {
                            string variableValue1 = variableValueArgs[0].Trim();
                            string variableValue2 = variableValueArgs[1].Trim();

                            Variables[Variables.FindIndex(x => x.Item1.Equals(variableName))] = new Tuple<string, int>(variableName, getValue(variableValue1) - getValue(variableValue2));
                            return true;
                        }
                    }
                    else if (variableValue.Contains("*"))
                    {
                        string[] variableValueArgs = variableValue.Split('*');
                        if (variableValueArgs.Length == 2)
                        {
                            string variableValue1 = variableValueArgs[0].Trim();
                            string variableValue2 = variableValueArgs[1].Trim();

                            Variables[Variables.FindIndex(x => x.Item1.Equals(variableName))] = new Tuple<string, int>(variableName, getValue(variableValue1) * getValue(variableValue2));
                            return true;
                        }

                    }
                }
                else
                {
                    int value = 0;
                    if (int.TryParse(variableValue, out value))
                    {
                        Variables.Add(new Tuple<string, int>(variableName, value));
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// The method to execute the single command from the progrom or command line.
        /// It will take the command and validate it.
        /// If it is valid, It will parse parameters from the command and use the canvas object to draw the shape.
        /// </summary>
        /// <param name="command">The String of commands to be executed</param>
        private bool execute(String command, int lineNumber = 0)
        {
            // As the commands are case insensitive, we convert the input string to lowercase.
            command = command.ToLower();

            // Check if the command is valid. If it is not valid, it will return false.
            if (Parser.isValidSyntex(command))
            {
                try
                {
                    Parser Parser = new Parser(command);

                    // Check if the given command is "run". 
                    if (command.Equals("run"))
                    {
                        /* If the command is "run".
                         * Get the commands from the richTextBox. It will be used to execute the program.
                         */
                        executerun(rtbInput.Text);
                    }
                    // Check if the command is "clear". 
                    else if (command.Equals("clear"))
                    {
                        /* If the command is "clear".
                         * Clear the canvas (picturebox).
                         */
                        Canvas.Clear();
                    }
                    // Check if the command is "reset".
                    else if (command.Equals("reset"))
                    {
                        /* If the command is "reset".
                         * Clear the canvas (picturebox) and reset it to default. 
                         * Clear the program box, file path textbox and command line.
                         */
                        rtbInput.Text = "";
                        tbCommandLine.Text = "";
                        tbFilePath.Text = "";
                        Canvas.Clear();
                    }
                    // Check if the command is "moveto".
                    else if (Parser.getCommandType().Equals("moveto"))
                    {
                        /* If the command is "moveto".
                         * Get the x and y coordinates from the parser object.
                         * Using canvas object, move the pointer to the given coordinates.
                         */
                        int x = getValue(Parser.parameters[0]);
                        int y = getValue(Parser.parameters[1]);
                        Canvas.MoveTo(x, y);
                    }
                    // Check if the command is "drawto".
                    else if (Parser.getCommandType().Equals("drawto"))
                    {
                        /* If the command is "drawto".
                         * Get the x and y coordinates from the parser object.
                         * Using canvas object, draw a line from the pointer to the given coordinates.
                         */
                        int x = getValue(Parser.parameters[0]);
                        int y = getValue(Parser.parameters[1]);

                        // Check whether the "pen" argumant was also passed to change the color of the line.
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            // Draw line without color.
                            Canvas.DrawTo(x, y);
                        }
                        else
                        {
                            // Draw line with color.
                            Canvas.DrawTo(x, y, Parser.parameters[2]);
                        }
                    }
                    // Check if the command is "circle". It will draw a circle with pointer as center.
                    else if (Parser.getCommandType().Equals("circle"))
                    {
                        /* If the command is "circle".
                         * Get the radius from the parser object.
                         * Using canvas object, draw a circle with pointer as center and given radius.
                         */
                        int radius = getValue(Parser.parameters[0]);

                        // Check if only the radius was passed.
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            // Draw circle with default color.
                            Canvas.DrawCircle(radius);
                        }
                        // Check if the "pen" argumant was also passed.
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            // Draw outlined circle with given color.
                            Canvas.DrawCircle(radius, Parser.parameters[1]);
                        }
                        // Check if the "fill" argumant was also passed.
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            // Draw filled circle with given color.
                            Canvas.DrawCircle(radius, Parser.parameters[1], Parser.parameters[2]);
                        }
                    }
                    // Check if the command is "triangle".
                    else if (Parser.getCommandType().Equals("triangle"))
                    {
                        /* If the command is "triangle".
                         * Get the size from the parser object.
                         * Using canvas object, draw a triangle with pointer as center and given size.
                         */
                        int size = getValue(Parser.parameters[0]);

                        // Check if only the size was passed.
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            // Draw triangle with default color.
                            Canvas.DrawTriangle(size);
                        }
                        // Check if the "pen" argumant was also passed.
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            // Draw outlined triangle with given color.
                            Canvas.DrawTriangle(size, Parser.parameters[1]);
                        }
                        // Check if the "fill" argumant was also passed.
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            // Draw filled triangle with given color.
                            Canvas.DrawTriangle(size, Parser.parameters[1], Parser.parameters[2]);
                        }
                    }
                    // Check if the command is "rectangle". It will draw a rectangle with pointer as center.
                    else if (Parser.getCommandType().Equals("rectangle"))
                    {
                        /* If the command is "rectangle".
                         * Get the width and height from the parser object.
                         * Using canvas object, draw a rectangle with pointer as center and given width and height.
                         */
                        int width = getValue(Parser.parameters[0]);
                        int height = getValue(Parser.parameters[1]);

                        // Check if only the width and height were passed.
                        if (Parser.getCommandArgsLength() == 2)
                        {
                            // Draw rectangle with default color.
                            Canvas.DrawRectangle(width, height);
                        }
                        // Check if the "pen" argumant was also passed.
                        else if (Parser.getCommandArgsLength() == 4)
                        {
                            // Draw outlined rectangle with given color.
                            Canvas.DrawRectangle(width, height, Parser.parameters[2]);
                        }
                        // Check if the "fill" argumant was also passed.
                        else if (Parser.getCommandArgsLength() == 6)
                        {
                            // Draw filled rectangle with given color.
                            Canvas.DrawRectangle(width, height, Parser.parameters[2], Parser.parameters[3]);
                        }
                    }
                    else
                    {
                        // Display the error message if the command is not recognized.
                        MessageBox.Show($"Line {lineNumber}: Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception)
                {
                    // Display the error message if a exception is thrown.
                    MessageBox.Show($"Line {lineNumber}: Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // The most important line. It will call the paint function of the picturebox to re-paint the changes in the picturebox
                pbOutput.Invalidate();

            }
            else
            {
                if (getVariable(command) == false)
                {
                    // Display the error message if the command is not recognized.
                    MessageBox.Show($"Line {lineNumber}: Invalid command, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Called when the exit button is pressed. It will exit the application.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Exit the application.
            Application.Exit();
        }

        /// <summary>
        /// Called when run button is pressed. It will run the program.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            // Run the program. It will run all the commands in the text box.
            //commandsGiven = "";
            //runButtonClicked = true;
            //executerun(rtbInput.Text);
            cb.programWindow(rtbInput.Text);
            pbOutput.Invalidate();
        }

        /// <summary>
        /// Called when the "enter" key is pressed while typing in command line. It will run the command.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the "enter" key is pressed.
            if (e.KeyCode == Keys.Enter)
            {
                // If the command executes successfully then empty the commandline.
                if (!execute(tbCommandLine.Text))
                {
                    string command = tbCommandLine.Text.ToLower().Trim();
                    // Check if the command is not of any sigle argument command types.
                    if (!(command.Equals("clear") || command.Equals("reset") || command.Equals("run")))
                    {
                        // Add the commands in the commandsGiven variable to provide input when the canvas size changes and it is redrawn.
                        commandsGiven += command + "\n";
                        tbCommandLine.Text = "";
                    }
                    else
                    {
                        // Check if the command is not "run".
                        if (!command.Equals("run"))
                        {
                            /* commandsGiven is used to store the commands that are used when the canvas (picturebox) size changes and it is redrawn.
                             * If it is not "run", then the commandsGiven variable is cleared to avoid the commands being repeated when the canvas is redrawn.
                             */
                            commandsGiven = ""; // Clear the commandsGiven variable.
                                                // Clear the variable list
                            Variables = new List<Tuple<string, int>>();
                            loops = new List<Tuple<string, List<string>>>();
                            methods = new List<Tuple<string, List<string>, List<string>>>();
                            tbCommandLine.Text = "";
                        }
                    }
                }
                else
                {
                    tbCommandLine.Text = "";
                }

                // The event was handled. So, set the Handled property to true.
                e.Handled = true;

                // Do not send the key to the control.
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Called when syntex check button is clicked. It will check the program whether is syntecally correct or not.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void btnSyntax_Click(object sender, EventArgs e)
        {
            // Check the program whether is syntecally correct or not.
            if (Parser.isValidSyntex(rtbInput.Text))
            {
                // Display the message if the program is syntecally correct.
                MessageBox.Show("All the command are written correctly.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Display the message if the program is not syntecally correct.
                MessageBox.Show("There is an error in the given program.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// This method will execute the program line by line. It will execute the commands in the richTextBox.
        /// It will ignore the empty lines. It will not start if the program is syntecally incorrect.
        /// </summary>
        /// <param name="input">The multi-command Strind seperated by newline character</param>
        private void executerun(String input, int lineNumber = 1)
        {
            try
            {
                String[] commands = input.Split('\n');

                // Loop through all the commands.
                for (int i = 0; i < commands.Length; i++)
                {
                    // skip if the line is empty
                    if (commands[i].Trim().Equals("")) continue;

                    string cmd = commands[i];
                    Parser parser = new Parser(cmd);


                    if (parser.getCommandType().Equals("while"))
                    {
                        // execute the loop on variable and return the index of command where the endloop is written
                        i = ExecuteLoop(i, commands);
                    }
                    else if (!execute(commands[i], lineNumber))
                    {

                    }
                    else
                    {
                        if (runButtonClicked)
                        {
                            string command = commands[i].ToLower().Trim();
                            // Check if the command is not of any sigle argument command types.
                            if (!(command.Equals("clear") || command.Equals("reset") || command.Equals("run")))
                            {
                                // Add the commands in the commandsGiven variable to provide input when the canvas size changes and it is redrawn.
                                commandsGiven += command + "\n";
                            }
                            else
                            {
                                // Check if the command is not "run".
                                if (!command.Equals("run"))
                                {
                                    /* commandsGiven is used to store the commands that are used when the canvas (picturebox) size changes and it is redrawn.
                                     * If it is not "run", then the commandsGiven variable is cleared to avoid the commands being repeated when the canvas is redrawn.
                                     */
                                    commandsGiven = ""; // Clear the commandsGiven variable.
                                                        // Clear the variable list
                                    Variables = new List<Tuple<string, int>>();
                                    loops = new List<Tuple<string, List<string>>>();
                                    methods = new List<Tuple<string, List<string>, List<string>>>();
                                }
                            }
                        }
                    }

                    lineNumber++;
                }
            }
            catch (Exception)
            {
                // Display the error message if a exception is thrown.
                MessageBox.Show("Invalid program, please try again", "Syntex Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            runButtonClicked = false;
        }

        /// <summary>
        /// Method to execte the commands between command 'while condition' and 'endloop' until the condition becomes false.
        /// </summary>
        /// <param name="i">The index of 'while condition' command in commands.</param>
        /// <param name="commands">All of the commands in the program.</param>
        /// <returns>Index of the next command after executing the loop.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private int ExecuteLoop(int i, string[] commands)
        {
            // get the condition of the loop
            string condition = commands[i].Substring(commands[i].IndexOf("while") + 5).Trim();

            // get the index of the endloop command
            int endloopIndex = getEndloopIndex(i, commands);

            // get the commands between while and endloop
            string[] loopCommands = commands.Skip(i + 1).Take(endloopIndex - i - 1).ToArray();

            // execute the loop until the condition becomes false
            while (executeCondition(condition))
            {
                // execute the loop commands
                executerun(string.Join("\n", loopCommands), i + 1);
            }

            // return the index of the next command after executing the loop
            return endloopIndex;
        }

        private bool executeCondition(string condition)
        {
            // get the left and right operands
            string[] operands = condition.Split(new string[] { "==", "<=", ">=", "<", ">" }, StringSplitOptions.None);

            // get the left and right values
            string left = operands[0].Trim();
            string right = operands[1].Trim();

            // get the left and right values
            int leftValue = getValue(left);
            int rightValue = getValue(right);

            // return the result of the condition
            if (condition.Contains("=="))
            {
                return leftValue == rightValue;
            }
            else if (condition.Contains("<="))
            {
                return leftValue <= rightValue;
            }
            else if (condition.Contains(">="))
            {
                return leftValue >= rightValue;
            }
            else if (condition.Contains("<"))
            {
                return leftValue < rightValue;
            }
            else if (condition.Contains(">"))
            {
                return leftValue > rightValue;
            }

            return true;
        }

        public int getValue(string left)
        {
            // check if the value is a variable
            string variableName = left;

            if (Variables.Any(x => x.Item1 == variableName))
            {
                // get the value of the variable
                return Variables.First(x => x.Item1 == variableName).Item2;
            }
            else
            {
                // return the value of the variable
                if (int.TryParse(left, out int value))
                {
                    return value;
                }
                else
                {
                    throw new Exception("Invalid variable name");
                }
            }
        }

        private int getEndloopIndex(int i, string[] commands)
        {
            int endloopIndex = -1;
            for (int j = i; j < commands.Length; j++)
            {
                if (commands[j].Trim().Equals("endloop"))
                {
                    endloopIndex = j;
                    break;
                }
            }

            return endloopIndex;
        }

        /// <summary>
        /// Called when the form is done resizing. It is used to resize and fit the canvas (picturebox).
        /// It is also used to redraw the canvas with the commands that are given before the canvas size changed.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            // If the form has same dimesions, do nothing. It means if the form is moved do nothing.
            if (this.Size.Width == formWidth && this.Size.Height == formHeight) return;

            // If the form is resized, then resize the canvas (picturebox) to fit the form.
            Canvas = new ShapeFactory(pbOutput);
            Variables = new List<Tuple<string, int>>();
            loops = new List<Tuple<string, List<string>>>();
            methods = new List<Tuple<string, List<string>, List<string>>>();

            // Redraw the canvas with the commands that are given before the canvas size changed.
            executerun(commandsGiven);
        }

        /// <summary>
        /// Called when the form is maximized. It is used to resize and fit the canvas (picturebox).
        /// It is also used to redraw the canvas with the commands that are given before the canvas size changed.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            // To catch the event when form is maximized.
            if (WindowState == FormWindowState.Maximized)
            {
                // If the form is resized, then resize the canvas (picturebox) to fit the form.
                Canvas = new ShapeFactory(pbOutput);
                Variables = new List<Tuple<string, int>>();
                loops = new List<Tuple<string, List<string>>>();
                methods = new List<Tuple<string, List<string>, List<string>>>();

                // Redraw the canvas with the commands that are given before the canvas size changed.
                executerun(commandsGiven);
            }
        }

        /// <summary>
        /// Called when the load button is clicked. It will load the data from the path given in tbFilePath into the program box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The arguments passed on this event</param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Check if the file exists at the given path exists.
            if (File.Exists(tbFilePath.Text))
            {
                // Read data from file and load the data in array of String.
                string[] lines = File.ReadAllLines(openFileDialog.FileName);

                // Join all the lines into one string and display it in the program richTextBox.
                String text = String.Join(Environment.NewLine, lines);
                rtbInput.Text = text;

                // After loading the data, clear the file path text box.
                tbFilePath.Text = "";
            }
            else
            {
                // Display the error message if the file does not exists.
                MessageBox.Show("No file found at the given path.", "File Not Found", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            // Free the dialog box resources.
            openFileDialog.Dispose();
        }

        /// <summary>
        /// Called when the save button is clicked. It will save the data to the path provided or selected through file dialog.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // If there is no path in the tbFilePath text box.
            if (tbFilePath.Text.Equals(""))
            {
                // Only text files are allowed.
                openFileDialog.Filter = "(*.txt;)| *.txt;";

                // Do not check if the file exists.
                openFileDialog.CheckFileExists = false;

                // Show the file dialog.
                DialogResult result = openFileDialog.ShowDialog();

                // If the user selects a file.
                if (result == DialogResult.OK)
                {
                    try
                    { // If the file on the path chosen exists.
                        if (File.Exists(openFileDialog.FileName))
                        { // Ask whether the user wants to overwrite the existing file.
                            if (MessageBox.Show("Do want to overwrite the selected file.", "File already exits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            { // Using the stream writer (resource) write the text in that file.
                                using (StreamWriter sw = File.CreateText(openFileDialog.FileName))
                                {
                                    // Write the text in the file.
                                    sw.Write(rtbInput.Text);
                                }
                                // Display the success message.
                                MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        else
                        { // Using the stream writer (resource) write the text in that file.
                            using (StreamWriter sw = File.CreateText(openFileDialog.FileName))
                            {
                                // Write the text in the file.
                                sw.Write(rtbInput.Text);
                            }
                            // Display the success message.
                            MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    catch (IOException)
                    {
                        // Display the error message if the file is in use.
                        MessageBox.Show("There was an error saving the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Free the dialog box resources.
                openFileDialog.Dispose();
            }
            else
            { // If the tbFilePath has a path in text field.
                try
                { // If the file on the path entered exists.
                    if (File.Exists(tbFilePath.Text))
                    { // Ask whether the user wants to overwrite the existing file.
                        if (MessageBox.Show("Do want to overwrite the selected file.", "File already exits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        { // Using the stream writer (resource) write the text in that file.
                            using (StreamWriter sw = File.CreateText(tbFilePath.Text))
                            {
                                // Write the text in the file.
                                sw.Write(rtbInput.Text);
                            }
                            // Display the success message.
                            MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    { // using the stream writer (resource) write the text in that file.
                        using (StreamWriter sw = File.CreateText(tbFilePath.Text))
                        {
                            // Write the text in the file.
                            sw.Write(rtbInput.Text);
                        }
                        // Display the success message.
                        MessageBox.Show("The file has been save successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                // Display the error message if an exception is thrown.
                catch (Exception) { MessageBox.Show("There was an error saving the file. The path entered can be incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void tColorTransition_Tick(object sender, EventArgs e)
        {
            if (pbColor1.BackColor == Color.Red && pbColor2.BackColor == Color.Blue & pbColor3.BackColor == Color.Black)
            {
                pbColor1.BackColor = Color.Green;
                pbColor2.BackColor = Color.Yellow;
                pbColor3.BackColor = Color.White;
            }
            else
            {
                pbColor1.BackColor = Color.Red;
                pbColor2.BackColor = Color.Blue;
                pbColor3.BackColor = Color.Black;
            }
        }

        /// <summary>
        /// Called when browse button is clicked. It will open the file dialog to select the file on local system.
        /// </summary>
        /// <param name="sender">The object that is the sender of this event</param>
        /// <param name="e">The arguments passed on this event</param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Only text files are allowed.
            openFileDialog.Filter = "Text File(*.txt)|*.txt";

            DialogResult result = openFileDialog.ShowDialog();

            // If the user selects a file.
            if (result == DialogResult.OK)
            {
                // Set the path in the tbFilePath.
                tbFilePath.Text = openFileDialog.FileName;
            }
            // Free the dialog box resources.
            openFileDialog.Dispose();
        }
    }
}