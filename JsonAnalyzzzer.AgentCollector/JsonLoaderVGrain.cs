using Democrite.Framework.Core;
using Democrite.Framework.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnalyzzzer.AgentCollector
{
    internal class JsonLoaderVGrain : VGrainBase<IJsonLoaderVGrain>, IJsonLoaderVGrain
    {
        public JsonLoaderVGrain(ILogger<IJsonLoaderVGrain> logger) : base(logger)
        {

        }

        public async Task<string> LoadJsonAsync(string FullPath, IExecutionContext Context)
        {
            
            var result = await File.ReadAllTextAsync(FullPath);
            return result;
 //      
        }
    }
}
