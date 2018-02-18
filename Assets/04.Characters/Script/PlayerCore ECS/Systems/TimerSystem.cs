using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerCoreECS
{
    public class TimerSystem : MonoBehaviour
    {
        private TimerProperty _timerProperty;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake ()
        {
            _timerProperty = TimerProperty.Instance;
            _timerProperty.m_timers = new Dictionary<int, Timer> (_timerProperty.m_initTimer);
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void FixedUpdate ()
        {
            var updateTimers = (from timer in _timerProperty.m_timers where timer.Value.UseFixedUpdate == true select timer).ToArray ();

            foreach (var updateTimer in updateTimers)
            {
                updateTimer.Value.AddTime (Time.fixedDeltaTime);
                updateTimer.Value.Update (updateTimer.Value.CurrentTime, updateTimer.Value.EndTime);
            }

            var excludeTimers = from timer in updateTimers
            where timer.Value.CurrentTime >= timer.Value.EndTime
            select timer.Key;

            foreach (var excludeTimer in excludeTimers)
            {
                _timerProperty.m_timers[excludeTimer].Dead ();
                _timerProperty.m_timers.Remove (excludeTimer);
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update ()
        {
            var updateTimers = (from timer in _timerProperty.m_timers where timer.Value.UseFixedUpdate == false select timer).ToArray ();

            foreach (var updateTimer in updateTimers)
            {
                updateTimer.Value.AddTime (Time.deltaTime);
                updateTimer.Value.Update (updateTimer.Value.CurrentTime, updateTimer.Value.EndTime);
            }

            var excludeTimers = from timer in updateTimers
            where timer.Value.CurrentTime >= timer.Value.EndTime
            select timer.Key;

            foreach (var excludeTimer in excludeTimers)
            {
                _timerProperty.m_timers[excludeTimer].Dead ();
                _timerProperty.m_timers.Remove (excludeTimer);
            }
        }
    }
}