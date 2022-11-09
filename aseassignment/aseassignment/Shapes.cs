using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    /// <summary>
    /// The main entry point for the application. The interface for different types of shapes.
    /// </summary>
    interface Shapes
    {
        /// <summary>
        /// The method that draws the shape.
        /// </summary>
        /// <param name="g"></param>
        void draw(Graphics g);
    }
}
