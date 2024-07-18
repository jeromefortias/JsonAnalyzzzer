using Democrite.Framework.Core.Abstractions;
using Democrite.Framework.Core.Abstractions.Attributes;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnalyzzzer.AgentInspector
{
    [VGrainIdFormat(Democrite.Framework.Core.Abstractions.Enums.IdFormatTypeEnum.String, FirstParameterTemplate = "{executionContext.Configuration}")]
    public interface IJsonInspectorVGrain : IVGrain
    {
        Task<string> AnalyzeJsonAsync(string json, IExecutionContext<string> ctx);
        [ReadOnly]
        Task<int> GetCounter(IExecutionContext<string> ctx);
    }
}
