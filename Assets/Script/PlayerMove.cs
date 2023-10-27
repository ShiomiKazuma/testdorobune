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
        // カメラのローカル座標系を基準に dir を変換する
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        // キャラクターを「現在の（XZ 平面上の）進行方向」に向ける
        Vector3 forward = _rb.velocity;
        forward.y = 0;

        if (forward != Vector3.zero)
        {
            this.transform.forward = forward;
        }

        
    }

    void FixedUpdate()
    {
        // 「力を加える」処理は力学的処理なので FixedUpdate で行うこと
        _rb.AddForce(_dir.normalized * _moveSpeed, ForceMode.Force);
    }

}
