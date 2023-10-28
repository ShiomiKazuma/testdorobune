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
        // カメラのローカル座標系を基準に dir を変換する
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        //単一化する
        _dir = _dir.normalized;
        vec = _dir * _moveSpeed;
        vec.y = _rb.velocity.y;
        // キャラクターを「現在の（XZ 平面上の）進行方向」に向ける
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
        // 「力を加える」処理は力学的処理なので FixedUpdate で行うこと
        //_rb.AddForce(_dir.normalized * _moveSpeed, ForceMode.Force);

        //プレイヤーの移動処理
        _rb.velocity = vec;
    }

}
