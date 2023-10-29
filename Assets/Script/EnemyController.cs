using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform _target;
    NavMeshAgent _agent;
    [SerializeField] float _enemySpeed = 5.0f;
    [SerializeField] float _enemySerchDis = 10.0f;
    [SerializeField] float _enemyAttackDis = 2.0f;
    [SerializeField] float _enemyAttackInterval = 1.0f;
    [SerializeField] UnityEvent _onHit;
    bool _isEnemyAttack = false;
    float _enemyAttackCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _enemySpeed;
        _agent.stoppingDistance = _enemyAttackDis;
    }

    // Update is called once per frame
    void Update()
    {
        //�U���Ԋu�𒲐�����
        if (_isEnemyAttack)
        {
            _enemyAttackCount += Time.deltaTime;
            if( _enemyAttackCount > _enemyAttackInterval )
            {
                _isEnemyAttack = false;
            }
        }
        //���͈͂ɓ�������ǂ�������
        if(Vector3.Distance(transform.position, _target.transform.position) <= _enemySerchDis)
        {
            if(Vector3.Distance(transform.position, _target.transform.position) <= _enemyAttackDis + 0.1f�@&& !_isEnemyAttack)
            {
                _onHit.Invoke();
                //��莞�ԍU���ł��Ȃ��悤�ɂ���
                _isEnemyAttack =true;
                _enemyAttackCount = 0;
            }
            _agent.destination = _target.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _enemySerchDis);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyAttackDis);
    }
}
