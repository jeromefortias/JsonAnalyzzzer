﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnalyzzzer.AgentInspector
{
    [GenerateSerializer]
    public class InspectorState
    {
        [Id(0)]
        public int counter { get; set; }
    }
}
