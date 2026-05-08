using System.Collections.Generic;
using UnityEngine;

namespace AIWars.Multiplayer
{
    // Deterministic Lockstep skeleton from the plan. Players send commands only; visuals
    // (explosions, flashes, sound) play locally per-client and are NOT synced.
    public class DeterministicLockstep : MonoBehaviour
    {
        public const float TickIntervalSec = 0.1f;   // 10 ticks per second
        public const int   InputDelayTicks = 3;       // 300 ms input delay

        [System.Serializable]
        public struct Command
        {
            public int playerId;
            public int tickToApply;
            public int commandType; // 0=move,1=attack,2=produce,3=possess
            public Vector3 target;
            public int unitId;
        }

        readonly List<Command> _pending = new();
        int _currentTick;

        public void EnqueueLocalCommand(Command c)
        {
            c.tickToApply = _currentTick + InputDelayTicks;
            _pending.Add(c);
            // TODO: ServerRpc-broadcast to peers (NGO).
        }

        void FixedUpdate()
        {
            // Fixed deterministic step.
            if (Time.fixedDeltaTime <= 0f) return;
            _currentTick++;

            for (int i = _pending.Count - 1; i >= 0; i--)
            {
                if (_pending[i].tickToApply <= _currentTick)
                {
                    Apply(_pending[i]);
                    _pending.RemoveAt(i);
                }
            }
        }

        void Apply(Command c)
        {
            // Apply commands deterministically. Visual FX must NOT live here.
            // Hook into UnitLocomotion.MoveTo / UnitProductionFacility.Order / etc.
        }
    }
}
