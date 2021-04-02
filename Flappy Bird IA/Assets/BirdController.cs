using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdController : MonoBehaviour
{
    // permet le mouvement du Bird
    public int force = 280;
    private Rigidbody2D rb;

    //parametre propra à chaque joueur
    private Transform transf;

    /*//Gere les rayons qui detecte le haut et le bas du tuyan
    public float distUp = 0f;
    public float distDown = 0f;
    public Transform targetUp;
    public Transform targetDown;*/
    //Gere chacuns des rayons de detection
  	public float maxDistance = 10f;
  	public float distForward = 0f;
  	public float distLeft = 0f;
  	public float distRight = 0f;
  	public float distDiagLeft = 0f;
  	public float distDiagRight = 0f;

    //Gere le fitness
    public float fitness = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * force);
        }

        //CalculDist();
        InteractRaycast();
    }

    void OnCollisionEnter2D()
    {
        //Dead
        SceneManager.LoadScene("Game");
    }

    /*void CalculDist()
    {
        GameObject[] Up = GameObject.FindGameObjectsWithTag("Up");
        distUp = Vector3.Distance(transform.position, Up.transform.position);

        GameObject[] Down = GameObject.FindGameObjectsWithTag("Down");
        distDown = Vector3.Distance(transform.position, Down.transform.position);

        Debug.Log(" dist Up : " + distUp);
        Debug.Log(" dist down : " + distDown);
    }*/

    void InteractRaycast()
    {
      transf = GetComponent<Transform>();
      Vector2 playerPosition = transform.position;

      //Cree les directions de chaque rayons
  		Vector2 forwardDirection = transf.right;
      Vector2 diagLeft1 = transf.TransformDirection (new Vector2 (1, 1));
  		Vector2 diagRight1 = transf.TransformDirection(new Vector2(1, -1));
  		Vector2 diagLeft2 = transf.TransformDirection (new Vector2 (2, 2));
  		Vector2 diagRight2 = transf.TransformDirection(new Vector2(2, -2));

  		//Cree les rayons
  		Ray myRay = new Ray(playerPosition, forwardDirection);
  		Ray leftRay = new Ray(playerPosition, diagLeft1);
  		Ray rightRay = new Ray(playerPosition, diagRight1);
  		Ray diagLeftRay = new Ray(playerPosition, diagLeft2);
  		Ray diagRightRay = new Ray(playerPosition, diagRight2);

      //Gere les collisions des rayons et retourne la distance si ils rencontrent quelque chose
  		RaycastHit hit;
  		 if (Physics.Raycast(myRay, out hit, maxDistance*10)&& hit.transform.tag == "Walls")
          {
  			distForward = hit.distance;
          }
  		if(Physics.Raycast(leftRay, out hit, maxDistance*10))
  		{
  			distLeft = hit.distance;
  		}
  		if(Physics.Raycast(rightRay, out hit, maxDistance*10))
  		{
  			distRight = hit.distance;
  		}
  		if(Physics.Raycast(diagLeftRay, out hit, maxDistance*10))
  		{
  			distDiagLeft = hit.distance;
  		}
  		if(Physics.Raycast(diagRightRay, out hit, maxDistance*10))
  		{
  			distDiagRight = hit.distance;
  		}

      //Affiche les rayons
  		Debug.DrawRay(transform.position, forwardDirection * maxDistance, Color.green);
  		Debug.DrawRay(transform.position, diagLeft1 * maxDistance, Color.green);
  		Debug.DrawRay(transform.position, diagRight1 * maxDistance, Color.green);
  		Debug.DrawRay(transform.position, diagLeft2 * maxDistance, Color.green);
  		Debug.DrawRay(transform.position, diagRight2 * maxDistance, Color.green);

    }
}
