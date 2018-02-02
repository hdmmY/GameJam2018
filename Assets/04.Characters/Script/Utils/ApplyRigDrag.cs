using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;


[System.Serializable]
public class RigDragApplier
{
    public float m_drag;

    public float m_angleDrag;

    public Rigidbody m_rig;
}


public class ApplyRigDrag : MonoBehaviour
{
    [SerializeField, Multiline(5)]
    private string _description;

    public List<RigDragApplier> m_dragAppliers;

    public float m_dragMutiplier = 1;

    public float m_angularDragMutiplier = 1;

    [ReadOnly]
    public bool m_enabled = false;

    private void FixedUpdate()
    {
        if (!m_enabled) return;

        foreach(var dragApplier in m_dragAppliers)
        {
            if(dragApplier.m_rig != null)
            {
                dragApplier.m_rig.drag = dragApplier.m_drag * m_dragMutiplier;
                dragApplier.m_rig.angularDrag = dragApplier.m_angleDrag * m_angularDragMutiplier;
            }
        }
    }
}