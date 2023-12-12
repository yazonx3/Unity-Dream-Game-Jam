using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public Transform player;
    public float sensivity = 1f;
    private float horizontal;
    private float vertical;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        horizontal += sensivity * Input.GetAxisRaw("Mouse X");
        vertical += sensivity * Input.GetAxisRaw("Mouse Y");
        transform.eulerAngles = new Vector3(-vertical, horizontal, 0);

        player.transform.eulerAngles = new Vector3(0, horizontal, 0);
    }
}
