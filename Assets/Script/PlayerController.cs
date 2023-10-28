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
    [SerializeField] float _grapSpeed = 3.0f;
    [SerializeField] float _grapDis = 50.0f;
    Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
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

        if (forward != Vector3.zero)
        {
            //this.transform.forward = forward;
            forward = _mainCamera.transform.forward;
            forward.y = 0;
            transform.forward = forward;
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            Grapring();
        }

        //Debug.DrawRay(this.transform.position, _mainCamera.transform.forward * _grapDis, Color.green, 2.0f, false);
    }

    void Grapring()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, _mainCamera.transform.forward, out hit, _grapDis))
        {
            this.transform.DOMove(hit.transform.position, _grapSpeed);
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    void FixedUpdate()
    {
        // �u�͂�������v�����͗͊w�I�����Ȃ̂� FixedUpdate �ōs������
        //_rb.AddForce(_dir.normalized * _moveSpeed, ForceMode.Force);

        //�v���C���[�̈ړ�����
        _rb.velocity = vec;
    }

}
