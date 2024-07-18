using Democrite.Framework.Core.Abstractions;
using Democrite.Framework.Core.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JsonAnalyzzzer.AgentStorage
{
    [VGrainIdFormat(Democrite.Framework.Core.Abstractions.Enums.IdFormatTypeEnum.String, FirstParameterTemplate = "{executionContext.Configuration}")]
    public interface IJsonSaveVGrain : IVGrain
    {
        Task<string> SaveJsonAsync(string json, IExecutionContext<string> ctx);
    }
}
