using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    public PlayerInput playerInput;
    private Camera followCam;  
    
    public float speed = 6f;
    public float jumpVelocity = 10f;

    public float hitradius;
    public Vector3 hitposition;
    public float hitforce;

    public bool isHit;
    [Range(0.01f, 1f)] public float airControlPercent;

    public float speedSmoothTime = 0.1f;
    public float turnSmoothTime = 0.1f;
    
    public float smoothTime = 0.1f;

    private float forceSmoothVelocity;
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;
    
    private float currentVelocityY;
    
    public float currentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

    public bool is_player_stunned = false;

    private const int INVINCIBLE_TIME = 2;

    public void StopMove(){
        characterController.Move(Vector3.zero);
    }
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        followCam = Camera.main;
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
    
        if (currentSpeed > 0.2f || playerInput.isfiredown || playerInput.isfire) 
            Rotate();

        if (is_player_stunned == false && playerInput.enabled)
        {
            Move(playerInput.moveInput);
        }
        
        if (playerInput.jump) 
            Jump();

        if(isHit)
            AddExplosionForce();

        if(!playerInput.enabled && !characterController.isGrounded){
            Move(Vector3.zero);
        }
    }

    public void Move(Vector2 moveInput)
    {
        var targetSpeed = speed * moveInput.magnitude;
        var moveDirection = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);
        
        targetSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        currentVelocityY += Time.deltaTime * Physics.gravity.y;

        var velocity = moveDirection * targetSpeed + Vector3.up * currentVelocityY;

        characterController.Move(velocity * Time.deltaTime);

        if (characterController.isGrounded) 
            currentVelocityY = 0;
    }

    public void Rotate()
    {
        var targetRotation = followCam.transform.eulerAngles.y;

        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                                    ref turnSmoothVelocity, turnSmoothTime);
    }

    public void Jump()
    {
        if (!characterController.isGrounded) return;
        currentVelocityY = jumpVelocity;
    }

    public void AddExplosionForce(){
        var playerSpeed = hitforce;
        var forcedirection = transform.position - hitposition;
        forcedirection = new Vector3(forcedirection.x, 0, forcedirection.z);

        playerSpeed = Mathf.SmoothDamp(playerSpeed, 0, ref forceSmoothVelocity, smoothTime);
        
        var velocity = forcedirection * playerSpeed;
        characterController.Move(velocity * Time.deltaTime);

        if(playerSpeed < 0.1f){
            isHit = false;
        }          
    }
      
    public void BecomeInvincible()
    {
        Debug.Log("player become invincible");
        gameObject.layer = 9;
        Invoke("CancleInvincible", INVINCIBLE_TIME);
    }
    private void CancleInvincible()
    {
        Debug.Log("player become Not_invincible");
        gameObject.layer = 8;
    }


}