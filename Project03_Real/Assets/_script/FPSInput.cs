using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FPSInput : MonoBehaviour
{
    [SerializeField] bool _invertVertical = false;
    public event Action<Vector3> MoveInput = delegate { };
    public event Action<Vector3> RotateInput = delegate { };
    public event Action JumpInput = delegate { };
    public event Action<Vector3> BulletInput = delegate { };
    Animator Anim;

    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            
            DetectBulletInput();
        }
        else
        {
            DetectMoveInput();
            DetectJumpInput();
            
        }
        DetectRotateInput();
    }
    void DetectBulletInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BulletInput.Invoke(Camera.main.transform.forward);
            Anim.Play("forward");
        }
        

    }


    void DetectMoveInput()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Anim.SetFloat("blendx", xInput);
        Anim.SetFloat("Blendy", yInput);
        if(xInput != 0 || yInput != 0)
        {

            Vector3 _horizontalMovement = transform.right * xInput;
            Vector3 _forwardMovement = transform.forward * yInput;

            Vector3 movement = (_horizontalMovement + _forwardMovement.normalized);
            MoveInput.Invoke(movement);

        }
    }

    void DetectRotateInput()
    {
        float xInput = Input.GetAxisRaw("Mouse X");
        float yInput = Input.GetAxisRaw("Mouse Y");

        if (_invertVertical == true)
        {

            yInput = -yInput;

        }
        Vector3 rotation = new Vector3(yInput, xInput, 0);

        RotateInput.Invoke(rotation);

    }

    void DetectJumpInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            JumpInput.Invoke();
           Anim.Play("jump");
        }

    }


}