using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerCore
{
	public class PlayerGrabChecker : MonoBehaviour
	{
		public GameObject m_item;

		public float m_grabCooldown = 0.3f;

		public bool m_finishGrab = false;

		private PlayerBody _body;

		private PlayerInput _input;

		private float _coolDown;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		private void Start ()
		{
			_body = transform.root.GetComponent<PlayerBody> ();
			_input = transform.root.GetComponent<PlayerInput> ();
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		private void Update ()
		{
			_coolDown -= Time.deltaTime;

			if (_input.ThrowWasPressed)
			{
				Throw();
			}
		}

		/// <summary>
		/// OnTriggerEnter is called when the Collider other enters the trigger.P
		/// </summary>
		/// <param name="other">The other Collider involved in this collision.</param>
		private void OnTriggerEnter (Collider other)
		{
			if (other.transform.root != transform.root && _input.PickWasPressed)
			{
				if (other.GetComponentInChildren<ItemInfo> ())
				{
					TryToGrabSomething (other.GetComponentInChildren<ItemInfo> ());
				}
				else if (other.GetComponentInParent<ItemInfo> ())
				{
					TryToGrabSomething (other.GetComponentInParent<ItemInfo> ());
				}
			}
		}

		/// <summary>
		/// OnTriggerStay is called once per frame for every Collider other
		/// that is touching the trigger.
		/// </summary>
		/// <param name="other">The other Collider involved in this collision.</param>
		private void OnTriggerStay (Collider other)
		{
			OnTriggerEnter (other);
		}

		public void TryToGrabSomething (ItemInfo item)
		{
			if (_coolDown > 0 || m_item || item.m_beingGrabbed)
			{
				return;
			}

			_coolDown = m_grabCooldown;
			m_item = item.gameObject;
			m_finishGrab = false;

			var torso = _body[BodyPart.Torso];

			item.transform.position =
				torso.BodyTransform.position +
				torso.BodyTransform.rotation * (-item.m_grabPosOffset);
			item.transform.forward = item.m_grabRotation * torso.BodyTransform.forward;

			var rig = m_item.GetComponent<Rigidbody> ();
			rig.useGravity = false;

			var joint = m_item.AddComponent<FixedJoint> ();
			joint.connectedBody = torso.BodyRigid;

			StartCoroutine (AttractHands ());
		}

		private IEnumerator AttractHands ()
		{
			PlayerTargetItem[] targetter = new PlayerTargetItem[4];

			targetter[0] = _body[BodyPart.LeftArm].BodyTransform.gameObject.AddComponent<PlayerTargetItem> ();
			targetter[1] = _body[BodyPart.LeftHand].BodyTransform.gameObject.AddComponent<PlayerTargetItem> ();
			targetter[2] = _body[BodyPart.RightArm].BodyTransform.gameObject.AddComponent<PlayerTargetItem> ();
			targetter[3] = _body[BodyPart.RightHand].BodyTransform.gameObject.AddComponent<PlayerTargetItem> ();

			for (int i = 0; i < 4; i++)
			{
				targetter[i].m_targetItem = m_item.GetComponent<ItemInfo> ();
			}

			bool leftAttach = false;
			bool rightAttach = false;

			while (!leftAttach || !rightAttach)
			{
				for (int i = 0; i < 2; i++)
				{
					if (targetter[i].m_attached) leftAttach = true;
				}

				for (int i = 2; i < 4; i++)
				{
					if (targetter[i].m_attached) rightAttach = true;
				}

				yield return new WaitForFixedUpdate ();
			}

			for (int i = 0; i < 4; i++)
			{
				Destroy (targetter[i]);
			}

			m_finishGrab = true;
		}

		private void Throw ()
		{
			if (m_item == null || !m_finishGrab)
			{
				return;
			}

			var joint = _body[BodyPart.LeftArm].BodyTransform.GetComponent<FixedJoint> ();
			if (joint != null) Destroy (joint);

			joint = _body[BodyPart.LeftHand].BodyTransform.GetComponent<FixedJoint> ();
			if (joint != null) Destroy (joint);

			joint = _body[BodyPart.RightArm].BodyTransform.GetComponent<FixedJoint> ();
			if (joint != null) Destroy (joint);

			joint = _body[BodyPart.RightHand].BodyTransform.GetComponent<FixedJoint> ();
			if (joint != null) Destroy (joint);

			Destroy (m_item.GetComponent<FixedJoint> ());

			m_item = null;
		}
	}
}