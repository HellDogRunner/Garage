using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _jumpHeight = 1;
    [SerializeField] private float _groudDistance;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;

    private CharacterController _controller;
    private InputController _inputController;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _isJumping;

    private const float GravityAcceleration = -9.81f;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
        _inputController.OnJumpKeyPressed += Jump;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(gameObject.transform.position, _groudDistance, _groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2;
        }
        float x = _inputController.MoveDirection.x;
        float z = _inputController.MoveDirection.z;

        Vector3 direction = transform.right * x + transform.forward * z;

        _controller.Move(direction * (Time.deltaTime * _speed));

        if (_isJumping && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * GravityAcceleration);
        }

        _velocity.y += GravityAcceleration * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnDisable()
    {
        _inputController.OnJumpKeyPressed -= Jump;
    }

    private void Jump(bool isJumpPressed)
    {
        _isJumping = isJumpPressed;
    }
}
