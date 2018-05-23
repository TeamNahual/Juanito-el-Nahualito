using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_MoveSpeedMultiplier = 5f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.25f;

		CharacterController m_Controller;
		Animator m_Animator;
		float m_TurnAmount;
		public float m_ForwardAmount;
		Vector3 m_GroundNormal;
		Vector3 m_Move = new Vector3(0,0,0);
		Vector3 m_LastPosition;
		Collider m_LastHit = null;

		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Controller = GetComponent<CharacterController>();
			m_GroundNormal = Vector3.up;

			m_LastPosition = transform.position;
		}


		public void Move(Vector3 move, bool jump)
		{
			m_Move = move * 0.25f;
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			m_Move.y = -1f; // Forces it to the ground
			
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			ApplyExtraTurnRotation();

			m_LastPosition = transform.position;
			m_Controller.Move(m_Move);
			UpdateAnimator(move);
			
			RaycastHit groundRaycast;
			bool grounded = CheckGroundStatus(transform.position, out groundRaycast);
			if (grounded) {
				m_GroundNormal = groundRaycast.normal;
				m_LastHit = groundRaycast.collider;
				//m_Animator.applyRootMotion = true;
			}
			else
			{
				if (m_LastHit != null) {
					Vector3 pointA = new Vector3(transform.position.x,
						m_LastPosition.y, transform.position.z);
					Vector3 pointB = m_LastPosition + (m_LastPosition - pointA) * 0.25f;
					RaycastHit ray;
					Physics.Linecast(pointA, pointB, out ray);
					if (ray.collider) {
						transform.position = ray.point;	
					} else {
						transform.position = m_LastPosition;
					}
				}
			}
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("OnGround", true);
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}

		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_Controller.isGrounded && Time.deltaTime > 0)
			{
				float multiplier = m_MoveSpeedMultiplier;
				m_Move.x *= multiplier;
				m_Move.z *= multiplier;
			}
		}
		
		bool CheckGroundStatus(Vector3 pos, out RaycastHit hitInfo)
		{
			return Physics.Raycast(pos + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, 2047);
		}
	}
}
