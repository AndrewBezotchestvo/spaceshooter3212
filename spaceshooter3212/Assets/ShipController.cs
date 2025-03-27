using UnityEngine;
using System;

public class ShipController : MonoBehaviour
{
    // shift ускорение
    // w, s - тангаж
    // a, d - крен
    // q, e - рыскание

    [SerializeField] private float _motorForce = 1000f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _maxSpeed = 50f;

    private Rigidbody _rb;

    private float _yawInput; //рыскание 
    private float _rollInput; //крен
    private float _pitchInput; //тангаж w s

    private bool _isMoving;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        _pitchInput = Input.GetAxis("Vertical");
        _rollInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.E))
        {
            _yawInput = 1f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _yawInput = -1f;
        }
        else 
        {
            _yawInput = 0f;
        }

        if (Input.GetKey(KeyCode.F))
        {
            Stybilyze();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isMoving = true;
        }
        else
        { 
            _isMoving = false; 
        }

    }

    private void FixedUpdate()
    {
        _rb.AddTorque(transform.up * _yawInput * _rotationSpeed * Time.fixedDeltaTime, 
            ForceMode.VelocityChange);
        _rb.AddTorque(transform.forward * _rollInput * _rotationSpeed * Time.fixedDeltaTime,
            ForceMode.VelocityChange);
        _rb.AddTorque(transform.right * _pitchInput * _rotationSpeed * Time.fixedDeltaTime,
            ForceMode.VelocityChange);

        if (_isMoving)
        {
            _rb.drag = 3;
            if (_rb.velocity.magnitude <= _maxSpeed)
            {
                _rb.AddForce(transform.forward * _motorForce);
            } 
        }
        else
        {
            if(_rb.velocity.magnitude > 0)
            {
                _rb.drag = 20;
            }
            else
            {
                _rb.drag = 3;
            }
        }
    }

    private void Stybilyze()
    {
        if (transform.rotation.y > 1)
        {
            _rb.AddTorque(transform.up * -_rotationSpeed * Time.deltaTime * 0.1f,
            ForceMode.VelocityChange);
        }
        else if (transform.rotation.y < -1)
        {
            _rb.AddTorque(transform.up * _rotationSpeed * Time.deltaTime * 0.1f,
            ForceMode.VelocityChange);
        }

        if (transform.rotation.x > 1)
        {
            _rb.AddTorque(transform.right * -_rotationSpeed * Time.deltaTime * 0.1f,
            ForceMode.VelocityChange);
        }
        else if (transform.rotation.x < -1)
        {
            _rb.AddTorque(transform.right * _rotationSpeed * Time.deltaTime * 0.01f,
            ForceMode.VelocityChange);
        }

        if (transform.rotation.z > 1)
        {
            _rb.AddTorque(transform.forward * -_rotationSpeed * Time.deltaTime * 0.01f,
            ForceMode.VelocityChange);
        }
        else if (transform.rotation.z < -1)
        {
            _rb.AddTorque(transform.forward * _rotationSpeed * Time.deltaTime * 0.01f,
            ForceMode.VelocityChange);
        }
    }
}
