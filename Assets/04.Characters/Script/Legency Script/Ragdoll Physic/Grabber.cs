using UnityEngine;
using Sirenix.OdinInspector;


public class Grabber : MonoBehaviour
{
    public ItemProperty m_grabItem;

    public Player m_grabPlayer;

    private GrabHandler _grabHander;

    private void Start()
    {
        _grabHander = transform.root.GetComponent<GrabHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_grabHander == null)
        {
            _grabHander = transform.root.GetComponent<GrabHandler>();
        }

        if (_grabHander.m_isHoldSomething || other.GetComponent<Rigidbody>() == null)
        {
            return;
        }

        m_grabItem = other.transform.GetComponent<ItemProperty>();

        // In case repeat m_grabItem
        foreach (var grabber in _grabHander.m_grabbers)
        {
            if (grabber != this && grabber.m_grabItem == m_grabItem)
            {
                m_grabItem = null;
                break;
            }
        }

        m_grabPlayer = other.transform.root.GetComponent<Player>();

        // In case repeat m_grabPlayer
        foreach(var grabber in _grabHander.m_grabbers)
        {
            if(grabber != this && grabber.m_grabPlayer == m_grabPlayer)
            {
                m_grabPlayer = null;
                break;
            }
        }             
    }

    private void OnTriggerExit(Collider other)
    {
        if (_grabHander == null)
        {
            _grabHander = transform.root.GetComponent<GrabHandler>();
        }

        if (m_grabItem == other.transform.GetComponent<ItemProperty>())
        {
            m_grabItem = null;
        }

        if(m_grabPlayer == other.transform.root.GetComponent<Player>())
        {
            m_grabPlayer = null;
        }
    }
}
