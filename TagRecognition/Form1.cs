using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Emgu;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.VideoSurveillance;


namespace TagRecognition
{

    public partial class MainForm : Form
    {
        string[] paths;      //all paths of images
        int index = 0;       //index photo
        bool action = false; //for btnRecognize: change text of button
        List<string> tags; // list of tags
        List<Rectangle> rectFaces = new List<Rectangle>();
        List<Rectangle> rectTags = new List<Rectangle>();
        long detectionTime; // время распознавания
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Загружаем кучу изображений в массив, обнуляем счетчики и тд и тп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            //if (LoadImageDialog.ShowDialog() == DialogResult.OK && LoadImageDialog.FileName != String.Empty)
            //{
            index = 0;
            paths = LoadImageDialog.FileNames;
            //paths = Directory.GetFiles(@"C:\Users\а\Desktop\test", "*.jpg");
            lblAllImg.Visible = true;
            lblAllImg.Text = "All: " + paths.Length.ToString();
            lblNowImg.Visible = true;
            lblNowImg.Text = "Now: " + (index + 1).ToString();
            btnRecognize.Enabled = true;
            btnLoad.Enabled = false;
            //}
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (index <= paths.Length - 1)
            {
                imgBox.ImageLocation = paths[index];
                //imgBox.ImageLocation = @"C:\Users\а\Desktop\15337.jpg";
                rectFaces.Clear();
                rectTags.Clear();
                tags.Clear();
                tags = Recognition.GetTags(paths[index], ref rectFaces, ref rectTags, out detectionTime);
                ShowImage(paths[index]);
                lblNowImg.Text = "Now: " + (index++ + 1).ToString();
                lblResult.Text = "Result " + Result(tags);
                if (index == paths.Length)
                    btnNext.Enabled = false;
            }

        }
        private void btnRecognize_Click(object sender, EventArgs e)
        {
            string text = String.Empty;
            lblResult.Visible = true;
            if (!action)
            {
                if (index == paths.Length)
                {
                    DialogResult result = MessageBox.Show("Begin new recognition?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DefoltCondition();
                    }
                    return;
                }
                btnRecognize.Text = "Stop";
                action = true;
                btnNext.Visible = true;
                btnNext.Enabled = true;
                //тут придется добавить, чтобы ждало окончания действия
                if (index <= paths.Length - 1)
                {
                    imgBox.ImageLocation = paths[index];
                    //index = 23; 
                    //25 бо
                    //34 бо
                    //41 ytn
                    //6 - ytn
                    //17 - бо 1 из двух тэгов
                    //15 1 ошибка 
                    //31 1 ошибка
                    //37? четверки слипаются + нтд
                    //19 - непонятный текст
                    //18, 23, 26,27,35,38,39,40,42,43,44 глянуть
                    //
                    tags = Recognition.GetTags(paths[index], ref rectFaces, ref rectTags, out detectionTime);
                    ShowImage(paths[index]);
                    lblNowImg.Text = "Now: " + (index++ + 1).ToString();
                    lblResult.Text = "Result: " + Result(tags);

                }
            }
            else
            {
                if (index != paths.Length)
                {
                    btnRecognize.Text = "Continue";
                }
                else
                {
                    btnRecognize.Text = "Recognize";
                }
                action = false;
                btnNext.Enabled = false;
            }





        }

        private void ShowImage(string path)
        {
            Mat img = CvInvoke.Imread(path, LoadImageType.AnyColor);
            int height = img.Size.Height,
                    width = img.Size.Width;

            //if (height * width > 4000000 && height > 2000 && width > 2000)
            //{
                if (width > height)
                {
                    double k = (double)width / 1805;
                    CvInvoke.Resize(img, img, new Size(1805, (int)Math.Round(height / k)));
                }
                else
                {
                    double k = (double)height / 1250;
                    CvInvoke.Resize(img, img, new Size((int)Math.Round(width / k), 1250));
                }
            //}
            for (int i = 0; i < rectFaces.Count; i++)
            {
                CvInvoke.Rectangle(img, rectFaces[i], new Bgr(Color.Blue).MCvScalar, 6);
                CvInvoke.Rectangle(img, rectTags[i], new Bgr(Color.Red).MCvScalar, 6);
            }
            imgBox.Image = img;
        }
        private void DefoltCondition()
        {
            btnNext.Visible = false;
            btnLoad.Enabled = true;
            btnRecognize.Enabled = false;
            lblResult.Visible = false;
            lblAllImg.Text = "All: ";
            lblNowImg.Text = "Now: ";
            lblResult.Text = "Result: ";
            lblResult.Visible = false;
            imgBox.ImageLocation = null;
            index = 0;
            rectFaces.Clear();
            rectTags.Clear();
            tags.Clear();
            paths = null;
        }
        private string Result(List<string> tags)
        {
            string result = String.Empty;
            int length = tags.Count;
            for (int i = 0; i < length; i++)
            {
                result += tags[i];
            }
            return result;
        }
    }
    // брать те прямоугольники, которые стоят рядом
    /*1. Вывести все это дело в отдельный класс
     *2. Попробовать заменить форыч на фор*/
}
