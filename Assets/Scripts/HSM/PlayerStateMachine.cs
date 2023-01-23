using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Data;
using Global;
using MyGame;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace HSM
{
    /**NpcStateMachine is a context*/
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject charactersContainer;
        [SerializeField] private Transform shootFromPoint;
        [SerializeField] private GameObject scratchView;

        [SerializeField] private GameObject gameManager;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private CharacterController characterController;

        [SerializeField] private Animator animator;

        [SerializeField] private float sensitivity;
        [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;
        [SerializeField] public Vector3 velocity;

        [SerializeField] private Transform groundDetector;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private AudioClip walkAudioClip;
        [SerializeField] private AudioClip jumpAudioClip;
        [SerializeField] private AudioClip hurtAudioClip;

        private float _currentHealth;
        private PlayerInputActions _playerInput;
        private InputAction _move;
        private Keyboard _keyboard;
        private InputAction _look;
        private InputAction _jump;
        private InputAction _run;
        private Mouse _mouse;
        private float _rotation;
        private float _gravity = 1.8f;


        private InputAction _fire;
        private InputAction _weaponChange;
        private Weapon _selectedWeapon;
        private int _currentWeaponIndex;
        private List<Weapon> _weapons;
        private IWeaponService _weaponService;

        public readonly int ShootTriggerAnim = Animator.StringToHash("shoot");
        public readonly int Run = Animator.StringToHash("run");
        public readonly int JumpTriggerAnim = Animator.StringToHash("jump");
        public readonly int SpeedFloatAnim = Animator.StringToHash("speed");
        public readonly int DieTriggerAnim = Animator.StringToHash("die");
        public const string EnemyTag = "Enemy";


        public PlayerBaseState CurrentState { get; set; }
        public PlayerStateFactory StateFactory { get; set; }

        public bool IsJumpPressed { get; set; }

        public float JumpHeight
        {
            get => jumpHeight;
            set => jumpHeight = value;
        }

        public Animator Animator
        {
            get => animator;
            set => animator = value;
        }

        public AudioSource SfxAudioSource
        {
            get => sfxAudioSource;
            set => sfxAudioSource = value;
        }

        public AudioClip JumpAudioClip
        {
            get => jumpAudioClip;
            set => jumpAudioClip = value;
        }

        public bool IsGrounded { get; set; }

        public bool IsJumping { get; set; }

        #region Unity methods

        private void Awake()
        {
            _playerInput = new PlayerInputActions();
            _mouse = InputSystem.GetDevice<Mouse>();
            _keyboard = InputSystem.GetDevice<Keyboard>();

            StateFactory = new PlayerStateFactory(this);
            CurrentState = StateFactory.Grounded();
            CurrentState.EnterState();
        }

        private void Start()
        {
            _currentHealth = PlayerPrefs.GetFloat(PlayerPrefNames.Health);
            _weaponService = ServiceProvider.WeaponService();
            _weapons = _weaponService.GetWeapons(shootFromPoint);
            SelectWeapon();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            HandleRotation();
            RunIfAltKeyIsPressed();

            velocity.y -= _gravity * Time.deltaTime;
            IsGrounded = !IsJumping && velocity.y < 0 && IsTouchingGround();
            Debug.Log(IsGrounded);
            HandleMovement();

            CurrentState.UpdateState();

            if (IsVictory())
            {
                Debug.Log("VICTORY");
                gameManager.GetComponent<IGameManager>().PlayLevelCompletedTimeline();
            }
        }


        protected void HandleMovement()
        {
            float moveX = _move.ReadValue<Vector2>().x;
            float moveZ = _move.ReadValue<Vector2>().y;

            Transform t = characterController.transform;

            Vector3 moveVector = t.forward * moveZ;
            moveVector += t.right * moveX;
            moveVector *= speed * Time.deltaTime;

            /*gravity*/
            // velocity.y -= _gravity * Time.deltaTime;
            //if (IsGrounded() && velocity.y < 0)
            //{
            //    velocity.y = 0;
            //}

            characterController.Move(moveVector + velocity);
            AnimateMovement(moveVector);
        }

        protected void OnEnable()
        {
            _move = _playerInput.Player.Move;
            _move.Enable();

            _jump = _playerInput.Player.Jump;
            _jump.Enable();
            _jump.performed += JumpInput;


            _look = _playerInput.Player.Look;
            _look.Enable();

            _fire = _playerInput.Player.Fire;
            _fire.Enable();
            _fire.performed += Shoot;

            _weaponChange = _playerInput.Player.WeaponChange;
            _weaponChange.Enable();
            _weaponChange.performed += ChangeWeapon;
        }

        protected void OnDisable()
        {
            _look.Disable();
            _move.Disable();
            _jump.Disable();
            _fire.Disable();
            _weaponChange.Disable();
        }

        protected void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundDetector.position, 0.3f);
        }

        #endregion

        public bool IsTouchingGround()
        {
            return Physics.CheckSphere(groundDetector.position, 0.3f, groundLayer);
        }

        public bool IsVictory()
        {
            /**Victory is when there are no more enemies in character container*/
            var characters = charactersContainer.GetComponentsInChildren<Transform>();
            return characters.All(item => !item.gameObject.CompareTag(EnemyTag));
        }

        public void GameOver()
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.Health, 0);
            _currentHealth = 0;

            //stop enemies
            Transform[] enemies = charactersContainer.GetComponentsInChildren<Transform>();
            foreach (Transform enemy in enemies)
            {
                if (enemy.TryGetComponent(out IEnemy enemyCharacter))
                {
                    enemyCharacter.Stop();
                }
            }

            sfxAudioSource.Stop();
            animator.SetTrigger(DieTriggerAnim);
            gameManager.GetComponent<IGameManager>().PlayGameOverTimeline();
        }

        private void HandleRotation()
        {
            Vector2 delta = _mouse.delta.ReadValue();
            float x = delta.x * sensitivity * Time.deltaTime;
            float y = delta.y * sensitivity * Time.deltaTime;

            characterController.transform.Rotate(Vector3.up * x);

            _rotation -= y;
            _rotation = Mathf.Clamp(_rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(_rotation, 0f, 0f);
            shootFromPoint.localRotation = Quaternion.Euler(_rotation,   0f, 0f);
        }

        #region IPlayer methods

        public IEnumerator TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                GameOver();
            }
            else
            {
                PlayerPrefs.SetFloat(PlayerPrefNames.Health, _currentHealth);
                healthBar.SetHealth(_currentHealth);
                scratchView.SetActive(true);
                var image = scratchView.GetComponentInChildren<Canvas>();


                yield return new WaitForSecondsRealtime(0.4f);
                image.enabled = false;

                yield return new WaitForSecondsRealtime(0.2f);
                image.enabled = true;

                yield return new WaitForSecondsRealtime(0.2f);
                scratchView.SetActive(false);
            }
        }

        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > GameData.MaxPlayerHealth)
            {
                _currentHealth = GameData.MaxPlayerHealth;
            }

            PlayerPrefs.SetFloat(PlayerPrefNames.Health, _currentHealth);
            healthBar.SetHealth(_currentHealth);
        }

        #endregion

        #region New Input System Callbacks

        private void Shoot(InputAction.CallbackContext ctx)
        {
            StartCoroutine(ShootAction());
        }

        private void ChangeWeapon(InputAction.CallbackContext ctx)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % _weapons.Count;
            SelectWeapon();
        }

        private void JumpInput(InputAction.CallbackContext ctx)
        {
            IsJumpPressed = true;
        }

        #endregion

        private IEnumerator ShootAction()
        {
            animator.SetTrigger(ShootTriggerAnim);

            yield return new WaitForSeconds(1f);

            _selectedWeapon.Shoot();
        }


        protected void SelectWeapon()
        {
            _selectedWeapon = _weapons[_currentWeaponIndex];
            UIManager.shared.SetWeaponName(_selectedWeapon.Name);
        }

        protected void RunIfAltKeyIsPressed()
        {
            if (_keyboard.altKey.wasPressedThisFrame)
            {
                animator.SetBool(Run, true);
                speed += 5;
                PlayRunSoundFx();
            }
            else if (_keyboard.altKey.isPressed)
            {
                PlayRunSoundFx();
            }

            else if (_keyboard.altKey.wasReleasedThisFrame)
            {
                speed -= 5;
                animator.SetBool(Run, false);
            }
        }

        protected virtual void PlayWalkSoundFx()
        {
            if (sfxAudioSource.isPlaying)
                return;

            sfxAudioSource.pitch = 0.5f;
            sfxAudioSource.PlayOneShot(walkAudioClip);
        }

        //protected virtual void PlayJumpSoundFx()
        //{
        //    if (sfxAudioSource.isPlaying)
        //        sfxAudioSource.Stop();

        //    sfxAudioSource.pitch = 0.5f;
        //    sfxAudioSource.PlayOneShot(jumpAudioClip);
        //}

        protected virtual void PlayRunSoundFx()
        {
            if (sfxAudioSource.isPlaying)
                return;
            sfxAudioSource.pitch = 1f;
            sfxAudioSource.PlayOneShot(walkAudioClip);
        }


        protected void AnimateMovement(Vector3 moveVector)
        {
            animator.SetFloat(SpeedFloatAnim, speed);

            if (IsPlayerRunning()) return;
            if (moveVector.x != 0 || moveVector.z != 0)
            {
                animator.SetFloat(SpeedFloatAnim, speed);
                PlayWalkSoundFx();
            }
            else
            {
                animator.SetFloat(SpeedFloatAnim, 0);
                StopCurrentSoundFx();
            }
        }

        protected void StopCurrentSoundFx()
        {
            sfxAudioSource.Stop();
        }

        private bool IsPlayerRunning() => _keyboard.altKey.isPressed;
    }
}