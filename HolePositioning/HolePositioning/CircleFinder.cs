using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Euresys.Open_eVision_2_2;

namespace HolePositioning
{
    class CircleFinder
    {
        public EWorldShape WorldShape;
        public ECircleGauge CircleGauge;

        public CircleFinder(float radius)
        {
            WorldShape = new EWorldShape();
            CircleGauge = new ECircleGauge
            {

                //for 外同軸
                Radius = radius,
                Tolerance = 10.00f,
                HVConstraint = false,
                Threshold = 1,
                MinAmplitude = 5,
                Thickness = 3,
                Smoothing = 1,
                TransitionChoice = ETransitionChoice.NthFromBegin,
                TransitionType = ETransitionType.Bw,
                NumFilteringPasses = 3,
                FilteringThreshold = 2.00f,
                SamplingStep = 5f
            };

            // CircleGauge.Labeled = true;

            // WorldShape.CalibrationModes = (int)ECalibrationMode.Scaled;
            CircleGauge.Attach(WorldShape);
        }

        /// <summary>
        /// 設定所要尋找的圓大概的圓心位置(以worldShape.CenterXY為原點)
        /// </summary>
        /// <param name="center"></param>
        public void SetCircleGaugeCenter(EPoint center)
        {
            CircleGauge.Center = center;
        }
        public void SetCircleGaugeCenter(System.Drawing.PointF center)
        {
            EPoint point = new EPoint(center.X, center.Y);
            SetCircleGaugeCenter(point);
        }

        /// <summary>
        /// 設定圓的尋找位置與範圍(目標影像中的ROI
        /// </summary>
        /// <param name="roi"></param>
        public void SetCircleGaugeRange(EROIBW8 roi)
        {
            WorldShape.SetSensorSize(roi.Width, roi.Height);
            WorldShape.SetCenterXY(roi.OrgX, roi.OrgY);
        }
        /// <summary>
        /// 設定圓的尋找範圍
        /// </summary>
        /// <param name="orgX">尋找範圍矩形的原點X座標</param>
        /// <param name="orgY">尋找範圍矩形的原點Y座標</param>
        /// <param name="width">尋找範圍矩形的寬</param>
        /// <param name="height">尋找範圍矩形的高</param>
        public void SetCircleGaugeRange(int orgX, int orgY, int width, int height)
        {
            WorldShape.SetSensorSize(width, height);
            WorldShape.SetCenterXY(orgX, orgY);
        }

        public void Measure(EBaseROI image)
        {
            EROIBW8 eROIBW8 = (EROIBW8)image;
            CircleGauge.Measure(eROIBW8);
        }
        public void Measure(EBaseROI image, System.Drawing.PointF center)
        {
            EROIBW8 eROIBW8 = (EROIBW8)image;
            SetCircleGaugeCenter(center);
            CircleGauge.Measure(eROIBW8);
        }

        public void Dispose()
        {
            CircleGauge.Dispose();
            WorldShape.Dispose();
        }
    }
}
