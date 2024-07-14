
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveState
{
    Walk,Sprint,Jump,Crouch,GroundPound,
}
public class ThirdPersonMovementScript : GameBehaviour
{
    [Header("CharacterController")]
    [SerializeField] CharacterController controller;
    [SerializeField] MoveState moveState;
    [SerializeField] float speedLerp = 0.5f;

    [Header("PlayerMove")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sensitivity = 1f;
    Vector2 _input;
    Vector3 _direction;
    float _speed = 0;


    [Header("PlayerRotate")]
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] Transform cam;

    float _turnSmoothVelocity;
    private bool _rotateOnMove;

    [Header("PlayerJump")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] float jumpHight = 3f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    Vector3 _velocity;
    bool _isGrounded;
    bool _jumpped;
    bool _doubleJumpUsed;

    [Header("Crouch")]
    [SerializeField] float crouchSpeed = 4f;

    [Header("Sprint")]
    [SerializeField] float sprintSpeed = 8f;

    [Header("GroundPound")]
    [SerializeField] float groundPoundSpeed = 0f;
    [SerializeField] float groundGravity = -100f;

    bool groundPoundUsed = false;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Move(InputAction.CallbackContext _context)
    {
        _input = _context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0, _input.y).normalized;

    }
    public void Jump(InputAction.CallbackContext _context)
    {
         if(_GM.gameState != GameState.Playing)
            return;

        if (_context.ReadValue<float>() == 1 && _isGrounded )
        {
            JumpAction(jumpHight);
        }
        else if (_context.ReadValue<float>() == 1 && !_doubleJumpUsed)
        {
            _velocity.y = Mathf.Sqrt(jumpHight * -2f * gravity);
            _doubleJumpUsed = true;
        }
    }

    public void JumpAction(float _jumpHight)
    {
        _velocity.y = Mathf.Sqrt(_jumpHight * -2f * gravity);
        moveState = MoveState.Jump;
        _jumpped = true;
        turnSmoothTime = 0.3f;
    }
    public void Crouch(InputAction.CallbackContext _context)
    {
        //print(_context.ReadValue<float>());
        if (_context.ReadValue<float>() == 1 && _isGrounded)
        {
            moveState = MoveState.Crouch;
        }
        else if (_context.ReadValue<float>() == 0 && _isGrounded)
        {
            moveState = MoveState.Walk;
        }
        else if (_context.ReadValue<float>() == 1 && !_isGrounded && _doubleJumpUsed)
        {
            gravity = groundGravity;
            groundPoundUsed = true;
        }
    }
    public void Sprint(InputAction.CallbackContext _context)
    {
        //print(_context.ReadValue<float>());
        if (_context.ReadValue<float>() == 1 && _isGrounded)
        {
            moveState = MoveState.Sprint;
        }
        else if (_context.ReadValue<float>() == 0 && _isGrounded)
        {
            moveState = MoveState.Walk;
        }
    }
    private void Update()
    {
        GroundCheck();
        LerpSpeed();
        MovePlayer();
        MovePlayerGravity();
    }

    private void MovePlayer()
    {
        if (_GM.gameState != GameState.Playing)
            return;

        if (_direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);

            if (_rotateOnMove)
            {
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * _speed * Time.deltaTime);
        }
    }

    private void MovePlayerGravity()
    {
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
            if (_jumpped)
            {
                moveState = MoveState.Walk;
                turnSmoothTime = 0.1f;
                _jumpped = false;
                _doubleJumpUsed = false;
                gravity = -9.81f;
                groundPound();
            }

        }
    }
    private void LerpSpeed()
    {
        float _S = 0;

        switch (moveState)
        {
            case MoveState.Walk:
                _S = walkSpeed;
                break;
            case MoveState.Sprint:
                _S = sprintSpeed;
                break;
            case MoveState.Crouch:
                _S = crouchSpeed;
                break;
            case MoveState.Jump:
                _S = jumpSpeed;
                break;

            case MoveState.GroundPound:
                _S = groundPoundSpeed;
                break;

            default:
                _S = walkSpeed;
                Debug.LogError($"MoveState.{moveState} Not Valid");
                break;
        }



        _speed = Mathf.Lerp(_speed, _S, speedLerp * Time.deltaTime);
    }

    private void groundPound()
    {
        if (groundPoundUsed)
        {
            groundPoundUsed = false;
            print("GroundPound");
        }
    }
    public void setSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }

    public void SetRotationOnMove(bool newRotationOnMove)
    {
        _rotateOnMove = newRotationOnMove;
    }
}
