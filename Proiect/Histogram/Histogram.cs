using AForge.Imaging;
using System.Drawing;

namespace Histogram
{
    public static class Histogram
    {
        public static void AForgeGetHistogram(Bitmap image)
        {
            var statistics = new ImageStatisticsHSL(image);
            var luminanceValues = statistics.Luminance.Values;

            var rgbStatistics = new ImageStatistics(image);
            var redValues = rgbStatistics.Red.Values;
            var greenValues = rgbStatistics.Green.Values;
            var blueValues = rgbStatistics.Blue.Values;
        }


    }


}
