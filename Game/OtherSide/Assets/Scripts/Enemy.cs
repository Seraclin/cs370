using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy : MonoBehaviourPunCallbacks


{

    /*The enemy uses the same walking system as the player -JC*/
    [SerializeField] public float speed;
    [SerializeField] bool isMoving = false;
    [SerializeField] public bool isPossessable = false;
    [SerializeField] public bool isDead = false;
    [SerializeField] bool isRanged;
    [SerializeField] LayerMask Collidables;
    [SerializeField] Vector2 input;
    [SerializeField] SpriteRenderer ren;
    [SerializeField] Animator anim;  // for animations to transition


    [SerializeField] public GameObject player;
    [SerializeField] public int maxhealth = 15;
    [SerializeField] public float damageCoef = 1f;
    [SerializeField] bool invincibility;
    [SerializeField] float invincibilityTime;
    [SerializeField] Collider2D col;
    [SerializeField] float deathTime;

    [SerializeField] float distancing; // for ranged enemy only
    [SerializeField] public int health;
    public PhotonView pv;
    [SerializeField] Vector3 ogPos;

    [SerializeField] GameObject particleHeal; // particle for healing
    [SerializeField] GameObject particlePossession; // particle for possession

    [SerializeField] bool isBoss = false; // if true, don't allow possession, otherwise false allows possesion



    void Start()
    {
   
        ogPos = this.transform.position;
        ren = gameObject.GetComponent<SpriteRenderer>();
        health = maxhealth;
        FindObjectOfType<AudioManager>().Play("ghostApproach");
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    void RemoveInvincibility()
    {
        col.enabled = true;
        invincibility = false;
    }

    public bool ChangeHealth(int h)
    {
        bool tookdamage = false; // if we take damage we then true, else false
        if (h < 0 && invincibility == false) // enemy isn't invincible so it takes damage
        {
            health += h;
            tookdamage = true; // health was changed
            invincibility = true;

            col.enabled = false;
            Invoke("RemoveInvincibility", invincibilityTime);
        }
        else if (h > 0)// enemy is healed
        {
            health += h;
            if (health > maxhealth)
            {
                health = maxhealth;
            }

            if (particleHeal != null && h > 0)
            {
                // heal particle effect, only when healing, i.e. h is greater than zero/positive
                GameObject phit = Instantiate(particleHeal, gameObject.transform);
                phit.GetComponent<ParticleSystem>().Play();
                Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
            }
            tookdamage = false;
        }


        if (health <= 0) // enemy dead
        {
            GameObject hitbox = this.gameObject.GetComponent<Transform>().GetChild(0).gameObject;
            GameObject df = this.gameObject.GetComponent<Transform>().GetChild(1).gameObject;
            isDead = true;
            isMoving = false;
            anim.SetBool("isMoving", false); // stop animations for enemy


            player = null;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            hitbox.GetComponent<BoxCollider2D>().enabled = false;
            hitbox.GetComponent<EnemyHitbox>().enabled = false;
            df.GetComponent<EnemyDetectionField>().enabled = false;
            df.GetComponent<CircleCollider2D>().enabled = false;

            // don't allow possession of bosses
            if (isBoss)
            {
                isPossessable = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false; // turn off collider so player doesn't get stuck

            }
            else // valid enemy possession
            {
                // enemy is made into a possessable entity
                if (particlePossession != null)
                {
                    // possession particle effect
                    // public static Object Instantiate(Object original, Transform parent);, makes a child of parent, should be destroyed with parent
                    GameObject phit = Instantiate(particlePossession, gameObject.transform);
                    phit.GetComponent<ParticleSystem>().Play();
                }
                isPossessable = true;
                ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, .5f);
                anim.enabled = false; // stop animations playing after dead, and indicate possession

            }


            pv.RPC("DestroyOnline", RpcTarget.OthersBuffered);
           
           
            Invoke("DestroyEnemy", deathTime);
            
        }
        return tookdamage;
    }

    [PunRPC] void DestroyOnline()
    {
        Destroy(this.gameObject);
    }
    void DestroyEnemy()
    {
        PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);

    }
    void FixedUpdate()
    { 
        if (!isRanged)
        {
            if (player != null)
            {
                if (player.GetComponent<SpriteRenderer>().color.a > 0.8f)
                {
                    if (player.transform.position.x < this.transform.position.x)
                    {
                        input.x = -1;
                        ren.flipX = true;

                    }
                    else
                    {
                        input.x = 1;
                        ren.flipX = false;

                    }

                    if (player.transform.position.y < this.transform.position.y)
                    {
                        input.y = -1;
                    }
                    else
                    {
                        input.y = 1;

                    }
                }

                if (input != Vector2.zero && !isMoving)
                {
                    var targetPos = transform.position;
                    targetPos.x += input.x / 16;
                    targetPos.y += input.y / 16;
                    if (CanWalk(targetPos))
                    {

                        StartCoroutine(Move(targetPos, input.x, input.y));
                    }
                }
            }
        } else
        {
            if (player != null)
            {
                float dis;
                dis = Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Enemy>().player.transform.position);

                if (player.GetComponent<SpriteRenderer>().color.a > 0.8f && dis < distancing)
                {
                    if (player.transform.position.x < this.transform.position.x)
                    {
                        input.x = 1;
                        ren.flipX = true;

                    }
                    else
                    {
                        input.x = -1;
                        ren.flipX = false;

                    }

                    if (player.transform.position.y < this.transform.position.y)
                    {
                        input.y = 1;
                    }
                    else
                    {
                        input.y = -1;

                    }
                } else if (player.GetComponent<SpriteRenderer>().color.a > 0.8f && dis > distancing)
                {
                    if (player.transform.position.x < this.transform.position.x)
                    {
                        input.x = -1;
                        ren.flipX = true;

                    }
                    else
                    {
                        input.x = 1;
                        ren.flipX = false;

                    }

                    if (player.transform.position.y < this.transform.position.y)
                    {
                        input.y = -1;
                    }
                    else
                    {
                        input.y = 1;

                    }
                } else
                {
                    input = Vector2.zero;
                }

                if (input != Vector2.zero && !isMoving)
                {
                    var targetPos = transform.position;
                    targetPos.x += input.x / 16;
                    targetPos.y += input.y / 16;
                    if (CanWalk(targetPos))
                    {

                        StartCoroutine(RangedMove(targetPos, input.x, input.y));
                    }
                }
            }
        }
    }
 
    IEnumerator Move(Vector3 targetPos, float inputx, float inputy)
    {

        isMoving = true;
        anim.SetBool("isMoving", true);  // play animations for enemy_run

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed*Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        anim.SetBool("isMoving", false);  // stop animations for enemy_run

    }

    IEnumerator RangedMove(Vector3 targetPos, float inputx, float inputy)
    {

        
        isMoving = true;
        anim.SetBool("isMoving", true);  // play animations for enemy_run

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        anim.SetBool("isMoving", false);  // stop animations for enemy_run

    }


    bool CanWalk(Vector3 targetPos)
    {
       

        if (Physics2D.OverlapCircle(targetPos, 0.4f, Collidables) != null)
        {
           
            return false;
        }

        return true;
    }


    

    
}

