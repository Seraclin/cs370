using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
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
    [SerializeField] public float speed = 25f;
    [SerializeField] bool isMoving = false;

    public SpriteRenderer ren;
    [SerializeField] public Animator anim;  // for animations to transition
    public Text playerName; //will probably move this to another script (displays player nickname)
    public GameObject playerCam;
   
    //Used for grid-based movement later on

    /*This is an InEngine layer which is pretty much is everything the player can collide with
     * For example: walls, enemies, etc.
     * Things that are not in this layer are things players cannot collide with
     * Fore example: The sky, clouds, etc -JC
     */
    [SerializeField] LayerMask Collidables;
    public PhotonView pv; //for online
 
    

    // Awake runs before start()
    void Awake()
    {
        speed = 1 / speed;
        if(pv.IsMine)
        {
            playerCam.SetActive(true);
        }
        else
        {
            playerCam.SetActive(false);
        }
        anim = GetComponent<Animator>();
        
    }
    public void CollisionForce(int xForce, int yForce)
    {
        //Play animation depending on direction hit
        
    }
    [PunRPC] //syncs animation flips across devices
     void FlipFalse()
    {
        ren.flipX = false;
    }
    [PunRPC]
     void FlipTrue()
    {
        ren.flipX = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Built-in Unity functions for player inputs
        if (pv.IsMine)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        }

        //For flipping animation
        if (input.x > 0)
        {
            pv.RPC("FlipFalse", RpcTarget.OthersBuffered);
            ren.flipX = false;


        }
        if (input.x < 0)
        {
            pv.RPC("FlipTrue", RpcTarget.OthersBuffered);
            ren.flipX = true;

        }
        if(input.y > 0) // facing up
        {
            // TODO: Might add an animation to have it facing up
        }

        //If input!=Zero then that means a button is being pressed -JC
        if (input != Vector2.zero && !isMoving)
        {
            anim.SetBool("isMoving", true); // start moving animations for player
            var targetPos = transform.position;
            targetPos.x += input.x/4; 
            targetPos.y += input.y/4;
            if (CanWalk(targetPos)) {
                /*The reason I use couroutine/IEnumerator instead of just moving on update is because
                 * 1. We are doing grid based movement. Each player should move a pixel at a time
                 * 2. Update runs every frame, which is not as efficient -JC
                 */
                StartCoroutine(Move(targetPos, input.x, input.y));
                 }
        } else if(input == Vector2.zero)
        {
            anim.SetBool("isMoving", false); // stop animations for player movement
        }
    }
    //This just is a way to make each player move 1 pixel at a time -JC
    IEnumerator Move(Vector3 targetPos, float inputx, float inputy)
    {
        isMoving = true;
        float elapsedTime = 0;
     while(elapsedTime < speed)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, elapsedTime/speed);
            elapsedTime += Time.deltaTime;
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
        
        if (Physics2D.OverlapCircle(targetPos-new Vector3(0,0.15f,0), 0.3f,Collidables) != null){
            return false;
        }
        
        return true;
    }
    // Use this to display player circular collider with red circle when selected in scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap Circle to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position-new Vector3(0, 0.15f, 0), 0.3f);
    }
}
