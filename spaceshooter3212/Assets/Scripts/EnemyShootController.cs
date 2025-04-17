using UnityEngine;

public class EnemyShootController : MonoBehaviour
{
    [SerializeField] private float _shootDeltaTime = 1;
    [SerializeField] private float _laserVisibleTime = 0.1f;
    [SerializeField] private float _maxDamage = 10;

    private LineRenderer _lineRenderer1;
    private LineRenderer _lineRenderer2;
    private Ray _ray;
    private RaycastHit _raycastHit;
    private GameObject _shootPoint1;
    private GameObject _shootPoint2;

    private float _shootTime;
    private float _laserTime;
    private bool _isCharged = true;

    void Start()
    {
        _isCharged = true;
        _shootTime = 0;
        _laserTime = 0;

        _shootPoint1 = transform.GetChild(0).gameObject;
        _shootPoint2 = transform.GetChild(1).gameObject;

        _lineRenderer1 = _shootPoint1.GetComponent<LineRenderer>();
        _lineRenderer2 = _shootPoint2.GetComponent<LineRenderer>();

        _lineRenderer1.positionCount = 2;
        _lineRenderer1.startWidth = 0.1f;
        _lineRenderer1.endWidth = 0.1f;

        _lineRenderer2.positionCount = 2;
        _lineRenderer2.startWidth = 0.1f;
        _lineRenderer2.endWidth = 0.1f;
    }

    void Update()
    {
        if (!_isCharged)
        {
            _shootTime += Time.deltaTime;
            _laserTime += Time.deltaTime;
            if (_shootTime >= _shootDeltaTime)
            {
                _isCharged = true;
                _shootTime = 0;
                _laserTime = 0;
            }

            if (_laserTime >= _laserVisibleTime)
            {
                _lineRenderer1.enabled = false;
                _lineRenderer2.enabled = false;
            }
        }
    }

    public void Shoot()
    {
        Vector3 targetPosition = LaserShoot();

        if (targetPosition != Vector3.zero)
        {
            if (!_lineRenderer1.enabled)
            {
                _lineRenderer1.enabled = true;
                _lineRenderer2.enabled = true;
            }

            _lineRenderer1.SetPosition(0, _shootPoint1.transform.position);
            _lineRenderer1.SetPosition(1, targetPosition);

            _lineRenderer2.SetPosition(0, _shootPoint2.transform.position);
            _lineRenderer2.SetPosition(1, targetPosition);
        }
        else
        {
            _lineRenderer1.enabled = false;
            _lineRenderer2.enabled = false;
        }
    }

    private Vector3 LaserShoot()
    {
        _ray = new Ray(_shootPoint1.transform.position, _shootPoint1.transform.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * 1000);

        if (Physics.Raycast(_ray, out _raycastHit))
        {
            if (_raycastHit.collider.gameObject.tag == "Enemy" && _isCharged)
            {
                _isCharged = false;
                _raycastHit.collider.gameObject.GetComponent<EnemyController>().GetDamage(Random.Range(1, _maxDamage));
            }
            else if (_raycastHit.collider.gameObject.tag == "Player" && _isCharged)
            {
                _raycastHit.collider.gameObject.GetComponent<ShipController>().GetDamage(Random.Range(1, _maxDamage));
                _isCharged = false;
            }
            else if (_raycastHit.collider.gameObject.tag == "Station" && _isCharged)
            {
                _raycastHit.collider.gameObject.GetComponentInParent<StationController>().GetDamage(Random.Range(1, _maxDamage));
                _isCharged = false;
            }
            return _raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
