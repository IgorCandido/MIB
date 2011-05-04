using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interfaces.Exceptions.DataBase;

namespace Blackbox.Data
{
    public static class MIB_DAL
    {

        private static void EliminateEventIfThereArentMoreSubscriptionsForIt(DataBaseMapperDataContext db, int eventId)
        {

            if (db.Event_Subscriptions.Where(event_sub => event_sub.Event == eventId).Count() == 0)
            {

                db.Events.DeleteOnSubmit(db.Events.SingleOrDefault(@event => @event.EventHash == eventId));

                db.SubmitChanges();

            }

        }

        #region Subscription

        /// <summary>
        /// Store a subscription on the database
        /// </summary>
        /// <param name="subscription">The subscription to store on the database</param>
        public static void SaveSubscription(Interfaces.ContractsInnerRepresentations.Subscription subscription)
        {
            try
            {
                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {

                    if( db.Subscriptions.Where( sub => sub.SubscriptionHash == subscription.GetHashCode()).Count() == 0)
                    {

                        db.Subscriptions.InsertOnSubmit( subscription.Map() );

                        db.SubmitChanges();

                    }

                }
            }
            catch (SqlException e)
            {

                throw new ConnectionToDataBaseException();
            }


        }

        /// <summary>
        /// Stores a list of subscriptions on the database
        /// </summary>
        /// <param name="subscriptions">The list of subscriptions subscriptions to be stored on the database</param>
        public static void SaveSubscriptions(List<Interfaces.ContractsInnerRepresentations.Subscription> subscriptions)
        {
            try
            {
                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {

                    db.Subscriptions.InsertAllOnSubmit(subscriptions.Select(sub => sub.Map()));

                    db.SubmitChanges();

                }
            }
            catch (SqlException e)
            {

                throw new ConnectionToDataBaseException();
            }
            
            
        }

        /// <summary>
        /// Gets all subscriptions currently stored on the database
        /// </summary>
        /// <returns></returns>
        public static List<Interfaces.ContractsInnerRepresentations.Subscription> GetAllSubscriptions()
        {
            try{

                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {

                    return db.Subscriptions.Select( sub => sub.Map() ).ToList();
                
                }
            }
            catch (SqlException e)
            {

                throw new ConnectionToDataBaseException();
            }

        }

        /// <summary>
        /// Deletes a subscription from the database and all the events persisted just because of this particular subscription
        /// </summary>
        /// <param name="subscription">The subscription to be deleted from the database</param>
        public static void DeleteSubscription(Interfaces.ContractsInnerRepresentations.Subscription subscription)
        {
            try
            {

                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {

                    Subscription subscriptionDb = db.Subscriptions.FirstOrDefault(sub => sub.SubscriptionHash == subscription.GetHashCode());

                    int [] events_persisted = db.Event_Subscriptions.Where(event_sub => event_sub.Subscription == subscription.GetHashCode()).Select(event_sub => event_sub.Event).ToArray();

                    //
                    // There is no subscription with that hash
                    //

                    if(subscriptionDb == null)
                    {

                        return;

                    }

                    db.Subscriptions.DeleteOnSubmit(subscriptionDb);

                    db.SubmitChanges();

                    //
                    // Cleanup
                    //

                    foreach (var eventId in events_persisted)
                    {
                        
                        //
                        // This was the last subscription wanting to persist this event, so remove it from the database
                        //

                        EliminateEventIfThereArentMoreSubscriptionsForIt(db, eventId);

                    }

                }

            }
            catch (SqlException e)
            {

                throw new ConnectionToDataBaseException();
            }
            

        }

        /// <summary>
        /// Deletes a list of subscriptions from the database
        /// </summary>
        /// <param name="subscriptions">The list subscriptions of subscriptions to be deleted from the database</param>
        public static void DeleteSubscriptions(List<Interfaces.ContractsInnerRepresentations.Subscription> subscriptions)
        {

            foreach (var subscription in subscriptions)
            {
                
                DeleteSubscription(subscription);

            }
        }

        #endregion

        #region Event_Subscriptions

        /// <summary>
        /// Saves an event, storing the relationships between this event and the subscriptions that required the event persistence
        /// </summary>
        /// <param name="subscriptions">The list of subscriptions requiring event persistence</param>
        /// <param name="event">The event to be stored at the database</param>
        public static void SaveEventWithSubscriptions(List<Interfaces.ContractsInnerRepresentations.Subscription> subscriptions, 
                                                      Interfaces.ContractsInnerRepresentations.Event @event)
        {

            try
            {

                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {
                    
                    //
                    // Store the event
                    //

                    db.Events.InsertOnSubmit(@event.Map());

                    //
                    // Store the list of subscriptions
                    //

                    foreach (var subscription in subscriptions)
                    {
                        
                        db.Event_Subscriptions.InsertOnSubmit(new Event_Subscription()
                                                                  {
                                                                      Event = @event.GetHashCode(), 
                                                                      Subscription = subscription.GetHashCode()
                                                                  }
                                                              );

                    }

                    db.SubmitChanges();

                }

            }
            catch (SqlException e)
            {

                throw new ConnectionToDataBaseException();
            }

        }

        public static List<Interfaces.ContractsInnerRepresentations.Event> GetEventsPersisted(Interfaces.ContractsInnerRepresentations.Subscription subscription)
        {

            try
            {

                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {

                    return db.Event_Subscriptions.Where(eventSub => eventSub.Subscription == subscription.GetHashCode()).
                        Select(
                            eventSub => db.Events.SingleOrDefault(@event => @event.EventHash == eventSub.Event).Map())
                        .ToList();

                }

            }
            catch (SqlException)
            {
                
                throw new ConnectionToDataBaseException();

            }

        }

        public static void EliminateTheInterestOfAnSubscriptionToAnEvent(Interfaces.ContractsInnerRepresentations.Event @event,
                                                                        Interfaces.ContractsInnerRepresentations.Subscription subscription)
        {

            try
            {

                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {

                    int eventHash = @event.GetHashCode();

                    Event_Subscription eventSubscription =
                        db.Event_Subscriptions.SingleOrDefault(
                            eventSub =>
                            eventSub.Subscription == subscription.GetHashCode() &&
                            eventSub.Event == eventHash);

                    //
                    // There is no interest of subscription on the event passed as parameter
                    //

                    if( eventSubscription != null )
                    {

                        db.Event_Subscriptions.DeleteOnSubmit(eventSubscription);

                        db.SubmitChanges();

                        EliminateEventIfThereArentMoreSubscriptionsForIt(db, eventHash);

                    }

                }

            }
            catch (SqlException)
            {
                
                throw new ConnectionToDataBaseException();
            }

        }

        #endregion

        #region Event

        public static Interfaces.ContractsInnerRepresentations.Event GetEvent(int eventHashCode)
        {

            try
            {

                using (DataBaseMapperDataContext db = new DataBaseMapperDataContext())
                {

                    Event retreviedEvent;

                    return ( retreviedEvent = db.Events.SingleOrDefault(@event => @event.EventHash == eventHashCode) ) != null ? 
                                                                                                            retreviedEvent.Map() : null;

                }

            }
            catch (SqlException)
            {
                
                throw new ConnectionToDataBaseException();
                throw;
            }

        }

        #endregion
    }

}
