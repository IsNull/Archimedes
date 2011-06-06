using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Archimedes.Geometry.Extensions
{
    public enum ScanSideBegin
    {
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }

    public static class BitmapBoundingBoxExtension
    {

        public static Rectangle BoundingBox(this Bitmap oBitmap, Color BackGrundColor) {
            var BoundingBox = new Rectangle();

            var DownRight = new Point(oBitmap.FindFirstPixelDist(ScanSideBegin.RIGHT, BackGrundColor), oBitmap.FindFirstPixelDist(ScanSideBegin.BOTTOM, BackGrundColor));
            var UpperLeft = new Point(oBitmap.FindFirstPixelDist(ScanSideBegin.LEFT, BackGrundColor), oBitmap.FindFirstPixelDist(ScanSideBegin.TOP, BackGrundColor));

            DownRight.Y -= 2; //Correction 

            BoundingBox.Location = UpperLeft;
            BoundingBox.Width = (DownRight.X - UpperLeft.X);
            BoundingBox.Height = (DownRight.Y - UpperLeft.Y);

            return BoundingBox;
        }


        public static int FindFirstPixelDist(this Bitmap oBitmap, ScanSideBegin uScanSide, Color BackGrundColor) {
            bool bColorFound = false;
            int i = 0;

            switch (uScanSide) {

            #region RIGHT

            case ScanSideBegin.RIGHT:    // ------------ Search First Pixel RIGHT 
                for (i = (oBitmap.Width - 1); i > 0; i--) {

                    for (int i2 = 1; i2 < oBitmap.Height; i2++) {
                        if (oBitmap.GetPixel(i, i2) != BackGrundColor) {
                            bColorFound = true;
                            break;
                        }
                    }
                    if (bColorFound) { break; }
                }
            break;

            #endregion

            #region LEFT

            case ScanSideBegin.LEFT: // ------------ Search First Pixel LEFT
            
                for (i = 1; i < oBitmap.Width; i++) {

                    for (int i2 = 1; i2 < oBitmap.Height; i2++) {
                        if (oBitmap.GetPixel(i, i2) != BackGrundColor) {
                            bColorFound = true;
                            break;
                        }
                    }
                    if (bColorFound) { break; }
                }
                //UpperLeft.X = i - 1;
            break;

            #endregion

            #region DOWN

            case ScanSideBegin.BOTTOM: // ------------ Search First Pixel Down
                for (i = (oBitmap.Height - 1); i > 0; i--) {

                    for (int i2 = 1; i2 < oBitmap.Width; i2++) {
                        if (oBitmap.GetPixel(i2, i) != BackGrundColor) {
                            bColorFound = true;
                            break;
                        }
                    }
                    if (bColorFound) { break; }
                }
                //DownRight.Y = i;
            break;

            #endregion

            #region UP

            case ScanSideBegin.TOP: // ------------ Search First Pixel UP
                for (i = 1; i < oBitmap.Height; i++) {
                    for (int i2 = 1; i2 < oBitmap.Width; i2++) {
                        if (oBitmap.GetPixel(i2, i) != BackGrundColor) {
                            bColorFound = true;
                            break;
                        }
                    }
                    if (bColorFound) { break; }
                }
                //UpperLeft.Y = i - 1;
            break;

            #endregion

            }

            return i;
        }
    }
}
