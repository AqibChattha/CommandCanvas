using aseassignment;
using System.Drawing;

namespace aseassignmentTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        /// <summary>
        /// Bitmap comparison
        /// </summary>
        /// <param name="bmp1">First bitmap</param>
        /// <param name="bmp2">First bitmap</param>
        /// <returns></returns>
        public bool CompareBitmapsLazy(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            //Compare bitmaps using GetPixel method
            for (int column = 0; column < bmp1.Width; column++)
            {
                for (int row = 0; row < bmp1.Height; row++)
                {
                    if (!bmp1.GetPixel(column, row).Equals(bmp2.GetPixel(column, row)))
                        return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Creates a valid unit test
        /// </summary>
        /// <param name="action">Takes function with the command parser as an argumnet</param>
        /// <param name="action1">Takes a function thatspassed a pen and graphics class</param>
        public void testHelp(Action<CommandParser> action, Action<System.Drawing.Graphics, System.Drawing.Pen> action1)
        {
            Bitmap bitmap1 = new Bitmap(640, 480);
            Bitmap bitmap2 = new Bitmap(640, 480);

            Graphics graphics1 = Graphics.FromImage(bitmap1);
            Graphics graphics2 = Graphics.FromImage(bitmap2);

            Canvas canvas = new Canvas(graphics2);
            commandParser parser = new commandParser(canvas);

            action(parser);
            action1(graphics1, new Pen(Color.Black));

            CompareBitmapsLazy(bitmap1, bitmap2);
        }
        /// <summary>
        /// Tests valid drawtocommand
        /// </summary>
        [TestMethod]

        public void parserClasstest()
        {
            testHelp(parser => { parser.programWindow("drawto 50,50"); },
                (g, pen) =>
                {
                    g.DrawLine(pen, 0, 0, 20, 20);
                }

                );
        }
        /// <summary>
        /// Test valid while command
        /// </summary>
        [TestMethod]
        public void WhileloopTest()
        {
            void action(commandParser parser)
            {
                using (StreamReader streamReader = File.OpenText("..\\..\\..\\Testing\\while.txt"))
                {
                    string reader = streamReader.ReadToEnd();
                    parser.programWindow(reader);
                }
            }
            void action1(Graphics graphics, Pen pen)
            {
                graphics.DrawEllipse(pen, 0, 0, 50, 50);
                graphics.DrawEllipse(pen, 0, 0, 60, 60);
                graphics.DrawEllipse(pen, 0, 0, 70, 70);

            }
            testHelp(action, action1);
        }
        /// <summary>
        /// Test valid moveto command
        /// </summary>
        [TestMethod]
        public void movetoTest()
        {
            void action(commandParser parser)
            {
                parser.programWindow("moveto 50,50");
                parser.programWindow("rectangle 10,40");
            }
            void action1(Graphics graphics, Pen pen)
            {
                graphics.DrawEllipse(pen, 0, 0, 70, 70);
            }
            testHelp(action, action1);
        }
        /// <summary>
        /// Test valid if statement
        /// </summary>
        [TestMethod]
        public void IfstatementTest()
        {
            void action(commandParser parser)
            {
                using (StreamReader streamReader = File.OpenText("..\\..\\..\\Testing\\if.txt"))
                {
                    string reader = streamReader.ReadToEnd();
                    parser.programWindow(reader);


                }
            }
            void action1(Graphics graphics, Pen pen)
            {
                graphics.DrawEllipse(pen, 0, 0, 50, 50);
                pen.Color = Color.Red;
                graphics.DrawEllipse(pen, 0, 0, 50, 100);
            }
            testHelp(action, action1);
        }
        /// <summary>
        /// Test valid variable
        /// </summary>
        [TestMethod]
        public void VaribleTest()
        {
            void action(commandParser parser)
            {
                using (StreamReader streamReader = File.OpenText("..\\..\\..\\Testing\\while.txt"))
                {
                    string reader = streamReader.ReadToEnd();
                    parser.programWindow(reader);
                }
            }

            void action1(Graphics graphics, Pen pen)
            {
                graphics.DrawLine(pen, 0, 0, 50, 50);
                graphics.DrawRectangle(pen, 50, 50, 70, 80);
                pen.Color = Color.FromArgb(50, 50, 200, 200);
                graphics.DrawRectangle(pen, 50, 60, 100, 100);
                pen.Color = Color.Red;
                graphics.DrawLine(pen, 100, 100, 200, 200);
            }
            testHelp(action, action1);
        }
        /// <summary>
        /// Valid clear test
        /// </summary>
        [TestMethod]
        public void ClearTest()
        {
            void action(commandParser parser)
            {
                parser.programWindow("fill on");
                parser.programWindow("circle 100");
                parser.programWindow("drawto 100,100");
                parser.programWindow("clear");

                parser.programWindow("rectangle 100 100");
            }
            void action1(Graphics graphics, Pen pen)
            {
                Brush brush = new SolidBrush(Color.FromArgb(100, 100, 150, 150));
                graphics.FillRectangle(brush, 50, 50, 100, 100);
            }
            testHelp(action, action1);
        }
    }
}
}