using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{

    /// <summary>
    /// The canvas class that contains the shapes. It also contains the methods that draw the shapes and the method that gets the string representation of the canvas.
    /// </summary>
    class Canvas
    {
        // The list of shapes. 
        Graphics g;
        // the x corrdinate of the cursor on the canvas
        // the y corrdinate of the cursor on the canvas
        public int xPos, yPos;
        // the 2D array that contains the pixels of the cursor
        Color[,] colors;
        // the bit representation of the canvas
        Bitmap bitmap;


        /// <summary>
        /// Constructor for the canvas class.
        /// </summary>
        /// <param name="pictureBox"></param>
        public Canvas(PictureBox pictureBox)
        {
            // set the initial position of the cursor to (0,0) on canvas
            xPos = yPos = 0;

            // set the bit representation of the canvas to the given picturebox
            bitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);

            // Set the image of the picturebox to the pixelated map
            pictureBox.Image = bitmap;

            // now assign the Graphics
            this.g = Graphics.FromImage(bitmap);

            // make a 2D array of the pointer pixels
            colors = new Color[3, 3];


            // save the state of the pixels before printing pointer
            saveStateBeforePointing();
            
            // now move the cursor to the given positions
            MoveTo(xPos, yPos);
        }

        /// <summary>
        /// Method to remove the pointer pixels and restore that point's state
        /// </summary>
        private void removePointer()
        {
            // the starting x cordinate of the pointer
            int x = xPos - 1;
            // the starting y cordinate of the pointer
            int y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    try { bitmap.SetPixel(x + i, y + j, colors[i, j]); } catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// Method to draw the pointer on the given location
        /// </summary>
        private void drawPointer()
        {
            // the starting x cordinate of the pointer
            int x = xPos - 1;
            // the starting y cordinate of the pointer
            int y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1) continue;
                    try { bitmap.SetPixel(x + i, y + j, Color.Red); } catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// Method to save the state of the location same as the size of the pointer.
        /// </summary>
        private void saveStateBeforePointing()
        {
            // the starting x cordinate of the pointer
            int x = xPos - 1;
            // the starting y cordinate of the pointer
            int y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    try { colors[i, j] = bitmap.GetPixel(x + i, y + j); } catch (Exception) { colors[i, j] = Color.DimGray; }
                }
            }
        }

        /// <summary>
        /// Method to move the pointer from the one location to another.
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        public void MoveTo(int toX, int toY)
        {
            // firstly remove the pointer and restore the canvas state
            removePointer();

            // now move the the destination
            xPos = toX;
            yPos = toY;

            // save the state of the destination
            saveStateBeforePointing();

            // draw the pointer on the destination
            drawPointer();
        }

        /// <summary>
        /// Draw a line from the current loaction to the destination.
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="penColor"></param>
        public void DrawTo(int toX, int toY, String penColor = "black")
        {
            // pen of the given of default color to draw the line to the destination
            Pen pen = new Pen(Color.Black, (float)1.3);
            if (penColor.Contains("red"))
            {
                pen = new Pen(Color.Red, (float)1.3);
            }
            else if (penColor.Contains("green"))
            {
                pen = new Pen(Color.Green, (float)1.3);
            }
            // remove the pointer and restore canvas
            removePointer();
            // draw the line from the current location to the destination on canvas
            g.DrawLine(pen, xPos, yPos, toX, toY);
            // now move to the destination
            xPos = toX;
            yPos = toY;
            // save the state of the destination
            saveStateBeforePointing();
            // draw the pointer on the destination
            drawPointer();
        }

        /// <summary>
        /// Draw a circle of the given parameters on the circle.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="penColor"></param>
        /// <param name="isFilled"></param>
        public void DrawCircle(int radius, String penColor = "black", String isFilled = "none")
        {
            // set the color of the shape to default or given choice.
            Color color = Color.Black;
            if (penColor.Contains("red"))
            {
                color = Color.Red;
            }
            else if (penColor.Contains("green"))
            {
                color = Color.Green;
            }
            // remove the pointer and restore canvas
            removePointer();
            // getting the half radius to draw the circle with pointer at center
            int halfRadius = radius / 2;

            // create a circle object
            Circle circle = new Circle(color, xPos, yPos, radius);

            if (isFilled.Equals("on"))
            {
                // if the fill is "on" then draw a filled circle
                circle.drawFilled(g);
            }
            else
            {
                // If no fill is given of os "none" then by default draw a outlined circle
                circle.drawOutLined(g);
            }
            // save the state of the destination
            saveStateBeforePointing();
            // draw the pointer on the destination
            drawPointer();
        }

        public void DrawTriangle(int size, String penColor = "black", String isFilled = "none")
        {
            // set the color of the shape to default or given choice.
            Color color = Color.Black;
            if (penColor.Contains("red"))
            {
                color = Color.Red;
            }
            else if (penColor.Contains("green"))
            {
                color = Color.Green;
            }
            // remove the pointer and restore canvas
            removePointer();

            // create a triangle object
            Triangle triangle = new Triangle(color, xPos, yPos, size);

            if (isFilled.Equals("on"))
            {
                // if the fill is "on" then draw a filled triangle
                triangle.drawFilled(g);
            }
            else
            {
                // If no fill is given of os "none" then by default draw a outlined triangle
                triangle.drawOutLined(g);
            }
            // save the state of the destination
            saveStateBeforePointing();
            // draw the pointer on the destination
            drawPointer();
        }

        public void DrawRectangle(int width, int height, String penColor = "black", String isFilled = "none")
        {
            // set the color of the shape to default or given choice.
            Color color = Color.Black;
            if (penColor.Contains("red"))
            {
                color = Color.Red;
            }
            else if (penColor.Contains("green"))
            {
                color = Color.Green;
            }
            // remove the pointer and restore canvas
            removePointer();
            // setting the parameters to draw the rectangle with the pointer as center
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            // create a rectangle object
            Rectangle rectangle = new Rectangle(color, xPos - halfWidth, yPos - halfHeight, width, height);

            if (isFilled.Equals("on"))
            {
                // if the fill is "on" then draw a filled rectangle
                rectangle.drawFilled(g);
            }
            else
            {
                // If no fill is given of os "none" then by default draw a outlined rectangle
                rectangle.drawOutLined(g);
            }
            // save the state of the destination
            saveStateBeforePointing();
            // draw the pointer on the destination
            drawPointer();
        }

        /// <summary>
        /// Method to clear the canvas.
        /// </summary>
        public void Clear()
        {
            g.Clear(Color.DimGray);
            // set the initial position of the cursor to (0,0) on canvas
            xPos = yPos = 0;
        }
        

        /// <summary>
        /// Destructor for the canvas class
        /// </summary>
        ~Canvas()
        {
            g.Dispose();
        }
    }
}
