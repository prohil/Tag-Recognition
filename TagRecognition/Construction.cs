using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.OCR;

namespace TagRecognition
{
    public class Construction
    {
        public struct Candidate
        {
            Rectangle face;
            Rectangle tag;
            public Rectangle Face { get { return face; } }
            public Rectangle Tag { get { return tag; } }
            public Candidate(Rectangle face, Rectangle tag) 
            {
                this.face = face;
                this.tag = tag;
            }

            //переделать структуру?
            //абстрактный класс?
        };
        //private Tesseract _ocr;
        //private void InitOcr(String path, String lang, OcrEngineMode mode);
        //public List<string> GetTags(Mat image, out long detectionTime);
        //private List<Candidate> GetCandidates(Mat image, UMat ugray);
        //private void DetectFaces(UMat ugray, String faceFileName, List<Rectangle> faces);
        //private string GetTag(Candidate candidate, UMat ugray);
        /*
        Recognizes tag numbers in each input image.
        Returns a vector of recognized tag numbers for each of the input images
        */
        //public List<List<int>> getAllTags(List<string> images, bool test_mode);

        //protected Mat getImage(char[,] filename, bool prescale);
        //protected List<Candidate> getCandidates(Mat frame);
        //protected uint recognizeText(List<Rectangle> bboxes, Mat image);
        //protected List<int> getTags(char[,] filename);
        //#ifdef DEBUG
        //    void drawCandidateBoxes(Mat& image, vector<Candidate> candidates);
        //#endif
    }
}
