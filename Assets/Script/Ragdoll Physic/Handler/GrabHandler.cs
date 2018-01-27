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
        _items.Clear();

        foreach (var grabber in m_grabbers)
        {
            var item = grabber.m_grabItem;

            if (item != null)
            {
                if (Vector3.Dot(_torso.velocity, m_itemPosition.localPosition) < 0)
                {
                    m_itemPosition.localPosition = new Vector3(0, m_itemPosition.localPosition.y, -m_itemPosition.localPosition.z);
                }

                item.m_rigidBody.useGravity = false;
                item.m_rigidBody.detectCollisions = false;
                item.m_rigidBody.freezeRotation = true;

                Vector3 itemWorldScale = item.transform.lossyScale;
                item.transform.SetParent(m_itemPosition);
                item.transform.localPosition = item.m_grabOffset;
                item.transform.localRotation = item.m_presetRotation;
                item.transform.localScale = new Vector3(itemWorldScale.x / m_itemPosition.lossyScale.x,
                                    itemWorldScale.y / m_itemPosition.lossyScale.y,
                                    itemWorldScale.z / m_itemPosition.lossyScale.z);

                item.m_followTarget = m_itemPosition;

                _items.Add(item);
            }
        }

        m_isHoldSomething = _items.Count > 0;
    }

    public void EndGrab()
    {
        // Push items
        foreach (var item in _items)
        {
            Vector3 itemWorldScale = item.transform.lossyScale;
            Vector3 forceDir = m_itemPosition.position - _torso.position;
            forceDir = new Vector3(forceDir.x, 0, forceDir.z);
            forceDir = forceDir.normalized + Vector3.up * 0.1f;

            float force = item.m_pushForce;
            force *= _torso.velocity.magnitude < 1 ? 1 : _torso.velocity.magnitude;
            Debug.Log(force * forceDir);

            item.transform.SetParent(null);
            item.transform.localScale = itemWorldScale;
            item.m_rigidBody.AddForce(forceDir * force, ForceMode.VelocityChange);
            item.m_rigidBody.useGravity = true;
            item.m_rigidBody.detectCollisions = true;
            item.m_rigidBody.freezeRotation = false;
            item.m_followTarget = null;
        }

        m_isHoldSomething = false;


        // Hands motion
        var bodyTrans = GetComponentInChildren<Torso>().transform;
        Vector3 bodyRight = bodyTrans.right;
        Vector3 bodyUp = bodyTrans.up;

        _rightHand.AddForce(m_dropForce * (bodyRight - bodyUp).normalized, ForceMode.Acceleration);
        _leftHand.AddForce(m_dropForce * (-bodyRight - bodyUp).normalized, ForceMode.Acceleration);
    }
}
