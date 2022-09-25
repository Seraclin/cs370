using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 input;
    
    [SerializeField] float speedMultiplier;
    [SerializeField] float speed = 2;
    [SerializeField] float topSpeed;
    [SerializeField] bool isMoving = false;
    [SerializeField] LayerMask Collidables;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        if (input != Vector2.zero && !isMoving)
        {
            var targetPos = transform.position;
            targetPos.x += input.x / 16;
            targetPos.y += input.y / 16;
            if (CanWalk(targetPos))
            StartCoroutine(Move(targetPos, input.x, input.y));
        }
    }
    IEnumerator Move(Vector3 targetPos, float inputx, float inputy)
    {
        isMoving = true;
     while((targetPos-transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
    bool CanWalk(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.5f,Collidables) != null){
            //(targetPos-new Vector3(0,0.25f,0), 1f, Collidables)!=null) //set to 0.25 and 0.5 originally

            Debug.Log("test");
            return false;
        }
        
        return true;
    }
}
