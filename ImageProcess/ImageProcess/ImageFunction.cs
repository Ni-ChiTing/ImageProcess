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

       // private Bitmap OrigPic = null;
        private string Filename = null;
        private List<Bitmap> Stack = new List<Bitmap>();
        public void SetFileName(string name)
        {
            Filename = name;
        }

        public Bitmap[] GetRGBandGraylevelPic()
        {
            Bitmap G = null;
            Bitmap R = null;
            Bitmap B = null;
            Bitmap Graylevel = null;
            Image i = null;
            if (Filename == null)
            {
                 i = Image.FromFile(Filename);
            }
            else
            {
                 i = Image.FromFile(".\\ExampleImage\\A_RGB.bmp");
            }
            Bitmap image = new Bitmap(i);
            if ( R != null)
            {
                R.Dispose();
            }
            if (G != null)
            {
                G.Dispose();
            }
            if (B != null)
            {
                B.Dispose();
            }
            if (Graylevel != null)
            {
                Graylevel.Dispose();
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
            Bitmap[] arr = { R, G, B, Graylevel };
            return arr;
        }
        
    }
}
