using UnityEngine;
using UnityEngine.UI;

public class PlayerController    : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject staminaBar;
    public GameObject hungerBar;
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float staminaDrainSpeed = 7f;
    public float staminaRegenSpeed = 4f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    private Bar staminaBarScript;
    private Bar hungerBarScript;
    private Bar healthBarScript;

    CharacterController characterController;
    void Start()
    {
        staminaBarScript = staminaBar.GetComponent<Bar>();
        hungerBarScript = hungerBar.GetComponent<Bar>();
        healthBarScript = healthBar.GetComponent<Bar>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //if (!Input.GetKeyDown(KeyCode.Q)) { return; }
        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && staminaBarScript.percentage > 0;
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        #region Handles bars
        if (isRunning)
        {
            staminaBarScript.DeceraseBar(Time.deltaTime * staminaDrainSpeed);
        }
        else if (hungerBarScript.percentage > 0 && staminaBarScript.percentage < 100)
        {
            float amount = Time.deltaTime * staminaRegenSpeed;
           staminaBarScript.IncreaseBar(amount);
            hungerBarScript.DeceraseBar(amount/5);
        }
        #endregion
        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

      
        
       
    }


    public void TakeDamge(float amount)
    {
        healthBarScript.DeceraseBar(amount);
    }
}
