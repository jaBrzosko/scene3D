using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Lights
{
    internal class LightSingleton
    {
        private static List<ILight> lights;

        public static List<ILight> GetInstance()
        {
            if(lights == null)
                lights = new List<ILight>();
            return lights;
        }

        public static void AddLight(ILight light)
        {
            if (lights == null)
                lights = new List<ILight>();
            lights.Add(light);
        }
    }
}
