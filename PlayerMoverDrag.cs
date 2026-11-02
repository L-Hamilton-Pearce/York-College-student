
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private Rigidbody rb;

    //camera
    [SerializeField] float mouseSpeed;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject pisole; 
    private float rotationX = 0f;
    private float hInput;
    private float vInput;
    //PlayerMove 
    [SerializeField] float moveSpeed;
    [SerializeField] float dragSpeed; 

    private float horiztalInput;
    private float verticalInput;
    //jumping 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airSpeed;
    private bool canJump; 


    Vector3 moveDirection;


    //grounded 
    [SerializeField] float hight;
    [SerializeField] LayerMask groundLayer; 
    private bool grounded;  


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        canJump = true;
 

    }


    void Update()
    {
        horiztalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        CameraMove();

     
   

        grounded = Physics.Raycast(transform.position, Vector3.down, hight, groundLayer);

        if (grounded)
        {
            rb.linearDamping = dragSpeed;
        }
        else
        {
            rb.linearDamping = 0f;
        } 
        pisole.transform.position = cameraTransform.position;
        pisole.transform.rotation = cameraTransform.rotation;
    }
    private void FixedUpdate()
    {
        PlayerMove();
        SpeedControl();


        if (Input.GetKey(KeyCode.Space) && grounded && canJump)
        {
            Jump(); 
        }
    }
    private void CameraMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed;

        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    private void PlayerMove()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horiztalInput;

        if (grounded)
        {

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airSpeed, ForceMode.Force);
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
   
            canJump = false;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
  
    }
    private void ResetJump()
    {
    
        canJump = true;

    }
}
