using Democrite.Framework.Core;
using Democrite.Framework.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Elasticsearch.Net;

namespace JsonAnalyzzzer.AgentStorage
{
    internal class JsonSaveVGrain : VGrainBase<IJsonSaveVGrain>, IJsonSaveVGrain
    {
        private ElasticClient client;
        public JsonSaveVGrain(ILogger<IJsonSaveVGrain> logger) : base(logger)
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("testagent"); 
            client = new ElasticClient(settings);
        }

        public Task<string> SaveJsonAsync(string json, IExecutionContext<string> ctx)
        {
            try
            {
                string path = "n:\\" + DateTime.Now.Ticks + "re.txt"; 
                File.WriteAllText(path, json);
                return Task.FromResult(path);
            }
            catch(Exception ex) {
             return Task.FromResult(ex.Message);
            }
            
        }
    }
}
