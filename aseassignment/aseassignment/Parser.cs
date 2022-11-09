using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    /// <summary>
    /// Parser class. It parses the input file and returns the type and parameters.
    /// </summary>
    internal class Parser
    {
        // the input commands
        String input;

        /// <summary>
        /// constructor for the parser class.
        /// </summary>
        /// <param name="command"></param>
        public Parser(String command)
        {
            this.input = command.ToLower();
        }

        /// <summary>
        /// This method gets the command type.
        /// </summary>
        /// <returns></returns>
        public String getCommandType()
        {
            String commandType = "";
            int i = 0;
            while (i < input.Length && input[i] != ' ')
            {
                commandType += input[i];
                i++;
            }
            return commandType;
        }

        /// <summary>
        /// This method gets all the command arguments
        /// </summary>
        /// <returns></returns>
        public String[] getCommandArgs()
        {
            String[] commandArgs = input.Split(' ');
            return commandArgs;
        }

        /// <summary>
        /// This method will get the parameters depending on the number of command arguments
        /// </summary>
        public List<String> parameters
        {
            get
            {
                string[] commands = getCommandArgs();
                String[] args = commands[1].Split(',');
                List<String> parameters = new List<string>();

                // get the size parameters
                for (int i = 0; i < args.Length; i++)
                {
                    parameters.Add(args[i]);
                }
                // if the pen argument is given than take the parameter of that argument
                if (commands.Length > 3) parameters.Add(commands[3]);
                // if the fill argument is given than take the parameter of that argument
                if (commands.Length > 4) parameters.Add(commands[5]);
                return parameters;
            }
        }

        /// <summary>
        /// gets the number of arguments given
        /// </summary>
        /// <returns></returns>
        public int getCommandArgsLength()
        {
            return getCommandArgs().Length;
        }

        /// <summary>
        /// Check whether the given commands are valid or not
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool isValidSyntex(String input)
        {
            try
            {
                input = input.ToLower();
                // first seperate the commands
                String[] commands = input.Split('\n');
                foreach (String command in commands)
                {
                    // now for each command check if that is not empty or is it a single argument command
                    if (!(command.Equals("") || command.Equals("clear") || command.Equals("reset") || command.Equals("run")))
                    {
                        // now seperate the command arguments
                        String[] parts = command.Split(' ');

                        // check if the arguments are of given size or incomplete
                        if (parts.Length == 2 || parts.Length == 4 || parts.Length == 6)
                        {
                            // check if the first argument is valid
                            if (parts[0].Equals("moveto") || parts[0].Equals("drawto") || parts[0].Equals("circle") || parts[0].Equals("triangle") || parts[0].Equals("rectangle"))
                            {
                                // now seperate the first argument parameters
                                String[] parameters = parts[1].Split(',');

                                // check if the parameters are 1 or 2 
                                if (parameters.Length == 1 || parameters.Length == 2)
                                {
                                    // check if the parameters are of integer data type as they are sizes
                                    try { Convert.ToInt32(parameters[0]); } catch (Exception) { return false; }
                                    if (parameters.Length == 2) try { Convert.ToInt32(parameters[1]); } catch (Exception) { return false; }
                                }
                                else { return false; }
                            }
                            else { return false; }

                            // check if the pen argument is also given
                            if (parts.Length > 2)
                            {
                                // check if the argument is valid and moveto command is not given
                                if (parts[2].Equals("pen") && !parts[0].Equals("moveto"))
                                {
                                    // check if the pen argument parameters are valid
                                    if (parts[3].Equals("black") || parts[3].Equals("red") || parts[3].Equals("green"))
                                    { }
                                    else { return false; }
                                }
                                else { return false; }
                            }

                            // check if the fill argument is also given
                            if (parts.Length > 4)
                            {
                                // check if the argument is valid and moveto and drawto commands are not given
                                if (parts[4].Equals("fill") && !parts[0].Equals("moveto") && !parts[0].Equals("drawto"))
                                {
                                    // check if the pen argument parameters are valid
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
            catch (Exception) { return false; }
            return true;
        }
    }
}
