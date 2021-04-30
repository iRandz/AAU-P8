using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterMovement : MonoBehaviour
{

    public CharacterController controller;
    

    [SerializeField] private float speed;
    [SerializeField] private float gravity = -9.81f;

    private Transform thisTrans;
    private Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, gravity, 0);
        thisTrans = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 moveBy = thisTrans.right * x + thisTrans.forward * z;

        controller.Move(moveBy * (speed * Time.deltaTime));

        controller.Move(velocity);
    }
}
