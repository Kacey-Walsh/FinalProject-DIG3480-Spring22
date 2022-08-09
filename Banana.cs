using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private RubyController RubyController;

void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        //PLAYER +1 HEALTH & DESTROY GAME OBJECT
            {
                controller.speed = 5.0f;
                Destroy(gameObject);


                //DEBUG
                Debug.Log("Banana Aquired");
            }
    }

}
