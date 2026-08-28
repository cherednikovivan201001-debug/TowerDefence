using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCameraController _cameracontroller;

    [SerializeField] private float _walkSpeed;

    [SerializeField] private Rigidbody _playerRB;

    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private AudioSource _playerFootsteps;

    [SerializeField] private float _lookSpeed = 5f;

    [SerializeField] private Vector2 _limits = new Vector2(10f, 10f);

    //[SerializeField] private float _jumpForce = 5f;

    [SerializeField] private PlayerAttackController _atckController;

    private float _currentSpeed = 0;

    private Vector3 startPosition;

    private float _targetLookY;

    //private const string _moveParameter = "MoveSpeed";

    //private const string _danceParam = "Dance";

    //private const string _groundTag = "Ground";

    private Vector3 _currentMoveVector;

    private bool _isGrounded = true;

    private float _moveX = 0;

    private float _moveZ = 0;


    private void OnEnable()
    {
        PlayerInput._onMoveCallback += OnMovePressed;
        PlayerInput._onLookCallback += OnLookPressed;
        PlayerInput._OnShoot += OnAttack;
        startPosition = transform.position;
        //PlayerInput._OnDance += OnDancePressed;
        //PlayerInput._OnJump += OnJumpPressed;
    }

    private void OnDisable()
    {
        PlayerInput._onLookCallback -= (OnLookPressed);
        PlayerInput._onMoveCallback -= (OnMovePressed);
        PlayerInput._OnShoot -= OnAttack;
        //PlayerInput._OnDance -= (OnDancePressed);
        //PlayerInput._OnJump -= (OnJumpPressed);
    }

    private void OnAttack(bool t)
    {
        Debug.Log($"Attack: {t}");
        _atckController.OnShootPerformed(t);
    }

    private void OnMovePressed(Vector2 moveInput)
    {
        Debug.Log(moveInput);

        _moveX = moveInput.x;

        _moveZ = moveInput.y;
    }

    private void OnLookPressed(Vector2 look)
    {
        _targetLookY = look.x;
    }


    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Rotate();
        //UpdateAnimation();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * _targetLookY * 5f * Time.deltaTime);
    }


    private void Move()
    {
     _currentMoveVector = new Vector3(_moveX, 0f, _moveZ) * _walkSpeed * Time.fixedDeltaTime;

     Vector3 newPosition = transform.position + transform.TransformDirection( _currentMoveVector );

        newPosition.x = Math.Clamp(newPosition.x, startPosition.x - _limits.x, startPosition.x + _limits.x);

        newPosition.z = Math.Clamp(newPosition.z, startPosition.z - _limits.y, startPosition.z + _limits.y);

        transform.position = newPosition;


    }

    //    private void UpdateAnimation()
    //    {
    //        _playerAnimator.SetFloat(_moveParameter, _currentSpeed);
    //    }

    //    private void OnJumpPressed()
    //    {
    //        if(!_isGrounded)
    //        {
    //            return;
    //        }
    //        _isGrounded = false;
    //        _playerRB.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    //    }

    //    private void /*OnCollisionEnter*/(Collision other)
    //    {
    //        if (other.gameObject.CompareTag(_groundTag))
    //        {
    //            _isGrounded = true;
    //        }
    //    }
    //    private void OnDancePressed()
    //    {
    //        _playerAnimator.SetTrigger(_danceParam);
    //    }
}
