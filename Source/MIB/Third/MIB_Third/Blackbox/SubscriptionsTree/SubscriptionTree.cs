using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

namespace Blackbox.SubscriptionsTree
{
    public class SubscriptionTree
    {
        
        private ITreeElement _root;

        private static Hashtable CreateSubcriptionAttributes(OperationContract operationContract)
        {

            Hashtable table = new Hashtable();

            foreach (ContentDescriptionAttribute attribute in operationContract.ContentDescription.AsEnumerable())
            {
                
                table.Add(attribute.Name,attribute.Value);

            }

            return table;

        }

        public SubscriptionTree()
        {
            
        }

        public SubscriptionTree(List<Subscription> subscriptions)
        {

            foreach (var subscription in subscriptions)
            {

                Add(subscription);

            }
            
        }

        public bool Add(Subscription subscription)
        {

            bool contains;

            Hashtable contentDescription = CreateSubcriptionAttributes(subscription);

            //
            // The first subscription on the tree
            //

            if ( _root == null )
            {

                _root = AddAlgorithm.AddSubscriptionAlgoritm(null, subscription, contentDescription, out contains );

            }

            //
            // Tree has already been created
            //

            else
            {

                _root.VisitAdd(subscription, contentDescription, out contains);

            }

            return contains;

        }

        public void Remove(Subscription subscription)
        {

            if ( _root != null )
            {

                _root = _root.VisitRemove(subscription, CreateSubcriptionAttributes(subscription));

            }

        }

        public List<Subscription> Get(Event @event)
        {

            return _root != null ? _root.VisitMatch(CreateSubcriptionAttributes(@event)) : new List<Subscription>();

        }

        public List<Subscription> GetAll()
        {

            return _root != null ? _root.VisitGetAll() : new List<Subscription>();

        }

    }
}
