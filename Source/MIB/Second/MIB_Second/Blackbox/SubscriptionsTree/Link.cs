

using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

namespace Blackbox.SubscriptionsTree
{
    public class Link : ITreeElement
    {
        
        public INode From { get; set; }
        public INode To { get; set; }
        public string Value { get; set; }

        #region Implementation of ITreeElement

        public ITreeElement VisitAdd(Subscription subscription, Hashtable subscriptionAttributes)
        {

            //
            // If there is a TO node
            //

            if( To != null )
            {

                //
                // Add subscription there
                // 

                To = (INode) To.VisitAdd(subscription, subscriptionAttributes);

            }

            else
            {

                //    
                // If subscription doesn't have more attributes    
                //

                if (subscriptionAttributes.Count == 0)
                {
                    
                    //
                    // Add a nodeleaf with subscription on it
                    //

                    To = new NodeLeaf(){Father = this, Subscriptions = new List<Subscription>(){subscription}};

                }

                else
                {
                    
                    //
                    // Insert a serie of Nodeconditions with nodeleaf with subscription on the end
                    //

                    To = AddAlgorithm.AddSubscriptionAlgoritm(null, subscription, subscriptionAttributes);
                }
            }

            return this;

        }

        public bool VisitRemove(Subscription subscription, Hashtable subscriptionAttributes)
        {

            return To == null || To.VisitRemove(subscription, subscriptionAttributes);

        }

        public List<Subscription> VisitMatch(Hashtable subscriptionAttributes)
        {

            //
            // Return the return of matching of To node
            //

            return To.VisitMatch(subscriptionAttributes);

        }

        #endregion

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

}

