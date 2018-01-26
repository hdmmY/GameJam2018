using UnityEngine;
using Sirenix.OdinInspector;

public class Animations : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    private JointAnimation[] _jointAnimations;

    private CharacterInformation _characterInfo;

    private void Start()
    {
        _jointAnimations = GetComponentsInChildren<JointAnimation>();
        _characterInfo = GetComponent<CharacterInformation>();
    }


    public void Run()
    {
        if(_characterInfo.m_sinceFallen >= 0f)
        {
            foreach(var jointAnim in _jointAnimations)
            {
                jointAnim.Animate(0, _characterInfo.m_leftSideForward);
            }
        }                                            
    }

}
