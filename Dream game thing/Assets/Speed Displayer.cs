using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedDisplayer : MonoBehaviour
{
    public Rigidbody Player;
    public Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      text.text = "Speed: "  + Mathf.Round(Player.velocity.magnitude).ToString();
    }
}

    