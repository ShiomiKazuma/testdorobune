using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] float _moveSpeed = 3f;
    Vector3 _dir;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        _dir = Vector3.forward * y + Vector3.right * x;
        // �J�����̃��[�J�����W�n����� dir ��ϊ�����
        _dir = Camera.main.transform.TransformDirection(_dir);
        // �J�����͎΂߉��Ɍ����Ă���̂ŁAY ���̒l�� 0 �ɂ��āuXZ ���ʏ�̃x�N�g���v�ɂ���
        _dir.y = 0;
        // �L�����N�^�[���u���݂́iXZ ���ʏ�́j�i�s�����v�Ɍ�����
        Vector3 forward = _rb.velocity;
        forward.y = 0;

        if (forward != Vector3.zero)
        {
            this.transform.forward = forward;
        }

        
    }

    void FixedUpdate()
    {
        // �u�͂�������v�����͗͊w�I�����Ȃ̂� FixedUpdate �ōs������
        _rb.AddForce(_dir.normalized * _moveSpeed, ForceMode.Force);
    }

}
