using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerCoreECS
{
    [System.Serializable]
    public struct BodyCollisionSystemNeedProperty
    {
        public BodyColliderProperty m_bodyCollider;
    }

    public class BodyCollisionSystem : MonoBehaviour
    {
        public List<BodyCollisionSystemNeedProperty> m_entities;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake ()
        {
            Update ();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update ()
        {
            var unRegistBodyCols = from entity in m_entities
            where entity.m_bodyCollider.m_hasRigistEvent == false
            select entity.m_bodyCollider;

            foreach (var unRegistBodyCol in unRegistBodyCols)
            {
                unRegistBodyCol.m_hasRigistEvent = true;
                unRegistBodyCol.CollisionEnter += CollisionEnter;
                unRegistBodyCol.CollisionStay += CollisionStay;
                unRegistBodyCol.CollisionExit += CollisionExit;
                unRegistBodyCol.TriggerEnter += TriggerEnter;
                unRegistBodyCol.TirggerStay += TriggerStay;
                unRegistBodyCol.TriggerExit += TriggerExit;
            }
        }

        private void CollisionEnter (Collision collision, BodyColliderProperty bodyCollider)
        {
            if (collision.transform.root == bodyCollider.BaseTransform)
            {
                return;
            }

            CheckIsGround (collision, bodyCollider);
        }

        private void CollisionStay (Collision collision, BodyColliderProperty bodyCollider)
        {
            if (collision.transform.root == bodyCollider.BaseTransform)
            {
                return;
            }

            CheckIsGround (collision, bodyCollider);
        }

        private void CollisionExit (Collision collision, BodyColliderProperty bodyCollider)
        {
            if (collision.transform.root == bodyCollider.BaseTransform)
            {
                return;
            }

            bodyCollider.m_onGround = false;
        }

        private void TriggerEnter (Collider collider, BodyColliderProperty bodyCollider)
        {

        }

        private void TriggerStay (Collider collider, BodyColliderProperty bodyCollider)
        {

        }

        private void TriggerExit (Collider collider, BodyColliderProperty bodyCollider)
        {

        }

        private void CheckIsGround (Collision collision, BodyColliderProperty bodyCollider)
        {
            foreach (var contact in collision.contacts)
            {
                if (SurfaceWithinAngle (contact, Vector3.up, 50f))
                {
                    bodyCollider.m_onGround = true;
                    return;
                }
            }

            bodyCollider.m_onGround = false;
        }

        private bool SurfaceWithinAngle (ContactPoint contact, Vector3 direction, float angle)
        {
            if (Vector3.Angle (contact.normal, direction) < angle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}