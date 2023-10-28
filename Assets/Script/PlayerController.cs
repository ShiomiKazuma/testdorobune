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
    [SerializeField] float _grapSpeed = 3.0f;
    [SerializeField] float _grapDis = 50.0f;
    [SerializeField] float _jumpPower = 3.0f;
    [SerializeField] Transform _debugHitPointTransform;
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
    State _state;
    enum State
    {
        Normal,
        FookShot,

    }
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        //�J�[�\�����o���Ȃ��悤�ɂ���
        Cursor.lockState = CursorLockMode.Locked;
        _state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == State.FookShot)
        {
            GrapringMove();
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

            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("�W�����v");
            }

            if (Input.GetMouseButtonDown(0))
            {
                GrapringStart();
            }

        }
        //Debug.DrawRay(this.transform.position, _mainCamera.transform.forward * _grapDis, Color.green, 2.0f, false);
    }

    void GrapringStart()
    {
        RaycastHit hitGrap;
        if (Physics.Raycast(this.transform.position, _mainCamera.transform.forward, out hitGrap, _grapDis))
        {
            _debugHitPointTransform.transform.position = hitGrap.point;
            _grapShotPos = hitGrap.point;
            _state = State.FookShot;
            //this.transform.DOMove(hitGrap.transform.position, _grapSpeed);
            //Debug.Log(hitGrap.collider.gameObject.name);
        }
    }

    void GrapringMove()
    {
        //�O���b�v�����O�̕��������߂�
        Vector3 grapDir = (_grapShotPos - transform.position).normalized;
        //�ړ�������
        _rb.velocity =  grapDir * _grapSpeed;
        //�I�_�ɂ������̏���
        if(Vector3.Distance(transform.position, _grapShotPos) < _reachGrapPos)
        {
            _state = State.Normal;
        }
        Debug.Log("�t�b�N�V���b�g��");
    }

    void FixedUpdate()
    {
        if(_state == State.Normal)
        {
            //�v���C���[�̈ړ�����
            if (IsGround)
            {
                vec = Vector3.ProjectOnPlane(vec, _normalVector);
                _rb.velocity = vec;
            }
            else
            {
                _rb.velocity = vec;
            }
        }
    }

    //�ڒn����
    bool CheckGround()
    {
        //return Physics.SphereCast()
        return true;
    }

    private void OnCollisionStay(Collision collision)
    {

        //�Փ˂����ʂ̐ڐG�����_�ɂ�����@�����擾
        _normalVector = collision.contacts[0].normal;
    }

    private void OnTriggerStay(Collider other)
    {
        IsGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsGround = false;
    }

    
}
