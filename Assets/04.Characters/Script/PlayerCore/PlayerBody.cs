using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PlayerCore
{
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
        LeftKnee,
        Anchor
    }

    [System.Flags]
    public enum BodyType
    {
        None = 0,
        Normal = 1,
        Vital = 2, // Take more damage
        MainBody = 4
    }

    [System.Serializable]
    public class BodyInfo
    {
        [SerializeField]
        private Transform _bodyTransform;

        public Transform BodyTransform
        {
            get { return _bodyTransform; }
        }

        public Rigidbody BodyRigid
        {
            get
            {
                return _bodyTransform.GetComponent<Rigidbody> ();
            }
        }

        public PlayerBodyCollider BodyCollider
        {
            get
            {
                return _bodyTransform.GetComponent<PlayerBodyCollider> ();
            }
        }

        [SerializeField]
        private BodyType _bodyType = BodyType.Normal;
        public BodyType BodyType
        {
            get { return _bodyType; }
        }
    }

    public class PlayerBody : SerializedMonoBehaviour
    {
        [ShowInInspector]
        public Dictionary<BodyPart, BodyInfo> m_bodyInfo;

        public BodyInfo this [BodyPart bodyPart]
        {
            get
            {
                if (m_bodyInfo.ContainsKey (bodyPart))
                {
                    return m_bodyInfo[bodyPart];
                }
                return null;
            }
        }
    }
}