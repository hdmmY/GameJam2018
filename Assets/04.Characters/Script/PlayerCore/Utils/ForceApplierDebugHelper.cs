using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class ForceApplierDebugHelper : MonoBehaviour
{
    [System.Serializable]
    public class ForceApplierState
    {
        public ApplyForce m_forceApplier;

        [ReadOnly]
        public bool m_isActive;
    }

    public List<ForceApplierState> m_forceAppliersState;

    private void Update()
    {
        ForceApplierState state;

        for(int i = 0; i < m_forceAppliersState.Count; i++)
        {
            state = m_forceAppliersState[i];

            if(state.m_forceApplier != null)
            {
                state.m_isActive = state.m_forceApplier.m_enabled;
            }
        }
    }
}
