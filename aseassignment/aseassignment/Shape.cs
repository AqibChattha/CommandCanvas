using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    /// <summary>
    /// The parent class for all shapes. It contains the color and the x and y coordinates of the shape.
    /// </summary>
    abstract class Shape : Shapes
    {
        // The color of the shape.
        protected Color color;
        // The x coordinate of the shape.
        // The y coordinate of the shape.
        protected int x, y;

        /// <summary>
        /// The constructor for the shape class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Shape(Color color, int x, int y)
        {
            this.color = color;
            this.x = x; this.y = y;
        }

        /// <summary>
        /// The method that draws the shape.
        /// </summary>
        /// <param name="g"></param>
        public abstract void draw(Graphics g);

        /// <summary>
        /// The method that gets the string representation of the shape.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + "    " + this.x + "," + this.y + ":";
        }

    }
}
