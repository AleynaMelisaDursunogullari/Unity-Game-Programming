using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript :MonoBehaviour
{
    private Rigidbody2D playerRB;
    private BoxCollider2D playerCollider;
    private Animator playerAnimator;
    private int WoodCount=0;
     sc_Player playerData;
    AudioSource AudioSource;
    AudioSource coin;
    AudioSource Water;
    private Vector3 respawnPoint;

    private float h_Input;  // Horizontal input (left-right movement)
    private float v_Input;  // Vertical input (up-down movement)
    private bool jump_pressed = false;

    private bool isidle = false;
    private bool isrunning = false;
    private bool isjumping = false;
    private bool isAttacking =false; 
    private bool isLadder=false;

    private bool previousDirection = true;
    private bool directionRight = true;

    [SerializeField] private float movementSpeed = 5.0f;  // Movement speed
    [SerializeField] private float jumpAmount = 5.0f;     // Jump power
    private bool isOnGround = false;
    [SerializeField] private GameObject kafa;

    private int money;
    
      private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(2.4f);  // Saldırı animasyon süresi kadar bekler
        isAttacking = false;
        Debug.Log("Attack Reset"); // Saldırı durumu sıfırlanır
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "gold")
    {
        Debug.Log("Coin ile etkileşimde bulunuldu: " + col.gameObject.name);
        GameManager.Instance.CollectCoin(col.gameObject); // Coin toplama işlemi
        Destroy(col.gameObject); // Coin'i yok et
        coin.Play(); // Coin sesi
        CoinCount.instance.IncreaseCoinCount(1);
        if(kafa!=null){
            Destroy(kafa);
        }
        else{
            return;
        }
    }
          
        
        if(col.gameObject.tag == "Water")
        {
            Water.Play();
        //movementSpeed *= 0.5f; // Hareket hızını yarıya düşür
        
        }
        if (col.gameObject.tag == "Ladder")
        {
            isLadder = true;
        }
       
        
    }
   
    
    void onCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name+":"+this.gameObject.name+":"+Time.time);
    }
    void Awake()
    {
        
         DontDestroyOnLoad(gameObject);
        playerRB = this.GetComponent<Rigidbody2D>();
        playerCollider = this.GetComponent<BoxCollider2D>();
        playerAnimator = this.GetComponent<Animator>();
        playerData=new sc_Player();
        WoodCount=0;
    }

    void Start()
    {
        GameObject playerObject = new GameObject("Player");
        sc_Player player1 = playerObject.AddComponent<sc_Player>();
        player1.printPlayerPos();
        AudioSource = GetComponent<AudioSource>();
       AudioSource[] audioSources = GetComponents<AudioSource>();
    if (audioSources.Length >= 2)
    {
        coin = audioSources[0]; // İlk AudioSource coin sesi için
        Water = audioSources[1]; // İkinci AudioSource su sesi için
    }
    }

    void Update()
    {
        CheckInput();
        CheckStates();

       
        
    }
    
    void CheckStates()
    {
        // Check if on ground and not moving, set idle
        isidle = isOnGround && !isrunning && !isjumping && !isLadder;
        
        // Update animator parameters
        playerAnimator.SetBool("isidle", isidle);
        playerAnimator.SetBool("isrunning", isrunning);
        playerAnimator.SetBool("isjumping", isjumping);
        playerAnimator.SetBool("isLadder", isLadder);
    }

    void CheckInput()
    {
        h_Input = Input.GetAxis("Horizontal");
        v_Input = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            jump_pressed = true;
            isjumping = true;
            isidle = false;
        }

        isrunning = Mathf.Abs(h_Input) > 0.01f;
        if (Input.GetKeyDown(KeyCode.Return) && !isAttacking)
        {
            isAttacking = true;
            playerAnimator.SetTrigger("isAttacking");
            StartCoroutine(ResetAttack());
        }
        if (h_Input < -0.01f)
        {
            directionRight = false;
        }
        else if (h_Input > 0.01f)
        {
            directionRight = true;
        }

       
        // Karakterin kendi dönüşünü uygula, ebeveyn objesinin dönüşünü etkileme
        if (previousDirection != directionRight)
       {
            if (directionRight)
             {
                   // Sağ tarafa dön
                     this.transform.localRotation = Quaternion.Euler(0, 0, 0); // Sağ yön için dönüş
             }
             else
              {
                   // Sola dön
                   this.transform.localRotation = Quaternion.Euler(0, 180, 0); // Sol yön için dönüş
                }
          previousDirection = directionRight;
        }


        
          if (Input.GetKeyDown(KeyCode.UpArrow) && !isLadder)
        {
           
            isLadder = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isLadder)
         {
            isLadder = true;
         }

        else if (isLadder)
        {
            isLadder = false;
        }
    }
  
    void FixedUpdate()
    {
        ApplyMovement();
        CheckGround();
    }

    void ApplyMovement()
    {
        playerRB.velocity = new Vector2(h_Input * movementSpeed, playerRB.velocity.y);
       
        if (jump_pressed && isOnGround)
        {
            jump_pressed = false;
            Jump();
            isjumping = true;
        }
    }

    void Jump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpAmount);
    }

    void CheckGround()
    {
        RaycastHit2D rayCastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + 0.5f);
        isOnGround = rayCastHit;
        
        if (isOnGround && isjumping)  // Landing
        {
            isjumping = false;
        }

        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + 0.5f), isOnGround ? Color.green : Color.red);
    }
}