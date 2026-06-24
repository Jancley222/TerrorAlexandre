using UnityEngine;
using UnityEngine.UI; // Necessário para interagir com a UI antiga ou uGUI
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 6.0f;
        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Header("Player Stamina")]
        [Tooltip("Maximum stamina the player can have")]
        public float MaxStamina = 100f;
        [Tooltip("How fast stamina drains while sprinting")]
        public float StaminaDrainRate = 20f;
        [Tooltip("How fast stamina regenerates while walking or standing still")]
        public float StaminaRegenRate = 15f;
        [Tooltip("UI Image of the stamina bar (Must be Image Type: Filled)")]
        public Image StaminaBarImage;
        [Tooltip("CanvasGroup attached to the Stamina UI for Fade effects")]
        public CanvasGroup StaminaCanvasGroup;
        [Tooltip("Speed of the fade in/out effect")]
        public float FadeSpeed = 5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

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

        // stamina internal
        private float _currentStamina;
        private bool _isSprintingActive;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;


#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
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
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            // Initialize stamina
            _currentStamina = MaxStamina;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            UpdateStamina();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
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
            // Verifica se há input de movimento real
            bool isMoving = _input.move != Vector2.zero;

            // Força o input de sprint a ser falso se a estamina acabar completamente
            if (_currentStamina <= 0f)
            {
                _input.sprint = false;
            }

            // O jogador só está correndo se pressionar o botão, estiver se movendo e tiver estamina
            _isSprintingActive = _input.sprint && isMoving && _currentStamina > 0f;

            // Define a velocidade alvo baseada no estado real de corrida recalculado acima
            float targetSpeed = _isSprintingActive ? SprintSpeed : MoveSpeed;

            // Se estiver correndo de verdade, consome a estamina
            if (_isSprintingActive)
            {
                _currentStamina -= StaminaDrainRate * Time.deltaTime;
                if (_currentStamina < 0f) _currentStamina = 0f;
            }

            // Se não houver movimento, a velocidade alvo é zero
            if (!isMoving) targetSpeed = 0.0f;

            // Referência para a velocidade horizontal atual
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // Acelera ou desacelera até a velocidade alvo
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // Normaliza a direção do input
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (isMoving)
            {
                inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }

            // Move o personagem
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void UpdateStamina()
        {
            // Regenera a estamina apenas se NÃO estiver correndo ativamente e se estiver abaixo do máximo
            if (!_isSprintingActive && _currentStamina < MaxStamina)
            {
                _currentStamina += StaminaRegenRate * Time.deltaTime;
                if (_currentStamina > MaxStamina) _currentStamina = MaxStamina;
            }

            // Atualiza o preenchimento da barra de UI (Apenas se ela foi atribuída no Inspector)
            if (StaminaBarImage != null)
            {
                StaminaBarImage.fillAmount = _currentStamina / MaxStamina;
            }

            // Controla o Fade In / Fade Out usando o CanvasGroup (Apenas se ele foi atribuído no Inspector)
            if (StaminaCanvasGroup != null)
            {
                float targetAlpha = (_currentStamina >= MaxStamina) ? 0f : 1f;
                StaminaCanvasGroup.alpha = Mathf.Lerp(StaminaCanvasGroup.alpha, targetAlpha, Time.deltaTime * FadeSpeed);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
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

            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }
    }
}