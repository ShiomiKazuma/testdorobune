using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _groundCheckRadius = 0.4f;
    [SerializeField] float _groundCheckOffsetY = 0.45f;
    [SerializeField] float _groundCheckDistance = 0.01f;
    [SerializeField] LayerMask _groundMask = ~0;
    [SerializeField] float _moveSpeed = 10f;
    RaycastHit _groundhit;
    bool _isGround;
    CapsuleCollider _capsuleCollider;
    Rigidbody _rb;
    Camera _mainCamera;
    Vector3 _dir;
    Vector3 _moveVec;
    // Start is called before the first frame update
    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        //カーソルを出さないようにする
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //接地判定をチェックする
        _isGround = IsGround();
        //移動入力
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        _dir = new Vector3(x, 0, z);
        // カメラのローカル座標系を基準に dir を変換する
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        //単一化する
        _dir = _dir.normalized;
        //_moveVec.y = _rb.velocity.y;
        
        Vector3 forward = _mainCamera.transform.forward;
        forward.y = 0;
        transform.forward = forward;

        _moveVec = _dir * _moveSpeed;
        _moveVec.y = _rb.velocity.y;
    }

    private void FixedUpdate()
    {
        if (_isGround)
        {
            _rb.velocity = _moveVec;
        }
    }

    bool IsGround()
    {
        float extent = Mathf.Max(0, _capsuleCollider.height * 0.5f - _capsuleCollider.radius);
        Vector3 origin = transform.TransformPoint(_capsuleCollider.center + Vector3.down * extent) + Vector3.up * _groundCheckDistance;
        Ray spereCastRay = new Ray(origin, Vector3.down);
        return Physics.SphereCast(spereCastRay, _capsuleCollider.radius, _groundCheckDistance * 2f, _groundMask);
    }
}
