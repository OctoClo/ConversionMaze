﻿using System.Collections;
using System.Collections.Generic;

// Source : http://www.willrmiller.com/?p=87

namespace ConversionFPS
{
    class GameEvent
    {

    }

    class EventManager
    {
        static EventManager eventManager = null;
        public static EventManager Instance
        {
            get
            {
                if (eventManager == null)
                    eventManager = new EventManager();
                return eventManager;
            }
        }

        public delegate void EventDelegate<T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);

        private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
        private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

        public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            // Early-out if we've already registered this delegate
            if (delegateLookup.ContainsKey(del))
                return;

            // Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke.
            void internalDelegate(GameEvent e) => del((T)e);
            delegateLookup[del] = internalDelegate;

            if (delegates.TryGetValue(typeof(T), out EventDelegate tempDel))
                delegates[typeof(T)] = tempDel += internalDelegate;
            else
                delegates[typeof(T)] = internalDelegate;
        }

        public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            if (delegateLookup.TryGetValue(del, out EventDelegate internalDelegate))
            {
                if (delegates.TryGetValue(typeof(T), out EventDelegate tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                        delegates.Remove(typeof(T));
                    else
                        delegates[typeof(T)] = tempDel;
                }

                delegateLookup.Remove(del);
            }
        }

        public void Raise(GameEvent e)
        {
            if (delegates.TryGetValue(e.GetType(), out EventDelegate del))
                del.Invoke(e);
        }
    }
}