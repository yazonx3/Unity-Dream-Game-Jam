using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 1f;
    private float verticalInput;
    private float horizontalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        verticalInput = Input.GetAxisRaw("Horizontal");
        horizontalInput = Input.GetAxisRaw("Vertical");

        transform.Translate(Vector3.right * Time.deltaTime * speed * verticalInput);
        transform.Translate(Vector3.forward * Time.deltaTime * speed * horizontalInput);
    }
}
