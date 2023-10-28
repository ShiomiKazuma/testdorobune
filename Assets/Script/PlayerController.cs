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
    /// <summary>グラップリングのスピード </summary>
    [SerializeField] float _grapSpeed = 3.0f;
    [SerializeField] float _grapDis = 50.0f;
    [SerializeField] float _jumpPower = 3.0f;
    [SerializeField] Transform _debugHitPointTransform;
    Vector3 vec;
    RaycastHit hit;
    /// <summary>法線のベクトル </summary>
    Vector3 _normalVector = Vector3.zero;
    /// <summary>登れる傾斜の角度を設定 </summary>
    [SerializeField] int _angle = 45;
    /// <summary>接地判定 </summary>
    bool IsGround = false;
    Vector3 _grapShotPos;
    /// <summary>グラップリングの終点位置からの距離 </summary>
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
        //カーソルを出さないようにする
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

            forward = _mainCamera.transform.forward;
            forward.y = 0;
            transform.forward = forward;
            //if (forward != Vector3.zero)
            //{
            //    //this.transform.forward = forward;

            //}

            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("ジャンプ");
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
        //グラップリングの方向を決める
        Vector3 grapDir = (_grapShotPos - transform.position).normalized;
        //移動させる
        _rb.velocity =  grapDir * _grapSpeed;
        //終点についた時の処理
        if(Vector3.Distance(transform.position, _grapShotPos) < _reachGrapPos)
        {
            _state = State.Normal;
        }
        Debug.Log("フックショット中");
    }

    void FixedUpdate()
    {
        if(_state == State.Normal)
        {
            //プレイヤーの移動処理
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

    //接地判定
    bool CheckGround()
    {
        //return Physics.SphereCast()
        return true;
    }

    private void OnCollisionStay(Collision collision)
    {

        //衝突した面の接触した点における法線を取得
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
