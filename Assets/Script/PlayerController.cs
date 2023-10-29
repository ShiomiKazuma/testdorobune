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
    float _grapSpeed;
    /// <summary>グラップリングの届く距離/// </summary>
    [SerializeField] float _grapDis = 50.0f;
    [SerializeField] float _jumpPower = 3.0f;
    [SerializeField] Transform _debugHitPointTransform;
    [SerializeField] Transform _grapShotTransform;
    /// <summary>グラップの長さ </summary>
    float _grapSize;
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
    /// <summary>グラップの速度の最大値/// </summary>
    [SerializeField] float _grapSpeedMax = 40f;
    /// <summary>グラップの速度の最小値/// </summary>
    [SerializeField] float _grapSpeedMin = 5f;
    bool _jumping = false;
    State _state;
    [SerializeField] ParticleSystem _particleSystem;
    //接地判定に必要な変数
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
        //カーソルを出さないようにする
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
            //プレイヤーの移動処理
            if (IsGround)
            {
                vec = Vector3.ProjectOnPlane(vec, _normalVector);
                if (_jumping)
                {
                    vec.y = _jumpPower;
                    IsGround = false;
                    Debug.Log("ジャンプ");
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
        //グラップリングの方向を決める
        Vector3 grapDir = (_grapShotPos - transform.position).normalized;
        //遠くなればなるほど早くして、近づくほど原則する
        _grapSpeed = Mathf.Clamp(Vector3.Distance(transform.position, _grapShotPos), _grapSpeedMin, _grapSpeedMax);
        float _grapSpeedMultiplier = 2f;
        //移動させる
        _rb.velocity =  grapDir * _grapSpeed * _grapSpeedMultiplier;
        //終点についた時の処理
        if(Vector3.Distance(transform.position, _grapShotPos) < _reachGrapPos)
        {
            StopGrapShot();
        }

        if(Input.GetMouseButtonDown(0))
        {
            //グラップリングキャンセル
            StopGrapShot();
        }

        if (Input.GetButtonDown("Jump"))
        {
            //グラップリングキャンセル
            StopGrapShot();
        }
    }

    //接地判定
    //bool CheckGround()
    //{
        
    //}

    private void OnCollisionStay(Collision collision)
    {

        //衝突した面の接触した点における法線を取得
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
