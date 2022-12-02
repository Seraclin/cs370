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

    [SerializeField] float deathTime;

    [SerializeField] float distancing; // for ranged enemy only
    [SerializeField] public int health;
    public PhotonView pv;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] Vector3 ogPos;

    [SerializeField] GameObject particleHeal; // particle for healing

    void Start()
    {
        enemySpawner = this.transform.parent.gameObject;
        ogPos = this.transform.position;
        ren = gameObject.GetComponent<SpriteRenderer>();
        health = maxhealth;
        FindObjectOfType<AudioManager>().Play("ghostApproach");
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    public bool ChangeHealth(int h)
    {
        bool tookdamage = false; // if we take damage we then true, else false
        health = health + h;
        if (health > maxhealth)
        {
            health = maxhealth;
        }

        if(h < 0) // incoming damage
        {
            tookdamage = true;
        } else if(h > 0) // healing
        {
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
            isPossessable = true;
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
            ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, .5f);
            
            pv.RPC("DestroyOnline", RpcTarget.OthersBuffered);
            if (PhotonNetwork.IsMasterClient)
            {
   
                EnemySpawner es = enemySpawner.GetComponent<EnemySpawner>();
                
                
            }
           
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

