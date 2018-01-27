using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GrabHandler : MonoBehaviour
{
    [ReadOnly]
    public List<Grabber> m_grabbers;

    public bool m_isHoldSomething;

    public Transform m_itemPosition;

    public float m_liftForce;

    public float m_dropForce;

    private Rigidbody _rightHand;
    private Rigidbody _leftHand;

    private Rigidbody _torso;

    private void Start()
    {
        m_grabbers = new List<Grabber>(GetComponentsInChildren<Grabber>());

        _rightHand = GetComponentInChildren<RightHand>().GetComponent<Rigidbody>();
        _leftHand = GetComponentInChildren<LeftHand>().GetComponent<Rigidbody>();
        _torso = GetComponentInChildren<Torso>().GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (m_isHoldSomething)
        {
            Vector3 force;

            force = m_liftForce * (m_itemPosition.position - _rightHand.transform.position);
            _rightHand.AddForce(force, ForceMode.Force);
            _torso.AddForce(-force, ForceMode.Force);

            force = m_liftForce * (m_itemPosition.position - _leftHand.transform.position);
            _leftHand.AddForce(force, ForceMode.Force);
            _torso.AddForce(-force, ForceMode.Force);
        }
    }

    public void StartGrab(Rigidbody rigidbody)
    {
        if (rigidbody)
        {
            m_isHoldSomething = true;

            if (Vector3.Dot(_torso.velocity, m_itemPosition.localPosition) < 0)
            {
                m_itemPosition.localPosition = new Vector3(0, m_itemPosition.localPosition.y, -m_itemPosition.localPosition.z);
            }
        }
        else
        {
            m_isHoldSomething = false;
        }

        //Just for test
        m_isHoldSomething = true;
    }

    public void EndGrab()
    {
        foreach (var grabber in m_grabbers)
        {
            if (grabber.m_joint)
            {
                Debug.Log(grabber.gameObject.name);
                Destroy(grabber.m_joint);
            }
        }

        m_isHoldSomething = false;

        var bodyTrans = GetComponentInChildren<Torso>().transform;
        Vector3 bodyRight = bodyTrans.right;
        Vector3 bodyUp = bodyTrans.up;

        _rightHand.AddForce(m_dropForce * (bodyRight - bodyUp).normalized, ForceMode.Acceleration);
        _leftHand.AddForce(m_dropForce * (-bodyRight - bodyUp).normalized, ForceMode.Acceleration);
    }
}
