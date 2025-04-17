using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class EnemyController : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private Transform _player;
    

    [SerializeField] private float _speed;
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _safeDistance;
    [SerializeField] public float _HP;

    private Transform _target;
    private float _playerDistance;
    private float _targetDistance;

    private Vector3 _direction;
    private bool _isPlayer;
    private bool _isTarget;

    void Start()
    {
        _target = FindStation().transform;
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

        if (_HP <= 0)
        {
            DestroyShip();
        }

        _playerDistance = Vector3.Distance(transform.position, _player.position);
        _targetDistance = Vector3.Distance(transform.position, _target.position);

        if (_playerDistance < _searchRadius || _targetDistance < _searchRadius)
        {
            gameObject.GetComponent<EnemyShootController>().Shoot();
        }

        if (_playerDistance < _searchRadius)
        {
            _isPlayer = true;
            _isTarget = false;
        }
        else
        {
            _isPlayer = false;
        }

        if (_target.gameObject == null || !_isTarget)
        {
            if (!_isPlayer)
            {
                _target = FindStation().transform;
                _isTarget = true;
            }
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
        
        if(_isTarget)
        {
            _direction = _target.position - transform.position;
            if (_targetDistance > _safeDistance *2)
            {
                _rb.AddForce(transform.forward * _speed * Time.fixedDeltaTime);
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(_direction), 0.1f);
    }

    public float GetHP()
    {
        return _HP;
    }

    public void GetDamage(float damage)
    {
        _HP -= damage;
    }

    public void DestroyShip()
    {
        Destroy(gameObject);
    }

    public GameObject FindStation()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1000);
        GameObject newTarget = null;
        float tagetDistance = 100000f;

        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders) 
            {
                if (collider.gameObject.tag == "Station")
                {
                    if (Vector3.Distance(collider.gameObject.transform.position, transform.position) < tagetDistance)
                    {
                        tagetDistance = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                        newTarget = collider.gameObject;
                    }
                }
            }    
        }
        return newTarget;
    }
}
