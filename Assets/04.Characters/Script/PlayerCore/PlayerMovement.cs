using System.Collections.Generic;
using UnityEngine;

namespace PlayerCore
{
	public enum Pose
	{
		Bent,
		Forward,
		Straight,
		Behind
	}

	public enum Side
	{
		Left,
		Right
	}

	public class PlayerMovement : MonoBehaviour
	{
		public float m_cycleSpeed;

		public float m_cycleTimer;

		public Pose m_leftLegPose = Pose.Straight;

		public Pose m_rightLegPose = Pose.Straight;

		public Pose m_leftArmPose = Pose.Straight;

		public Pose m_rightArmPose = Pose.Straight;

		public static Vector3 RunVecForce10 = new Vector3 (0, 10, 0);

		public static Vector3 RunVecForce5 = new Vector3 (0, 5, 0);

		public static Vector3 RunVecForce2 = new Vector3 (0, 2, 0);

		private PlayerState _state;

		private PlayerBody _body;

		private PlayerControl _control;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		private void Start ()
		{
			_state = GetComponent<PlayerState> ();
			_body = GetComponent<PlayerBody> ();
			_control = GetComponent<PlayerControl> ();
		}

		/// <summary>
		/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
		/// </summary>
		private void FixedUpdate ()
		{
			if (_state.HasState (State.Run)) Run ();
			if (_state.HasState (State.Jump)) Jump ();
			if (_state.HasState (State.Fall)) Fall ();
			if (_state.HasState (State.Stand)) Stand ();

			_state.m_lastState = _state.m_state;
		}

		private void Run ()
		{
			m_cycleSpeed = 0.18f;

			if (_state.m_state != _state.m_lastState)
			{
				if (Random.Range (0, 1) == 1)
				{
					m_leftLegPose = Pose.Bent;
					m_rightLegPose = Pose.Straight;
					m_leftArmPose = Pose.Straight;
					m_rightArmPose = Pose.Bent;
				}
				else
				{
					m_leftLegPose = Pose.Straight;
					m_rightLegPose = Pose.Bent;
					m_leftArmPose = Pose.Bent;
					m_rightArmPose = Pose.Straight;
				}
			}

			RunCycleUpdate ();
			RunCycleRotateAnchor ();
			RunCycleMainBody ();

			RunCyclePoseLeg (Side.Left, m_leftLegPose);
			RunCyclePoseLeg (Side.Right, m_rightLegPose);
		}

		private void RunCycleUpdate ()
		{
			if (m_cycleTimer < m_cycleSpeed)
			{
				m_cycleTimer += Time.fixedDeltaTime;
			}
			else
			{
				m_cycleTimer = 0f;

				int pose = (int) m_leftLegPose;
				m_leftLegPose = (Pose) (++pose <= 3 ? pose : 0);

				pose = (int) m_rightLegPose;
				m_rightLegPose = (Pose) (++pose <= 3 ? pose : 0);

				pose = (int) m_leftArmPose;
				m_leftArmPose = (Pose) (++pose <= 3 ? pose : 0);

				pose = (int) m_rightArmPose;
				m_rightArmPose = (Pose) (++pose <= 3 ? pose : 0);
			}
		}

		private void RunCycleRotateAnchor ()
		{
			_body[BodyPart.Anchor].BodyRigid.angularVelocity = Vector3.zero;

			Vector3 dir = new Vector3 (_control.m_direction.z, 0, -_control.m_direction.x);

			if (_body[BodyPart.Anchor].BodyRigid.velocity.magnitude < 3 * _control.m_applyForce)
			{
				_body[BodyPart.Anchor].BodyRigid.maxAngularVelocity = 20;
				_body[BodyPart.Anchor].BodyRigid.angularVelocity = dir * 20f;
			}
		}

		private void RunCycleMainBody ()
		{
			_body[BodyPart.Torso].BodyRigid.AddForce (
				(RunVecForce10 + 0.6f * _control.m_direction) * _control.m_applyForce,
				ForceMode.VelocityChange);
			_body[BodyPart.Hip].BodyRigid.AddForce (
				(-RunVecForce5 + 0.5f * _control.m_direction) * _control.m_applyForce,
				ForceMode.VelocityChange);
			_body[BodyPart.Anchor].BodyRigid.AddForce (
				(-RunVecForce5 + 0.6f * _control.m_direction) * _control.m_applyForce,
				ForceMode.VelocityChange);

			ApplyForceUtils.AlignToVector (_body[BodyPart.Head], _body[BodyPart.Head].BodyTransform.forward,
				_control.m_lookDirection, 0.1f, 5f * _control.m_applyForce);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Head], _body[BodyPart.Head].BodyTransform.up,
				Vector3.up, 0.1f, 5f * _control.m_applyForce);

			ApplyForceUtils.AlignToVector (_body[BodyPart.Torso], _body[BodyPart.Torso].BodyTransform.forward,
				_control.m_direction + Vector3.down, 0.1f, 10f * _control.m_applyForce);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Torso], _body[BodyPart.Torso].BodyTransform.up,
				Vector3.up, 0.1f, 10f * _control.m_applyForce);

			ApplyForceUtils.AlignToVector (_body[BodyPart.Hip], _body[BodyPart.Hip].BodyTransform.forward,
				_control.m_direction, 0.1f, 10f * _control.m_applyForce);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Hip], _body[BodyPart.Hip].BodyTransform.up,
				Vector3.up, 0.1f, 10f * _control.m_applyForce);
		}

		private void RunCyclePoseLeg (Side side, Pose pose)
		{
			Transform hip = _body[BodyPart.Hip].BodyTransform;

			Transform sideLeg = null;
			Transform unsideKnee = null;

			Rigidbody sideLegRig = null;
			Rigidbody sideKneeRig = null;
			Rigidbody unsideLegRig = null;

			switch (side)
			{
				case Side.Left:
					sideLeg = _body[BodyPart.LeftLeg].BodyTransform;
					unsideKnee = _body[BodyPart.RightKnee].BodyTransform;
					sideLegRig = _body[BodyPart.LeftLeg].BodyRigid;
					unsideLegRig = _body[BodyPart.RightLeg].BodyRigid;
					sideKneeRig = _body[BodyPart.LeftKnee].BodyRigid;
					break;
				case Side.Right:
					sideLeg = _body[BodyPart.RightLeg].BodyTransform;
					unsideKnee = _body[BodyPart.LeftKnee].BodyTransform;
					sideLegRig = _body[BodyPart.RightLeg].BodyRigid;
					unsideLegRig = _body[BodyPart.LeftLeg].BodyRigid;
					sideKneeRig = _body[BodyPart.RightKnee].BodyRigid;
					break;
			}

			switch (pose)
			{
				case Pose.Bent:
					ApplyForceUtils.AlignToVector (sideLegRig, -sideLeg.up, _control.m_direction, 0.1f, 4f * _control.m_applyForce);
					break;
				case Pose.Forward:
					ApplyForceUtils.AlignToVector (sideLegRig, -sideLeg.up, _control.m_direction - hip.up / 2, 0.1f, 4f * _control.m_applyForce);
					sideLegRig.AddForce (-_control.m_direction * 2f, ForceMode.VelocityChange);
					sideKneeRig.AddForce (_control.m_direction * 2f, ForceMode.VelocityChange);
					break;
				case Pose.Straight:
					ApplyForceUtils.AlignToVector (sideLegRig, sideLeg.up, Vector3.up, 0.1f, 4f * _control.m_applyForce);
					sideLegRig.AddForce (hip.up * 2f * _control.m_applyForce);
					sideKneeRig.AddForce (-hip.up * 2f * _control.m_applyForce);
					break;
				case Pose.Behind:
					ApplyForceUtils.AlignToVector (sideLegRig, sideLeg.up, _control.m_direction * 2f, 0.1f, 4f * _control.m_applyForce);
					break;
			}
		}
		private void Jump ()
		{
			if (_state.m_state != _state.m_lastState)
			{
				if (_control.m_jump)
				{
					Vector3 hipVelocity = _body[BodyPart.Hip].BodyRigid.velocity;
					if (hipVelocity.y < 2f)
					{
						_body[BodyPart.Torso].BodyRigid.AddForce (_control.m_direction * 0.5f, ForceMode.VelocityChange);
						_body[BodyPart.Hip].BodyRigid.AddForce (_control.m_direction * 0.5f, ForceMode.VelocityChange);

						float torsoY = _body[BodyPart.Torso].BodyTransform.position.y;
						float hipY = _body[BodyPart.Hip].BodyTransform.position.y;

						if (torsoY > hipY)
						{
							Vector3 jumpForce = Vector3.up * 20f;
							_body[BodyPart.Torso].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
							_body[BodyPart.Hip].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
						}
						else
						{
							Vector3 jumpForce = Vector3.up * 10f;
							_body[BodyPart.Torso].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
							_body[BodyPart.Hip].BodyRigid.AddForce (jumpForce, ForceMode.VelocityChange);
						}
					}
				}
				_control.m_jump = false;
			}

			if (InputUtils.ValidMove (new Vector2 (_control.m_rawDirection.x, _control.m_rawDirection.z)))
			{
				_body[BodyPart.Torso].BodyRigid.AddForce (Vector3.up * 2f, ForceMode.VelocityChange);
				_body[BodyPart.Hip].BodyRigid.AddForce (Vector3.up * 2f, ForceMode.VelocityChange);

				ApplyForceUtils.AlignToVector (
					_body[BodyPart.Torso], _body[BodyPart.Torso].BodyTransform.forward,
					_control.m_direction + Vector3.down, 0.1f, 10f);
				ApplyForceUtils.AlignToVector (
					_body[BodyPart.Hip], _body[BodyPart.Hip].BodyTransform.forward,
					_control.m_direction + Vector3.up, 0.1f, 10f);

				ApplyForceUtils.AlignToVector (
					_body[BodyPart.LeftArm], _body[BodyPart.LeftArm].BodyTransform.up,
					_body[BodyPart.Torso].BodyTransform.right + _body[BodyPart.Torso].BodyTransform.forward,
					0.1f, 4f);
				ApplyForceUtils.AlignToVector (
					_body[BodyPart.LeftElbow], _body[BodyPart.LeftArm].BodyTransform.up,
					_body[BodyPart.Torso].BodyTransform.right - _body[BodyPart.Torso].BodyTransform.forward,
					0.1f, 4f);

				ApplyForceUtils.AlignToVector (
					_body[BodyPart.RightArm], _body[BodyPart.RightArm].BodyTransform.up, -_body[BodyPart.Torso].BodyTransform.right + _body[BodyPart.Torso].BodyTransform.forward,
					0.1f, 4f);
				ApplyForceUtils.AlignToVector (
					_body[BodyPart.RightElbow], _body[BodyPart.RightElbow].BodyTransform.up, -_body[BodyPart.Torso].BodyTransform.right - _body[BodyPart.Torso].BodyTransform.forward,
					0.1f, 4f);

				ApplyForceUtils.AlignToVector (
					_body[BodyPart.LeftLeg], _body[BodyPart.LeftLeg].BodyTransform.up,
					_body[BodyPart.Hip].BodyTransform.up - _body[BodyPart.Hip].BodyTransform.forward,
					0.1f, 4f);
				ApplyForceUtils.AlignToVector (
					_body[BodyPart.RightLeg], _body[BodyPart.RightLeg].BodyTransform.up,
					_body[BodyPart.Hip].BodyTransform.up - _body[BodyPart.Hip].BodyTransform.forward,
					0.1f, 4f);
			}
		}

		private void Fall ()
		{
			ApplyForceUtils.AlignToVector (_body[BodyPart.Head], _body[BodyPart.Head].BodyTransform.up,
				Vector3.up, 0.1f, 20f);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Head], _body[BodyPart.Head].BodyTransform.forward,
				_control.m_lookDirection, 0.1f, 20f);

			ApplyForceUtils.AlignToVector (_body[BodyPart.Torso], _body[BodyPart.Torso].BodyTransform.up,
				Vector3.up, 0.1f, 20f);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Torso], _body[BodyPart.Torso].BodyTransform.forward,
				_control.m_direction + Vector3.down, 0.1f, 10f);

			ApplyForceUtils.AlignToVector (_body[BodyPart.Hip], _body[BodyPart.Hip].BodyTransform.forward,
				_control.m_direction + Vector3.up, 0.1f, 20f);

			ApplyForceUtils.AlignToVector (_body[BodyPart.LeftLeg], _body[BodyPart.LeftLeg].BodyTransform.up,
				_body[BodyPart.Hip].BodyTransform.up, 0.1f, 20f);
			ApplyForceUtils.AlignToVector (_body[BodyPart.RightLeg], _body[BodyPart.RightLeg].BodyTransform.up,
				_body[BodyPart.Hip].BodyTransform.up, 0.1f, 20f);

			if (InputUtils.ValidMove (new Vector2 (_control.m_rawDirection.x, _control.m_rawDirection.z)))
			{
				_body[BodyPart.Hip].BodyRigid.AddForce (
					(_control.m_direction + Vector3.up) * 0.2f, ForceMode.VelocityChange);
			}
		}

		private void Stand ()
		{
			ApplyForceUtils.AlignToVector (_body[BodyPart.Head], _body[BodyPart.Head].BodyTransform.forward,
				_control.m_lookDirection, 0.1f, 20f * _control.m_applyForce, true);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Head], _body[BodyPart.Head].BodyTransform.up,
				Vector3.up, 0.1f, 50f * _control.m_applyForce, true);

			ApplyForceUtils.AlignToVector (_body[BodyPart.Torso], _body[BodyPart.Torso].BodyTransform.forward,
				_control.m_direction, 0.1f, 20f * _control.m_applyForce, true);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Torso], _body[BodyPart.Torso].BodyTransform.up,
				Vector3.up, 0.1f, 80f * _control.m_applyForce, true);

			ApplyForceUtils.AlignToVector (_body[BodyPart.Hip], _body[BodyPart.Hip].BodyTransform.forward,
				_control.m_direction, 0.1f, 20f * _control.m_applyForce, true);
			ApplyForceUtils.AlignToVector (_body[BodyPart.Hip], _body[BodyPart.Hip].BodyTransform.up,
				Vector3.up, 0.1f, 80f * _control.m_applyForce, true);

			ApplyForceUtils.AlignToVector (_body[BodyPart.LeftLeg], _body[BodyPart.LeftLeg].BodyTransform.up,
				_body[BodyPart.Hip].BodyTransform.up, 0.1f, 50f * _control.m_applyForce);
			ApplyForceUtils.AlignToVector (_body[BodyPart.RightLeg], _body[BodyPart.RightLeg].BodyTransform.up,
				_body[BodyPart.Hip].BodyTransform.up, 0.1f, 50f * _control.m_applyForce);

			ApplyForceUtils.Straighten (_body[BodyPart.Torso], _body[BodyPart.Hip],
				Vector3.up, 4 * _control.m_applyForce, ForceMode.VelocityChange);
		}
	}
}