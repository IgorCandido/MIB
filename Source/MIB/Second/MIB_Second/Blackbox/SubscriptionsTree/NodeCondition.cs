using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

namespace Blackbox.SubscriptionsTree
{
    class NodeCondition : INode
    {
        
        public Link DontCareLink { get; set; }      // * Link
        public Hashtable Childreen { get; set; }    // Links
        public string Field { get; set; }           // Field

        public NodeCondition()
        {
            Childreen = new Hashtable();
        }

        #region Implementation of ITreeElement

        public ITreeElement VisitAdd(Subscription subscription, Hashtable subscriptionAttributes)
        {

            string subscriptionValueAttribute;

            //
            // If subscription has defined a value for the condition this node defines
            //

            if( ( subscriptionValueAttribute = (string)subscriptionAttributes[Field] ) != null )
            {

                Link linkCorrespondingToEventValue;

                //
                // The field corresponding to current NodeCondition has already been tested, so remove it from 
                // subscription fields to test
                //

                subscriptionAttributes.Remove(Field);

                //
                // There is a link that has the same value as the one defined in subscription for the node condition
                //

                if ( ( linkCorrespondingToEventValue = (Link)Childreen[subscriptionValueAttribute] ) != null )
                {

                    linkCorrespondingToEventValue.VisitAdd(subscription, subscriptionAttributes);
                   
                }

                //
                // There is no link with a value associated equals to the one that the same attribute of the event has
                //

                else
                {

                    Link createdLink = new Link() { From = this, Value = subscriptionValueAttribute };

                    Childreen.Add(subscriptionValueAttribute, createdLink);

                    createdLink.VisitAdd(subscription, subscriptionAttributes);

                }

            }

            //
            // The event doesn't define the attribute corresponding to NodeCondition field
            //

            else
            {

                //
                // Add a don't care link, if there isn't one
                //

                if( DontCareLink == null )
                {

                    DontCareLink = new Link() {From = this};

                }

                //
                // Add the subscription to that don't care link
                //

                DontCareLink.VisitAdd(subscription, subscriptionAttributes);

            }

            return this;

        }

        public bool VisitRemove(Subscription subscription, Hashtable subscriptionAttributes)
        {
            
            string subscriptionValueAttribute;

            //
            // See if there is an attribute in event that match NodeCondition field
            //

            if ((subscriptionValueAttribute = (string)subscriptionAttributes[Field]) != null)
            {

                Link linkCorrespondingToEventValue;

                //
                // Remove attribute from event, because it has already been evaluated
                //

                subscriptionAttributes.Remove(Field);

                //
                // See if there is a link with the value equal to the one in event
                //

                if ((linkCorrespondingToEventValue = (Link)Childreen[subscriptionValueAttribute]) != null)
                {

                    //
                    // If this link is removed, remove it from childreen of current nodeCondition
                    //

                    if( linkCorrespondingToEventValue.VisitRemove(subscription, subscriptionAttributes) )
                    {
                        
                        Childreen.Remove(linkCorrespondingToEventValue.Value);

                    }
                }

            }

            //
            // Try to remove nodes from don't care link
            //

            if (DontCareLink != null)
            {

                //
                // If don't care link is removed, remove it from current nodeCondition
                //

                if( DontCareLink.VisitRemove(subscription, subscriptionAttributes) )
                {

                    DontCareLink = null;

                }

            }

            //
            // There aren't links on current nodeCondition, so it can't be removed
            //

            return Childreen.Count == 0 && DontCareLink == null;
            

        }

        public List<Subscription> VisitMatch(Hashtable Attributes)
        {

            List<Subscription> matchedSubscriptions = new List<Subscription>();

            string subscriptionValueAttribute;
            
            //
            // See if there is an attribute in event that match NodeCondition field
            //

            if( ( subscriptionValueAttribute = (string)Attributes[Field] ) != null )
            {
                
                Link linkCorrespondingToEventValue;

                //
                // Remove attribute from event, because it has already been evaluated
                //

                Attributes.Remove(Field);

                //
                // See if there is a link with the value equal to the one in event
                //

                if ( ( linkCorrespondingToEventValue = (Link)Childreen[subscriptionValueAttribute] ) != null )
                {
                    
                    //
                    // Return the return of match through the selected link
                    //

                    matchedSubscriptions.AddRange(linkCorrespondingToEventValue.VisitMatch(Attributes));

                }
               
            }

            //
            // Return the result of don't care link, if there is one, else return empty list
            //

            if( DontCareLink != null )
            {

                matchedSubscriptions.AddRange(DontCareLink.VisitMatch(Attributes));

            }

            return matchedSubscriptions;

        }

        #endregion
    }

}