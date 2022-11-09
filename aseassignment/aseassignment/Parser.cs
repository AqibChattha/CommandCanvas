using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    internal class Parser
    {
        String input;

        public Parser(String command)
        {
            this.input = command;
        }

        // get the commad type that is the first part before a ' '
        public String getCommandType()
        {
            String commandType = "";
            int i = 0;
            while (input[i] != ' ' && i < input.Length)
            {
                commandType += input[i];
                i++;
            }
            return commandType;
        }

        // get the command arguments that are after the first ' '
        public String[] getCommandArgs()
        {
            String[] commandArgs = input.Split(' ');
            return commandArgs;
        }

        // get the parameters after the command type seperated by a ','
        public List<String> parameters
        {
            get
            {
                string[] commands = getCommandArgs();
                String[] args = commands[1].Split(',');
                List<String> parameters = new List<string>();

                for (int i = 0; i < args.Length; i++)
                {
                    parameters.Add(args[i]);
                }
                if (commands.Length > 3) parameters.Add(commands[3]);
                if (commands.Length > 4) parameters.Add(commands[5]);
                return parameters;
            }
        }

        public int getCommandArgsLength()
        {
            return getCommandArgs().Length;
        }

        public static bool isValidSyntex(String input)
        {
            try
            {
                String[] commands = input.Split('\n');
                foreach (String command in commands)
                {
                    String[] parts = command.Split(' ');
                    if (parts.Length == 2 || parts.Length == 4 || parts.Length == 6)
                    {
                        if (parts[0].Equals("moveto") || parts[0].Equals("drawto") || parts[0].Equals("circle") || parts[0].Equals("triangle") || parts[0].Equals("rectangle"))
                        {
                            String[] parameters = parts[1].Split(',');
                            if (parameters.Length == 1 || parameters.Length == 2)
                            {
                                try { Convert.ToInt32(parameters[0]); } catch (Exception) { return false; }
                                if (parameters.Length == 2) try { Convert.ToInt32(parameters[1]); } catch (Exception) { return false; }
                            }
                            else { return false; }
                        }
                        else { return false; }

                        if (parts.Length > 2)
                        {
                            if (parts[2].Equals("pen"))
                            {
                                if (parts[3].Equals("black") || parts[3].Equals("red") || parts[3].Equals("green"))
                                { }
                                else { return false; }
                            }
                            else { return false; }
                        }

                        if (parts.Length > 4)
                        {
                            if (parts[4].Equals("fill"))
                            {
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
            catch (Exception) { return false; }
            return true;
        }

    }
}
