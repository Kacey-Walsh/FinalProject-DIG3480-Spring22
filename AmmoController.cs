using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public AudioClip CogSound;
    private RubyController RubyController;

void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        //PLAYER +1 HEALTH & DESTROY GAME OBJECT
                controller.cogs += 4;

                //RubyController.AmmoPouch();        
                //controller.cogs(1);
                Destroy(gameObject);


                //SOUND
                controller.PlaySound(CogSound);
                


                //DEBUG
                Debug.Log("Cogs Aquired");

    }

}
