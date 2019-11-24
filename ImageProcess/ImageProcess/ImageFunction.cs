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
            public bool UseTrackBar;
            public bool UseHitogram;
            public int[] historgramvalue;
        }
        // private Bitmap OrigPic = null;
        private string Filename = null;
        private int total_step = 0;
        public List<Picture> Stack = new List<Picture>();
        private bool UseResultPic = false;
        private string PreStepLabel = "";
        private int threshold = 128;
        private Bitmap Q5_pic = null;
        public int GetThreshold()
        {
            return threshold;
        }
        public void SetThreshold(int v)
        {
            threshold = v;
        }
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
        private void AddStack(string l,Bitmap b,bool track,bool hist,int [] value)
        {
            Picture temp = new Picture();
            temp.step = total_step;
            temp.pic = b;
            temp.label = l;
            temp.UseTrackBar = track;
            temp.UseHitogram = hist;
            temp.historgramvalue = value;
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
        public void Normalize(List<int> data,int size,Bitmap pic)
        {
            double max = data.Max();
            double min = data.Min();
            int x = 0;
            int y = 0;
            Debug.Print("Max = " + max.ToString());
            Debug.Print("Min = " + min.ToString());
            for (int i = 0;i < data.Count; ++i)
            {
                x = i / pic.Height;
                y = i % pic.Height;
                if (data[i] > size)
                {
                    data[i] = size;

                }
                //data[i] = (int)(((data[i] - min) / (max - min)) * size);
                pic.SetPixel(x, y, Color.FromArgb(pic.GetPixel(x, y).A, data[i], data[i], data[i]));
            }
            
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
            AddStack("Source", image,false,false,null);
            AddStack("R_Channel", R,false,false,null);
            AddStack("G_Channel", G,false,false,null);
            AddStack("B_Channel", B,false,false,null);
            AddStack("GrayLevel", Graylevel,false,false,null);
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
            AddStack("Source", image,false,false,null);
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
            AddStack("Mean", Mean,false,false,null);
            AddStack("Median", Median,false,false,null);
            ++total_step;
        }
        public void HistorgramEqualization()
        {
            Image i = null;
            int[] value = new int[256];
            int[] cdf = new int[256];
            int[] value2 = new int[256];
            for (int k = 0; k < value.Length; ++k)
            {
                value[k] = 0;
                cdf[k] = 0;
                value2[k] = 0;
            }
            if (Filename == null)
            {
                i = Image.FromFile(".\\ExampleImage\\C_dark2.bmp");
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
            Bitmap Graylevel = new Bitmap(image.Width, image.Height);
            Bitmap Equa = new Bitmap(image.Width, image.Height);
            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    Color C = image.GetPixel(x, y);
                    //Debug.Print(C.ToString())
                    int grayScale = (int)((C.R * 0.3) + (C.G * 0.59) + (C.B * 0.11));
                    ++value[grayScale]; 
                    Color Gray = Color.FromArgb(C.A, grayScale, grayScale, grayScale);
                    Graylevel.SetPixel(x, y, Gray);
                }
            }
            AddStack("Source", Graylevel, false, true, value);
            
            cdf[0] = value[0];
            int min;
            if (cdf[0] > 0)
            {
                min = cdf[0];
            }
            else
            {
                min = int.MaxValue;
            }
            
           for (int k = 1; k < cdf.Length; ++k)
            {
                cdf[k] = cdf[k - 1] + value[k];
                if (cdf[k] > 0 && cdf[k] < min)
                {
                    min = cdf[k];
                }
                
            }
            Debug.Print(min.ToString());
            double size = image.Width * image.Height - min;
            for (int x = 0; x < Graylevel.Width; ++x)
            {
                for (int y = 0; y < Graylevel.Height; ++y)
                {
                    Color C = Graylevel.GetPixel(x, y);
                    int v =(int)Math.Round(((cdf[C.R] - min) / size)*255);
//S Debug.Print(v.ToString());
                    ++value2[v];
                    Color Gray = Color.FromArgb(C.A, v, v, v);
                    Equa.SetPixel(x, y, Gray);
                }
            }
            
            AddStack("Result", Equa, false, true, value2);
            ++total_step;
        }
        public void ThresholdCal()
        {
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
            Bitmap Graylevel = new Bitmap(image.Width, image.Height);
            Bitmap Graylevel_t = new Bitmap(image.Width, image.Height);
            Debug.Print("threshold = " +threshold.ToString());
            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    Color C = image.GetPixel(x, y);
                    
                    int grayScale = (int)((C.R * 0.3) + (C.G * 0.59) + (C.B * 0.11));
                    Color Gray = Color.FromArgb(C.A, grayScale, grayScale, grayScale);
                    Color t;
                    if (grayScale < threshold)
                        t = Color.FromArgb(C.A, 0, 0, 0);
                    else
                        t = Color.FromArgb(C.A, 255, 255, 255);
                    Graylevel.SetPixel(x, y, Gray);
                    Graylevel_t.SetPixel(x, y, t);
                }
            }
            AddStack("Source", Graylevel, true, false, null);
            AddStack("Result", Graylevel_t, true, false, null);
            ++total_step;
        }
        public void SobelFilter()
        {
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
           // Bitmap Graylevel = new Bitmap(image.Width, image.Height);
            Bitmap Graylevel_extend = ExtendBitmap(image);
            Bitmap X_Sobel = new Bitmap(image.Width, image.Height);
            Bitmap Y_Sobel = new Bitmap(image.Width, image.Height);
            Bitmap Combined = new Bitmap(image.Width, image.Height);

            int[] sobelX = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            int[] sobelY = { 1, 2, 1, 0, 0, 0, -1, -2, -1 };
            int[] data = new int[9];
            List<int> datas = new List<int>();
            List<int> datas2 = new List<int>();
            List<int> datas3 = new List<int>();
            for (int x = 1; x <Graylevel_extend.Width - 1; ++x)
            {
                for (int y = 1; y < Graylevel_extend.Height -1 ; ++y)
                {
                    data[0] = Graylevel_extend.GetPixel(x - 1, y - 1).R;
                    data[1] = Graylevel_extend.GetPixel(x , y - 1).R;
                    data[2] = Graylevel_extend.GetPixel(x + 1, y - 1).R;
                    data[3] = Graylevel_extend.GetPixel(x - 1, y ).R;
                    data[4] = Graylevel_extend.GetPixel(x, y ).R;
                    data[5] = Graylevel_extend.GetPixel(x + 1, y).R;
                    data[6] = Graylevel_extend.GetPixel(x - 1, y + 1).R;
                    data[7] = Graylevel_extend.GetPixel(x, y + 1).R;
                    data[8] = Graylevel_extend.GetPixel(x + 1, y + 1).R;
                    int gx = FilterProcess(sobelX,data);
                    int gy = FilterProcess(sobelY, data);
                    datas.Add(Math.Abs(gx));
                    datas2.Add(Math.Abs(gy));
                    datas3.Add(Math.Abs(gx) + Math.Abs(gy));
                    Y_Sobel.SetPixel(x - 1, y - 1, Color.FromArgb(image.GetPixel(x - 1, y - 1).A, 0, 0, 0));
                    X_Sobel.SetPixel(x - 1, y - 1, Color.FromArgb(image.GetPixel(x -1,y-1).A,0,0,0));
                    Combined.SetPixel(x - 1, y - 1, Color.FromArgb(image.GetPixel(x - 1, y - 1).A, 0, 0, 0));
                }
            }
            Normalize(datas, 255,X_Sobel);
            Normalize(datas2, 255, Y_Sobel);
            Normalize(datas3, 255, Combined);
            AddStack("Source", image, false, false, null);
            AddStack("Verticle", X_Sobel, false, false, null);
            AddStack("Horizontal", Y_Sobel, false, false, null);
            AddStack("Combined", Combined, false, false, null);
            Q5_pic = Combined;
            ++total_step;
        }
        public void OverlapImage()
        {

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
