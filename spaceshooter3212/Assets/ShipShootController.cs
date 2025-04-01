using UnityEngine;

public class ShipShootController : MonoBehaviour
{
    private LineRenderer _lineRenderer1;
    private LineRenderer _lineRenderer2;
    private LineRenderer _lineRenderer3;

    private Ray _ray;
    private RaycastHit _raycastHit;

    private GameObject _shootPoint1;
    private GameObject _shootPoint2;
    private GameObject _shootPoint3;

    void Start()
    {
        _shootPoint1 = transform.GetChild(0).gameObject;
        _shootPoint2 = transform.GetChild(1).gameObject;
        _shootPoint3 = transform.GetChild(2).gameObject;

        _lineRenderer1 = _shootPoint1.GetComponent<LineRenderer>();
        _lineRenderer2 = _shootPoint2.GetComponent<LineRenderer>();
        _lineRenderer3 = _shootPoint3.GetComponent<LineRenderer>();

        _lineRenderer1.positionCount = 2;
        _lineRenderer1.startWidth = 0.1f;
        _lineRenderer1.endWidth = 0.1f;

        _lineRenderer2.positionCount = 2;
        _lineRenderer2.startWidth = 0.1f;
        _lineRenderer2.endWidth = 0.1f;

        _lineRenderer3.positionCount = 2;
        _lineRenderer3.startWidth = 0.1f;
        _lineRenderer3.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 targetPosition = LaserShoot();

            if (targetPosition != Vector3.zero)
            {
                _lineRenderer1.enabled = true;
                _lineRenderer1.SetPosition(0, _shootPoint1.transform.position);
                _lineRenderer1.SetPosition(1, targetPosition);

                _lineRenderer2.enabled = true;
                _lineRenderer2.SetPosition(0, _shootPoint2.transform.position);
                _lineRenderer2.SetPosition(1, targetPosition);

                _lineRenderer3.enabled = true;
                _lineRenderer3.SetPosition(0, _shootPoint3.transform.position);
                _lineRenderer3.SetPosition(1, targetPosition);
            }
        }
        else
        {
            _lineRenderer1.enabled = false;
            _lineRenderer2.enabled = false;
            _lineRenderer3.enabled = false;
        }
    }

    private Vector3 LaserShoot()
    {
        _ray = new Ray(_shootPoint1.transform.position, _shootPoint1.transform.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * 1000);

        if (Physics.Raycast(_ray, out _raycastHit))
        {
            if (_raycastHit.collider.gameObject.tag == "Enemy")
            {
                Destroy(_raycastHit.collider.gameObject);
            }

            return _raycastHit.point;
        }
        else
        {
            return transform.position + transform.forward * 100;
        }
    }
}
