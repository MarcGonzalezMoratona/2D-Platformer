using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnScript : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public GameObject BulletPrefab;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private int Health = 5;
    private float LastShot;
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        
        if(Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if(Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); 
        Animator.SetBool("Running", Horizontal != 0.0f);
        
        if(Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        } 
        else 
        {
            Grounded = false;
        }
        
        if(Grounded)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
            
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }

        }
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > LastShot + 0.25f)
        {
            Shoot();
            LastShot = Time.time;
        }
    }
    
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }    

    private void Shoot()
    {
    	Vector3 direction;
    	
    	if(transform.localScale.x == 1.0f) direction = Vector2.right;
    	else direction = Vector2.left;
    
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
    	bullet.GetComponent<BulletScript>().SetDirection(direction);
    }    

    private void FixedUpdate()
    {
    	Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }
    
    public void Hit()
    {
        Health = Health - 1;
        if(Health == 0) Destroy(gameObject);
    }
}
