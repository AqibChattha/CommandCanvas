using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace aseassignment
{
    /// <summary>
    /// The rectangle class. It inherits from the shape class. It draws the rectangle on the canvas.
    /// </summary>
    class Rectangle : Shape
    {
        // The width of the rectangle.
        // The height of the rectangle.
        int width, height;

        /// <summary>
        /// Constructor for the rectangle class. The initial position of the rectangle is the top left corner.
        /// width and height are the width and height of the rectangle.
        /// We substract half the width and height from the x and y coordinates to make a rectangle with cursor at the center.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(Color color, int x, int y, int width, int height) : base(color, x - (width / 2), y - (height / 2))
        {
            this.width = width;
            this.height = height;

        }

        /// <summary>
        /// The draw method of the rectangle class. It draws the rectangle on the canvas in both outlined and filled format.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            // pen will only draw the outlining line of the shape
            Pen penb = new Pen(color, (float)1.3);
            // brush will fill the shape of the triangle with the given color
            SolidBrush b = new SolidBrush(color);
            // drawing the recatangle
            g.FillRectangle(b, x, y, width, height);
            g.DrawRectangle(penb, x, y, width, height);
            penb.Dispose();
            b.Dispose();
        }

        /// <summary>
        /// This method draws a triangel shape filled with the given color.
        /// </summary>
        /// <param name="g"></param>
        public void drawFilled(Graphics g)
        {
            // brush will fill the shape of the triangle with the given color
            SolidBrush b = new SolidBrush(color);
            // drawing the recatangle
            g.FillRectangle(b, x, y, width, height);
            b.Dispose();
        }

        /// <summary>
        /// This method draws a triangel shape outlined with the given color.
        /// </summary>
        /// <param name="g"></param>
        public void drawOutLined(Graphics g)
        {
            // pen will only draw the outlining line of the shape
            Pen penb = new Pen(color, (float)1.3);
            // drawing the recatangle
            g.DrawRectangle(penb, x, y, width, height);
            penb.Dispose();
        }

        public override string ToString()
        {
            return base.ToString() + "    " + this.width + "," + this.height;
        }
    }
}
