using System.Collections.Generic;
using System.Linq;
using Interfaces.RequestHandling;

namespace Interfaces.ContractsInnerRepresentations
{

    //
    // This interface is the root of all operation contracts that blackbox receive from IReceivers
    //

    public abstract class OperationContract
    {
        
        public List<ContentDescriptionAttribute> ContentDescription { get; set; }


        public abstract void HandleRequestVisit(IHandleRequest handler, object args);

        public override bool Equals(object obj)
        {

            OperationContract arg;

            if( ( arg = obj as OperationContract ) != null )
            {
                return ContentDescription.All(contentDescriptionAttribute => arg.ContentDescription.Exists(t => t.Value.Equals(contentDescriptionAttribute.Value) && t.Name.Equals(contentDescriptionAttribute.Name)));
            }

            return base.Equals(obj);
        }
    }
}
