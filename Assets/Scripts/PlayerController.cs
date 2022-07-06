using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private Transform shootFromPoint;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float sensitivity = 100f;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private Transform groundDetector;

    [SerializeField]
    private LayerMask groundLayer;

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
        //Cursor.visible = false;
    }

    void Update()
    {
        //Rotation of a player
        float x = Input.GetAxis(InputName.MouseX) * sensitivity * Time.deltaTime;
        float y = Input.GetAxis(InputName.MouseY) * sensitivity * Time.deltaTime;

        characterController.transform.Rotate(Vector3.up * x);

        rotation -= y;
        rotation = Mathf.Clamp(rotation, -70f, 60f);

        camera.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        //So player can shoot where she looks at
        shootFromPoint.localRotation = Quaternion.Euler(rotation, 0f, 0f);

        //Movement
        float moveX = Input.GetAxis(InputName.Horizontal);
        float moveZ = Input.GetAxis(InputName.Vertical);

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
        if (Input.GetButtonDown(InputName.JumpButton) && IsGrounded())
        {
            StartCoroutine(Jump());
        }

        //move character
        characterController.Move(moveVector + velocity);

        //animate walk and run
        if (characterController.velocity.x > 0 || characterController.velocity.y > 0)
        {
            animator.SetFloat("speed", 1);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
        if (Input.GetButtonDown(InputName.Run))
        {
            animator.SetBool("run", true);
            speed = 2;
            Debug.Log("run");
        }
        if (Input.GetButtonUp(InputName.Run))
        {
            speed = 1;
            animator.SetBool("run", false);
        }


        //shooting
        if (Input.GetButtonDown(InputName.ShootButton))
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
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100f))
        {
            Debug.Log("Did hit: " + hit.transform.name);
            selectedWeapon.Shoot();
            // Destroy(hit.transform.gameObject);
        }
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
}
