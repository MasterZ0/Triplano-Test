﻿using System;
using UnityEngine;

namespace TriplanoTest.Shared
{
    /// <summary>
    /// Event subscriber and sender
    /// </summary>
    [CreateAssetMenu(menuName = MenuPath.ScriptableObjects + "Events/Game Event", fileName = "NewGameEvent")]
    public class GameEvent : ScriptableObject
    {
        /// <summary> Used to remind developers what this does </summary>
        [SerializeField, TextArea(0, 40)] private string description;

        private Action listeners = delegate { };

        public void Invoke()
        {
            listeners();
        }

        public static GameEvent operator +(GameEvent gameEvent, Action action)
        {
            gameEvent.listeners += action;
            return gameEvent;
        }

        public static GameEvent operator -(GameEvent gameEvent, Action action)
        {
            gameEvent.listeners -= action;
            return gameEvent;
        }

        public static implicit operator Action(GameEvent gameEvent) => gameEvent.Invoke;
    }

    public abstract class GameEvent<T> : ScriptableObject
    {
        /// <summary> Used to remind developers what this does </summary>
        [SerializeField, TextArea(0, 40)] private string description;

        private Action<T> listeners = delegate { };

        public void Invoke(T value)
        {
            listeners(value);
        }

        public static GameEvent<T> operator +(GameEvent<T> gameEvent, Action<T> action)
        {
            gameEvent.listeners += action;
            return gameEvent;
        }

        public static GameEvent<T> operator -(GameEvent<T> gameEvent, Action<T> action)
        {
            gameEvent.listeners -= action;
            return gameEvent;
        }

        public static implicit operator Action<T>(GameEvent<T> gameEvent) => gameEvent.Invoke;
    }
}