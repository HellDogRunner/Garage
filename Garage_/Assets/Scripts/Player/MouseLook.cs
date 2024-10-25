using UnityEngine;
using Zenject;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 1;
    [SerializeField] private Transform _playerBody;

    private float _xRotation = 0;
    private InputController _inputController;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = _inputController.MousePosition.x * _mouseSensitivity * 100 * Time.deltaTime;
        float mouseY = _inputController.MousePosition.y * _mouseSensitivity * 100 * Time.deltaTime;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90);
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}
