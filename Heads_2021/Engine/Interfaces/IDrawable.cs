﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    interface IDrawable
    {
        DrawLayer Layer { get; }
        void Draw();
    }
}
