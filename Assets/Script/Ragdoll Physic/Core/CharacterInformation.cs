using UnityEngine;
using Sirenix.OdinInspector;


public class CharacterInformation : MonoBehaviour
{
    public Material m_myMaterial;

    // Torso Rigidbody
    public Rigidbody m_mainRig;

    [BoxGroup("Bools")]
    public bool m_awaysStand;

    [BoxGroup("Bools"), DisableInPlayMode]
    public bool m_isGround;

    [BoxGroup("Bools"), DisableInPlayMode]
    public bool m_isDead;

    [BoxGroup("Bools"), DisableInPlayMode]
    public bool m_isBeingGrabbed;

    [BoxGroup("Bools"), DisableInPlayMode]
    public bool m_leftSideForward;


    [BoxGroup("Times"), DisableInPlayMode]
    public float m_sinceGrounded;

    [BoxGroup("Times"), DisableInPlayMode]
    public float m_sinceWall = 1f;

    [BoxGroup("Times"), DisableInPlayMode]
    public float m_sinceJumped;

    [BoxGroup("Times"), DisableInPlayMode]
    public float m_sinceFallen = 2f;

    [BoxGroup("Times"), DisableInPlayMode]
    public float m_sinceGrabbed;

    [BoxGroup("Times"), DisableInPlayMode]
    public float m_sinceThrown = 1f;

    public int m_paceState;

    [ShowInInspector, ReadOnly]
    public float ShortestDistanceFromHeadToGround { get; private set; }

    [ShowInInspector, ReadOnly]
    public Vector3 WallNormal { get; set; }

    [ShowInInspector, ReadOnly]
    public Transform HeadTransform { get; private set; }

    [ShowInInspector, ReadOnly]
    public Vector3 LandPosition { get; private set; }

    [ShowInInspector, ReadOnly]
    private CheckForGroundCollision[] _groundCheckers;

    private void Start()
    {
        foreach (var rb in transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
        {
            if (!rb.GetComponent<CheckForGroundCollision>())
            {
                rb.gameObject.AddComponent<CheckForGroundCollision>();
            }
        }

        _groundCheckers = GetComponentsInChildren<CheckForGroundCollision>();
        HeadTransform = GetComponentInChildren<Head>().transform;
    }

    private void Update()
    {
        if (m_isDead)
        {
            m_sinceFallen = -10f;
        }

        m_sinceFallen += Time.deltaTime;
        m_sinceJumped += Time.deltaTime;
        m_sinceWall += Time.deltaTime;
        m_sinceGrabbed = m_isBeingGrabbed ? 0 : m_sinceGrabbed + Time.deltaTime;
        m_sinceThrown += Time.deltaTime;

        CheckIfGrounded();
        m_sinceGrounded = m_isGround ? 0 : m_sinceGrounded + Time.deltaTime;
    }

    private void CheckIfGrounded()
    {
        m_isGround = false;

        for (int i = 0; i < _groundCheckers.Length; i++)
        {
            if (_groundCheckers[i].IsGrounded)
            {
                float length = Vector3.Distance(HeadTransform.position, _groundCheckers[i].CollisionPosition);

                if (length <= ShortestDistanceFromHeadToGround || m_isGround)
                {
                    ShortestDistanceFromHeadToGround = length;
                    LandPosition = _groundCheckers[i].CollisionPosition;
                }
                if (m_sinceJumped > 0.2f)
                {
                    m_isGround = true;
                }
            }
        }

        if (m_awaysStand)
        {
            m_isGround = true;
        }
    }


}
