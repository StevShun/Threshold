using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using log4net;
using threshold.Events.Actions;
using threshold.Events.Types;
using System.Threading;

namespace threshold.Events.Conduit
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
    class DefaultEventConduit : IEventConduit
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultEventConduit));
        private BlockingCollection<IAction> Actions;
        private Dictionary<EventType, List<WeakReference<IEventListener>>> EventTypesToListeners;
        private BackgroundWorker _BackgroundWorker;
        private List<IEventListener> RemoveItems;

        public DefaultEventConduit()
        {
            Actions = new BlockingCollection<IAction>();
            EventTypesToListeners = new Dictionary<EventType, List<WeakReference<IEventListener>>>();
            _BackgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
        }

        public void AddEventListener(IEventListener eventListener)
        {
            Log.Debug("Added event listener " + eventListener.ToString());
            Actions.Add(new AddListenerAction(eventListener));
        }

        public void RemoveEventListener(IEventListener eventListener)
        {
            Log.Debug("Removed event listener " + eventListener.ToString());
            Actions.Add(new RemoveListenerAction(eventListener));
        }

        public void SendEvent(IEvent _event)
        {
            EventType eventType = _event.GetEventType();
            if (_BackgroundWorker.IsBusy)
            {
                Log.Debug("Sending event: " + eventType.ToString());
                Actions.Add(new OfferEventAction(_event));
            }
            else
            {
                Log.Warn("Failed to send event " + eventType.ToString()
                    + " because the conduit thread is not running");
            }
        }

        public void Start()
        {
            Log.Info("Starting Event Conduit thread...");
            _BackgroundWorker.DoWork += ProcessEvents;
            _BackgroundWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            _BackgroundWorker.CancelAsync();
            Log.Info("Stopping Event Conduit thread...");
            bool isCleanStop = false;
            for (int i = 0; i < 10; i++)
            {
                if (_BackgroundWorker.IsBusy)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    isCleanStop = true;
                    break;
                }
            }
            if (isCleanStop)
            {
                Log.Debug("Stopped event conduit");
            }
            else
            {
                Log.Error("Failed to stop event conduit");
            }
        }

        private void ProcessEvents(object sender, DoWorkEventArgs e)
        {
            RemoveItems = new List<IEventListener>();
            while (!_BackgroundWorker.CancellationPending)
            {
                IAction action = Actions.Take();
                switch (action.GetActionType())
                {
                    case ActionType.OfferEvent:
                        OfferEventAction offerEventAction = (OfferEventAction)action;
                        ProcessOfferEventAction(offerEventAction);
                        break;
                    case ActionType.AddListener:
                        AddListenerAction addListenerAction = (AddListenerAction)action;
                        ProcessAddListenerAction(addListenerAction);
                        break;
                    case ActionType.RemoveListener:
                        RemoveListenerAction removeListenerAction = (RemoveListenerAction)action;
                        ProcessRemoveListenerAction(removeListenerAction);
                        break;
                }
            }
            e.Cancel = true;
        }

        private void ProcessOfferEventAction(OfferEventAction offerEventAction)
        {
            IEvent _event = offerEventAction.GetEvent();
            if (_event != null)
            {
                List<WeakReference<IEventListener>> activeListeners;
                if (EventTypesToListeners.TryGetValue(_event.GetEventType(), out activeListeners))
                {
                    List<WeakReference<IEventListener>> removeListeners = new List<WeakReference<IEventListener>>();

                    foreach (WeakReference<IEventListener> weakReference in activeListeners)
                    {
                        IEventListener eventListener;
                        if (weakReference.TryGetTarget(out eventListener))
                        {
                            eventListener.OnEvent(_event);
                        }
                        else
                        {
                            removeListeners.Add(weakReference);
                        }
                    }
 
                    foreach (WeakReference<IEventListener> remove in removeListeners)
                    {
                        activeListeners.Remove(remove);
                    }
                }

            }
        }

        private void ProcessAddListenerAction(AddListenerAction addListenerAction)
        {
            if (addListenerAction != null)
            {
                IEventListener eventListener = addListenerAction.GetEventListener();
                if (eventListener != null)
                {
                    List<WeakReference<IEventListener>> weakReferences;
                    foreach (EventType eventType in eventListener.GetNotifyTypes())
                    {
                        if (!EventTypesToListeners.TryGetValue(eventType, out weakReferences))
                        {
                            weakReferences = new List<WeakReference<IEventListener>>();
                        }
                        weakReferences.Add(new WeakReference<IEventListener>(eventListener));
                        EventTypesToListeners.Add(eventType, weakReferences);
                    }
                }
            }
        }

        private void ProcessRemoveListenerAction(RemoveListenerAction removeListenerAction)
        {
            if (removeListenerAction != null)
            {
                IEventListener eventListener = removeListenerAction.GetEventListener();
                if (eventListener != null)
                {
                    List<WeakReference<IEventListener>> weakReferences;
                    foreach (EventType eventType in eventListener.GetNotifyTypes())
                    {
                        if (EventTypesToListeners.TryGetValue(eventType, out weakReferences))
                        {
                            foreach (WeakReference<IEventListener> weakReference in weakReferences)
                            {
                                IEventListener _eventListener;
                                if (weakReference.TryGetTarget(out _eventListener))
                                {
                                    if (_eventListener.Equals(eventListener))
                                    {
                                        weakReferences.Remove(weakReference);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
