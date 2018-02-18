using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCore
{
	public class PlayerBodyCollider : MonoBehaviour
	{
		[ReadOnly] public bool m_onGround;

		/// <summary>
		/// OnCollisionEnter is called when this collider/rigidbody has begun
		/// touching another rigidbody/collider.
		/// </summary>
		/// <param name="other">The Collision data associated with this collision.</param>
		private void OnCollisionEnter (Collision other)
		{
			if (other.transform.root == transform.root)
			{
				return;
			}

			CheckIsGround(other);
		}

		/// <summary>
		/// OnCollisionStay is called once per frame for every collider/rigidbody
		/// that is touching rigidbody/collider.
		/// </summary>
		/// <param name="other">The Collision data associated with this collision.</param>
		private void OnCollisionStay(Collision other)
		{
			if(other.transform.root == transform.root)
			{
				return;
			}

			CheckIsGround(other);
		}

		/// <summary>
		/// OnCollisionExit is called when this collider/rigidbody has
		/// stopped touching another rigidbody/collider.
		/// </summary>
		/// <param name="other">The Collision data associated with this collision.</param>
		private void OnCollisionExit(Collision other)
		{
			if(other.transform.root == transform.root)
			{
				return;
			}

			m_onGround = false;
		}

		private void CheckIsGround (Collision collision)
		{
			foreach (var contact in collision.contacts)
			{
				if (SurfaceWithinAngle (contact, Vector3.up, 50f))
				{
					m_onGround = true;
					return;
				}
			}

			m_onGround = false;
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