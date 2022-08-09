using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
            //HEALTH PARTICLES
    public ParticleSystem healthEffect;
    
    void Start()
    {
        healthEffect.Stop();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        healthEffect.Stop();
        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                //PLAYER +1 HEALTH & DESTROY GAME OBJECT
                controller.ChangeHealth(1);
                Destroy(gameObject);

                //SOUND
                controller.PlaySound(collectedClip);
                


                //DEBUG
                Debug.Log("Health Aquired");
            }
        }

    }
}