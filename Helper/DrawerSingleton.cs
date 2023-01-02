using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Helper
{
    internal class DrawerSingleton
    {
        private static Drawer? drawer;
        public static Drawer GetInstance(int width, int height)
        {
            if (drawer == null)
                drawer = new Drawer(width, height);
            return drawer;
        }
    }
}
