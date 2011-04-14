using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Pipeline
{
    public static class PipelineFactory
    {

        private static void RegisterPipelineEventHandler(Pipeline pipeline, IPipelineEventHandler eventHandler)
        {
            
            eventHandler.RegisterEventHandlers(pipeline);

        }

        public static Pipeline CreatePipeline(IPipelineEventHandler handler)
        {
            
            Pipeline createdPipeline = new Pipeline();

            RegisterPipelineEventHandler(createdPipeline, handler);

            return createdPipeline;

        }

        public static Pipeline CreatePipeline(List<IPipelineEventHandler> handlers)
        {

            Pipeline createdPipeline = new Pipeline();

            foreach (var pipelineEventHandler in handlers)
            {
                
                RegisterPipelineEventHandler(createdPipeline, pipelineEventHandler);

            }
            
            return createdPipeline;

        }

    }
}
