using UnityEngine;

public enum BodyPart
{
    None = 0,
    Head,
    Neck,
    Torso,
    RightArm,
    RightElbow,
    RightHand,
    LeftArm,
    LeftElbow,
    LeftHand,
    Hip,
    RightLeg,
    RightKnee,
    LeftLeg,
    LeftKnee
}


[System.Flags]
public enum BodyType
{
    None = 0,
    Normal = 1,
    Vital = 2,
}


public class BodyProperty : MonoBehaviour
{
    [SerializeField]
    private BodyPart _bodyPart;
    public BodyPart BodyPart
    {
        get { return _bodyPart; }
    }

    [SerializeField]
    private BodyType _bodyType = BodyType.Normal;
    public BodyType BodyType
    {
        get { return _bodyType; }
    }            
}
