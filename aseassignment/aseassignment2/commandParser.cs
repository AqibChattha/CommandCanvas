using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace aseassignment2
{
    /// <summary>
    /// This is the primary command parser class.
    /// It makes use of a number of additional classes to aid in parsing and keeping track of state
    /// </summary>
    public class commandParser
    {
        //Default pen color is set to  black
        Color color = Color.Black;

        //Default fill is set to zero/off
        int fill = 0;
        Canvas myCanvas;

        //Integer testing variable is created
        int Integer;

        //Start of array declaration
        List<string> Vname = new List<string>();

        //End of array declaration
        List<int> Vvalue = new List<int>();

        //Datatable for testing expression
        DataTable datatable = new DataTable();

        //If and While flag set to true
        bool If_flag = true;
        bool While_flag = true;
        public commandParser(Canvas canvas)
        {
            myCanvas = canvas;
        }

        /// <summary>
        /// Program window method
        /// </summary>
        /// <param name="script">Sting passed from the script</param>
        public void programWindow(string script)
        {
            //Creates char array called vs
            char[] vs = new[] { '\r', '\n' };
            //Splits the scripts string by \r and \n
            String[] vs1 = script.Split(vs, StringSplitOptions.RemoveEmptyEntries);


            int i = 0;
            int Counter = 0;

            //while loop using the length of the splitted string
            while (i < vs1.Length)
            {
                try
                {
                    //Takes input from the script into the string
                    String line = vs1[i].ToLower();

                    //Splits the string variable using space
                    String[] vs2 = line.Split(' ');

                    //First partion of array[0]
                    String command = vs2[0].ToLower();

                    //Checks if the command is 'drawto' and if the array partition is greater than 1
                    if (command.Equals("drawto") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            // Splits parameter using a ',' into a parameter Array 
                            string[] parameter = vs2[1].Split(',');

                            //Checks if the passed parameter is an int
                            if (parameter.Length != 2)
                            {
                                throw new Exception("Invalid Parameter. Line command require 2 Parameter");
                                i++;
                            }
                            else if (int.TryParse(parameter[0], out Integer) == false || int.TryParse(parameter[1], out Integer) == false)
                            {
                                int paramValue1;
                                int paramValue2;

                                if (int.TryParse(parameter[0], out Integer) == false)
                                {
                                    int position1 = Vname.IndexOf(parameter[0].ToLower());
                                    if (position1 > -1)
                                    {
                                        paramValue1 = Vvalue[position1];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown first parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue1 = Int16.Parse(parameter[0]);
                                }
                                if (int.TryParse(parameter[1], out Integer) == false)
                                {
                                    int position2 = Vname.IndexOf(parameter[1].ToLower());
                                    if (position2 > -1)
                                    {
                                        paramValue2 = Vvalue[position2];
                                    }
                                    else
                                    {

                                        throw new Exception("Variable does not exist or Unknown second parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue2 = Int16.Parse(parameter[1]);
                                }
                                if (paramValue1 != 0 && paramValue2 != 0)
                                {
                                    //Drawline method called
                                    myCanvas.DrawLine(color, paramValue1, paramValue2);
                                    i++;
                                }

                                else
                                {
                                    throw new Exception("Unknown Variable!");
                                    i++;
                                }
                            }
                            else
                            {
                                //Drawline method called
                                myCanvas.DrawLine(color, Int16.Parse(parameter[0]), Int16.Parse(parameter[1]));

                                i++;

                            }

                        }
                    }
                    // Checks if command is "moveto" and if array partion is greater than 1
                    else if (command.Equals("moveto") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            //Splits the parameter using ',' 
                            string[] parameter = vs2[1].Split(',');

                            //If returned array lenght is not equals to2 and exception is thrown
                            if (parameter.Length != 2)
                            {
                                // Reports invalid Parameter  with its the line number
                                throw new Exception("Invalid Parameter. Moveto command require 2 Parameter");
                                i++;
                            }
                            else if (int.TryParse(parameter[0], out Integer) == false || int.TryParse(parameter[1], out Integer) == false)
                            {
                                int paramValue1;
                                int paramValue2;

                                if (int.TryParse(parameter[0], out Integer) == false)
                                {
                                    int position1 = Vname.IndexOf(parameter[0].ToLower());
                                    if (position1 > -1)
                                    {
                                        paramValue1 = Vvalue[position1];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown first parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue1 = Int16.Parse(parameter[0]);
                                }
                                if (int.TryParse(parameter[1], out Integer) == false)
                                {
                                    int position2 = Vname.IndexOf(parameter[1].ToLower());
                                    if (position2 > -1)
                                    {
                                        paramValue2 = Vvalue[position2];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown second parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue2 = Int16.Parse(parameter[1]);
                                }
                                if (paramValue1 != 0 && paramValue2 != 0)
                                {
                                    //Moveto method called
                                    myCanvas.Moveto(paramValue1, paramValue2);
                                    i++;
                                }

                                else
                                {
                                    throw new Exception("Unknown Variable!");
                                    i++;
                                }
                            }
                            else
                            {
                                //Moveto method called
                                myCanvas.Moveto(Int16.Parse(parameter[0]), Int16.Parse(parameter[1]));

                                i++;

                            }

                        }
                    }
                    // checks if command is = to "rectangle"and if array partion is greater than 1
                    else if (command.Equals("rectangle") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            //Splits parameter using ','
                            string[] parameter = vs2[1].Split(',');
                            // if the returned parameter array lenglth is not equal to 2 an exception is throen
                            if (parameter.Length != 2)
                            {
                                //Reports invalid parameter with line no
                                throw new Exception("Invalid Parameter. Ractangle command require 2 Parameter");
                                i++;
                            }
                            else if (int.TryParse(parameter[0], out Integer) == false || int.TryParse(parameter[1], out Integer) == false)
                            {
                                int paramValue1;
                                int paramValue2;

                                if (int.TryParse(parameter[0], out Integer) == false)
                                {
                                    int position1 = Vname.IndexOf(parameter[0].ToLower());
                                    if (position1 > -1)
                                    {
                                        paramValue1 = Vvalue[position1];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown first parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue1 = Int16.Parse(parameter[0]);
                                }
                                if (int.TryParse(parameter[1], out Integer) == false)
                                {
                                    int position2 = Vname.IndexOf(parameter[1].ToLower());
                                    if (position2 > -1)
                                    {
                                        paramValue2 = Vvalue[position2];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown second parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue2 = Int16.Parse(parameter[1]);
                                }
                                if (paramValue1 != 0 && paramValue2 != 0)
                                {
                                    //Drawrectangle method is called
                                    myCanvas.DrawRectangle(color, fill, paramValue1, paramValue2);
                                    i++;
                                }

                                else
                                {
                                    throw new Exception("Unknown Variable!");
                                    i++;
                                }
                            }
                            else
                            {
                                //Drawrectangle method is called
                                myCanvas.DrawRectangle(color, fill, Int16.Parse(parameter[0]), Int16.Parse(parameter[1]));

                                i++;

                            }

                        }
                    }
                    //If command  = to reset
                    else if (command.Equals("reset") == true)

                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            //Reset method called
                            myCanvas.Reset();
                            i++;
                        }
                    }
                    // If command  = clear
                    else if (command.Equals("clear") == true)

                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            //Clear method  called
                            myCanvas.Clear();
                            i++;
                        }
                    }

                    //Checks if command is triangle and if array partion is > 1
                    else if (command.Equals("triangle") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            //Splits parameter using ','
                            string[] parameter = vs2[1].Split(",");

                            //Four paremter is required for the triangle method
                            if (parameter.Length != 4)
                            {
                                //Reports invalid parameter
                                throw new Exception("Invalid parameter. Triangle command requires 4 parameter");
                                i++;
                            }
                            else if (int.TryParse(parameter[0], out Integer) == false || int.TryParse(parameter[1], out Integer) == false || int.TryParse(parameter[2], out Integer) == false || int.TryParse(parameter[3], out Integer) == false)
                            {
                                int paramValue1;
                                int paramValue2;
                                int paramValue3;
                                int paramValue4;

                                if (int.TryParse(parameter[0], out Integer) == false)
                                {
                                    int position1 = Vname.IndexOf(parameter[0].ToLower());
                                    if (position1 > -1)
                                    {
                                        paramValue1 = Vvalue[position1];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown first parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue1 = Int16.Parse(parameter[0]);
                                }

                                if (int.TryParse(parameter[1], out Integer) == false)
                                {
                                    int position2 = Vname.IndexOf(parameter[1].ToLower());
                                    if (position2 > -1)
                                    {
                                        paramValue2 = Vvalue[position2];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown second parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue2 = Int16.Parse(parameter[1]);
                                }

                                if (int.TryParse(parameter[2], out Integer) == false)
                                {
                                    int position3 = Vname.IndexOf(parameter[2].ToLower());
                                    if (position3 > -1)
                                    {
                                        paramValue3 = Vvalue[position3];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown third parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue3 = Int16.Parse(parameter[2]);
                                }

                                if (int.TryParse(parameter[3], out Integer) == false)
                                {
                                    int position4 = Vname.IndexOf(parameter[3].ToLower());
                                    if (position4 > -1)
                                    {
                                        paramValue4 = Vvalue[position4];
                                    }
                                    else
                                    {
                                        throw new Exception("Varible does not exist or Unknown fourth parameter");
                                        i++;
                                    }
                                }
                                else
                                {
                                    paramValue4 = Int16.Parse(parameter[3]);
                                }

                                if (paramValue1 != 0 && paramValue2 != 0 && paramValue3 != 0 && paramValue4 != 0)
                                {
                                    // DrawTriangle method called
                                    myCanvas.DrawTriangle(color, fill, paramValue1, paramValue2, paramValue3, paramValue4);
                                    i++;
                                }
                                else
                                {
                                    throw new Exception("Unknown variable");
                                    i++;
                                }
                            }

                            else
                            {
                                //Drawtriangle method called
                                myCanvas.DrawTriangle(color, fill, Int16.Parse(parameter[0]), Int16.Parse(parameter[1]), Int16.Parse(parameter[2]), Int16.Parse(parameter[3]));
                                i++;

                            }
                        }
                    }
                    // Checks if command is circle and if the first array partition is >  1
                    else if (command.Equals("circle") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            // If array lenght is > 2 an exception is thrown
                            if (vs2.Length != 2)
                            {
                                //Reports invalid parameter
                                throw new Exception("Invalid parameter. Circle requires 1 parameter");
                                i++;
                            }
                            //Checks if parameter is an int 
                            else if (int.TryParse(vs2[1], out Integer) == false)
                            {
                                int position1 = Vname.IndexOf(vs2[1].ToLower());
                                if (position1 > -1)
                                {
                                    int paramValue1 = Vvalue[position1];
                                    //Drawcircle method is called
                                    myCanvas.DrawCircle(color, fill, paramValue1);
                                    i++;
                                }
                                else
                                {
                                    throw new Exception("Unknown variable");
                                    i++;
                                }

                            }
                            else
                            {
                                //Drawcircle methodf is called
                                myCanvas.DrawCircle(color, fill, Int16.Parse(vs2[1]));
                                i++;
                            }
                        }
                    }
                    // Checks if the comand = colours and if array partition > 1
                    else if (command.Equals("colour") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            //If array length > 2 and exception is thrown
                            if (vs2.Length != 2)
                            {
                                throw new Exception("Invalid parameter. Colour command required 1 parameter");
                                i++;
                            }
                            // Check if colour parameter is red,blue, and yellow
                            else if (vs2[1].ToLower() == "black" || vs2[1].ToLower() == "red" || vs2[1].ToLower() == "blue" || vs2[1].ToLower() == "yellow")
                            {
                                if (vs2[1].ToLower() == "black")
                                {
                                    color = Color.Black;
                                    i++;
                                }

                                if (vs2[1].ToLower() == "red")
                                {
                                    // Color is set to red, if second array is partition is red
                                    color = Color.Red;
                                    i++;
                                }
                                if (vs2[1].ToLower() == "blue")
                                {
                                    // Color is set to blue, if second array is partition is blue
                                    color = Color.Blue;
                                    i++;
                                }
                                if (vs2[1].ToLower() == "yellow")
                                {
                                    // Color is set to red, if second array is partition is yellow
                                    color = Color.Yellow;
                                    i++;
                                }
                            }
                            else
                            {
                                //Reports inavlid parameter
                                throw new Exception("Invalid colour. Colour should be red,blue or yellow");
                                i++;
                            }
                        }

                    }

                    // Checks if command = fill and if array partion >1
                    else if (command.Equals("fill") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            // if array length > 2, an exception is thrown.
                            if (vs2.Length != 2)
                            {
                                // Reports invalid parameter with line number
                                throw new Exception("Invalid parameter. Fill command requires 1 parameter");
                                i++;
                            }
                            else if (vs2[1].ToLower() == "off" || vs2[1].ToLower() == "on")
                            {
                                if (vs2[1].ToLower() == "off")
                                {
                                    //fill is off; if the second array partition is off
                                    fill = 0;
                                    i++;
                                }
                                else if (vs2[1].ToLower() == "on")
                                {
                                    //fill is on; if the second array partition is on
                                    fill = 1;
                                    i++;
                                }
                            }
                            else
                            {
                                throw new Exception("Fill parameter should be on or off");
                                i++;
                            }
                        }

                    }
                    // Checks if command = while  and if array partion > 1
                    else if (command.Equals("while") && vs2.Length > 1)
                    {
                        //Four parameter is required for the while command
                        if (vs2.Length != 4)
                        {
                            //Reports invalid command with linenumber
                            throw new Exception("Invalid While loop command. While command required 3 parameter");
                            i++;
                        }
                        //Checks if the first  parameter array is an int
                        else if (int.TryParse(vs2[3], out Integer) == false)
                        {
                            //Reports invalid parameters
                            throw new Exception("Parameter should be an integer value");
                            i++;

                        }
                        else
                        {
                            int position1 = Vname.IndexOf(vs2[1].ToLower());
                            if (position1 > -1)
                            {
                                string paramValue1 = Vvalue[position1].ToString();
                                string paramValue2 = paramValue1 + " " + vs2[2] + " " + vs2[3];

                                //bool result1 = (bool)datatable.Compute(paramValue2, null);
                                bool result1 = (bool)datatable.Compute(paramValue2, "");
                                if (result1 == false)
                                {
                                    While_flag = false;
                                    i++;
                                    continue;
                                }
                                else
                                {
                                    Counter = i;
                                    i++;
                                }
                            }
                            else
                            {
                                throw new Exception("Unknown variable");
                                i++;
                            }
                        }
                    }
                    // Checks if command = endwhile  
                    else if (command.Equals("endwhile") == true && vs2.Length > 0)
                    {
                        //Checks if returned parameter arrays lenght is != 1
                        if (vs2.Length != 1)
                        {
                            //Reports invalid endwhile command
                            throw new Exception("Invalid endwhile command. no parameter required");
                            i++;
                        }
                        else
                        {
                            if (While_flag == false)
                            {
                                While_flag = true;
                                i++;
                                continue;
                            }
                            else
                            {
                                //Returns to loop iteration
                                i = Counter;
                                continue;
                            }
                        }
                    }


                    // command = 'if'
                    else if (command.Equals("if") == true && vs2.Length > 1)
                    {
                        //Four parameter required
                        if (vs2.Length != 4)
                        {
                            //Reports invalid if command
                            throw new Exception("Invalid if command. If command requires 3 parameter");
                            i++;
                        }
                        else if (int.TryParse(vs2[3], out Integer) == false)
                        {
                            //Reports invalid parameter
                            throw new Exception("Parameter must be an int");
                            i++;
                        }
                        else
                        {
                            int position1 = Vname.IndexOf(vs2[1].ToLower());
                            if (position1 > -1)
                            {
                                string paramValue1 = Vvalue[position1].ToString();
                                string paramValue2 = paramValue1 + " " + vs2[2] + " " + vs2[3];

                                bool result2 = (bool)datatable.Compute(paramValue2, null);

                                if (result2 == false)
                                {
                                    If_flag = false;
                                    i++;
                                    continue;
                                }
                                else
                                {
                                    i++;
                                    continue;

                                }
                            }
                        }
                    }
                    // if command = 'endif'
                    else if (command.Equals("endif") == true && vs2.Length > 0)
                    {
                        //checks the return parameter array lenght is !=1
                        if (vs2.Length != 1)
                        {
                            //Reports invalid commands
                            throw new Exception("Invalid endif command. Endif command requires no parameters");
                            i++;
                        }
                        else
                        {
                            i++;
                            If_flag = true;
                        }
                    }
                    else if (vs2[1].Equals("=") == true && vs2.Length > 1)
                    {
                        if (If_flag == false || While_flag == false)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            if (int.TryParse(vs2[2], out Integer) == false)
                            {
                                //Expression variable
                                string[] parameter = vs2[2].Split('+');

                                int position1 = Vname.IndexOf(parameter[0].ToLower());

                                if (position1 > -1)
                                {
                                    string paramValue1 = Vvalue[position1].ToString();
                                    string paramValue2 = paramValue1 + "+" + parameter[1];
                                    var result1 = datatable.Compute(paramValue2, null);
                                    int value = Int16.Parse(result1.ToString());
                                    Vvalue[position1] = value;
                                    i++;
                                }
                                else
                                {
                                    throw new Exception("Invalid variable declaration");
                                    i++;
                                }

                            }
                            else
                            {
                                // Variable Declarartion
                                int value = Int16.Parse(vs2[2]);
                                int f = CheckVariable(command);

                                if (f == 1)
                                {
                                    Vname.Add(command);
                                    Vvalue.Add(value);
                                    i++;
                                }
                                else
                                {
                                    //go through the names of the variables using their index positions
                                    for (int j = 0; j < Vname.Count(); j++)
                                    {
                                        // if the current variable matches the one we want
                                        if (Vname[j].Equals(command))
                                        {
                                            // updates the variables value
                                            Vvalue[j] = value;
                                        }
                                    }
                                    i++;
                                }

                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid syntax command");
                        i++;
                    }





                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("error on line{0} :{1}", (i + 1), e.Message));
                    i++;
                }
            }

        }

        //Method for executing a user enetered command within a string
        public void CommandLine(string script, int i)
        {
            string line = script.Trim().ToLower();
            string[] vs2 = line.Split(' ');
        }
        /// <summary>
        /// creates a checkvariacble method 
        /// </summary>
        /// <param name="command"> String passed from the command</param>
        /// <returns></returns>
        public int CheckVariable(string command)
        {
            if (Vname == null)
            {
                return 1;

            }
            else
            {
                if (Vname.Contains(command))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}











