using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

namespace Blackbox.Pipeline
{
    public interface IPipelineEventHandler
    {

        void RegisterEventHandlers(Pipeline pipeline);

        void UnRegisterEventHandlers(Pipeline pipeline);

    }
}
