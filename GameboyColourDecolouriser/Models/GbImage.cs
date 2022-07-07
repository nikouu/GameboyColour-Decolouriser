using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyColourDecolouriser.Models
{
    public class GbImage : DmgImage
    {
        public GbImage(int width, int height, ITile[,] tiles) : base(width, height, tiles)
        {
        }
    }
}
