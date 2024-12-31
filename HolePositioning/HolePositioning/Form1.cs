using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Euresys.Open_eVision_2_2;
//using Euresys.Open_eVision_2_2.MachineVision;
using System.IO;

namespace HolePositioning
{
    public partial class Form1 : Form
    {
        //Todo test
        //private EImageBW8 EBW8Image1;
        //private EImageBW8 EdgeImage;
        //EImageBW8 InputImage;
        //EWorldShape WorldShape;
        //ECircleGauge CircleGauge;
        //

        private PointF[] HoleCenter;//定位孔圓心
        internal CircleFinder MyCircleFinder;
        
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog SourceFile = new OpenFileDialog();

        EImageC24 EC24ImageColor = new EImageC24(); //EC24Image1, EImageC24 instance 
        EImageBW8 EBW8ImageGrayscale = new EImageBW8(); //EBW8Image2, EImageBW8 instance
        EImageBW8 EBW8ImageThreshold = new EImageBW8(); //EBW8Image3, EImageBW8 instance

        //EWorldShape EWorldShape1 = new EWorldShape(); // EWorldShape instance        
        //EImageBW8 EBW8Image4 = new EImageBW8(); // EImageBW8 instance
        //EImageBW8 EBW8Image5 = new EImageBW8(); //Test
        //ECircleGauge ECircleGauge1 = new ECircleGauge(); // ECircleGauge instance
        //ECircle Circle1 = new ECircle(); // ECircle instance
        //ECircle measuredCircle = null; // ECircle instance
        
        private void buttonSourcePic_Click(object sender, EventArgs e)
        {
            DialogResult result = SourceFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (this.pictureBoxSource.Image != null)
                {
                    this.pictureBoxSource.Image.Dispose(); //釋放目前顯示的圖片
                    this.pictureBoxSource.Image = null;    //設定為null，避免再次使用時衝突
                }

                this.pictureBoxSource.Image = Image.FromFile(SourceFile.FileName);                                
                //InputImage = new EImageBW8();
                //InputImage.Load(SourceFile.FileName);
            }
        }

        //判斷是否為灰階
        static bool IsGrayscale(Bitmap bitmap)
        {
            // 迭代圖片的每個像素
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    // 獲取當前像素的顏色
                    Color pixelColor = bitmap.GetPixel(x, y);

                    // 如果像素的RGB值不相等，則表示圖片不是灰階
                    if (pixelColor.R != pixelColor.G || pixelColor.G != pixelColor.B)
                    {
                        return false;
                    }
                }
            }
            // 如果所有像素的RGB值相等，則表示圖片是灰階
            return true;
        }

        private EImageBW8 Binarization(string imagePath)
        {
            var wc = new System.Net.WebClient();
            var bm = new Bitmap(wc.OpenRead(/*SourceFile.FileName*/ imagePath));
            var gs = IsGrayscale(bm);
            
            try
            {
                //判斷是否灰階
                if (!gs) //若非灰階
                {
                    //轉換彩色影像為灰階，並二值化
                    EC24ImageColor.Load(/*SourceFile.FileName*/ imagePath);
                    int width = EC24ImageColor.Width;
                    int height = EC24ImageColor.Height;

                    //初始化灰階影像
                    EBW8ImageGrayscale.SetSize(width, height);
                    EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8ImageGrayscale);
                    EasyImage.Convert(EC24ImageColor, EBW8ImageGrayscale);
                }
                else
                {
                    //直接載入灰階影像
                    EBW8ImageGrayscale.Load(/*SourceFile.FileName*/ imagePath);
                }

                //二值化處理
                EBW8ImageThreshold.SetSize(EBW8ImageGrayscale.Width, EBW8ImageGrayscale.Height);
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8ImageThreshold);
                int thresholdMode = (int)EThresholdMode.MinResidue;
                EasyImage.Threshold(EBW8ImageGrayscale, EBW8ImageThreshold, thresholdMode);

                // 儲存二值化後的影像
                //EBW8Image3.SaveJpeg("D:\\img\\threshold.jpeg", 75);                
            }
            catch (EException ex)
            {
                MessageBox.Show($"Threshold Error : {ex}");
            }
            return EBW8ImageThreshold;
        }

        //Todo Test
        //public void CircleFinder(float radius)
        //{
        //    WorldShape = new EWorldShape();
        //    CircleGauge = new ECircleGauge
        //    {
        //        //for 外同軸
        //        Radius = radius,
        //        //Tolerance = 10.00f,
        //        //HVConstraint = false,
        //        Threshold = 1,
        //        MinAmplitude = 5,
        //        //Thickness = 3,
        //        //Smoothing = 1,
        //        TransitionChoice = ETransitionChoice.NthFromBegin,
        //        TransitionType = ETransitionType.Bw,
        //        //NumFilteringPasses = 3,
        //        //FilteringThreshold = 2.00f,
        //        SamplingStep = 5f
        //    };

        //    CircleGauge.Attach(WorldShape);
        //}

        ////要尋找的圓大致圓心位置
        //public void SetCircleGaugeCenter(EPoint center)
        //{
        //    //For test
        //    CircleGauge = new ECircleGauge();
        //    //
        //    CircleGauge.Center = center;
        //}
        //public void SetCircleGaugeCenter(PointF center)
        //{
        //    EPoint point = new EPoint(center.X, center.Y);
        //    SetCircleGaugeCenter(point);
        //}

        ////設定圓的尋找位置&範圍
        //public void SetCircleGaugeRange(EROIBW8 roi)
        //{
        //    WorldShape.SetSensorSize(roi.Width, roi.Height);
        //    WorldShape.SetCenterXY(roi.OrgX, roi.OrgY);
        //}

        ////設定圓的尋找範圍
        //public void SetCircleGaugeRange(int orgX, int orgY, int width, int height)
        //{
        //    WorldShape = new EWorldShape();
        //    WorldShape.SetSensorSize(width, height);
        //    WorldShape.SetCenterXY(orgX, orgY);
        //}
        //

        private /*void*/ ECircle[] MatchByHole(/*EROIBW8 findHoleImage*/)
        {
            ECircle[] fittingCircle = null;

            //learnPattern
            EImageBW8 holeImage = new EImageBW8();
            //SourceImage
            EImageBW8 findHoleImage = new EImageBW8();

            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LearnPattern", "holeImage.png");            
            holeImage.Load(imagePath);
            holeImage = Binarization(imagePath);

            //For test
            //holeImage.Save("D:\\Ryan_Chiu\\TestImg\\LearnPattern.jpeg");
            //

            //float initialRadius = 0f; //初步測量半徑
            float maxRadius = 0f; //最大半徑

            try
            {
                findHoleImage.Load(SourceFile.FileName);
                findHoleImage = Binarization(SourceFile.FileName);

                //For test                
                //findHoleImage.Save("D:\\Ryan_Chiu\\TestImg\\InputImage.jpeg");
                //

                //可能的最大半徑
                float imageWidth = findHoleImage.Width;
                float imageHeight = findHoleImage.Height;
                
                maxRadius = (float)Math.Sqrt(imageWidth * imageWidth + imageHeight * imageHeight) / 2;

                //if (HoleCenter == null)
                //{
                    //match 定位孔
                    EMatcher matcher = new EMatcher();
                    matcher.MaxPositions = 3;
                    matcher.CorrelationMode = ECorrelationMode.OffsetNormalized;                    
                    matcher.LearnPattern(holeImage);
                    matcher.Match(findHoleImage);
                    
                    fittingCircle = new ECircle[matcher.NumPositions];
                    HoleCenter = new PointF[matcher.NumPositions];

                    //迴圈嘗試不同半徑，從小半徑開始逐漸到最大
                    for (int i = 0; i < matcher.NumPositions; i++)
                    {
                        PointF matchCenter = new PointF(matcher.GetPosition(i).CenterX, matcher.GetPosition(i).CenterY);

                        for (float radius = 100; radius <= maxRadius; radius += 1)
                        {
                            MyCircleFinder = new CircleFinder(radius); //每次用新的半徑
                            MyCircleFinder.Measure(findHoleImage, matchCenter);

                            //若測量成功跳出迴圈(誤差 <= n像素)
                            if (MyCircleFinder.CircleGauge.AverageDistance < 1f)
                            {
                                fittingCircle[i] = MyCircleFinder.CircleGauge.MeasuredCircle;
                                HoleCenter[i] = new PointF(fittingCircle[i].CenterX, fittingCircle[i].CenterY);
                                
                                break;
                            }
                        }
                        
                        //若無成功匹配圓，拋出錯誤
                        if (fittingCircle[i] == null)
                        {
                            throw new Exception("未精準定位圓");
                        }
                    }
                //}
                //else
                //{
                //    fittingCircle = new ECircle[HoleCenter.Length];

                //    //用先前的 HoleCenter 進行測量
                //    for (int i = 0; i < HoleCenter.Length; i++)
                //    {
                //        for (float radius = 100; radius <= maxRadius; radius += 1)
                //        {
                //            MyCircleFinder = new CircleFinder(radius);
                //            MyCircleFinder.Measure(findHoleImage, HoleCenter[i]);

                //            if (MyCircleFinder.CircleGauge.AverageDistance < 1f)
                //            {
                //                fittingCircle[i] = MyCircleFinder.CircleGauge.MeasuredCircle;
                //                break;
                //            }
                //        }
                        
                //        //若無成功匹配圓，拋出錯誤
                //        if (fittingCircle[i] == null)
                //        {
                //            throw new Exception("未精準定位圓!");
                //        }
                //    }
                //}                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return fittingCircle;
        }
        
        private void buttonExecute_Click(object sender, EventArgs e)
        {
            if(pictureBoxSource.Image == null)
            {
                MessageBox.Show($"請先選擇圖片");
                return;
            }
            else
            {                    
                try
                {
                    string directoryPath = @"D:\Ryan_Chiu\TestImg";
                    string fileName = "marked_image.jpeg";
                    string filePath = Path.Combine(directoryPath, fileName);
                    
                    var fittingCircle = MatchByHole();
                    Draw(fittingCircle, filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.ToString()}");
                }                
            }            
        }
        
        public void Draw(ECircle[] measuredCircles, string filePath)
        {
            //檢查是否有偵測到圓形
            if (measuredCircles != null && measuredCircles.Length > 0)
            {                
                // 載入原始圖片
                Bitmap originalImage = new Bitmap(SourceFile.FileName);
                Bitmap rgbImage = new Bitmap(originalImage.Width, originalImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                using (Graphics g = Graphics.FromImage(rgbImage))
                {
                    //將原圖繪製到新轉換的 RGB 格式影像上
                    g.DrawImage(originalImage, new Rectangle(0, 0, rgbImage.Width, rgbImage.Height));

                    //定義十字線顏色與寬度
                    Pen pen = new Pen(Color.Red, 2);

                    //繪製每個圓形
                    foreach (ECircle circle in measuredCircles)
                    {
                        //確保每個圓形有效
                        if (circle != null)
                        {                            
                            MessageBox.Show($"圓形位置 : X = {circle.CenterX}, Y = {circle.CenterY}, Radius = {circle.Radius}");

                            //繪製水平線
                            g.DrawLine(pen, new PointF(circle.CenterX - 50, circle.CenterY),
                                               new PointF(circle.CenterX + 50, circle.CenterY));

                            //繪製垂直線
                            g.DrawLine(pen, new PointF(circle.CenterX, circle.CenterY - 50),
                                               new PointF(circle.CenterX, circle.CenterY + 50));
                        }
                    }

                    //儲存已標記圖片
                    if (File.Exists(filePath))
                    {
                        //刪除已存在的檔案
                        File.Delete(filePath);
                    }
                    rgbImage.Save(filePath, ImageFormat.Jpeg);
                }
                //顯示已標記圖片
                this.pictureBoxSource.Image = Image.FromFile(filePath);
            }
            else
            {
                MessageBox.Show("未檢測到圓形");
            }

        }   
    }
}
