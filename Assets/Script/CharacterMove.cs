using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent (typeof(CharacterController))]
public class CharacterMove : MonoBehaviour
{
    [SerializeField] float _mouseSensitivity = 1f;
    CharacterController _characterController;
    float _cameraVerticalAngle;
    float _characterVelocityY;
    Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController> ();
        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacterLook();
        HandleCaracterMovement();
    }

    void HandleCharacterLook()
    {
        //�}�E�X�̈ړ�
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");
        //�������̃J�����̒���
        transform.Rotate(new Vector3(0f, lookX * _mouseSensitivity, 0f), Space.Self);
        //�c�����̃J�����̒���
        _cameraVerticalAngle -= lookY * _mouseSensitivity;
        _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);
        _mainCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
    }

    void HandleCaracterMovement()
    {
        //�����̓���
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
    }
}
