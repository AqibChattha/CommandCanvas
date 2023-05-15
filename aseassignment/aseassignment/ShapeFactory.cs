using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{

    /// <summary>
    /// It is the canvas class. 
    /// It consists of a Graphics object which is used to draw the shapes on the canvas.
    /// Is also has a bitmap which as the name suggests is used to manipulate the pixels of the canvas.
    /// Note: The canvas mentioned here is the picturebox on which the shapes are drawn.
    /// It has basic methods like moveto (to move on the canvas),
    /// drawto (to draw a line on the canvas), clear (to clear the canvas), etc.
    /// It also contains the methods that draw the 3 basic shapes (circle, triangle and rectangle).
    /// </summary>
    public class ShapeFactory
    {
        // This graphic object is used to draw different shapes on the canvas. 
        Graphics g;

        // x and y corrdinates of the cursor on the canvas.
        // The cursor shows your location on the canvas and from where you can start drawing.
        public int xPos, yPos;

        // The 2 dimensional array that contains the pixels of the cursor on the canvas.
        // It is used to store the pixels of the spot the cursor is moved on, so that it can be restored later.
        Color[,] cursor;

        // It is the bitmap that is used to manipulate the pixels of the canvas.
        // It contains all the pixels of the canvas in the form of a 2 dimensional array of colors.
        Bitmap bitmap;


        /// <summary>
        /// Constructor for the canvas class.
        /// It takes the picturebox as a parameter and uses it to initialize the bitmap and the graphics objects,
        /// because we want to make the shapes inside the given picturebox.
        /// </summary>
        /// <param name="pictureBox">The instance of picturebox in which the shapes and drawing is to be drawn</param>
        public ShapeFactory(PictureBox pictureBox)
        {
            // Set the initial position of the cursor to (0,0) cordinated on canvas (or picturebox)
            xPos = yPos = 0;

            // Make an object of 2 dimensional pixel array, size same as of picturebox. Store it in bitmap.
            bitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);

            // Set the 2 dimensional pixel array of the bitmap to the 2 dimensional pixel array of the picturebox.
            // Image is also like a 2 dimensional array of pixels.
            // Bitmap is also the implementation of Image so we can store bitmap in Image.
            pictureBox.Image = bitmap;

            // Get the graphics object of the picturebox.
            // Now the graphics object can be used to draw the shapes on the canvas (picturebox).
            this.g = Graphics.FromImage(bitmap);

            // Initialize th curor 2 dimensional array. You can make the cursor of your own size preference.
            // but make sure you also change the support methods (RemoveCursor, DrawCursor and SaveStateBeforeCursor) accordingly.
            cursor = new Color[3, 3];
            
            // This method is used to store the pixels where the cursor will be drawn on.
            SaveStateBeforeCursor();

            // This will move the cursor to the initial position (0,0) on the canvas.
            MoveTo(xPos, yPos);
        }

        /// <summary>
        /// Method to remove the cursor pixels and restore that point's state where the cursor was on.
        /// It will use the cursor 2 dimensional array to restore the pixels which are stored before moving the cursor tha location.
        /// </summary>
        private void RemoveCursor()
        {
            // x and y cordinate of the cursor image, to remove the cursor as a rectangle of 3x3 pixels
            // around the current location pointer we subtract 1 from the both xPos and yPos. 
            int x = xPos - 1, y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Set the pixel at (x,y) cordinates to the color stored in the cursor,
                    // 2 dimensional array to restore the canvas without pointer.
                    // In some cases like the when starting, the pixel may not be in range So,
                    // it's written in try catch to catch any unecessory exception.
                    try { bitmap.SetPixel(x + i, y + j, cursor[i, j]); } catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// Method to draw the cursor on the current location.
        /// It will use the bitmap to draw the cursor in 3x3 rectangle around the current location.
        /// </summary>
        private void DrawCursor()
        {
            // x and y cordinate of the cursor image, to draw the cursor as a rectangle of 3x3 pixels
            // around the current location pointer we subtract 1 from the both xPos and yPos. 
            int x = xPos - 1, y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Leave the center of the cursor as it is.
                    if (i == 1 && j == 1) continue;
                    // Set the pixel at (x,y) cordinates to the color red as we use a red colored cursor.
                    // In some cases like the when starting, the pixel may not be in range So,
                    // it's written in try catch to catch any unecessory exception.
                    try { bitmap.SetPixel(x + i, y + j, Color.Red); } catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// Method to save the pixels of the location where the cursor is to be drawn.
        /// It will store all those pixels in the cursor 2 dimensional array which is the same size as our cursor.
        /// </summary>
        private void SaveStateBeforeCursor()
        {
            // x and y cordinate of the cursor image, to save pixels before the cursor as a rectangle of 3x3 pixels
            // around the current location pointer we subtract 1 from the both xPos and yPos. 
            int x = xPos - 1, y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Store the pixel at (x,y) cordinates in the cursor, 2 dimensional array to restore the canvas without cursor.
                    // In some cases like the when starting, the pixel may not be in range So,
                    // it's written in try catch to catch any unecessory exception and just assign it default color.
                    try { cursor[i, j] = bitmap.GetPixel(x + i, y + j); } catch (Exception) { cursor[i, j] = Color.DimGray; }
                }
            }
        }

        /// <summary>
        /// Method to move the cursor from the one location to another. It changes the cursor location and
        /// Uses the helper functions to remove the cursor from the old location and draw it on the new location without affecting the canvas.
        /// </summary>
        /// <param name="toX">Destination X cordiante in the canvas</param>
        /// <param name="toY">Destination Y cordiante in the canvas</param>
        public void MoveTo(int toX, int toY)
        {
            // Remove the cursor from the old location. It also restore the canvas.
            RemoveCursor();

            // Change the location of the cursor to the given location
            xPos = toX;
            yPos = toY;

            // At the new location, save the pixels of the location where the cursor is to be drawn.
            SaveStateBeforeCursor();

            // Draw the cursor on the new location.
            DrawCursor();
        }

        /// <summary>
        /// Method to draw a line from the current cursor cordinates to the destination cordinates. It also changes the cursor location and
        /// Uses the helper functions to remove the cursor from the old location and draw it on the new location without affecting the canvas.
        /// </summary>
        /// <param name="toX">Destination X cordiante in the canvas</param>
        /// <param name="toY">Destination Y cordiante in the canvas</param>
        /// <param name="penColor">The color of the line to be drawn</param>
        public void DrawTo(int toX, int toY, String penColor = "black")
        {
            // Make a object of Pen class to draw the line. Make it of default color black.
            Pen pen = new Pen(Color.Black, (float)1.3);

            // If the color is given, change the color of the pen.
            if (penColor.Equals("red")) { pen.Color = Color.Red; }
            else if (penColor.Equals("green")) { pen.Color = Color.Green; }

            // Remove the cursor from the old location. It also restore the canvas.
            RemoveCursor();

            // Draw the line from the current cursor cordinates to the destination cordinates on canvas.
            g.DrawLine(pen, xPos, yPos, toX, toY);

            // Change the location of the cursor to the given location
            xPos = toX;
            yPos = toY;

            // At the new location, save the pixels of the location where the cursor is to be drawn.
            SaveStateBeforeCursor();
            
            // Draw the cursor on the new location.
            DrawCursor();
        }

        /// <summary>
        /// Draw a circle of the given radius on the canvas with the cursor at the center.
        /// The color of the circle is given by the penColor.
        /// The circle can be filled if isFilled is "on".
        /// </summary>
        /// <param name="radius">Radius of the circle, the distance from boundry to the cursor</param>
        /// <param name="penColor">The color of which the circle will be drawn, default is black</param>
        /// <param name="isFilled">If "on" is passed the circle can be filled, otherwise by default "none" it is outlined</param>
        public void DrawCircle(int radius, String penColor = "black", String isFilled = "none")
        {
            // Make a object of Color class. Make it of default color black.
            Color color = Color.Black;

            // If the color is given, change the color of the pen.
            if (penColor.Equals("red")) { color = Color.Red; }
            else if (penColor.Equals("green")) { color = Color.Green; }

            // Remove the cursor from the old location. It also restore the canvas.
            RemoveCursor();

            // Create an object of Circle class to draw the circle. Give the radius, color and the cursor cordinates.
            Circle circle = new Circle(color, xPos, yPos, radius);

            // If the circle is to be filled, fill it.
            if (isFilled.Equals("on"))
            {
                // If the fill is "on" then draw a circle filled with color.
                circle.drawFilled(g);
            }
            else
            {
                // If the fill is "none" then by default draw a outlined circle.
                circle.drawOutLined(g);
            }

            // At the new location, save the pixels of the location where the cursor is to be drawn.
            SaveStateBeforeCursor();
            
            // Draw the cursor on the new location.
            DrawCursor();
        }

        /// <summary>
        /// Draw a triangle of the given size on the canvas with the cursor at the center.
        /// The color of the triangle is given by the penColor.
        /// The triangle can be filled if isFilled is "on".
        /// </summary>
        /// <param name="size">Size of the triangle, the distance from boundry to the cursor</param>
        /// <param name="penColor">The color of which the triangle will be drawn, default is black</param>
        /// <param name="isFilled">If "on" is passed the triangle can be filled, otherwise by default "none" it is outlined</param>
        public void DrawTriangle(int size, String penColor = "black", String isFilled = "none")
        {
            // Make a object of Color class. Make it of default color black.
            Color color = Color.Black;

            // If the color is given, change the color of the pen.
            if (penColor.Equals("red")) { color = Color.Red; }
            else if (penColor.Equals("green")) { color = Color.Green; }

            // Remove the cursor from the old location. It also restore the canvas.
            RemoveCursor();

            // Create an object of Triangle class to draw the triangle. Give the size, color and the cursor cordinates.
            Triangle triangle = new Triangle(color, xPos, yPos, size);

            // If the triangle is to be filled, fill it.
            if (isFilled.Equals("on"))
            {
                // If the fill is "on" then draw a triangle filled with color.
                triangle.drawFilled(g);
            }
            else
            {
                // If the fill is "none" then by default draw a outlined triangle.
                triangle.drawOutLined(g);
            }

            // At the new location, save the pixels of the location where the cursor is to be drawn.
            SaveStateBeforeCursor();

            // Draw the cursor on the new location.
            DrawCursor();
        }

        /// <summary>
        /// Draw a rectangle of the given width and height on the canvas with the cursor at the center.
        /// The color of the rectangle is given by the penColor.
        /// The rectangle can be filled if isFilled is "on".
        /// </summary>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        /// <param name="penColor">The color of which the rectangle will be drawn, default is black</param>
        /// <param name="isFilled">If "on" is passed the rectangle can be filled, otherwise by default "none" it is outlined</param>
        public void DrawRectangle(int width, int height, String penColor = "black", String isFilled = "none")
        {
            // Make a object of Color class. Make it of default color black.
            Color color = Color.Black;

            // If the color is given, change the color of the pen.
            if (penColor.Equals("red")) { color = Color.Red; }
            else if (penColor.Equals("green")) { color = Color.Green; }

            // Remove the cursor from the old location. It also restore the canvas.
            RemoveCursor();

            // Create an object of Rectangle class to draw the rectangle. Give the width, height, color and the cursor cordinates.
            Rectangle rectangle = new Rectangle(color, xPos, yPos, width, height);

            // If the rectangle is to be filled, fill it.
            if (isFilled.Equals("on"))
            {
                // If the fill is "on" then draw a rectangle filled with color.
                rectangle.drawFilled(g);
            }
            else
            {
                // If the fill is "none" then by default draw a outlined rectangle.
                rectangle.drawOutLined(g);
            }

            // At the new location, save the pixels of the location where the cursor is to be drawn.
            SaveStateBeforeCursor();

            // Draw the cursor on the new location.
            DrawCursor();
        }

        /// <summary>
        /// Method to clear the canvas (picturebox). It will clear all the drawings and restore the canvas to the default color.
        /// It will also reset the cursor to the initial (0,0) position.
        /// </summary>
        public void Clear()
        {
            // Clear all the drawing or shapes on the canvas and make all the pixels default dim grey color.
            g.Clear(Color.DimGray);
            
            // set the initial position of the cursor to (0,0) on canvas
            xPos = yPos = 0;
            
            // This method is used to store the pixels where the cursor will be drawn on.
            SaveStateBeforeCursor();

            // This will move the cursor to the initial position (0,0) on the canvas.
            MoveTo(xPos, yPos);
        }
        

        /// <summary>
        /// Destructor for the canvas class
        /// </summary>
        ~ShapeFactory()
        {
            g.Dispose();
        }
    }
}
