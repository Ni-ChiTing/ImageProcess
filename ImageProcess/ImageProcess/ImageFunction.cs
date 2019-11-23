using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ImageProcess
{
    class ImageFunction
    {

        public struct Picture
        {
            public Bitmap pic;
            public int step;
            public string label;
        }
        // private Bitmap OrigPic = null;
        private string Filename = null;
        private int total_step = 0;
        public List<Picture> Stack = new List<Picture>();
        private bool UseResultPic = false;
        private string PreStepLabel = "";
        public void SetPreStepLabel(string s)
        {
            PreStepLabel = s;
        }
        public void SetResultPic (bool a)
        {
            UseResultPic = a;
        }
        private Bitmap FindBitMapByLabal(List<Picture> list)
        {
            for (int i = 0; i< list.Count; ++i)
            {
                if (list[i].label == PreStepLabel)
                {
                    return list[i].pic;
                }
            }
            return null;
        }
        public List<Picture> GetNowStepPicture()
        {
            return Stack.FindAll(p => p.step == (total_step - 1));
        }
        public void SetFileName(string name)
        {
            Filename = name;
        }
        private void AddStack(string l,Bitmap b)
        {
            Picture temp = new Picture();
            temp.step = total_step;
            temp.pic = b;
            temp.label = l;
            Stack.Add(temp);
        }
        public int GetNowStep()
        {
            return total_step - 1;
        }
        private int FilterProcess(int[] filter,int[] data)
        {
            int result = 0;
            for (int i = 0;i <filter.Length; ++i)
            {
                result = result + filter[i] * data[i];
            }
            return result;
        }
        private int[] FilterProcess(int[] filter, Color[] data)
        {
            int R = 0;
            int G = 0;
            int B = 0;
            int A = 0;
            for (int i = 0; i < filter.Length; ++i)
            {
                R = R + data[i].R;
                G = G + data[i].G;
                B = B + data[i].B;
                A = A + data[i].A;
            }
            int[] result = { A, R, G, B };
            return result;
        }
        public void GetRGBandGraylevelPic()
        {
            Bitmap G = null;
            Bitmap R = null;
            Bitmap B = null;
            Bitmap Graylevel = null;
            Image i = null;
            if (Filename == null)
            {
                 i = Image.FromFile(".\\ExampleImage\\A_RGB.bmp");
            }
            else
            {
                 i = Image.FromFile(Filename);
            }
            Bitmap image = null;
            if (UseResultPic)
            {
                image = FindBitMapByLabal(GetNowStepPicture());
            }
            else
            {
                image = new Bitmap(i);
            }
            if (image == null)
            {
                image = new Bitmap(i);
            }

            G = new Bitmap(image.Width, image.Height);
            R = new Bitmap(image.Width, image.Height);
            B = new Bitmap(image.Width, image.Height);
            Graylevel = new Bitmap(image.Width, image.Height);
            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    Color C = image.GetPixel(x, y);
                    //Debug.Print(C.ToString());
                    Color GC = Color.FromArgb(C.A, C.G, C.G, C.G);
                    Color BC = Color.FromArgb(C.A, C.B, C.B, C.B);
                    Color RC = Color.FromArgb(C.A, C.R, C.R, C.R);
                    int grayScale = (int)((C.R * 0.3) + (C.G * 0.59) + (C.B * 0.11));
                    Color Gray = Color.FromArgb(C.A, grayScale, grayScale, grayScale);
                    G.SetPixel(x, y, GC);
                    B.SetPixel(x, y, BC);
                    R.SetPixel(x, y, RC);
                    Graylevel.SetPixel(x, y, Gray);
                }
            }
            AddStack("Source", image);
            AddStack("R_Channel", R);
            AddStack("G_Channel", G);
            AddStack("B_Channel", B);
            AddStack("GrayLevel", Graylevel);
            ++total_step;
            //Bitmap[] arr = { R, G, B, Graylevel };
            //return arr;
        }
        private Bitmap ExtendBitmap(Bitmap image)
        {
            Bitmap image_extend = new Bitmap(image.Width + 2, image.Height + 2);
            for (int x = 0; x < image_extend.Width; ++x)
            {
                for (int y = 0; y < image_extend.Height; ++y)
                {
                    if (x == 0 || x == image_extend.Width - 1 || y == 0 || y == image_extend.Height - 1)
                    {
                        image_extend.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                    }
                    else
                    {
                        image_extend.SetPixel(x, y, image.GetPixel(x - 1, y - 1));
                    }
                }
            }
            return image_extend;
        }

        public void SmoothFilter()
        {
            Bitmap Mean = null;
            Bitmap Median = null;
            Image i = null;
            if (Filename == null)
            {
                i = Image.FromFile(".\\ExampleImage\\B_noisy.bmp");
            }
            else
            {
                i = Image.FromFile(Filename);
            }

            Bitmap image = null;
            if (UseResultPic)
            {
                image = FindBitMapByLabal(GetNowStepPicture());
            }
            else
            {
                image = new Bitmap(i);
            }
            if (image == null)
            {
                image = new Bitmap(i);
            }
            Bitmap image_extend = ExtendBitmap(image);
            AddStack("Source", image);
            Mean = new Bitmap(image.Width, image.Height);
            Median = new Bitmap(image.Width, image.Height);
            Color[] C = new Color[9];
            int[] MeanFilter = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] MedianFilter = { 0, 0, 0, 0, 1, 0, 0, 0, 0 };

            for (int x = 1; x < image_extend.Width - 1; ++x)
            {
                for(int y = 1; y < image_extend.Height - 1; ++y)
                {
                    C[0] = image_extend.GetPixel(x - 1, y - 1);
                    C[1] = image_extend.GetPixel(x , y - 1);
                    C[2] = image_extend.GetPixel(x + 1, y - 1);
                    C[3] = image_extend.GetPixel(x - 1, y );
                    C[4] = image_extend.GetPixel(x, y );
                    C[5] = image_extend.GetPixel(x + 1, y);
                    C[6] = image_extend.GetPixel(x - 1, y + 1);
                    C[7] = image_extend.GetPixel(x, y + 1);
                    C[8] = image_extend.GetPixel(x + 1, y + 1);
                    int[] t = FilterProcess(MeanFilter, C);
                    int[] A = { C[0].A, C[1].A, C[2].A, C[3].A, C[4].A, C[5].A, C[6].A, C[7].A, C[8].A };
                    int[] R = { C[0].R, C[1].R, C[2].R, C[3].R, C[4].R, C[5].R, C[6].R, C[7].R, C[8].R };
                    int[] G = { C[0].G, C[1].G, C[2].G, C[3].G, C[4].G, C[5].G, C[6].G, C[7].G, C[8].G };
                    int[] B = { C[0].B, C[1].B, C[2].B, C[3].B, C[4].B, C[5].B, C[6].B, C[7].B, C[8].B };
                    Mean.SetPixel(x - 1, y - 1,Color.FromArgb(t[0]/9, t[1]/9,t[2]/9,t[3]/9));
                    Array.Sort(A);
                    Array.Sort(G);
                    Array.Sort(R);
                    Array.Sort(B);
                    Median.SetPixel(x - 1, y - 1, Color.FromArgb(FilterProcess(MedianFilter, A), FilterProcess(MedianFilter, R), FilterProcess(MedianFilter, G), FilterProcess(MedianFilter, B)));

                }
            }
            AddStack("Mean", Mean);
            AddStack("Median", Median);
            ++total_step;
        }
        public void Undo()
        {
            if (total_step > 0)
            {
                Stack.RemoveAll(r => r.step == total_step - 1);
                total_step = total_step - 1;
            }
            
        }
    }
}
