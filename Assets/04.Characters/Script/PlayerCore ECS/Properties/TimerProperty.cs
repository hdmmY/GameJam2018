using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCoreECS
{
    public class TimerProperty : BaseProperty
    {
        #region  Singleton

        private static TimerProperty _instance;

        public static TimerProperty Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TimerProperty> ();
                    if (FindObjectsOfType<TimerProperty> ().Length > 1)
                    {
                        return _instance;
                    }
                    if (_instance == null)
                    {
                        _instance = new GameObject ("TimerProperty").AddComponent<TimerProperty> ();
                        DontDestroyOnLoad (_instance.gameObject);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake ()
        {
            foreach (var timerProperty in FindObjectsOfType<TimerProperty> ())
            {
                if (timerProperty != _instance)
                {
                    Destroy (timerProperty.gameObject);
                }
            }
        }

        #endregion

        public int m_initTimer = 50;

        [ShowInInspector]
        public int CurrentTimer
        {
            get
            {
                return m_timers.Count;
            }
        }

        public Dictionary<int, Timer> m_timers;
    }

    public class Timer
    {
        public bool UseFixedUpdate
        {
            get;
            private set;
        }

        public float EndTime
        {
            get;
            private set;
        }

        public float Speed
        {
            get;
            private set;
        }

        public float CurrentTime
        {
            get;
            set;
        }

        public Timer (float endTime, float speed, bool useFixedUpdate = true)
        {
            EndTime = endTime;
            Speed = speed;
            CurrentTime = 0f;
            UseFixedUpdate = useFixedUpdate;
        }

        /// <summary>
        /// Update(float curTimr, float endTime)
        /// </summary>
        public Action<float, float> Update;

        /// <summary>
        /// Dead(float endTime)
        /// </summary>
        public Action Dead;
    }
}