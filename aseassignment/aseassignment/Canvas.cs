using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{

    class Canvas
    {

        Graphics g;
        Pen pointer;
        Pen remover;
        public int xPos, yPos;
        Color[,] colors;
        Bitmap bitmap;
        int imageWidth;
        int imageHeight;


        public Canvas(PictureBox pictureBox)
        {
            xPos = yPos = 0;
            imageWidth = pictureBox.Size.Width;
            imageHeight = pictureBox.Size.Height;
            
            pointer = new Pen(Color.Red, 1);
            remover = new Pen(Color.DimGray, 2);

            bitmap = new Bitmap(imageWidth, imageHeight);

            pictureBox.Image = bitmap;

            this.g = Graphics.FromImage(bitmap);

            colors = new Color[3, 3];


            int x = xPos - 1;
            int y = yPos - 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    try { colors[i, j] = bitmap.GetPixel(x + i, y + j); } catch (Exception) { colors[i, j] = Color.DimGray; }
                }
            }

            MoveTo(xPos, yPos);
        }

        private void removePointer()
        {
            int x = xPos - 1;
            int y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    try { bitmap.SetPixel(x + i, y + j, colors[i, j]); } catch (Exception) { }
                }
            }
        }

        private void drawPointer()
        {
            int x = xPos - 1;
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

        private void saveStateBeforePointing()
        {
            int x = xPos - 1;
            int y = yPos - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    try { colors[i, j] = bitmap.GetPixel(x + i, y + j); } catch (Exception) { colors[i, j] = Color.DimGray; }
                }
            }
        }

        public void MoveTo(int toX, int toY)
        {
            removePointer();

            xPos = toX;
            yPos = toY;

            saveStateBeforePointing();

            drawPointer();
        }

        public void DrawTo(int toX, int toY, String penColor = "black")
        {
            Pen pen = new Pen(Color.Black, (float)1.3);
            if (penColor.Contains("red"))
            {
                pen = new Pen(Color.Red, (float)1.3);
            }
            else if (penColor.Contains("green"))
            {
                pen = new Pen(Color.Green, (float)1.3);
            }
            removePointer();
            g.DrawLine(pen, xPos, yPos, toX, toY);
            xPos = toX;
            yPos = toY;

            saveStateBeforePointing();

            drawPointer();
        }

        public void DrawCircle(int radius, String penColor = "black", String isFilled = "none")
        {
            Color color = Color.Black;
            if (penColor.Contains("red"))
            {
                color = Color.Red;
            }
            else if (penColor.Contains("green"))
            {
                color = Color.Green;
            }
            removePointer();
            int halfRadius = radius / 2;

            Circle circle = new Circle(color, xPos, yPos, radius);

            if (isFilled.Equals("on"))
            {
                circle.drawFilled(g);
            }
            else
            {
                circle.drawOutLined(g);
            }

            drawPointer();
        }

        public void DrawTriangle(int size, String penColor = "black", String isFilled = "none")
        {
            Color color = Color.Black;
            if (penColor.Contains("red"))
            {
                color = Color.Red;
            }
            else if (penColor.Contains("green"))
            {
                color = Color.Green;
            }
            removePointer();
            Triangle triangle = new Triangle(color, xPos, yPos, size);

            if (isFilled.Equals("ok"))
            {
                triangle.drawFilled(g);
            }
            else
            {
                triangle.drawOutLined(g);
            }

            drawPointer();
        }

        public void DrawRectangle(int width, int height, String penColor = "black", String isFilled = "none")
        {
            Color color = Color.Black;
            if (penColor.Contains("red"))
            {
                color = Color.Red;
            }
            else if (penColor.Contains("green"))
            {
                color = Color.Green;
            }
            removePointer();
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            Rectangle rectangle = new Rectangle(color, xPos - halfWidth, yPos - halfHeight, width, height);

            if (isFilled.Equals("on"))
            {
                rectangle.drawFilled(g);
            }
            else
            {
                rectangle.drawOutLined(g);
            }
            drawPointer();
        }

        ~Canvas()
        {
            g.Dispose();
            pointer.Dispose();
            remover.Dispose();
        }
    }
}
