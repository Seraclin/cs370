using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*IMPORTANT So in Unity, it's good practice to not use public variables and instead use
     * [SerializeField]. SerializeField variables can be changed via the Unity Engine itself.
     * So like for example, we can set the speed to another number without having to compile 
     * and run the code -JC

    /*Instead of using 4 if statements, this allows for multiple different control setups
     *like Switch controllers, keyboards, etc without having to code them all separately.
     *Yall might need to download the input manager plugin (forgot the exact name) -JC
     */
    Vector2 input; //Input by players (Example: The "Up" button) -JC
    [SerializeField] float speed;
    [SerializeField] bool isMoving = false; //Used for grid-based movement later on

    /*This is an InEngine layer which is pretty much is everything the player can collide with
     * For example: walls, enemies, etc.
     * Things that are not in this layer are things players cannot collide with
     * Fore example: The sky, clouds, etc -JC
     */
    [SerializeField] LayerMask Collidables;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Built-in Unity functions for player inputs
        input.x = Input.GetAxisRaw("Horizontal"); 
        input.y = Input.GetAxisRaw("Vertical");

        //If input!=Zero then that means a button is being pressed -JC
        if (input != Vector2.zero && !isMoving)
        {
            var targetPos = transform.position;
            targetPos.x += input.x / 16; //I'm assuming the game will be 16X16pixels here.
            targetPos.y += input.y / 16;
            if (CanWalk(targetPos)) {
                /*The reason I use couroutine/IEnumerator instead of just moving on update is because
                 * 1. We are doing grid based movement. Each player should move a pixel at a time
                 * 2. Update runs every frame, which is not as efficient -JC
                 */
                StartCoroutine(Move(targetPos, input.x, input.y));
                 }
        }
    }
    //This just is a way to make each player move 1 pixel at a time -JC
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
    bool CanWalk(Vector3 targetPos) //This checks if there's an object in the way -JC
    {
        /*I use a circular collider instead of a square one so that the players won't get stuck
         * on corners -JC
         */
        if (Physics2D.OverlapCircle(targetPos, 0.4f,Collidables) != null){
            return false;
        }
        
        return true;
    }
}
