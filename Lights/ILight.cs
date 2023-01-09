using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Scene3D.Lights
{
    internal interface ILight
    {
        public Vector3 GetLightColor(Vector3 L, float m);
        public Vector3 GetWorldPosition();
    }
}
