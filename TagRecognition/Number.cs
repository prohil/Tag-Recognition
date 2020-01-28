using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.OCR;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.UI;
namespace TagRecognition
{
    class Number
    {
        Rectangle rectangle;
        public bool NumberNear { get; set; }
        public Rectangle Rectangle { get { return rectangle; } }
        private Point Center
        {
            get
            {
                return new Point((int)Math.Round(rectangle.X + rectangle.Width / 2.0), (int)Math.Round(rectangle.Y + rectangle.Height / 2.0));
            }
        }
        public Number(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            NumberNear = false;
        }
        private bool IsPointBelong(Point point)
        {
                Point cornerLeftDown = new Point(rectangle.X, rectangle.Y);
                Point cornerRightUp = new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                if (cornerLeftDown.X < point.X && point.X < cornerRightUp.X &&
                    cornerLeftDown.Y < point.Y && point.Y < cornerRightUp.Y)
                {
                    return true;
                }
                else
                    return false;
        }
        private bool IsHeightEqual(Number number)
        {
            int
                rectHeight = Rectangle.Height,
                numberRectHeight = number.Rectangle.Height;
            if (0.8 * rectHeight < numberRectHeight && numberRectHeight < 1.2 * rectHeight)
                return true;
            else
                return false;
        }
        private bool IsWidthEqual(Number number)
        {
            int
                rectWidth = Rectangle.Width,
                numberRectWidth = number.Rectangle.Width;
            if (0.5 * rectWidth < numberRectWidth && numberRectWidth < 1.5 * rectWidth)
                return true;
            else
                return false;
        }
        //private bool IsNumberOneLine(Number number)
        //{
        //    if (Math.Abs(number.Center.Y - Center.Y) < (int)Math.Round(number.Rectangle.Height / 2.0))
        //        return true;
        //    else
        //        return false;
            //else
            //{
            //    if (Math.Abs(number.RotatedRectangle.Center.Y - RotatedRectangle.Center.Y) < (int)Math.Round(number.RotatedRectangle.Size.Height / 2.0))
            //        return true;
            //    else
            //        return false;
            //}
        //}
        //на вход - прямоугольник, который проверяют на то, что он рядом с цифрой
        public bool IsNumbersNear(Number number)
        {
                int distance = (int)Math.Round(rectangle.Width / 2.0);
                int y = (int)number.Center.Y;
                Point pointRight = new Point(
                    (int)Math.Round(distance + number.Center.X + number.Rectangle.Width / 2.0), y);
                Point pointLeft = new Point(
                    (int)Math.Round(number.Center.X - distance - number.Rectangle.Width / 2.0), y);
                if ((IsPointBelong(pointRight) || IsPointBelong(pointLeft)) && IsHeightEqual(number) && IsWidthEqual(number) /*&& IsNumberOneLine(number)*/)
                    return true;
                else
                    return false;
        }
    }
}
