using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class EnemyController : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _target;

    [SerializeField] private float _speed;
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _safeDistance;

    private float _playerDistance;
    private float _targetDistance;

    private Vector3 _direction;
    private bool _isPlayer;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.drag = 1;
        _rb.angularDrag = 1;

        _speed = 1000f;
        _searchRadius = 100f;
        _safeDistance = 20f;
    }

    void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, _player.position);
        _targetDistance = Vector3.Distance(transform.position, _target.position);

        if (_playerDistance < _searchRadius)
        {
            _isPlayer = true;
        }
        else
        {
            _isPlayer = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isPlayer) 
        {
            _direction = _player.position - transform.position;
            if (_playerDistance > _safeDistance)
            {
                _rb.AddForce(transform.forward * _speed * Time.fixedDeltaTime);
            }
        }
        else
        {
            _direction = _target.position - transform.position;
            if (_targetDistance > _safeDistance)
            {
                _rb.AddForce(transform.forward * _speed * Time.fixedDeltaTime);
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(_direction), 0.1f);
    }
}
