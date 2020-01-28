//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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
    public class Recognition : Construction
    {
        private static Tesseract _ocr;
        //private static Tesseract ocr2;
        //добавил статик к методу и полю
        public static List<string> GetTags(string path, ref List<Rectangle> rectFaces, ref List<Rectangle> rectTags, out long detectionTime)
        {
            Mat image = CvInvoke.Imread(path, LoadImageType.AnyColor);
            List<string> tags = new List<string>();
            Stopwatch watch = Stopwatch.StartNew();
            string res = String.Empty;
            using (UMat ugray = new UMat())
            {
                CvInvoke.CvtColor(image, ugray, ColorConversion.Bgr2Gray);
                int height = image.Size.Height,
                    width = image.Size.Width;

                //if (height * width > 4000000 && ((height > 2000 && width > 2000)||(height > 1200 && width > 1200)))
                //{
                if (width > height)
                {
                    double k = (double)width / 1805;
                    CvInvoke.Resize(ugray, ugray, new Size(1805, (int)Math.Round(height / k)));
                }
                else
                {
                    double k = (double)height / 1250;
                    CvInvoke.Resize(ugray, ugray, new Size((int)Math.Round(width / k), 1250));
                }
                //}
                UMat pyr = new UMat();
                CvInvoke.PyrDown(ugray, pyr);
                CvInvoke.PyrUp(pyr, ugray);

                //ResizeAndShow("AfterEqualize", ugray);
                //use image pyr to remove noise
                List<Candidate> candidates = GetCandidates(ugray);
                //candidates.Sort()
                for (int i = 0; i < candidates.Count; i++)
                {
                    //
                    //if (candidates[i].Face.Height * candidates[i].Face.Width > 200 * 200 && candidates[i].Face.Height * candidates[i].Face.Width < 210 * 210)
                    //{
                    rectFaces.Add(candidates[i].Face);
                    rectTags.Add(candidates[i].Tag);
                    res = GetTag(candidates[i], ugray);
                    //}
                    if (res != String.Empty)
                        tags.Add(res);
                }
            }
            watch.Stop();
            detectionTime = watch.ElapsedMilliseconds;
            return tags;
        }
        private static List<Candidate> GetCandidates(UMat ugray)
        {
            List<Candidate> candidates = new List<Candidate>();
            List<Rectangle> faces = new List<Rectangle>();
            using (UMat img = new UMat())
            {
                CvInvoke.EqualizeHist(ugray, img);
                DetectFaces(img, "haarcascade_frontalface_default.xml", faces);
                foreach (Rectangle face in faces)
                {
                    if (face.Width * face.Height > 1000)
                    {
                        Point center = new Point((int)Math.Round(face.X + face.Width / 2.0), (int)Math.Round(face.Y + face.Height / 2.0));
                        int tagWidth = (int)Math.Round(2.4 * face.Width),
                            tagHeight = (int)Math.Round(3.5 * face.Height),
                            imgWidth = img.Size.Width,
                            imgHeight = img.Size.Height;
                        Point coordinateCorner = new Point((int)Math.Round(center.X - tagWidth / 2.0), face.Y + 2 * face.Height);
                        if (coordinateCorner.X < 0)
                            coordinateCorner.X = 0;
                        if (coordinateCorner.Y < 0)
                            coordinateCorner.Y = 0;
                        if (coordinateCorner.X > imgWidth || coordinateCorner.Y > imgHeight)
                            continue;
                        if (coordinateCorner.X + tagWidth > imgWidth)
                            tagWidth = imgWidth - coordinateCorner.X - 1;
                        if (coordinateCorner.Y + tagHeight > imgHeight)
                            tagHeight = imgHeight - coordinateCorner.Y - 1;
                        if (tagWidth > 100 && tagHeight > 100)
                        {
                            Rectangle tag = new Rectangle(coordinateCorner, new Size(tagWidth, tagHeight));
                            candidates.Add(new Candidate(face, tag));
                        }
                    }
                }
            }
            return candidates;
        }
        private static void DetectFaces(UMat ugray, String faceFileName, List<Rectangle> faces)
        {
            using (CascadeClassifier face = new CascadeClassifier(faceFileName))
            {
                Rectangle[] facesDetected = face.DetectMultiScale(
                   ugray,
                   1.1,
                   3,
                   new Size(20, 20));
                faces.AddRange(facesDetected);
            }

        }
        private static string GetTag(Candidate candidate, UMat ugray)
        {
            UMat grayImgRectangle = new UMat(ugray, candidate.Tag);
            List<RotatedRect> possibleTagRectangles = new List<RotatedRect>();
            string result = String.Empty;
            using (Mat canny = new Mat())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())//Видимо, это что-то типа двумерного массива под точку
            {
                //use image pyr to remove noise
                UMat temp = grayImgRectangle.Clone();
                //ResizeAndShow("GrayImg", grayImgRectangle);
                //double threshold = 100;
                CvInvoke.Erode(temp, temp, null, new Point(-1, -1), 2, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
                //ResizeAndShow("Temp", temp);
                //CvInvoke.AdaptiveThreshold(temp, canny, 255, AdaptiveThresholdType.GaussianC, ThresholdType.BinaryInv, 31, 4);
                CvInvoke.Canny(temp, canny, 100, 40);
                //CvInvoke.Ad
                CvInvoke.Dilate(canny, canny, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
                ResizeAndShow("Canny", canny.ToUMat(AccessType.ReadWrite));
                //Retrieves (получает) contours from the binary image as a contour tree. The pointer firstContour is filled by
                //the function. It is provided as a convenient (удобный) way to obtain (получить) 
                //the hierarchy (иерархия - неожиданно, да?) value as int[,]. 
                //The function modifies the source image content 
                int[,] hierachy = CvInvoke.FindContourTree(canny, contours, ChainApproxMethod.ChainApproxSimple);
                int length = hierachy.Length;
                if (length != 0)
                    FindRegtanglesOnImage(contours, hierachy, 0, temp, ref possibleTagRectangles);
                if (possibleTagRectangles.Count != 0)
                    result = GetTextFromRect(possibleTagRectangles, grayImgRectangle);
                //result = Test(contours, hierachy, grayImgRectangle);
            }
            return result;
        }
        private static void InitOcr(String path, String lang, OcrEngineMode mode)
        {
            try
            {
                if (_ocr != null)
                {
                    _ocr.Dispose();
                    _ocr = null;
                }
                //_ocr = new Tesseract()
                _ocr = new Tesseract(path, lang, mode, "1234567890");
                //_ocr.SetVariable("tessedit_char_whitelist", "1234567890");
            }
            catch (Exception e)
            {
                _ocr = null;
            }
        }
        private static int GetNumberOfChildren(int[,] hierachy, int idx)
        {
            //first child
            idx = hierachy[idx, 2];
            if (idx < 0)
                return 0;

            int count = 1;
            while (hierachy[idx, 0] > 0)
            {
                count++;
                idx = hierachy[idx, 0];
            }
            return count;
        }
        private static string GetTextFromRect(List<RotatedRect> rectangles, UMat grayRectImg)
        {
            //CvInvoke.Erode(grayRectImg, grayRectImg, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            //CvInvoke.Dilate(grayRectImg, grayRectImg, null, new Point(-1, -1), 2, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            UMat pyr = new UMat();
            CvInvoke.PyrDown(grayRectImg, pyr);
            CvInvoke.PyrUp(pyr, grayRectImg);
            string tag = String.Empty;
            for (int i = 0; i < rectangles.Count; i++)
            {
                using (UMat tmp1 = new UMat())
                using (UMat tmp2 = new UMat())
                {
                    PointF[] srcCorners = rectangles[i].GetVertices();
                    PointF[] destCorners = new PointF[] {
                        new PointF(0, rectangles[i].Size.Height - 1),
                        new PointF(0, 0),
                        new PointF(rectangles[i].Size.Width - 1, 0), 
                        new PointF(rectangles[i].Size.Width - 1, rectangles[i].Size.Height - 1)};
                    using (Mat rot = CvInvoke.GetAffineTransform(srcCorners, destCorners))
                    {
                        //Applies (применяет) an affine transformation to an image. 
                        CvInvoke.WarpAffine(grayRectImg, tmp1, rot, Size.Round(rectangles[i].Size));
                    }
                    //ResizeAndShow("contourAfterAffine", tmp1);
                    Size approxSize = new Size(240, 180);
                    //resize the license plate such that the front is ~ 10-12. 
                    //This size of front results in better accuracy (четко,правильно) from tesseract
                    double scale = Math.Min((double)approxSize.Width / tmp1.Size.Width, (double)approxSize.Height / tmp1.Size.Height);
                    Size newSize = new Size((int)Math.Round(tmp1.Size.Width * scale), (int)Math.Round(tmp1.Size.Height * scale));
                    CvInvoke.Resize(tmp1, tmp2, newSize, 0, 0, Inter.Cubic);
                    //CvInvoke.Imshow("3. Resize", tmp2);
                    //removes some pixels from the edge
                    int edgePixelSize = 2;
                    Rectangle newRoi = new Rectangle(new Point(edgePixelSize, edgePixelSize),
                       tmp2.Size - new Size(2 * edgePixelSize, 2 * edgePixelSize));
                    UMat tagRegion = new UMat(tmp2, newRoi);
                    tagRegion = FilterRegion(tmp2);
                    ResizeAndShow("TagRegion", tagRegion);
                    //Get the region of interest on the texts
                    InitOcr("", "eng", OcrEngineMode.TesseractOnly);
                    using (UMat textRegion = tagRegion.Clone())
                    {
                        _ocr.Recognize(textRegion);
                        Tesseract.Character[] ch = _ocr.GetCharacters();
                        //int maxHeight = -1;
                        //for (int j = 0; j < ch.Length; j++)
                        //{
                        //    if (ch[j].Region.Height > maxHeight)
                        //        maxHeight = ch[j].Region.Height;
                        //}
                        //tag += "GetCharacters: ";
                        for (int j = 0; j < ch.Length; j++)
                        {
                            //if (0.75 * maxHeight <= ch[j].Region.Height && ch[j].Region.Height <= 1.25 * maxHeight)
                            //{
                            tag += ch[j].Text;
                            //}
                        }
                        tag += "\n";
                    }
                }
            }
            return tag;
        }
        private static void FindRegtanglesOnImage(VectorOfVectorOfPoint contours, int[,] hierachy, int idx, UMat grayRectImg, ref List<RotatedRect> possibleTagRectangles)
        {
            int count = possibleTagRectangles.Count;
            for (; idx >= 0; idx = hierachy[idx, 0])
            {
                int numberOfChildren = GetNumberOfChildren(hierachy, idx);
                //if it does not contains any children (charactor), it is not a license plate region
                if (numberOfChildren == 0) continue;

                using (VectorOfPoint contour = contours[idx])
                {
                    double area = CvInvoke.ContourArea(contour);
                    if (area > 1000)
                    {
                        if (numberOfChildren < 3)
                        {
                            //If the contour has less than 3 children, it is not a license plate (assuming license plate has at least 3 charactor)
                            //However we should search the children of this contour to see if any of them is a license plate
                            FindRegtanglesOnImage(contours, hierachy, hierachy[idx, 2], grayRectImg, ref possibleTagRectangles);
                            continue;
                        }
                        //Finds a rotated (поворачивающийся) rectangle of the minimum area
                        //enclosing (вшитый) the input 2D point set. 
                        RotatedRect box = CvInvoke.MinAreaRect(contour);
                        //if (box.Angle > -7 && box.Angle < -6)
                        //if (count == 2)
                        //    count = 2;
                        double whRatio = (double)box.Size.Width / box.Size.Height;
                        double perim = contour.Size;
                        double k = perim / (2 * Math.Sqrt(area));
                        //Поворачиваем прямоугольник, если он нас сильно не устраивает в плане наклона
                        //приводим его к [-45; 45]
                        if (box.Angle < -45.0)//угол
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle += 90.0f;
                        }
                        else if (box.Angle > 45.0)
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle -= 90.0f;
                        }

                        //CvInvoke.DrawContours(temp2, contours, idx, new MCvScalar(255, 0, 0), 1, LineType.EightConnected);
                        //if (count == 2)
                        //    count = 2;
                        //ResizeAndShow("All contours", temp2);
                        //if (0.6 < k && k < 1.6)
                        //{
                        //    CvInvoke.DrawContours(temp1, contours, idx, new MCvScalar(255, 0, 0), 1, LineType.EightConnected);
                        //    ResizeAndShow("Rectangle contours", temp1);
                        //}
                        double imgArea = grayRectImg.Size.Height * grayRectImg.Size.Width;
                        //if (!(0.5 < whRatio && whRatio < 3.0) || !(0.5 < k && k < 1.6))
                        if ((whRatio < 0.5 || whRatio > 3.0) || (k < 0.6 || k > 1.4) || area > 0.4 * imgArea)
                        {
                            //if the width height ratio (коэффициент) is not in the specific
                            //range, it is not a license plate 
                            //However we should search the children of this contour to see if any of them is a license plate
                            if (hierachy[idx, 2] > 0)
                            {
                                FindRegtanglesOnImage(contours, hierachy, hierachy[idx, 2], grayRectImg, ref possibleTagRectangles);
                            }
                            count++;
                            continue;
                        }
                        possibleTagRectangles.Add(box);
                    }
                }
            }
        }

        private static void ResizeAndShow(string text, UMat temp1)
        {
            UMat temp2 = new UMat();
            CvInvoke.Resize(temp1, temp2, new Size(768, 480), 0, 0, Inter.Cubic);
            CvInvoke.Imshow(text, temp2);
        }

        private static UMat FilterRegion(UMat trNew)
        {
            //что такое thresolding???
            UMat thresh = new UMat();
            CvInvoke.EqualizeHist(trNew, trNew);
            CvInvoke.AdaptiveThreshold(trNew, thresh, 255, AdaptiveThresholdType.GaussianC, ThresholdType.BinaryInv, 11, 2);
            CvInvoke.Dilate(thresh, thresh, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            //trNew.MinMax(out minValues,out maxValues,out minLocations,out maxLocations);
            //CvInvoke.Imshow("Threshold", thresh);
            Size trNewSize = trNew.Size;
            using (Mat trMask = new Mat(trNewSize.Height, trNewSize.Width, DepthType.Cv8U, 1))
            using (Mat trCanny = new Mat())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                trMask.SetTo(new MCvScalar(255.0));
                UMat temp = trNew.Clone();
                //CvInvoke.Erode(trNew, trNew, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
                //CvInvoke.Imshow("2. threshonimg", thresh);
                //CvInvoke.EqualizeHist(trNew, trNew);
                CvInvoke.Canny(thresh, trCanny, 100, 40);
                //CvInvoke.Imshow("3. BeforeDilate", trCanny);
                CvInvoke.Dilate(trCanny, trCanny, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
                //CvInvoke.Imshow("4. AfterDilate", trCanny);
                int[,] trHierachy = CvInvoke.FindContourTree(trCanny, contours, ChainApproxMethod.ChainApproxNone);
                int count = contours.Size;
                //int k = 0;
                List<double> areas = new List<double>();
                List<Rectangle> unsortedNumbers = new List<Rectangle>();
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {
                        double area = CvInvoke.ContourArea(contour);
                        //double imgArea = trNewSize.Height * trNewSize.Width;
                        //почему-то цифры не проходят через этот этап
                        if (250 < area && area < 5000 && GetNumberOfChildren(trHierachy, i) <= 2)
                        {
                            //areas.Add(area);
                            CvInvoke.DrawContours(temp, contours, i, new MCvScalar(255, 0, 0), 1, LineType.EightConnected);
                            Rectangle rect = CvInvoke.BoundingRectangle(contour);
                            //int rectHeight = rect.Height;
                            //int trHeight = trNewSize.Height >> 2;
                            //double perim = 2 * (rect.Width + rect.Height);
                            //double area = rect.Width * rect.Height;
                            //double koef = perim / (2 * Math.Sqrt(area));
                            //if (k == 2)
                            //    k = 2;
                            //k++;
                            //if (rectHeight > trHeight)
                            //{
                            rect.X -= 1; rect.Y -= 1; rect.Width += 2; rect.Height += 2;
                            unsortedNumbers.Add(rect);
                            //}
                        }

                    }
                }
                //кажется, номер не прошел по ширине
                List<Number> numbers = FilterNumbers(unsortedNumbers);
                int numbersCount = numbers.Count;
                for (int i = 0; i < numbersCount; i++)
                {
                    if (numbers[i].NumberNear)
                    {
                        Rectangle rect = numbers[i].Rectangle;
                        Rectangle roi = new Rectangle(Point.Empty, trNew.Size);
                        rect.Intersect(roi);
                        CvInvoke.Rectangle(trMask, rect, new MCvScalar(), -1);
                    }
                }

                CvInvoke.Imshow("ContourNew", temp);
                CvInvoke.Imshow("TextRegionMask", trMask);
                thresh.SetTo(new MCvScalar(), trMask);
                CvInvoke.Imshow("thresh", thresh);
            }
            CvInvoke.Erode(thresh, thresh, null, new Point(-1, -1), 2, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            CvInvoke.Dilate(thresh, thresh, null, new Point(-1, -1), 2, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            return thresh;
        }
        private static List<Number> FilterNumbers(List<Rectangle> unsortedNumbers)
        {
            List<Number> numbers = new List<Number>();
            int maxHeight = -1;
            for (int i = 0; i < unsortedNumbers.Count; i++)
            {
                numbers.Add(new Number(unsortedNumbers[i]));
            }
            //цикл проходит дважды! убрать
            //сделать свойство, которое возвращает булево значение в каждом объекте, изменять его, если рядом есть цифра
            int length = numbers.Count;
            for (int j = 0; j < length; j++)
            {
                if (!numbers[j].NumberNear)
                {
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        if (j != i && numbers[j].IsNumbersNear(numbers[i]))
                        {
                            numbers[j].NumberNear = true;
                            numbers[i].NumberNear = true;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < length; i++)
            {
                if (numbers[i].NumberNear)
                {
                    if (maxHeight < numbers[i].Rectangle.Height)
                        maxHeight = numbers[i].Rectangle.Height;
                }
            }

            for (int i = 0; i < length; i++)
            {
                if (0.8 * maxHeight > numbers[i].Rectangle.Height || numbers[i].Rectangle.Height > 1.2 * maxHeight)
                {
                    numbers[i].NumberNear = false;
                }
            }
            return numbers;
        }
    }
}
//создает список цифр - новый класс. 
//метод на тру фалсе стоят ли два прямоугольника рядом{
//центр + расстояние до края + расстояние между прямогольниками - если эта точка принадлежит прямоугольнику - 
//}
//
//У них внутри принадлежит ли точка другому прямоугольнику
//Как улучшить детектор границ Кени? Увеличить контрастность?
//мб задать ROI на фото? примерно в центре. И там
//возможно немного увеличить вниз прямоугольник
//44 - слипаются четверки:(
//надо бы применять thresh фильтр в зависимости от общей яркости изображения
//если две строки из прямоугольников подходят - брать ту, что с большей высотой