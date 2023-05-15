using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace aseassignment2

{/// <summary>
/// An area that controls all the drawing element
/// </summary>
    public class Canvas

    {
        Graphics g;
        Pen Pen;

        int xPos, yPos; // Pen position



        public Canvas(Graphics g)
        {
            this.g = g;

            xPos = yPos = 0;
        }

        /// <summary>
        /// Draws a line connecting two position specifed by two coordinate pairs
        /// </summary>
        /// <param name="colour">pen colour</param>
        /// <param name="toX"> the first position to connect</param>
        /// <param name="toY">the second postion to connect</param>
        public void DrawLine(Color colour, int toX, int toY)
        {
            Pen = new Pen(colour);
            g.DrawLine(Pen, xPos, yPos, toX, toY);
            xPos = toX;
            yPos = toY;

        }
        /// <summary>
        ///Draws a rectangle specified by coordinate of width and height
        /// 
        /// </summary>
        /// <param name="colour">Pen colour</param>
        /// <param name="width">The width of rectangle</param>
        /// <param name="height">The height of rectangle</param>
        public void DrawRectangle(Color colour, int fill, int width, int height)
        {
            int filled = fill;

            if (filled == 0)
            {
                Pen = new Pen(colour);
                g.DrawRectangle(Pen, xPos, yPos, xPos + width, yPos + height);
            }
            else if (filled == 1)
            {
                SolidBrush solidBrush = new SolidBrush(colour);
                g.FillRectangle(solidBrush, xPos, yPos, xPos + width, yPos + height);
            }


        }
        /// <summary>
        /// Draws a circle specified by radius 
        /// </summary>
        /// <param name="colour">pen colour</param>
        /// <param name="radius">the radius of the circle</param>
        /// <param name="fill">specifies the fill on/off</param>
        public void DrawCircle(Color colour, int fill, int radius)
        {
            int filled = fill;
            if (filled == 0)
            {
                Pen = new Pen(colour);
                g.DrawEllipse(Pen, xPos, yPos, radius, radius);
            }
            else if (filled == 1)
            {
                SolidBrush solidBrush = new SolidBrush(colour);
                g.FillEllipse(solidBrush, xPos, yPos, radius, radius);
            }
        }

        /// <summary>
        /// Draws a triangle specified from coordinates
        /// </summary>
        /// <param name="colour">Pen colour</param>
        /// <param name="fill">specifies fill on/off </param>
        /// <param name="l1">The middle x-axix coordinates</param>
        /// <param name="l2">The middle y-axix coordinates</param>
        /// <param name="w1">The ending x-axix coordinates</param>
        /// <param name="w2">the ending y-axix coordinates</param>
        public void DrawTriangle(Color colour, int fill, int l1, int l2, int w1, int w2)

        {
            int filled = fill;

            if (filled == 0)
            {
                Pen p = new Pen(colour);
                Point[] points = { new Point(xPos, yPos), new Point(l1, l2), new Point(w1, w2) };
                g.DrawPolygon(p, points);
            }
            else if (filled == 1)
            {
                SolidBrush solidBrush = new SolidBrush(colour);
                Point[] points = { new Point(xPos, yPos), new Point(l1, l2), new Point(w1, w2) };
                g.FillPolygon(solidBrush, points);

            }
        }
        /// <summary>
        /// Resets pen postion to the starting point
        /// </summary>
        public void Reset()
        {
            xPos = yPos = 0;
        }
        /// <summary>
        /// Moves pen postion specified by two coordinates
        /// </summary>
        /// <param name="toX">pen position</param>
        /// <param name="toY">pen postion</param>
        public void Moveto(int toX, int toY)
        {
            xPos = toX;
            yPos = toY;
        }

        /// <summary>
        /// clears the display window and fill the background colour with gray
        /// </summary>
        public void Clear()
        {
            g.Clear(Color.Gray);
        }




    }
}
