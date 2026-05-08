using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AIWars.Data
{
    [CreateAssetMenu(menuName = "AIWars/Events/Game Event", fileName = "Event_")]
    public class GameEvent : ScriptableObject
    {
        readonly List<GameEventListener> _listeners = new();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener l) { if (!_listeners.Contains(l)) _listeners.Add(l); }
        public void UnregisterListener(GameEventListener l) => _listeners.Remove(l);
    }

    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] GameEvent evt;
        [SerializeField] UnityEvent response;

        void OnEnable()  { if (evt != null) evt.RegisterListener(this); }
        void OnDisable() { if (evt != null) evt.UnregisterListener(this); }
        public void OnEventRaised() => response?.Invoke();
    }
}
