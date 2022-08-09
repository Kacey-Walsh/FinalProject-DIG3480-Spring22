using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RubyController : MonoBehaviour
{
    //================VARIABLES================
    //MOVEMENT
    public float speed = 3.0f;
    float horizontal;
    float vertical;

    Rigidbody2D rigidbody2d;
    Vector2 lookDirection = new Vector2(1,0);

    //ANIMATION
    Animator animator;

    //HEALTH
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

        //BEING HIT
        public float timeInvincible = 2.0f;
        bool isInvincible;
        float invincibleTimer;

    //AMMO
    public int maxCogs = 12;
    public float cogs;
    public Text cogText;

        //PROJECTILE
        public GameObject projectilePrefab;
    
    //SCORE
    public int score;
    public Text scoreText;

        //GAME STATES
            //WINNING
            public GameObject winTextObject;
            public GameObject winAudioTextObject;
            private bool playerWin;

            //LOSING
            public GameObject looseTextObject;
            public GameObject loseAudioTextObject;
            private bool playerLoose;

            //MOVING LEVELS
            public GameObject newLevelTextObject;
            private bool newLevel;


    //AUDIO
    public AudioClip throwSound;
    public AudioClip hitSound;
    AudioSource audioSource;

    public AudioClip winSound;
    public AudioClip looseSound;

    public AudioClip CogSound;
    public AudioClip AppleSound;

    //PARTICLES
    public ParticleSystem healthEffect;
    public ParticleSystem hitEffect;

    //DONT KNOW IF THESE ARE USEFUL
    //private bool loseAudioText;
    //private bool winAudioText;

    //NPC's
        //JAMBI
        public AudioClip JambiSound;

        //CYRUS-RELEVANT
        public int cyrusCollectables;
        public Text cyrusCollectablesText;
        
        
        public AudioClip CyrusSound;
        public bool questActive;


    //================START================
    //Starts at first update
    void Start()
    {
        //MOVEMENT + ANIMATOR
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        //HEALTH + AMMO
        currentHealth = maxHealth;
        cogs = 4;

        //AUDIO
        audioSource = GetComponent<AudioSource>();

        //PARTICLE EFFECTS
        hitEffect.Stop();
        healthEffect.Stop();

        //GAME STATES
        playerWin = false;
        playerLoose = false;
        newLevel = false;

        //TEXT OBJECTS
        winTextObject.SetActive(false);
        looseTextObject.SetActive(false);
        newLevelTextObject.SetActive(false);

        loseAudioTextObject.SetActive(false);
        winAudioTextObject.SetActive(false);

        //cyrusCollectablesText.SetActive(false);

        //NPCS
        questActive = false;
    }

    //================UPDATE================
    //Updates every frame
    void Update()
    {
        //MOVEMENT================
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
            //DIRECTION LOOKING
            if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
    
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        
        //HEALTH================

            //INVINCIBILITY AFTER BEING HIT
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }
        

        //PROJECTILES================
        //LAUNCHING PROJECTILES
            //if more than 1 cog and press 'C'
        if((cogs >= 1) && (Input.GetKeyDown(KeyCode.C)))
        {
            Launch();
        }
        
        //NON-PLAYABLE CHARACTERS================
        //TALKING TO NPCS
            //Talking to Jambi 1 - Robot prompter ("press x")
            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                        PlaySound(JambiSound);
                    }
                }
            }

        //Talking to Jambi 2. - All robots fixed, move to level 2
            if ((score >= 6) && (Input.GetKeyDown(KeyCode.X)))
            {
                            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        transform.position = new Vector3(71, 5f, 0f);
                        Destroy(newLevelTextObject);
                        newLevel = false;
                    }
                }
            }

            //Talking to Cyrus - Robot prompter ("press x")
            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Cyrus"));
                if (hit.collider != null)
                {
                    
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                        PlaySound(CyrusSound);
                        questActive = true;

                    }
                }
            }


            //Talking to Dog - ("press x")
            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Dog"));
                if (hit.collider != null)
                {
                    
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();

                    }
                }
            }


        //GAME STATES
            //MOVING TO SECOND LEVEL
            //Does not auto move the character but unlock's Jambi's second interaction
            if (score == 6)
            {
                newLevelTextObject.SetActive(true);
                newLevel = true;
            }
        
            //WINNING THE GAME
            if ((cyrusCollectables == 3) && (score == 8))
            {
                speed = 0;
                playerWin = true;
                winTextObject.SetActive(true);
                audioSource.Stop();
                PlaySound(winSound);
            }

            //LOSE CONDITION
            if (health == 0)
            {
                speed = 0;
                playerLoose = true;
                PlaySound(looseSound);
                looseTextObject.SetActive(true);
                //audioSource.Stop();
                //PlaySound(looseSound);
            }

        //ENDING GAME

                //QUITTING THE GAME
            if (Input.GetKeyDown("escape"))
            {
                Application.Quit();
            }

            //RESTARTING LEVEL
                //UPON A LOSS
            if(Input.GetKey(KeyCode.R) && playerLoose == true)
            {
                Application.LoadLevel(Application.loadedLevel);
            }

                //UPON A WIN
            if(Input.GetKey(KeyCode.R) && playerWin == true)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
    
        //AUDIO TESTING
        //Just in case
            //Win sound
            if (Input.GetKeyUp(KeyCode.Q))
            {
                audioSource.Stop();
                PlaySound(winSound);
                winAudioTextObject.SetActive(true);
            }
            //Lose Sound
            if (Input.GetKeyUp(KeyCode.E))
            {
                audioSource.Stop();
                PlaySound(looseSound);
                loseAudioTextObject.SetActive(true);
            }

        
    }

    //================FIXED UPDATE================
    //MOVEMENT
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    //================CHANGE HEALTH================
    public void ChangeHealth(int amount)
    {
        //if damage is taken - register hit (play sound/animation/debug)
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            PlaySound(hitSound);
            animator.SetTrigger("Hit");
            Debug.Log("HIT");

            Instantiate(hitEffect, transform.position + Vector3.up *0.5f, Quaternion.identity);
        }

        if (amount > 0)
        {
            Instantiate(healthEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        
        //Healthbar UI
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    //================CYRUS SCORE================
    public void CyrusScore()
    {
        cyrusCollectablesText.text = "Cyrus Apples: " + cyrusCollectables.ToString();
    }

    //================CHANGE SCORE================
    public void ChangeScore()
    {
        score += 1;
        scoreText.text = "Robots Fixed: " + score.ToString();
    }
    //================PROJECTILE================
        //ADDING AMMO
        public void ChangeCogs()
        {
            cogs += 1;
            cogText.text = "Cogs: " + cogs.ToString();
            PlaySound(CogSound);            
        }

        //LAUNCHING AMMO
        void Launch()
        {
            cogs -= 1;
            cogText.text = "Cogs: " + cogs.ToString();
            
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");
            
            PlaySound(throwSound);
        } 
    
    //================?????????================
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}