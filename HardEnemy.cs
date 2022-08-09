//Cant get smoke to stop
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardEnemy : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public float damagemultiplier;

    public ParticleSystem smokeEffect;
    
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    
    Animator animator;

    
    //SCORE AKA SAVED ROBOTS
    public float fixedRobots;
    private int robotScore = 0;
    
    private RubyController RubyController;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        GameObject RubyControllerObject = GameObject.FindWithTag("RubyController");

        if (RubyControllerObject != null)
        {
            RubyController = RubyControllerObject.GetComponent<RubyController>();

            print("Found the RubyController Script!");
        }

        if (RubyController == null)
        {
            print("Cannot find GameController Script!");
        }

    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-2);
        }

    }
    
    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;

        //SCORE
        if (RubyController != null)
        {
            RubyController.ChangeScore();
            Debug.Log("Change score");
        }
        
        //ANIMATION
        animator.SetTrigger("Fixed");
        
        //PARTICLES
        //need to make sure you drag the child smoke NOT the prefab smoke into the robot
        smokeEffect.Stop();
        Debug.Log("ONE HARD ROBOT COLLECTED");
    }
}