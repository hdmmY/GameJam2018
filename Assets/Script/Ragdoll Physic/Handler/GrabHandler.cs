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

    [SerializeField, ReadOnly]
    private List<ItemProperty> _items = new List<ItemProperty>();

    [SerializeField, ReadOnly]
    private Player _grabPlayer;

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

    public void StartGrab()
    {
        if (_grabPlayer != null) return;

        if (Vector3.Dot(_torso.velocity, m_itemPosition.localPosition) < 0)
        {
            m_itemPosition.localPosition = new Vector3(0, m_itemPosition.localPosition.y, -m_itemPosition.localPosition.z);
        }

        foreach (var grabber in m_grabbers)
        {
            var grabPlayer = grabber.m_grabPlayer;

            if (grabPlayer != null)
            {
                if (grabPlayer.GetComponent<GrabHandler>().m_isHoldSomething)
                {
                    grabPlayer.GetComponent<GrabHandler>().EndGrab();
                    m_isHoldSomething = false;
                }

                grabPlayer.transform.SetParent(m_itemPosition);
                grabPlayer.GetComponent<Controller>().enabled = false;

                Vector3 offset = m_itemPosition.position - grabPlayer.GetComponentInChildren<LeftLeg>().transform.position;
                foreach(var rig in grabPlayer.GetComponentsInChildren<Rigidbody>())
                {
                    rig.isKinematic = true;
                    rig.position += offset;
                    rig.detectCollisions = false;

                    var follow = rig.gameObject.AddComponent<SimpleFollow>();
                    follow.m_target = m_itemPosition;
                    follow.m_offset = rig.position - m_itemPosition.position;
                }          

                _grabPlayer = grabPlayer;
            }                     

            var item = grabber.m_grabItem;

            if (item != null)
            {        
                item.m_rigidBody.useGravity = false;
                item.m_rigidBody.freezeRotation = true;

                item.transform.SetParent(m_itemPosition);
                item.transform.localPosition = item.m_grabOffset;
                item.transform.localRotation = item.m_presetRotation;

                item.gameObject.layer = _torso.gameObject.layer;

                item.m_followTarget = m_itemPosition;

                _items.Add(item);
            }
        }

        m_isHoldSomething = _items.Count > 0 || _grabPlayer;
    }

    public void EndGrab()
    {
        Vector3 forceDir = m_itemPosition.position - _torso.position;
        forceDir = new Vector3(forceDir.x, 0, forceDir.z);
        forceDir = forceDir.normalized + Vector3.up * 0.1f;

        float force;
                        
        // Push Player
        if (_grabPlayer != null)
        {
            force = 5f;
            force *= _torso.velocity.magnitude < 1 ? 1 : _torso.velocity.magnitude;

            _grabPlayer.transform.SetParent(null);
            foreach (var rig in _grabPlayer.GetComponentsInChildren<Rigidbody>())
            {
                rig.isKinematic = false;
                rig.detectCollisions = true;

                rig.AddForce(forceDir * force, ForceMode.VelocityChange);
                rig.GetComponent<SimpleFollow>().enabled = false;
                Destroy(rig.GetComponent<SimpleFollow>());
            }
            _grabPlayer.GetComponent<Sleep>().SleepForWhile(5f);
            _grabPlayer.GetComponent<Controller>().enabled = true;
            _grabPlayer = null;
        }                      

        // Push items
        foreach (var item in _items)
        {
            force = item.m_pushForce;
            force *= _torso.velocity.magnitude < 1 ? 1 : _torso.velocity.magnitude;

            item.transform.SetParent(null);
            item.m_rigidBody.AddForce(forceDir * force, ForceMode.VelocityChange);
            item.m_rigidBody.useGravity = true;
            item.m_rigidBody.freezeRotation = false;
            item.gameObject.layer = LayerMask.NameToLayer("Default");
            item.m_followTarget = null;
        }

        m_isHoldSomething = false;


        // Hands motion
        var bodyTrans = GetComponentInChildren<Torso>().transform;
        Vector3 bodyRight = bodyTrans.right;
        Vector3 bodyUp = bodyTrans.up;

        _rightHand.AddForce(m_dropForce * (bodyRight - bodyUp).normalized, ForceMode.Acceleration);
        _leftHand.AddForce(m_dropForce * (-bodyRight - bodyUp).normalized, ForceMode.Acceleration);

        _items.Clear();
    }
}
