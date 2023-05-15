using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    /// <summary>
    /// The triangle class. It inherits from the shape class. It draws the triangle on the canvas.
    /// </summary>
    public class Triangle : Shape
    {
        // The size of the triangle.
        int size;

        /// <summary>
        /// The constructor of the triangle class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        public Triangle(Color color, int x, int y, int size) : base(color, x, y)
        {
            this.size = size;

        }

        /// <summary>
        /// The draw method of the triangle class. It draws the triangle on the canvas in both outlined and filled format.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            // pen will only draw the outlining line of the shape
            Pen pen = new Pen(color, (float)1.3);
            // brush will fill the shape of the triangle with the given color
            SolidBrush b = new SolidBrush(color);
            int halfSize = size / 2;
            int xvar = x - halfSize;
            int yvar = y - halfSize;
            // Setting the three points of a triangle
            Point[] points = new Point[3];
            points[0] = new Point(xvar, yvar);
            points[1] = new Point(xvar + size, yvar);
            points[2] = new Point(xvar + size / 2, yvar + size);
            g.FillPolygon(b, points);
            g.DrawPolygon(pen, points);
            pen.Dispose();
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
            int halfSize = size / 2;
            int xvar = x - halfSize;
            int yvar = y - halfSize;
            // Setting the three points of a triangle
            Point[] points = new Point[3];
            points[0] = new Point(xvar, yvar);
            points[1] = new Point(xvar + size, yvar);
            points[2] = new Point(xvar + size / 2, yvar + size);
            g.FillPolygon(b, points);
            b.Dispose();
        }

        /// <summary>
        /// This method draws a triangel shape outlined with the given color.
        /// </summary>
        /// <param name="g"></param>
        public void drawOutLined(Graphics g)
        {
            // pen will only draw the outlining line of the shape
            Pen pen = new Pen(color, (float)1.3);
            int halfSize = size / 2;
            int xvar = x - halfSize;
            int yvar = y - halfSize;
            // Setting the three points of a triangle
            Point[] points = new Point[3];
            points[0] = new Point(xvar, yvar);
            points[1] = new Point(xvar + size, yvar);
            points[2] = new Point(xvar + size / 2, yvar + size);
            g.DrawPolygon(pen, points);
            pen.Dispose();
        }

        /// <summary>
        /// This method will return the string representation of the triangle shape.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + "    " + this.size;
        }
    }
}
