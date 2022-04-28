/*
*  Author:             Ryan Jenkins, Abhishek Das - based on Unity Start Asset package
*  Creation Date:      4/19/2022
*  Last Updated:       4/20/2022
*  Description:        
*/

using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
*/

#region RequiredComponents
[RequireComponent(typeof(Animator))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(FragPartyInputs))]
#endif
[RequireComponent(typeof(GrenadeInventory))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(BasicRigidBodyPush))]
#endregion
public class FragPartyController : MonoBehaviour
{
	// publicly declared variables
	#region PublicFields
	
	[Header("Player Manager")]
	[Tooltip("The integer ID assigned to the player character at runtime")]
	[HideInInspector] public int PlayerID;
	[Tooltip("The team ID string assigned to the player at runtime")]
	[HideInInspector] public string Team;
	
	[Space(10)]
	[Header("Player Locomotion")]
	[Tooltip("Movement speed of the character in m/s")]
	public float moveSpeed = 6.0f;
	[Tooltip("Slide time of the character in seconds")] 
	public float slideTime = 0.6f;
	[Tooltip("Dive time of the character in seconds")]
	public float diveTime = 0.4f;
	
	[Space(10)]
	[Tooltip("Boolean to set if the player should currently rotate to face away from the camera")]
	public bool updateRotation = true;
	[Tooltip("The speed the character and camera rotate in place")]
	[Range(0.01f, 10f)] public float rotationSpeed = 1f;
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	public float speedChangeRate = 10.0f;
	
	[Space(10)]
	[Tooltip("The mass of the player when applying force")]
	public float mass = 3f;

	[Space(10)]
	[Tooltip("The height the player can jump")]
	public float jumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float gravity = -15.0f;

	[Space(10)]
	[Header("Player Timeouts")]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float jumpTimeout = 0.50f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float fallTimeout = 0.15f;

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
	
	#endregion

	// privately declared variables
	#region PrivateFields

	// cinemachine
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;

	// player locomotion
	private float _speed;
	private float _targetRotation = 0.0f;
	private float _rotationVelocity;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;
	private bool _lockMovement = false;
	
	// animation blending fields
	private float _animationBlendHorizontal;
	private float _animationBlendVertical;
	
	// pseudo-physics fields
	private Vector3 _forceVector = Vector3.zero;
	private Vector3 _appliedForceVector = Vector3.zero;
	private bool _impacted = false;

	// timeout delta-time
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	// locomotion timers
	private float _slideTime;
	private float _diveTime;
	
	// animation IDs
	private int _animIDVerticalMovement;
	private int _animIDHorizontalMovement;
	private int _animIDGrounded;
	private int _animIDJump;
	private int _animIDFreeFall;
	private int _animIDMotionSpeed;
	private int _animIDSlide;
	private int _animIDDive;
	private int _animIDToss;

	// input threshold
	private const float _THRESHOLD = 0.01f;

	// animator component bool
	private bool _hasAnimator;
	
	#endregion

	// object component reference variables
	#region ComponentReferences

	private Animator _animator;
	private CharacterController _controller;
	private FragPartyInputs _input;
	private GrenadeInventory _inventory;
	[SerializeField] private Transform _grenadeRoot;

	#endregion

	#region UnityFunctions

	// called when the script is loaded, used to initialize variables before the application starts
	private void Awake()
	{
		// get a reference to the main camera if a separate reference isn't already provided in the inspector
		if (playerCamera == null)
		{
			playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}

	// called on the frame when a script is enabled just before any of the Update methods are called the first time.
	private void Start()
	{
		// set component references
		_hasAnimator = TryGetComponent(out _animator);
		_controller = GetComponent<CharacterController>();
		_input = GetComponent<FragPartyInputs>();
		_inventory = GetComponent<GrenadeInventory>();
		
		AssignAnimationIDs();

		// reset locomotion timeouts on start
		_jumpTimeoutDelta = jumpTimeout;
		_fallTimeoutDelta = fallTimeout;
		
		// reset locomotion timers on start
		_slideTime = slideTime;
		_diveTime = diveTime;

		TeamAssign();
	}

	// called once every frame, if the MonoBehaviour is enabled
	private void Update()
	{
		JumpAndGravity();
		GroundedCheck();
		Slide();
		Dive();
		
		if (!_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive))
		{
			Move();
		}
		Toss();
		
		_inventory.UpdateInventory();
	}

	private void LateUpdate()
	{
		CameraRotation();
	}

	#endregion

	#region StandardFunctions

	private void TeamAssign()
    {
		if (PlayerID == 0 || PlayerID == 1)
		{
			Team = "Team_A";
			Debug.Log("Team A");
		}

		if (PlayerID == 2 || PlayerID == 3)
		{
			Team = "Team_B";
			Debug.Log("Team B");
		}
	}
	
	private void AssignAnimationIDs()
	{
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
		if (_input.look.sqrMagnitude >= _THRESHOLD && !lockCameraPosition)
		{
			if (!_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive))
			{
				_cinemachineTargetYaw += _input.look.x * Time.deltaTime;
			}
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
		// Rotate player to match the the camera rotation
		if (updateRotation)
		{
			_targetRotation = playerCamera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f); // rotate to face input direction relative to camera position
		}

		if (!_lockMovement && !_impacted)
		{
			
			float targetSpeed = moveSpeed; // set target speed based on move speed

			// a simplistic acceleration and deceleration
			
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero)
			{
				targetSpeed = 0.0f;
			}
			
			// a reference to the players current horizontal velocity
			var velocity = _controller.velocity; 
			float currentMovementSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentMovementSpeed < targetSpeed - speedOffset || currentMovementSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentMovementSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			
			// update the blend state parameters
			Vector2 targetDirectionalSpeed = _input.move * targetSpeed;
			targetDirectionalSpeed.x = Mathf.Round(targetDirectionalSpeed.x * 1000f) / 1000f;
			targetDirectionalSpeed.y = Mathf.Round(targetDirectionalSpeed.y * 1000f) / 1000f;
			_animationBlendHorizontal = Mathf.Lerp(_animationBlendHorizontal, targetDirectionalSpeed.x, Time.deltaTime * speedChangeRate);
			_animationBlendVertical = Mathf.Lerp(_animationBlendVertical, targetDirectionalSpeed.y, Time.deltaTime * speedChangeRate);

			// normalize the input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// rotate the target direction based on the user input
			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * inputDirection;

			// move the player character
			_controller.Move(targetDirection * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using characters
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDVerticalMovement, _animationBlendVertical);
				_animator.SetFloat(_animIDHorizontalMovement, _animationBlendHorizontal);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}
		else  if (_impacted)
		{
			// if sufficient force remains, move the character
			if (_forceVector.magnitude > 0.2f)
			{
				// Move the character according to the added force
				_controller.Move(_appliedForceVector * Time.deltaTime);
				
				// Apply gravity to force vector
				_appliedForceVector = new Vector3(_forceVector.x, _appliedForceVector.y + gravity * Time.deltaTime, _forceVector.z);

				// Reduce the added force
				_forceVector = Vector3.Lerp(_forceVector, Vector3.zero, mass * Time.deltaTime);
			}
			else
			{
				// Reset the force vector and remove the impacted state
				_forceVector = Vector3.zero;
				_impacted = false;
			}
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
			if (_input.jump && _jumpTimeoutDelta <= 0.0f && !_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive)  && !_impacted)
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

	#endregion
	
	#region ExpandedFunctions
	
	private void Slide()
	{
		if (grounded && ActionReady())
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

                _lockMovement = true;
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
		if (grounded && ActionReady())
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

                _lockMovement = true;
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
		if (!_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive) && !_impacted)
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

	// Triggers the grenade to be released from the player
	private void TossGrenade()
	{
		// release the grenade to be thrown
		bool grenadeThrown = _inventory.ThrowGrenade(_grenadeRoot.position, _grenadeRoot.rotation, _grenadeRoot.forward);
		
		if (grenadeThrown)
		{
			Debug.Log(("Grenade Thrown!"));	
		}
		else
		{
			Debug.Log("Unable to throw grenade.");
		}
	}

	// Returns true if the player input for jump. slide and dive are all false and the player is not impacted
	private bool ActionReady()
	{
		return (!_animator.GetBool(_animIDSlide) && !_animator.GetBool(_animIDDive) && !_animator.GetBool(_animIDJump) && !_impacted);
	}

	// Call this function to add an impact force:
	public void AddForce(Vector3 direction, float force)
	{
		direction.Normalize();
		if (grounded && direction.y < 0) direction.y = -direction.y; // reflect down force on the ground
		_forceVector += direction.normalized * force / mass;
		_appliedForceVector = _forceVector;
		_impacted = true;
	}

	public void ForceSlide()
	{
		_input.slide = true;
	}

	public void ForceDive()
	{
		_input.dive = true;
	}

	// unlock that characters ability to move
	public void UnlockMovement()
	{
		_lockMovement = false;
	}
	
	#endregion

	#region HelperFunctions

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

		Gizmos.color = grounded ? transparentGreen : transparentRed;
		
		// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
		var position = transform.position;
		Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
	}

	#endregion
}