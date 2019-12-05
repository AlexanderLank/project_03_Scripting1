using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour
{
    public ParticleSystem doublej;
    public TrailRenderer bulletj;
    public event Action Land = delegate { };
    [SerializeField] AudioClip jump1;
    [SerializeField] AudioClip jump2;
    [SerializeField] AudioClip jump3;
    [SerializeField] GroundDetector _groundDetector = null;
    bool _isGrounded = false;
    int _doublejump = 0;
    
    [SerializeField] GameObject _camera = null;
    Rigidbody _rigidbody = null;
    Vector3 _movementThisFrame = Vector3.zero;
    float _turnAroundThisFrame = 0;
    float _lookAroundThisFrame = 0;
    [SerializeField] float _cameraAngleLimit = 70f;
    private float _currentCameraRotationX = 0;
    // Use this for initialization

    public void Start()
    {
        bulletj.enabled = false;
        
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }
    public void Move(Vector3 requestedMovement)
    {
        _movementThisFrame = requestedMovement;
    }

    public void Turn(float turnAmount)
    {
        _turnAroundThisFrame = turnAmount;
    }

    public void Look(float lookAmount)
    {
        _lookAroundThisFrame = lookAmount;
    }

    public void Jump(float jumpForce)
    {
        if (_isGrounded == false && _doublejump == 1)
            return;
        if (_isGrounded == true && _doublejump == 1)
        {
            _doublejump = 0;
        }
        
        if (_isGrounded == false)
        {
            doublej.Play();
            _doublejump++;
            AudioSource.PlayClipAtPoint(jump2, gameObject.transform.position, 100);
         }
        else
            AudioSource.PlayClipAtPoint(jump1, gameObject.transform.position, 100);
        _rigidbody.AddForce(Vector3.up * jumpForce);
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementThisFrame);
        ApplyTurn(_turnAroundThisFrame);
        ApplyLook(_lookAroundThisFrame);
    }

    public void Bullet(Vector3 direction)
    {
        if (direction == Vector3.zero || _isGrounded == false)
            return;
        AudioSource.PlayClipAtPoint(jump3, gameObject.transform.position, 100);
        _rigidbody.AddForce(direction * 700);
        bulletj.enabled = true;
        StartCoroutine(RemoveAfterSeconds(1, bulletj));
    }
    IEnumerator RemoveAfterSeconds(int seconds, TrailRenderer obj)
    {
        yield return new WaitForSeconds(seconds);
        obj.enabled = false;
    }

    void ApplyMovement(Vector3 moveVector)
    {

        if (moveVector == Vector3.zero)
        {
            return;
        }
        _rigidbody.MovePosition(_rigidbody.position + moveVector);

        _movementThisFrame = Vector3.zero;
    }

    void ApplyTurn(float rotateAmount)
    {

        if (rotateAmount == 0)
            return;

        Quaternion newRotation = Quaternion.Euler(0, rotateAmount, 0);
        _rigidbody.MoveRotation(_rigidbody.rotation * newRotation);
        _turnAroundThisFrame = 0;

    }

    void ApplyLook(float lookAmount)
    {

        if (lookAmount == 0)
            return;

        _currentCameraRotationX -= lookAmount;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraAngleLimit, _cameraAngleLimit);
        _camera.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0, 0);
        _lookAroundThisFrame = 0;

    }

    private void OnEnable()
    {
        _groundDetector.GroundDetected += OnGroundDetected;
        _groundDetector.GroundVanished += OnGroundVanished;
    }
    private void OnDisable()
    {
        _groundDetector.GroundDetected -= OnGroundDetected;
        _groundDetector.GroundVanished -= OnGroundVanished;
    }
    void OnGroundDetected()
    {
        _isGrounded = true;
        _doublejump = 0;
        Land.Invoke();
    }
    void OnGroundVanished()
    {
        _isGrounded = false;
    }
}
