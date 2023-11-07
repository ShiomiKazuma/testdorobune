using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _groundCheckRadius = 0.4f;
    [SerializeField] float _groundCheckOffsetY = 0.45f;
    [SerializeField] float _groundCheckDistance = 0.01f;
    [SerializeField] LayerMask _groundMask = ~0;
    RaycastHit _groundhit;
    bool _isGround;
    CapsuleCollider _capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IsGround()
    {
        float extent = Mathf.Max(0, _capsuleCollider.height * 0.5f - _capsuleCollider.radius);
        Vector3 origin = transform.TransformPoint(_capsuleCollider.center + Vector3.down * extent) + Vector3.up * _groundCheckDistance;
        Ray spereCastRay = new Ray(origin, Vector3.down);
        return Physics.SphereCast(spereCastRay, _capsuleCollider.radius, _groundCheckDistance * 2f, _groundMask);
    }
}
