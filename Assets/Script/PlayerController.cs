using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] float _moveSpeed = 3f;
    Vector3 _dir;
    Camera _mainCamera;
    /// <summary>�O���b�v�����O�̃X�s�[�h </summary>
    float _grapSpeed;
    /// <summary>�O���b�v�����O�̓͂�����/// </summary>
    [SerializeField] float _grapDis = 50.0f;
    [SerializeField] float _jumpPower = 3.0f;
    [SerializeField] Transform _debugHitPointTransform;
    [SerializeField] Transform _grapShotTransform;
    /// <summary>�O���b�v�̒��� </summary>
    float _grapSize;
    Vector3 vec;
    RaycastHit hit;
    /// <summary>�@���̃x�N�g�� </summary>
    Vector3 _normalVector = Vector3.zero;
    /// <summary>�o���X�΂̊p�x��ݒ� </summary>
    [SerializeField] int _angle = 45;
    /// <summary>�ڒn���� </summary>
    bool IsGround = false;
    Vector3 _grapShotPos;
    /// <summary>�O���b�v�����O�̏I�_�ʒu����̋��� </summary>
    [SerializeField] float _reachGrapPos = 1.0f;
    /// <summary>�O���b�v�̑��x�̍ő�l/// </summary>
    [SerializeField] float _grapSpeedMax = 40f;
    /// <summary>�O���b�v�̑��x�̍ŏ��l/// </summary>
    [SerializeField] float _grapSpeedMin = 5f;
    bool _jumping = false;
    State _state;
    [SerializeField] ParticleSystem _particleSystem;
    //�ڒn����ɕK�v�ȕϐ�
    [SerializeField] float _groundDistance = 0.2f;
    [SerializeField] LayerMask _groundMask = ~0;
    CapsuleCollider _capsuleCollider;
    [SerializeField] GameObject _camera;
    enum State
    {
        Normal,
        GrapShotFlying,
        GrapTrow
    }
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        //�J�[�\�����o���Ȃ��悤�ɂ���
        Cursor.lockState = CursorLockMode.Locked;
        _state = State.Normal;
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _grapShotTransform.gameObject.SetActive(false);
        _debugHitPointTransform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //IsGround = CheckGround();
        if (_state == State.GrapShotFlying)
        {
            GrapringMove();
        }
        else if(_state == State.GrapTrow)
        {
            GrapShotThrow();
        }
        else if (_state == State.Normal)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            //_dir = Vector3.forward * y + Vector3.right * x;
            _dir = new Vector3(x, 0, z);
            // �J�����̃��[�J�����W�n����� dir ��ϊ�����
            _dir = Camera.main.transform.TransformDirection(_dir);
            // �J�����͎΂߉��Ɍ����Ă���̂ŁAY ���̒l�� 0 �ɂ��āuXZ ���ʏ�̃x�N�g���v�ɂ���
            _dir.y = 0;
            //�P�ꉻ����
            _dir = _dir.normalized;
            vec = _dir * _moveSpeed;
            vec.y = _rb.velocity.y;
            // �L�����N�^�[���u���݂́iXZ ���ʏ�́j�i�s�����v�Ɍ�����
            Vector3 forward = _rb.velocity;
            forward.y = 0;

            forward = _mainCamera.transform.forward;
            forward.y = 0;
            transform.forward = forward;
            //if (forward != Vector3.zero)
            //{
            //    //this.transform.forward = forward;

            //}
            if (Input.GetMouseButtonDown(0))
            {
                GrapringStart();
            }

            if(IsGround)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    _jumping = true;
                }
            }
            else
            {
                _jumping = false;
            }

        }
        Debug.Log(IsGround);
    }

    void FixedUpdate()
    {
        if (_state == State.Normal)
        {
            //�v���C���[�̈ړ�����
            if (IsGround)
            {
                vec = Vector3.ProjectOnPlane(vec, _normalVector);
                if (_jumping)
                {
                    vec.y = _jumpPower;
                    IsGround = false;
                    Debug.Log("�W�����v");
                }
                _rb.velocity = vec;
            }
            else
            {
                _rb.velocity = vec;
            }
        }
    }

    void GrapringStart()
    {
        RaycastHit hitGrap;
        if (Physics.Raycast(this.transform.position, _mainCamera.transform.forward, out hitGrap, _grapDis))
        {
            _debugHitPointTransform.gameObject.SetActive(true);
            _debugHitPointTransform.transform.position = hitGrap.point;
            _grapShotPos = hitGrap.point;
            _grapSize = 0f;
            _grapShotTransform.gameObject.SetActive(true);
            _grapShotTransform.localScale = Vector3.zero;
            _state = State.GrapTrow;
        }
    }

    private void GrapShotThrow()
    {
        _grapShotTransform.LookAt(_grapShotPos);
        float grapThrowSpeed = 500f;
        _grapSize += grapThrowSpeed * Time.deltaTime;
        _grapShotTransform.localScale = new Vector3(1, 1, _grapSize);

        if(_grapSize >= Vector3.Distance(transform.position, _grapShotPos))
        {
            _state = State.GrapShotFlying;
            _particleSystem.Play();
        }
    }
    void GrapringMove()
    {
        IsGround = false;
        _grapShotTransform.LookAt(_grapShotPos);
        //�O���b�v�����O�̕��������߂�
        Vector3 grapDir = (_grapShotPos - transform.position).normalized;
        //�����Ȃ�΂Ȃ�قǑ������āA�߂Â��قǌ�������
        _grapSpeed = Mathf.Clamp(Vector3.Distance(transform.position, _grapShotPos), _grapSpeedMin, _grapSpeedMax);
        float _grapSpeedMultiplier = 2f;
        //�ړ�������
        _rb.velocity =  grapDir * _grapSpeed * _grapSpeedMultiplier;
        //�I�_�ɂ������̏���
        if(Vector3.Distance(transform.position, _grapShotPos) < _reachGrapPos)
        {
            StopGrapShot();
        }

        if(Input.GetMouseButtonDown(0))
        {
            //�O���b�v�����O�L�����Z��
            StopGrapShot();
        }

        if (Input.GetButtonDown("Jump"))
        {
            //�O���b�v�����O�L�����Z��
            StopGrapShot();
        }
    }

    //�ڒn����
    //bool CheckGround()
    //{
        
    //}

    private void OnCollisionStay(Collision collision)
    {

        //�Փ˂����ʂ̐ڐG�����_�ɂ�����@�����擾
        _normalVector = collision.contacts[0].normal;
    }

    private void ResetGravity()
    {
        vec.y = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsGround = true;
    }

    private void StopGrapShot()
    {
        _state = State.Normal;
        ResetGravity() ;
        _grapShotTransform.gameObject.SetActive(false);
        _debugHitPointTransform.gameObject.SetActive(false);
        _particleSystem.Stop();
    }
}
