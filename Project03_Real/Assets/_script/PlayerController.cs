using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FPSInput))]
[RequireComponent(typeof(FPSMotor))]

public class PlayerController : MonoBehaviour
{
    //FPSInput _Input = null;
    [SerializeField] float _moveSpeed = .1f;
    [SerializeField] float _turnSpeed = 6f;
    [SerializeField] float _jumpStrength = 10;
    FPSInput _input = null;
    FPSMotor _motor = null;
    public ParticleSystem prefab;
    public bool truth = false;
    // Use this for initialization
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        _input = GetComponent<FPSInput>();
        _motor = GetComponent<FPSMotor>();
        

    }

    private void OnEnable()
    {
        _input.MoveInput += OnMove;
        _input.RotateInput += OnRotate;
        _input.JumpInput += OnJump;
        _input.BulletInput += OnBullet;
    }

    private void OnDisable()
    {
        _input.BulletInput -= OnBullet;
        _input.MoveInput -= OnMove;
        _input.RotateInput -= OnRotate;
        _input.JumpInput -= OnJump;
    }

    void OnMove(Vector3 movement)
    {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _motor.Move(movement * _moveSpeed * 2);
            }
            else
            {
                _motor.Move(movement * _moveSpeed);
            }
    }

    void OnRotate(Vector3 rotation)
    {
        _motor.Turn(rotation.y * _turnSpeed);
        _motor.Look(rotation.x * _turnSpeed);
    }

    void OnJump()
    {
        _motor.Jump(_jumpStrength);
    }
    
    void OnBullet(Vector3 direction)
    {

        _motor.Bullet(direction);
    }
 
    
}
