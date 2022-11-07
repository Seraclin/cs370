using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour


{

    /*The enemy uses the same walking system as the player -JC*/
    [SerializeField] float speed;
    [SerializeField] bool isMoving = false;
    [SerializeField] bool isPossessable = false;
    [SerializeField] LayerMask Collidables;
    [SerializeField] Vector2 input;
    [SerializeField] SpriteRenderer ren;

    [SerializeField] public GameObject player;
    [SerializeField] public bool isDead = false;
    [SerializeField] int health = 15;

    public void ChangeHealth(int h)
    {
        Debug.Log(h);
        health = health - h;
        if (health <= 0) Destroy(this.gameObject);
    }
    void FixedUpdate()
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
    }

    IEnumerator Move(Vector3 targetPos, float inputx, float inputy)
    {
        isMoving = true;
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
