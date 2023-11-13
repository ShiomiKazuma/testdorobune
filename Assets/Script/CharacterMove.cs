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
        //マウスの移動
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");
        //横方向のカメラの調整
        transform.Rotate(new Vector3(0f, lookX * _mouseSensitivity, 0f), Space.Self);
        //縦方向のカメラの調整
        _cameraVerticalAngle -= lookY * _mouseSensitivity;
        _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);
        _mainCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
    }

    void HandleCaracterMovement()
    {
        //方向の入力
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
    }
}
