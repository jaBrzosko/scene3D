using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Lights
{
    internal class LightSingleton
    {
        private static List<Light> lights;

        public static List<Light> GetInstance()
        {
            if(lights == null)
                lights = new List<Light>();
            return lights;
        }

        public static void AddLight(Light light)
        {
            if (lights == null)
                lights = new List<Light>();
            lights.Add(light);
        }
    }
}
