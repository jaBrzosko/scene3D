﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3D.Objects.Chess
{
    internal class Knight : Piece
    {
        public Knight(Model model, int row, int col, float offset, float step, int numberOfSteps) : base(model, row, col, offset, step, numberOfSteps)
        { }
    }
}
