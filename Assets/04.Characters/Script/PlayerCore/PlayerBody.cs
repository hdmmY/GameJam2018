using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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

        private Rigidbody _bodyRigid;
        public Rigidbody BodyRigid
        {
            get
            {
                if (_bodyRigid == null)
                {
                    _bodyRigid = _bodyTransform.GetComponent<Rigidbody> ();
                }
                return _bodyRigid;
            }
        }

        private PlayerBodyCollider _bodyCollider;
        public PlayerBodyCollider BodyCollider
        {
            get
            {
                if (_bodyCollider == null)
                {
                    _bodyCollider = _bodyTransform.GetComponent<PlayerBodyCollider> ();
                }
                return _bodyCollider;
            }
        }

        private PlayerBodyPicker _bodyPicker;
        public PlayerBodyPicker BodyPicker
        {
            get
            {
                if (_bodyPicker == null)
                {
                    _bodyPicker = _bodyTransform.GetComponent<PlayerBodyPicker> ();
                }
                return _bodyPicker;
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

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake ()
        {
            BodyInfo[] bodys = new BodyInfo[m_bodyInfo.Count];
            m_bodyInfo.Values.CopyTo (bodys, 0);

            for (int i = 0; i < bodys.Length; i++)
            {
                for (int j = i + 1; j < bodys.Length; j++)
                {
                    Physics.IgnoreCollision (
                        bodys[i].BodyTransform.GetComponent<Collider> (),
                        bodys[j].BodyTransform.GetComponent<Collider> ());
                }
            }
        }
    }
}