using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    public AudioClip AppleSound;
    private RubyController RubyController;

void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        //PLAYER +1 HEALTH & DESTROY GAME OBJECT
            {
                controller.cyrusCollectables += 1;

                //RubyController.AmmoPouch();        
                //controller.cogs(1);
                Destroy(gameObject);


                //SOUND
                controller.PlaySound(AppleSound);
                


                //DEBUG
                Debug.Log("Apple Aquired");
            }
    }

}
