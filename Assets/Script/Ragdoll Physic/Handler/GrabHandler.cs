using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GrabHandler : MonoBehaviour
{
    [BoxGroup("Bools")]
    public bool m_isGrabbing;

    [BoxGroup("Bools")]
    public bool m_isHoldSomething;

    [BoxGroup("Bools")]
    public bool m_isHoldSomethingAnchored;

    [BoxGroup("Bools")]
    public bool m_isCarrySomething;

    [BoxGroup("Grabbers"), ReadOnly]
    public List<Grabber> m_grabbers;

    [BoxGroup("Grabbers")]
    public float m_grabStrength;




    public void StartGrab(Rigidbody rigidbody)
    {

    }

    public void EndGrab()
    {

    }

}
