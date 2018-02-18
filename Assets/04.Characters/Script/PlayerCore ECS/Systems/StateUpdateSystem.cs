using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCoreECS
{
    public class StateUpdateSystem : MonoBehaviour
    {
        public List<StateProperty> m_stateProperties;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start ()
        {
            StartCoroutine (UpdateProperty ());
        }

        private IEnumerator UpdateProperty ()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate ();
                foreach (var state in m_stateProperties)
                {
                    state.m_lastState = state.m_state;
                }
            }
        }
    }
}