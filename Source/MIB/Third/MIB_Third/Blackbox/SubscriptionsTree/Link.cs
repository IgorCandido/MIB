

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
        
        public ITreeElement To { get; set; }
        public string Value { get; set; }

        #region Implementation of ITreeElement

        public ITreeElement VisitAdd(Subscription subscription, Hashtable subscriptionAttributes, out bool contains)
        {

            //
            // If there is a TO node
            //

            if( To != null )
            {

                //
                // Add subscription there
                // 

                To = To.VisitAdd(subscription, subscriptionAttributes, out contains);

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

                    To = new NodeLeaf(){ Subscriptions = new List<Subscription>(){subscription}};

                    //
                    // Node leaf created so this subscription cannot already exist
                    //

                    contains = false;

                }

                else
                {
                    
                    //
                    // Insert a serie of Nodeconditions with nodeleaf with subscription on the end
                    //

                    To = AddAlgorithm.AddSubscriptionAlgoritm(null, subscription, subscriptionAttributes, out contains);
                }
            }

            return this;

        }

        public ITreeElement VisitRemove(Subscription subscription, Hashtable subscriptionAttributes)
        {

            To = To != null ? (To = To.VisitRemove(subscription, subscriptionAttributes)) : null;

            return To != null ? this : null;

        }

        public List<Subscription> VisitMatch(Hashtable subscriptionAttributes)
        {

            //
            // Return the return of matching of To node
            //

            return To.VisitMatch(subscriptionAttributes);

        }

        public List<Subscription> VisitGetAll()
        {

            return To != null ? To.VisitGetAll() : null;

        }

        #endregion

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

}

