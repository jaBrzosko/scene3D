using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Movers
{
    internal class Vibrator
    {
        public float MaxR { get; set; }
        private Random random;
        public Vibrator(float maxR, int? seed = null)
        {
            MaxR = maxR;
            random = seed != null ? new Random((int)seed) : new Random();
        }

        public Vector3 GetVibration()
        {
            return new Vector3((float)(random.NextDouble() * 2 * MaxR - MaxR),
                            (float)(random.NextDouble() * 2 * MaxR - MaxR),
                            (float)(random.NextDouble() * 2 * MaxR - MaxR));
        }

    }
}
