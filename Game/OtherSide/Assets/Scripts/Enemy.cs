using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour


{

    /*The enemy uses the same walking system as the player -JC*/
    [SerializeField] float speed;
    [SerializeField] bool isMoving = false;
    [SerializeField] public bool isPossessable = false;
    [SerializeField] public bool isDead = false;
    [SerializeField] bool isRanged;
    [SerializeField] LayerMask Collidables;
    [SerializeField] Vector2 input;
    [SerializeField] SpriteRenderer ren;

    [SerializeField] public GameObject player;
    [SerializeField] int health = 15;

    void Start()
    {
        ren = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeHealth(int h)
    {
        Debug.Log(h);
        health = health - h;
        if (health <= 0)
        {
            GameObject df = this.gameObject.GetComponent<Transform>().GetChild(1).gameObject;
            isPossessable = true;
            isDead = true;
            isMoving = false;
            player = null;
            df.GetComponent<EnemyDetectionField>().enabled = false;
            df.GetComponent<CircleCollider2D>().enabled = false;
            ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, .2f);
            Destroy(this.gameObject, 10.0f);
        }
    }
    void FixedUpdate()
    { 
        if (!isRanged)
        {
            if (player != null)
            {
                if (player.GetComponent<SpriteRenderer>().color.a > 0.5f)
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
                if (player.GetComponent<SpriteRenderer>().color.a > 0.5f)
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
        Debug.Log((targetPos - transform.position).sqrMagnitude);
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed*Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    IEnumerator RangedMove(Vector3 targetPos, float inputx, float inputy)
    {
        isMoving = true;
        Debug.Log((targetPos - transform.position).sqrMagnitude);
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
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

