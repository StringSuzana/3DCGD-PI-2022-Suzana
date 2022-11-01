using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MyGame
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [SerializeField] private HealthBar HealthBar;
        private float currentHealth;

        public GameObject charactersContainer;
        [Tooltip("Camera")] public new Camera camera;

        [Tooltip("Shooting")] public Transform shootFromPoint;

        [Tooltip("Motion")] public CharacterController characterController;

        public Animator animator;

        public float sensitivity;

        public float speed;

        public float jumpHeight;

        public Vector3 velocity = Vector3.zero;

        [Tooltip("Ground")] public Transform groundDetector;
        public LayerMask groundLayer;


        [Tooltip("Damage intake")] public GameObject scratchView;

        [Tooltip("Audio sources")] public AudioSource audioSource;

        public AudioClip walkAudioClip;
        public AudioClip jumpAudioClip;
        public AudioClip hurtAudioClip;

        [Header("Input Settings")] private PlayerInputActions playerInput;
        private InputAction look;
        private InputAction fire;
        private InputAction move;
        private InputAction weaponChange;
        private Mouse mouse;
        private Keyboard keyboard;
        private float rotation = 0f;
        private float gravity = -9.81f;

        //weapons
        private List<Weapon> weapons;
        private int currentWeaponIndex = 0;
        private Weapon selectedWeapon;
        private IWeaponService weaponService;

        void Awake()
        {
            playerInput = new PlayerInputActions();
            mouse = InputSystem.GetDevice<Mouse>();
            keyboard = InputSystem.GetDevice<Keyboard>();
        }

        void Start()
        {
            currentHealth = PlayerPrefs.GetFloat(PlayerPrefNames.Health);
            weaponService = ServiceProvider.WeaponService();
            weapons = weaponService.GetWeapons(shootFromPoint);
            SelectWeapon();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnEnable()
        {
            look = playerInput.Player.Look;
            look.Enable();
            //look.performed += LookAt;

            fire = playerInput.Player.Fire;
            fire.Enable();
            fire.performed += Shoot;

            weaponChange = playerInput.Player.WeaponChange;
            weaponChange.Enable();
            weaponChange.performed += ChangeWeapon;
        }

        private void OnDisable()
        {
            look.Disable();
            fire.Disable();
            move.Disable();
            weaponChange.Disable();
        }

        private void HandleRotation()
        {
            var pos = mouse.delta.ReadValue();
            Debug.Log($"Position: {pos}");
            float x = pos.x * sensitivity * Time.deltaTime;
            float y = pos.y * sensitivity * Time.deltaTime;
            /* Ray ray = camera.ScreenPointToRay(pos);
 
             if (Physics.Raycast(ray, out var hit, 100))
             {
                 transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
             }*/

            characterController.transform.Rotate(Vector3.up * x);

            rotation -= y;
            rotation = Mathf.Clamp(rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
            //So player can shoot where she looks at
            shootFromPoint.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        }

        void Update()
        {
            #if ENABLE_INPUT_SYSTEM
            HandleRotation();
            #endif

            #if ENABLE_LEGACY_INPUT_MANAGER
            //Rotation of a player
            float x = Input.GetAxis(InputNames.MouseX) * sensitivity * Time.deltaTime;
            float y = Input.GetAxis(InputNames.MouseY) * sensitivity * Time.deltaTime;

            characterController.transform.Rotate(Vector3.up * x);

            rotation -= y;
            rotation = Mathf.Clamp(rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
            //So player can shoot where she looks at
            shootFromPoint.localRotation = Quaternion.Euler(rotation, 0f, 0f);

            //Movement
            float moveX = Input.GetAxis(InputNames.Horizontal);
            float moveZ = Input.GetAxis(InputNames.Vertical);

            Vector3 moveVector = characterController.transform.forward * moveZ;
            moveVector += characterController.transform.right * moveX;

            moveVector *= speed * Time.deltaTime;

            //gravity
            velocity.y += gravity * Time.deltaTime * Time.deltaTime;

            if (IsGrounded() && velocity.y < 0)
            {
                velocity.y = 0;
            }

            //jump
            if (Input.GetButtonDown(InputNames.JumpButton) && IsGrounded())
            {
                StartCoroutine(Jump());
            }

            //move character
            characterController.Move(moveVector + velocity);

            //animate walk and run
            if ((moveVector + velocity).x > 0 || (moveVector + velocity).z > 0)
            {
                animator.SetFloat("speed", speed);
                if (!audioSource.isPlaying)
                {
                    audioSource.pitch = 0.5f;
                    audioSource.PlayOneShot(walkAudioClip);
                }
            }
            else
            {
                animator.SetFloat("speed", 0);
                audioSource.Stop();
            }
            if (Input.GetButtonDown(InputNames.Run))
            {
                animator.SetBool("run", true);
                speed += 5;

                audioSource.pitch = 1f;
                audioSource.PlayOneShot(walkAudioClip);
                Debug.Log("run");
            }
            if (Input.GetButtonUp(InputNames.Run))
            {
                speed -= 5;
                animator.SetBool("run", false);
            }


            if (IsVictory())
            {
                Debug.Log("VICTORY");
            }
            #endif
        }

        private IEnumerator Jump()
        {
            animator.SetTrigger("jump");

            yield return new WaitForSeconds(0.5f);

            velocity.y = jumpHeight;
        }

        private void Shoot(InputAction.CallbackContext ctx)
        {
            selectedWeapon.Shoot();
        }

        private void ChangeWeapon(InputAction.CallbackContext ctx)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            SelectWeapon();
        }

        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundDetector.position, 0.3f, groundLayer);
        }

        private void SelectWeapon()
        {
            selectedWeapon = weapons[currentWeaponIndex];
            UIManager.shared.SetWeaponName(selectedWeapon.name);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundDetector.position, 0.3f);
        }


        public IEnumerator TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                //TODO
                //game over!
            }

            HealthBar.SetHealth(currentHealth);
            scratchView.SetActive(true);
            var image = scratchView.GetComponentInChildren<Canvas>();

            yield return new WaitForSecondsRealtime(0.4f);
            image.enabled = false;

            yield return new WaitForSecondsRealtime(0.2f);
            image.enabled = true;

            yield return new WaitForSecondsRealtime(0.2f);
            scratchView.SetActive(false);
        }

        public bool IsVictory()
        {
            var characters = charactersContainer.GetComponentsInChildren<Transform>();
            foreach (var item in characters)
            {
                if (item.gameObject.tag == "Enemy")
                {
                    return false;
                }
            }

            return true;
        }
    }

    public interface IPlayer
    {
        IEnumerator TakeDamage(float damageAmount);
    }
}