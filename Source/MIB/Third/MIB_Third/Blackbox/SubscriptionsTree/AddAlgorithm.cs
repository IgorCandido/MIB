using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

//
// Temporary file with add algorithm
//

namespace Blackbox.SubscriptionsTree
{
    public static class AddAlgorithm
    {

        //
        // This method create a chain of NodeConditions corresponding to subscription
        //

        public static INode AddSubscriptionAlgoritm(NodeLeaf currentNodeLeaf, Subscription subscription,
                                                    Hashtable subscriptionAttributes, out bool contains)
        {

            IDictionaryEnumerator enumerator = subscriptionAttributes.GetEnumerator();

            int numberOfAttributes = subscriptionAttributes.Count;

            //
            // Content description value
            //

            //
            // Check if enumerator has to be moved next before first use
            //

            enumerator.MoveNext();

            string value = (string) enumerator.Value;

            NodeCondition topOfNodeConditionTree = CreateConditionNode(null, (string)enumerator.Key, null);

            //
            // If there is a nodeLeaf it has to be added to the first nodeCondition on don't care 
            //

            if (currentNodeLeaf != null)
            {

                topOfNodeConditionTree.DontCareLink = new Link() {To = currentNodeLeaf};

            }

            NodeCondition current = topOfNodeConditionTree;

            //
            // Create the series of nodeConditions
            //

            while ( numberOfAttributes-- > 1 )
            {

                //
                // Get next attribute
                //

                enumerator.MoveNext();

                //
                // Create one more nodeCondition, and add it to the nodecondition series
                //

                current = CreateConditionNode(current, (string) enumerator.Key, value);

                value = (string) enumerator.Value;

            }

            //
            // Create the nodeLeaf 
            //

            CreateNodeLeaf(current,value, subscription, out contains);

            //
            // Create a NodeLeaf with subscription
            // 


            return topOfNodeConditionTree;

        }

        private static NodeCondition CreateConditionNode(NodeCondition current, string subscriptionAttribute, 
                                                         string previousSubscriptionAttributeValue)
        {
            
            NodeCondition createdNodeCondition = new NodeCondition();

            createdNodeCondition.Field = subscriptionAttribute;

            //
            // Attach the created node to the previous node
            //

            AttachCreatedNodeToCurrentNode(current, previousSubscriptionAttributeValue , createdNodeCondition);

            return createdNodeCondition;

        }

        private static void CreateNodeLeaf(NodeCondition nodeCondition, string subscriptionAttributeValue, Subscription subscription, 
                                           out bool contains)
        {

            NodeLeaf createdNodeLeaf = new NodeLeaf()
                                           {
                                               Subscriptions = new List<Subscription>() {subscription}
                                           };

            contains = false;

            //
            // Attach the created node to the previous node
            //

            AttachCreatedNodeToCurrentNode(nodeCondition, subscriptionAttributeValue, createdNodeLeaf);


            
        }

        private static void AttachCreatedNodeToCurrentNode(NodeCondition current, string previousSubscriptionAttributeValue, 
                                                           INode createdNode)
        {
            //
            // If this node isn't the first of nodeCondition series
            //

            if (current != null)
            {

                //
                // Add the created node to the previows nodecondition of the series
                //

                current.Childreen.Add(previousSubscriptionAttributeValue,
                                                   new Link()
                                                   {
                                                       To = createdNode,
                                                       Value = previousSubscriptionAttributeValue
                                                   });

            }
        }
    }
}
