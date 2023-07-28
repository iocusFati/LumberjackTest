using System;
using System.Collections.Generic;
using Gameplay.Player.Lumberjack;
using UnityEngine;

namespace Infrastructure
{
    public class Ticker : MonoBehaviour, ITicker
    {
        private readonly List<ITickable> _tickables = new();
        private readonly List<ILateTickable> _lateTickables = new();

        private void Update()
        {
            foreach (var tickable in _tickables) 
                tickable.Tick();
        }

        private void LateUpdate()
        {
            foreach (var lateTickable in _lateTickables)
            {
                lateTickable.LateTick();
            }
        }

        public void AddTickable(ITickable tickable)
        {
            _tickables.Add(tickable);
        }

        public void AddLateTickable(ILateTickable tickable)
        {
            _lateTickables.Add(tickable);
        }
    }
}