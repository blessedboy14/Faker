﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public interface IGenerator
    {
        object Generate(GeneratorInfo info);
    }
}
