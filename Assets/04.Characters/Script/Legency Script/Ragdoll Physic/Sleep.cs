using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{                                 
    private Rigidbody[] _rigs;

    private CharacterInformation _characterInfo;

    private void Start()
    {
        _rigs = GetComponentsInChildren<Rigidbody>();
        _characterInfo = GetComponent<CharacterInformation>();
    }

    public void SleepForWhile(float seconds)
    {
        StartCoroutine(StartSleep(seconds));
    }
        
    private IEnumerator StartSleep(float seconds)
    {
        _characterInfo.m_sleep = true;
        
        var standing = GetComponent<Standing>();
        var controller = GetComponent<Controller>();
        var balance = GetComponent<Balance>();

        float time = Time.time;

        standing.enabled = false;
        controller.enabled = false;
        balance.enabled = false;
        

        while (Time.time < time + seconds)
        {
            foreach(var rig in _rigs)
            {
                rig.AddForce(Vector3.down * 1000, ForceMode.Force);
            }
            yield return null;
        }

        standing.enabled = true;
        controller.enabled = true;
        balance.enabled = true;

        _characterInfo.m_sleep = false;
    }

}
