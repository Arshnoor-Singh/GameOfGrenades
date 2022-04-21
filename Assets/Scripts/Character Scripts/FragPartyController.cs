using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace FragParty
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FragPartyController : MonoBehaviour
	{
		[Header("Player")]
		// [Tooltip("Move speed of the character in m/s")]
		// public float moveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float moveSpeed = 6.0f;
		[Tooltip("Slide time of the character in seconds")] 
		public float slideTime = 0.6f;
		[Tooltip("Dive time of the character in seconds")]
		public float diveTime = 0.4f;
		[Tooltip("The speed the character and camera rotate in place")]
		[Range(0.01f, 100f)]
		public bool rotationSpeed;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float rotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float speedChangeRate = 10.0f;
		
		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float jumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float jumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float fallTimeout = 0.15f;
		// [Tooltip("Time required to pass before being able to slide again. Set to 0f to instantly slide again")]
		// public float slideTimeout = 0.50f;
		// [Tooltip("Time required to pass before being able to dive again. Set to 0f to instantly dive again")]
		// public float diveTimeout = 0.50f;
		// [Tooltip("Time required to pass before being able to throw again. Set to 0f to instantly throw again")]
		// public float tossTimeout = 0.50f;

		[Space(10)]
		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool grounded = true;
		[Tooltip("Useful for rough ground")]
		public float groundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float groundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask groundLayers;

		[Space(10)]
		[Header("Cinemachine")]
		[Tooltip("The follow camera that will follow the player")]
		public GameObject playerCamera;
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject cinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float topClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float bottomClamp = -30.0f;
		[Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked")]
		public float cameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool lockCameraPosition = false;
		
		
		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private Vector2 _movementSpeed;
		private float _animationBlend;
		private float _animationBlendHorizontal;
		private float _animationBlendVertical;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// locomation timers
		private float _slideTime;
		private float _diveTime;
		
		// animation IDs
		private int _animIDSpeed;
		private int _animIDVerticalMovement;
		private int _animIDHorizontalMovement;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;
		private int _animIDSlide;
		private int _animIDDive;
		private int _animIDToss;
		
		private Animator _animator;
		private CharacterController _controller;
		private FragPartyInputs _input;

		private const float _threshold = 0.01f;

		private bool _hasAnimator;

		private void Awake()
		{
			// get a reference to our main camera
			if (playerCamera == null)
			{
				playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<FragPartyInputs>();

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
			_fallTimeoutDelta = fallTimeout;
			
			// reset our locomotion timers on start
			_slideTime = slideTime;
			_diveTime = diveTime;
		}

		private void Update()
		{
			_hasAnimator = TryGetComponent(out _animator);
			

			JumpAndGravity();
			
			GroundedCheck();
			Slide();
			Dive();
			if (!_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive))
			{
				Move();
			}
			Toss();
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
			_animIDDive = Animator.StringToHash("Dive");
			_animIDSlide = Animator.StringToHash("Slide");
			_animIDToss = Animator.StringToHash("Toss");
			_animIDVerticalMovement = Animator.StringToHash("VerticalMovement");
			_animIDHorizontalMovement = Animator.StringToHash("HorizontalMovement");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 position = transform.position;
			Vector3 spherePosition = new Vector3(position.x, position.y - groundedOffset, position.z);
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
			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			// if (_input.move != Vector2.zero)
			if (true)
			{
				// _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
				_targetRotation = playerCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
			
				// rotate to face input direction relative to camera position
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}

			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = moveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero)
			{
				targetSpeed = 0.0f;
			}
			
			// a reference to the players current horizontal velocity
			var velocity = _controller.velocity;
			float currentMovementSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;
			var clampedSpeed = new Vector2(
				Mathf.Clamp(moveSpeed * Mathf.Abs(_input.move.x), 0f, moveSpeed) * Mathf.Sign(_input.move.x),
				Mathf.Clamp(moveSpeed * Mathf.Abs(_input.move.y), 0f, moveSpeed) * Mathf.Sign(_input.move.y));

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentMovementSpeed < targetSpeed - speedOffset ||
			    currentMovementSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentMovementSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);
				_movementSpeed.x = Mathf.Lerp(velocity.x, clampedSpeed.x, Time.deltaTime * speedChangeRate);
				_movementSpeed.y = Mathf.Lerp(velocity.y, clampedSpeed.y, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
				_movementSpeed.x = Mathf.Round(_movementSpeed.x * 1000f) / 1000f;
				_movementSpeed.y = Mathf.Round(_movementSpeed.y * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
				_movementSpeed = new Vector2(
					Mathf.Round(clampedSpeed.x * 1000f) / 1000f,
					Mathf.Round(clampedSpeed.y * 1000f) / 1000f);
			}
			
			// update the blend state parameter
			// _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
			_animationBlendHorizontal = Mathf.Lerp(_animationBlendHorizontal, _movementSpeed.x, Time.deltaTime * speedChangeRate);
			_animationBlendVertical = Mathf.Lerp(_animationBlendVertical, _movementSpeed.y, Time.deltaTime * speedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * inputDirection;

			// move the player
			_controller.Move(targetDirection * (_speed * Time.deltaTime) +
			                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			// _controller.Move((new Vector3(_movementSpeed.x, 0.0f, _movementSpeed.y) * Time.deltaTime) + 
			//                  new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				// _animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDVerticalMovement, _animationBlendVertical);
				_animator.SetFloat(_animIDHorizontalMovement, _animationBlendHorizontal);
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
				if (_input.jump && _jumpTimeoutDelta <= 0.0f && !_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive))
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

		private void Slide()
		{
			if (grounded && !_animator.GetBool(_animIDSlide))
			{
				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDSlide, false);
				}
				
				// Slide
                // if (_input.slide && _slideTimeoutDelta <= 0.0f)
	            if (_input.slide)
                {
					// update animator if using character
                 	if (_hasAnimator)
                    {
	                    _animator.SetBool(_animIDSlide, true);
                    }
                    
                    // set the slide timer
                    _slideTime = slideTime;
                }
			}
			else if (_animator.GetBool(_animIDSlide) && _slideTime > 0f)
			{
				_slideTime -= Time.deltaTime;
				
				// set target speed based sprint speed
				float targetSpeed = moveSpeed;

				// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

				// a reference to the players current horizontal velocity
				float currentHorizontalSpeed = new Vector3(0.0f, 0.0f, moveSpeed).magnitude;

				float speedOffset = 0.1f;
				float inputMagnitude = 1f;

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

				// move the player
				// _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
				_controller.Move(transform.forward * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			}
			else
			{
				// reset the slide timer
				_slideTime = 0f;
				
				// stop the slide
				_input.slide = false;
				_animator.SetBool(_animIDSlide, false);
			}
		}

		private void Dive()
		{
			if (grounded && !_animator.GetBool(_animIDDive))
			{
				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDDive, false);
				}
				
				// Dive
				if (_input.dive)
                {
					// update animator if using character
                 	if (_hasAnimator)
                    {
	                    _animator.SetBool(_animIDDive, true);
                    }
                    
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    
                    // set the dive timer
                    _diveTime = diveTime;
                }
			}
			else if (_animator.GetBool(_animIDDive) && _diveTime > 0f)
			{
				_diveTime -= Time.deltaTime;
				
				// set target speed based sprint speed
				float targetSpeed = moveSpeed;

				// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

				// a reference to the players current horizontal velocity
				float currentHorizontalSpeed = new Vector3(0.0f, 0.0f, moveSpeed).magnitude;

				float speedOffset = 0.1f;
				float inputMagnitude = 1f;

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

				// move the player
				_controller.Move(transform.forward * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			}
			else
			{
				// reset the dive timer
				_diveTime = 0f;
				
				// stop the dive
				_input.dive = false;
				_animator.SetBool(_animIDDive, false);
			}
		}
 		
		private void Toss()
		{
			// Toss grenade
			if (!_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive))
			{
				// update animator if using character
				if (_hasAnimator)
				{
					_animator.ResetTrigger(_animIDToss);
				}
				
				// toss grenade
				if (_input.toss)
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetTrigger(_animIDToss);
						_input.toss = false;
					}
				}
			}
		}

		private void ReleaseGrenade()
		{
			// release the grenade to be thrown
			Debug.Log(("Grenade Thrown!"));
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
			var position = transform.position;
			Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
		}
	}
}