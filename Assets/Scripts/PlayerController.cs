using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [Tooltip("Camera")]
        [SerializeField]
        private new Camera camera;

        [Tooltip("Shooting")]
        [SerializeField]
        private Transform shootFromPoint;

        [Tooltip("Motion")]
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float sensitivity;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float jumpHeight;

        [SerializeField]
        private Vector3 velocity = Vector3.zero;

        [Tooltip("Ground")]
        [SerializeField]
        private Transform groundDetector;

        [SerializeField]
        private LayerMask groundLayer;


        [Tooltip("Damage intake")]
        [SerializeField]
        private GameObject scratchView;

        [Tooltip("Audio source for walk, run..")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip walkAudioClip;
        [SerializeField]
        private AudioClip jumpAudioClip;
        [SerializeField]
        private AudioClip hurtAudioClip;

        private Vector3 moveInput;
        private float rotation = 0f;
        private float gravity = -9.81f;

        //weapons
        List<Weapon> weapons;
        int currentWeaponIndex = 0;
        Weapon selectedWeapon;
        IWeaponService weaponService = ServiceProvider.WeaponService();


        void Start()
        {
            weapons = weaponService.GetWeapons(shootFromPoint);
            SelectWeapon();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        void Update()
        {
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


            //shooting
            if (Input.GetButtonDown(InputNames.ShootButton))
            {
                Shoot();
            }
            //change weapon
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
                SelectWeapon();
            }
        }
        private IEnumerator Jump()
        {
            animator.SetTrigger("jump");

            yield return new WaitForSeconds(0.5f);

            velocity.y = jumpHeight;
        }
        private void Shoot()
        {
            selectedWeapon.Shoot();
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
            scratchView.SetActive(true);
            var image = scratchView.GetComponentInChildren<Canvas>();

            yield return new WaitForSecondsRealtime(0.4f);
            image.enabled = false;

            yield return new WaitForSecondsRealtime(0.6f);
            image.enabled = true;

            yield return new WaitForSecondsRealtime(0.8f);
            scratchView.SetActive(false);
        }
    }
    public interface IPlayer
    {

        IEnumerator TakeDamage(float damageAmount);

    }

}
