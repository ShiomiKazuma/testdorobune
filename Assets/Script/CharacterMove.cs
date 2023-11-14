using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent (typeof(CharacterController))]
public class CharacterMove : MonoBehaviour
{
    [SerializeField] float _mouseSensitivity = 1f;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _jumpPower = 20f;
    [SerializeField] float _gravityDownForce = -60f;
    [SerializeField] Transform _hitPointTransform;
    CharacterController _characterController;
    float _cameraVerticalAngle;
    float _characterVelocityY;
    Camera _mainCamera;
    State _state;
    Vector3 _hookShotPos;
    enum State
    {
        Normal,
        HookshotFlying,

    }
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
        switch(_state)
        {
            case State.Normal:
                HandleCharacterLook();
                HandleCaracterMovement();
                HandleHookshotStart();
                break;
            case State.HookshotFlying:
                HandleCharacterLook();
                HandHookshotMovement();
                break;
        }
        
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

        Vector3 characterVelocity = transform.right * moveX * _moveSpeed + transform.forward * moveZ * _moveSpeed;

        if(_characterController.isGrounded)
        {
            _characterVelocityY = 0f;

            if(Input.GetButtonDown("Jump"))
            {
                _characterVelocityY = _jumpPower;
            }
        }

        _characterVelocityY += _gravityDownForce * Time.deltaTime;
        characterVelocity.y = _characterVelocityY;
        _characterController.Move(characterVelocity * Time.deltaTime);
    }
    //�O���b�v�����O�n�߂̏���
    void HandleHookshotStart()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit raycastHit))
            {
                //�q�b�g�����ꍇ�̏���
                _hitPointTransform.forward = _mainCamera.transform.forward; //�t�b�N�|�C���g�𐳖ʂɂ���
                _hitPointTransform.position = raycastHit.point; //�t�b�N�V���b�g���ړ�������
                _hookShotPos = raycastHit.point;
                _state = State.HookshotFlying;
            }
        }
    }

    void HandHookshotMovement()
    {
        Vector3 hookDir = (_hookShotPos - transform.position).normalized;
        float hookshotSpeedMax = 40f;
        float hookshotSpeedMin = 10f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, _hookShotPos), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotMultiplier = 2f; //�t�b�N�V���b�g�̉����x
        _characterController.Move(hookDir * hookshotSpeed * hookshotMultiplier * Time.deltaTime);

        if(Vector3.Distance(transform.position, _hookShotPos) < 1f)
        {
            _state = State.Normal;
            Debug.Log("���B");
        }
    }
}
