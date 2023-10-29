using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] GameObject _target;
    Transform _targetPos;
    private void Start()
    {
        _targetPos = _target.transform;
    }

    private void Update()
    {
        Vector3 cameraPos = _target.transform.position;
        cameraPos.y = 100f;
        this.transform.position = cameraPos;
    }
}
