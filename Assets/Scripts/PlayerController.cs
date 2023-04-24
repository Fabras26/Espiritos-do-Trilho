using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace StarterAssets
{
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class PlayerController : MonoBehaviour
	{
		public float horizontal;
		public float vertical;
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
		public bool movendoCaixa = false;
		[Space(10)]
		public float jumpForce = 5f; // Força do salto
		public float fallMultiplier = 2.5f; // Multiplicador de queda
		public float lowJumpMultiplier = 2f; // Multiplicador de salto baixo
		[Header("Vozes")]
		public AudioClip[] vozesJogador;



		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
	
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private Vector3 moveDirection = Vector3.zero;
		private Vector3 smoothMoveVelocity = Vector3.zero;



#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		//private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private Rigidbody rb;

		private AudioSource audioSource;
		private bool IsCurrentDeviceMouse
		{
			get
			{
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
			}
		}

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
			audioSource = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

		}

        private void LateUpdate()
        {

		}

		private void Update()
        {
			JumpAndGravity();
			GroundedCheck();
			
			if(!movendoCaixa)
            {
				Move();
				CameraRotation();
			}
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset

			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers);

		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			float speed = GetPlayerSpeed();

			
			Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");


			// Multiplica o vetor de entrada pela rotação do jogador

			
				inputDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * inputDirection;
			//rb.velocity = inputDirection * speed;

			if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
			{
				Vector3 targetVelocity = inputDirection * speed;
				targetVelocity.y = rb.velocity.y;
				rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * 10f);
				if(!audioSource.isPlaying && Grounded)
                {
					GetComponent<Passos>().TocarSomPasso(audioSource);
                }
			}
            else
            {
				rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
				
		}
		public void SetVelocidade(float vel)
        {
			rb.velocity = new Vector3(vel, rb.velocity.y, vel);

		}
		float GetPlayerSpeed()
		{/*
			if (Input.GetKey(KeyCode.LeftShift))
			{
				return SprintSpeed;
			}
			else
			{
				return MoveSpeed;
			}*/
			return MoveSpeed;
		}
	
		
		private void JumpAndGravity()
		{
			if (Grounded)
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
				}

				// Aplica multiplicadores de queda e salto baixo
				if (rb.velocity.y < 0)
				{
					rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
				}
				else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
				{
					rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
				}
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

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}