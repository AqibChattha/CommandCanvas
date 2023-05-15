using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    /// <summary>
    /// The circle class. It inherits from the shape class. It draws the circle on the canvas.
    /// </summary>
    public class Circle : Shape
    {
        // The radius of the circle.
        int radius;

        /// <summary>
        /// Constructor for the circle class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public Circle(Color color, int x, int y, int radius) : base(color, x, y)
        {
            this.radius = radius;

        }

        /// <summary>
        /// The draw method of the circle class. It draws the circle on the canvas in both outlined and filled format.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            // pen will only draw the outlining line of the shape
            int halfRadius = radius / 2;
            Pen penb = new Pen(color, (float) 1.3);
            // brush will fill the shape of the circle with the given color
            SolidBrush b = new SolidBrush(color);
            // drawing the circle
            g.FillEllipse(b, x - halfRadius, y - halfRadius, radius, radius);
            g.DrawEllipse(penb, x - halfRadius, y - halfRadius, radius, radius);
            penb.Dispose();
            b.Dispose();

        }

        /// <summary>
        /// This method draws a circle shape filled with the given color.
        /// </summary>
        /// <param name="g"></param>
        public void drawFilled(Graphics g)
        {
            int halfRadius = radius / 2;
            // brush will fill the shape of the circle with the given color
            SolidBrush b = new SolidBrush(color);
            // drawing the circle
            g.FillEllipse(b, x - halfRadius, y - halfRadius, radius, radius);
            b.Dispose();
        }

        /// <summary>
        /// This method draws a circle shape outlined with the given color.
        /// </summary>
        /// <param name="g"></param>
        public void drawOutLined(Graphics g)
        {
            int halfRadius = radius / 2;
            // pen will only draw the outlining line of the shape
            Pen penb = new Pen(color, (float)1.3);
            // drawing the circle
            g.DrawEllipse(penb, x - halfRadius, y - halfRadius, radius, radius);
            penb.Dispose();
        }

        /// <summary>
        /// This method will return the string representation of the circle shape.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + "    " + this.radius;
        }
    }
}
