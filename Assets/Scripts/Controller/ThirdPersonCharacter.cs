using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 8f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		//[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.25f;

		[SerializeField] PhysicMaterial dynamicPhysMat;
		[SerializeField] PhysicMaterial staticPhysMat;

		CharacterController m_Controller;
		Animator m_Animator;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		public float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		//Vector3 m_CamOffset = new Vector3(10,10,-10);
		Vector3 m_Move = new Vector3(0,0,0);
		Vector3 m_LastPosition;
		Collider m_LastHit = null;

		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Controller = GetComponent<CharacterController>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_OrigGroundCheckDistance = m_GroundCheckDistance;
			m_LastPosition = transform.position;
		}


		public void Move(Vector3 move, bool jump)
		{
			m_Move = move * 0.25f;
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			m_Move.y = -5;
			
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();

			// send input and other state parameters to the animator
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
					Vector3 correctedPosition = new Vector3(transform.position.x,
						m_LastPosition.y, transform.position.z);
					
					
					transform.position = m_LastHit.ClosestPointOnBounds(correctedPosition);
				} else {
					//transform.position = m_LastPosition;
				}
				//m_GroundNormal = Vector3.up;
			}
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("OnGround", true);
			
			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_Controller.isGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_Controller.isGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}

        public float movementSpeed = 5;
		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_Controller.isGrounded && Time.deltaTime > 0)
			{
				float multiplier = movementSpeed;
				m_Move.x *= multiplier;
				m_Move.z *= multiplier;
			}
		}
		
		bool CheckGroundStatus(Vector3 pos, out RaycastHit hitInfo)
		{
			bool raycastCheck = Physics.Raycast(pos + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance);
			/*if (raycastCheck)
			{
				m_GroundNormal = hitInfo.normal;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}*/
			return raycastCheck;
		}
	}
}
