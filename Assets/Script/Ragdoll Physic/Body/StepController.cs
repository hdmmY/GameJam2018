using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepController : MonoBehaviour
{
    public float m_legAngle = 70f;

    public float m_switchTime = 0.3f;

    private float _timer;

    private Transform _rightLeg;

    private Transform _leftLeg;

    private CharacterInformation _characterInfo;

    private GrabHandler _grabHandler;

    private void Start()
    {
        _rightLeg = GetComponentInChildren<RightLeg>().transform;
        _leftLeg = GetComponentInChildren<LeftLeg>().transform;

        _characterInfo = GetComponent<CharacterInformation>();
        _grabHandler = GetComponent<GrabHandler>();

        _timer = 0f;
    }


    private void Update()
    {
        _timer += Time.deltaTime;

        float switchTime = _grabHandler.m_isHoldSomething ? m_switchTime * 2 : m_switchTime;

        if (Vector3.Angle(_leftLeg.up, _rightLeg.up) > m_legAngle &&
           _timer > switchTime &&
           _characterInfo.m_sinceFallen > 0.1f &&
           _characterInfo.m_paceState != 0)
        {
            _characterInfo.m_leftSideForward = !_characterInfo.m_leftSideForward;
            _timer = 0f;
        }
    }
}
