using Democrite.Framework.Core;
using Democrite.Framework.Core.Abstractions;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnalyzzzer.AgentInspector
{
    internal class JsonInspectorVGrain : VGrainBase<InspectorState, IJsonInspectorVGrain>, IJsonInspectorVGrain
    {
        public JsonInspectorVGrain(ILogger<IJsonInspectorVGrain> logger,
                                   [PersistentState("Inspector")] IPersistentState<InspectorState> persistentState) 
            : base(logger, persistentState)
        {

        }

        public Task<string> AnalyzeJsonAsync(string json, IExecutionContext<string> ctx)
        {
            this.State.counter++;

            json = json.Replace("{", "{"+Environment.NewLine);
            return Task.FromResult(json);
            
            //throw new NotImplementedException();
        }

        public Task<int> GetCounter(IExecutionContext<string> ctx)
        {
            return Task.FromResult(this.State.counter);
            
            throw new NotImplementedException();
        }
    }
}
