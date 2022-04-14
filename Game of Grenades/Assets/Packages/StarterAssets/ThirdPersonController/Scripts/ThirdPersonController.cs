﻿/*
 * Editing Author:		Ryan Jenkins
 * Original Author:		Unity
 * Installation Date:	4/13/2022
 * Last Updated:		4/14/2022
 * Description:			A third person character controller integrated to work with Unity's Input System. We will be
 *						expanding the functionality of this script to match our games design needs including systems
 *						for diving, sliding, throwing grenades, interfacing with the grenade inventory system, and
 *						player attributes such as health, playerID and teamID.
 */

using UnityEngine;
using UnityEngine.Serialization;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		[FormerlySerializedAs("MoveSpeed")]
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float moveSpeed = 2.0f;
		[FormerlySerializedAs("SprintSpeed")] [Tooltip("Sprint speed of the character in m/s")]
		public float sprintSpeed = 5.335f;
		[FormerlySerializedAs("RotationSmoothTime")]
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float rotationSmoothTime = 0.12f;
		[FormerlySerializedAs("SpeedChangeRate")] [Tooltip("Acceleration and deceleration")]
		public float speedChangeRate = 10.0f;

		[FormerlySerializedAs("JumpHeight")]
		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float jumpHeight = 1.2f;
		[FormerlySerializedAs("Gravity")] [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;

		[FormerlySerializedAs("JumpTimeout")]
		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float jumpTimeout = 0.50f;
		[FormerlySerializedAs("FallTimeout")] [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float fallTimeout = 0.15f;

		[FormerlySerializedAs("Grounded")]
		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool grounded = true;
		[FormerlySerializedAs("GroundedOffset")] [Tooltip("Useful for rough ground")]
		public float groundedOffset = -0.14f;
		[FormerlySerializedAs("GroundedRadius")] [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float groundedRadius = 0.28f;
		[FormerlySerializedAs("GroundLayers")] [Tooltip("What layers the character uses as ground")]
		public LayerMask groundLayers;

		[FormerlySerializedAs("CinemachineCameraTarget")]
		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject cinemachineCameraTarget;
		[FormerlySerializedAs("TopClamp")] [Tooltip("How far in degrees can you move the camera up")]
		public float topClamp = 70.0f;
		[FormerlySerializedAs("BottomClamp")] [Tooltip("How far in degrees can you move the camera down")]
		public float bottomClamp = -30.0f;
		[FormerlySerializedAs("CameraAngleOverride")] [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float cameraAngleOverride = 0.0f;
		[FormerlySerializedAs("LockCameraPosition")] [Tooltip("For locking the camera position on all axis")]
		public bool lockCameraPosition = false;
		
		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;

		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool _hasAnimator;
		
		// character states
		[Header("States")]
		[SerializeField] private bool _initialized = false;
		
		// character attributes
		[Header("Attributes")]
		[SerializeField] private int _currentHealth;
		[SerializeField] private int _maxHealth = 100;
		[SerializeField] private int _playerID;
		[SerializeField] private int _teamID;
		
		// grenade management
		[Header("Inventory")]
		[SerializeField] private GrenadeInventory _inventory;
		
		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
			_fallTimeoutDelta = fallTimeout;
			
			// initialize the character to their default values for play
			_initialized = InitializePlayer();
		}

		private void Update()
		{
			_hasAnimator = TryGetComponent(out _animator);
			
			JumpAndGravity();
			GroundedCheck();
			Move();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
			grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.look.sqrMagnitude >= _threshold && !lockCameraPosition)
			{
				_cinemachineTargetYaw += _input.look.x * Time.deltaTime;
				_cinemachineTargetPitch += _input.look.y * Time.deltaTime;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

			// Cinemachine will follow this target
			cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? sprintSpeed : moveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);

				// rotate to face input direction relative to camera position
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}


			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

			// move the player
			_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}

		private void JumpAndGravity()
		{
			if (grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = fallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
		}

		// Deal damage to the player and check to see if they were killed and what attack source killed them
		public void Damage(int value, int attackerID)
		{
			
		}

		// Moves the character based on the provided force amount and direction
		public void AddForce(float force, Vector3 direction)
		{
			
		}

		// Initializes the the players character to prepare them for play
		private bool InitializePlayer()
		{
			// set the playerID to the next ID according to the Game Manager
			
			_currentHealth = _maxHealth; // set the characters current health value

			// get the characters team from the Game Manager
			
			// set the characters spawn location
			
			// set the characters grenade inventory
			
			// set the characters control device
			
			return true; // returns true once the player is done being initialized
		}

		private void Respawn()
		{
			// reset the players health
		}
		
		// Returns the players ID
		public int GetPlayerID()
		{
			return _playerID;
		}
		
	}
}