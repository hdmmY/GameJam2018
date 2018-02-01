using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySetting : MonoBehaviour
{
    public float m_drag;

    public float m_angularDrag;

    public float m_maxAngularVelocity;

    public float m_weightMultiplier = 1f;

    public List<DragHandler> DragHandlers { get; private set; }

    private CharacterInformation _characterInfo;

    private bool _fallen;

    private void Start()
    {
        _characterInfo = GetComponent<CharacterInformation>();

        foreach(var rb in GetComponentsInChildren<Rigidbody>())
        {
            if(!rb.transform.parent.CompareTag("IgnoreRigidbody"))
            {
                rb.mass *= m_weightMultiplier;
                rb.maxAngularVelocity = m_maxAngularVelocity;

                if(rb.drag == 0f && !rb.CompareTag("IgnoreDrag"))
                {
                    rb.drag = m_drag;

                    DragHandler dragHander = rb.gameObject.AddComponent<DragHandler>();
                    dragHander.m_startDrag = rb.drag;
                    dragHander.m_startAngularDrag = rb.angularDrag;
                }                                      
            }
        }

        DragHandlers = new List<DragHandler>(GetComponentsInChildren<DragHandler>());
    }


    private void Update()
    {
        if(_characterInfo.m_sinceFallen <= 0.01f && !_fallen)
        {
            Fall();    
        }
        else if(_characterInfo.m_sinceFallen > 0f && _fallen)
        {
            GetUp();
        }
    }


    private void Fall()
    {
        _fallen = true;

        foreach(var dragHandler in DragHandlers)
        {
            dragHandler.SetFallenDrag();
        }
    }


    private void GetUp()
    {
        _fallen = false;

        foreach(var dragHandler in DragHandlers)
        {
            dragHandler.SetStandingDrag();
        }
    }

}
