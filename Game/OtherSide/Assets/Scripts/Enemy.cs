using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour


{

    /*The enemy uses the same walking system as the player -JC*/
    [SerializeField] float speed;
    [SerializeField] bool isMoving = false;
    [SerializeField] LayerMask Collidables;
    Vector2 input;
    
    public GameObject player;
   

    void FixedUpdate()
    { if (player != null) {
            if(player.transform.position.x <this.transform.position.x)
            {
                input.x = -1;
            }
            else
            {
                input.x = 1;

            }

            if (player.transform.position.y < this.transform.position.y)
            {
                input.y = -1;
            }
            else
            {
                input.y = 1;

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
  
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
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


    //The green circle collider around the enemy is the enemy's range -JC
    private void OnTriggerEnter(Collider2D collision)
    {
   
        if (collision.gameObject.tag =="Player")
        {
            player = collision.gameObject;
            
        }
    }

    
}

