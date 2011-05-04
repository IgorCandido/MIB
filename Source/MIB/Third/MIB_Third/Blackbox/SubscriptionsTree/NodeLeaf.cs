using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

namespace Blackbox.SubscriptionsTree
{
    public class NodeLeaf : INode
    {

        public List<Subscription> Subscriptions { get; set; }

        public NodeLeaf()
        {
            
            Subscriptions = new List<Subscription>();

        }


        #region Implementation of ITreeElement

        public ITreeElement VisitAdd(Subscription subscription, Hashtable subscriptionAttributes, out bool contains)
        {

            //    
            // If subscription doesn't have more attributes    
            //

            if (subscriptionAttributes.Count == 0)
            {

                //
                // Add subscription to current Nodeleaf
                //

                contains = Subscriptions.Contains(subscription);

                if( !contains )
                {
                    
                    Subscriptions.Add(subscription);

                }

                return this;

            }

            else
            {

                //
                // Insert a serie of Nodeconditions with nodeleaf with subscription on the end
                //

               return AddAlgorithm.AddSubscriptionAlgoritm(this, subscription, subscriptionAttributes, out contains);
            }
        }

        public ITreeElement VisitRemove(Subscription subscription, Hashtable subscriptionAttributes)
        {
            
            //
            // The remove traversing has reached a matched subscription, remove it from nodeLeaf and if there isn't another
            // subscription remove this nodeLeaf from tree
            //

            Subscriptions.Remove(subscription);

            return Subscriptions.Count == 0 ? null : this;

        }

        public List<Subscription> VisitMatch(Hashtable subscriptionAttributes)
        {

            return Subscriptions;

        }

        public List<Subscription> VisitGetAll()
        {

            return Subscriptions;

        }

        #endregion
    }

}