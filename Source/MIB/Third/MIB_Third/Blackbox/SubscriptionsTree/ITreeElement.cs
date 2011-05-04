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
    public interface ITreeElement
    {

        ITreeElement VisitAdd(Subscription subscription, Hashtable subscriptionAttributes, out bool contains);

        //
        // Return: True means that ITreeElement can be removed from the tree
        //

        ITreeElement VisitRemove(Subscription subscription, Hashtable subscriptionAttributes);

        List<Subscription> VisitMatch(Hashtable eventAttributes);

        List<Subscription> VisitGetAll();

    }
}
