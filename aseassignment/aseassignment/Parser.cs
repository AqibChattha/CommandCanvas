using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    /// <summary>
    /// Parser class. It parses the input string commands.
    /// It validates the commands and returns the type and parameters required from the command.
    /// </summary>
    public class Parser
    {
        // The input entered by the user. It can be a single as well as multiple commands seperated by newline character.
        String input;

        /// <summary>
        /// constructor for the parser class.
        /// It takes the input string as a parameter and assigns it to input attribute after lowercasing it.
        /// </summary>
        /// <param name="commands">The String of commands to be parsed</param>
        public Parser(String commands)
        {
            // As the commands are case insensitive, we convert the input string to lowercase.
            this.input = commands.ToLower();
        }

        /// <summary>
        /// This method gets the command type. The command type is the first word in the command.
        /// </summary>
        /// <returns>It returns the command's type</returns>
        public String getCommandType()
        {
            String commandType = "";
            int i = 0;
            // As the command type is the first word in the command, we iterate through the input string until we find a space.
            while (i < input.Length && input[i] != ' ')
            {
                // We append the characters to the commandType string until we find a space.
                commandType += input[i];
                i++;
            }
            // We return the commandType string.
            return commandType;
        }

        /// <summary>
        /// This method gets all the command arguments and values. The command arguments are the words in the command seperated by comma.
        /// For example, if the command is "rectangle 10,20" then the argument is rectangle and parameters are 10 and 20.
        /// </summary>
        /// <returns>It return the array of arguments after spliting the command on space(' ')</returns>
        public String[] getCommandArgs()
        {
            String[] commandArgs = input.Split(' ');
            return commandArgs;
        }

        /// <summary>
        /// This method will get the parameters depending on the number of command arguments. Parameters are the values of the arguments.
        /// For example, if the command is "rectangle 10,20" then the parameters are 10 and 20.
        /// </summary>
        public List<String> parameters
        {
            // The getter will return the list of parameters.
            get
            {
                // Get the raw arguments and values of the command.
                string[] commands = getCommandArgs();

                // First argument is the command type and its parameters are cordinates which are seperated by comma.
                String[] values = commands[1].Split(',');

                // The list of parameters to be returned.
                List<String> parameters = new List<string>();

                // Get all the cordinates and add them to the list of parameters.
                for (int i = 0; i < values.Length; i++)
                {
                    parameters.Add(values[i]);
                }

                // If the pen argument is given than take the parameter of that argument
                if (commands.Length > 3) parameters.Add(commands[3]);

                // If the fill argument is given than take the parameter of that argument
                if (commands.Length > 4) parameters.Add(commands[5]);

                return parameters;
            }
        }

        /// <summary>
        /// Get the number of argument and values in the command.
        /// </summary>
        /// <returns>It will return the number of arguments and values</returns>
        public int getCommandArgsLength()
        {
            return getCommandArgs().Length;
        }

        /// <summary>
        /// Method to validate the command. It checks if the command is valid or not.
        /// </summary>
        /// <param name="input">The String of commands to be validated</param>
        /// <returns>Return true of the command is valid otherwise false</returns>
        public static bool isValidSyntex(String input)
        {
            try
            {
                // As the commands are case insensitive, we convert the input string to lowercase.
                input = input.ToLower();

                // Seperate the command program into individual commands.
                String[] commands = input.Split('\n');

                // Iterate through all the commands.
                foreach (String command in commands)
                {
                    // For each command check if that string is not empty or is it a single argument command.
                    if (!(command.Equals("") || command.Equals("clear") || command.Equals("reset") || command.Equals("run")))
                    {
                        // Seperate the command arguments from the command.
                        String[] parts = command.Split(' ');

                        // Check if the arguments are of any valid lengths.
                        if (parts.Length == 2 || parts.Length == 4 || parts.Length == 6)
                        {
                            // Check if the first argument is valid.
                            if (parts[0].Equals("moveto") || parts[0].Equals("drawto") || parts[0].Equals("circle") || parts[0].Equals("triangle") || parts[0].Equals("rectangle"))
                            {
                                // Seperate the first argument parameters on the basis on comma.
                                String[] parameters = parts[1].Split(',');

                                // Check if the parameters are of valid length 1 or 2.
                                if (parameters.Length == 1 || parameters.Length == 2)
                                {
                                    // Check if the parameters are of integer data type as they are cordinates.
                                    // We used try catch here as if the parameters are not of integer data type then it will throw an exception.
                                    try
                                    {
                                        if (Form1.Instance.getValue(parameters[0]) == -1) { return false; }
                                    }
                                    catch (Exception) { return false; }
                                    if (parameters.Length == 2) try
                                        {
                                            if (Form1.Instance.getValue(parameters[1]) == -1) { return false; }
                                        }
                                        catch (Exception) { return false; }
                                }
                                else { return false; }
                            }
                            else { return false; }

                            // Check if the pen argument is also given.
                            if (parts.Length > 2)
                            {
                                // Check if the argument is valid and moveto command is not given as moveto command does not have pen argument.
                                if (parts[2].Equals("pen") && !parts[0].Equals("moveto"))
                                {
                                    // Check if the pen argument parameter is valid.
                                    if (parts[3].Equals("black") || parts[3].Equals("red") || parts[3].Equals("green"))
                                    { }
                                    else { return false; }
                                }
                                else { return false; }
                            }

                            // Check if the fill argument is also given.
                            if (parts.Length > 4)
                            {
                                // Check if the argument is valid and moveto and drawto commands are not given as moveto and drawto commands do not have fill argument.
                                if (parts[4].Equals("fill") && !parts[0].Equals("moveto") && !parts[0].Equals("drawto"))
                                {
                                    // Check if the fill argument parameter is valid.
                                    if (parts[5].Equals("none") || parts[5].Equals("on"))
                                    { }
                                    else { return false; }
                                }
                                else { return false; }
                            }
                        }
                        else { return false; }
                    }
                }
            }
            // If there is any exception then the command is not valid.
            catch (Exception) { return false; }

            // If there is no exception and program reaches here then the command is valid.
            return true;
        }
    }
}
