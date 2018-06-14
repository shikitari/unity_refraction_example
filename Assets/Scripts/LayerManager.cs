using UnityEngine;
using System.Collections;

namespace main
{
    public class LayerManager
    {
        public enum layer : int
        {
            LensLayer = 1 << 8,
            LensBackLayer = 1 << 9,
            LensAndWall = LensLayer + (1 << 10),

            DefautAndCup = CupInner + CupBottom + 1,
            

            CupInner = 1 << 8,
            CupBottom = 1 << 9,
            CupInnerAndCupBottom = CupInner + CupBottom,
            CupWaterSurface = 1 << 10,
            DefautAndCupWater = CupWaterSurface + CupInner + CupBottom + 1,
        } 
    }
}