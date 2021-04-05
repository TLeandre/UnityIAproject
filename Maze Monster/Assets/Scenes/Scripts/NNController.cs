using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNController : MonoBehaviour {


	private Transform transf;

	private Vector3 movDir = Vector3.zero;	//Vecteur de deplacement
	private float rotSpeed = 2f;        	//Vitesse de rotation
	private float speed = 0.8f;           	//Facteur de vitesse
	private float initialVelocity = 0.0f;	//Vitesse initiale
	private float finalVelocity  = 2f;		//Vitesse maximale
	public float currentVelocity  = 0.0f;	//Vitesse actuelle
	private float accelerationRate = 1.2f;	//Taux d'acceleration

	//Gere chacuns des rayons de detection
	public float maxDistance = 5f;
	public float distForward = 35f;
	public float distLeft = 5f;
	public float distRight = 5f;
	public float distDiagLeft = 5f;
	public float distDiagRight = 5f;

	public float fitness = 0f;			//Gere le fitness
	private Vector3 lastPosition;
	private float distanceTraveled;

	public float[] results;				// Resultat du feed forward. Contient deux float, le premier pour l'acceleration, le deuxieme pour l'orientation
	public bool active = true;			//Passe a False si l'agent heurte un mur


	void Start()
	{
		lastPosition = transform.position;
	}
	// Chaque frame :
	void Update ()
	{
		//Tant que l'agent est en vie
		if (active) {
			CharacterController controller = gameObject.GetComponent<CharacterController> ();

			// Si le neural network nous a dit quoi faire... Bah le tichat le fait
			if (results.Length != 0) {

				currentVelocity += (accelerationRate * Time.deltaTime)*results [0];//Applique le resultat du premier neurone d'output sur la vitesse
				currentVelocity = Mathf.Clamp (currentVelocity, initialVelocity, finalVelocity);// Clamp la velocite entre la valeur initiale et finale

				movDir = new Vector3 (0, 0, currentVelocity); //Met la velocite actuelle sous forme de vecteur

				movDir *= speed;
				movDir = transform.TransformDirection (movDir);

				controller.Move (movDir);//Deplace le tichat
				transform.Rotate (0, results [1] * rotSpeed * Time.deltaTime, 0);//Tourne le tichat
			}
			InteractRaycast ();
		}
		distanceTraveled += Vector3.Distance(transform.position, lastPosition);
		lastPosition = transform.position;
		fitness += distanceTraveled/1000;//Augmente le fitness en fonction de la distance parcourue
		fitness -= 0.01f; //Decroit le fitness au long du temps
	}

    // Collisions
    //Si le tichat touche un mur, PAF il est mort
    //Par contre si il touche un checkpoint, il gagne en fitness
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Mur"){
			active = false;
			fitness/=10;
		}
		else if (other.gameObject.tag == "checkpoint"){
			fitness += 4f;
		}
		else if (other.gameObject.tag == "Sortie"){
			fitness += 50f;
			active = false;
		}
	}

	//Gere les rayons pour detecter les murs
	void InteractRaycast()
	{

		transf = GetComponent<Transform>();
		Vector3 playerPosition = transform.position;

		//Cree les directions de chaque rayons
		Vector3 forwardDirection = transf.forward*10;
		Vector3 leftDirection = transf.right * -10;
		Vector3 rightDirection = transf.right*10;
		Vector3 diagLeft = transf.TransformDirection (new Vector3 (maxDistance / 5, 0f, maxDistance / 5));
		Vector3 diagRight = transf.TransformDirection(new Vector3(-maxDistance/5, 0f, maxDistance/5));

		//Cree les rayons
		Ray myRay = new Ray(playerPosition, forwardDirection);
		Ray leftRay = new Ray(playerPosition, leftDirection);
		Ray rightRay = new Ray(playerPosition, rightDirection);
		Ray diagLeftRay = new Ray(playerPosition, diagLeft);
		Ray diagRightRay = new Ray(playerPosition, diagRight);

		//Gere les collisions des rayons et recupere la distance si ils rencontrent quelque chose
		RaycastHit hit;
		if(Physics.Raycast(myRay, out hit, maxDistance*10)&& hit.transform.tag == "Mur")
		{
			distForward = hit.distance;
		}
		if(Physics.Raycast(leftRay, out hit, maxDistance*10)&& hit.transform.tag == "Mur")
		{
			distLeft = hit.distance;
		}
		if(Physics.Raycast(rightRay, out hit, maxDistance*10)&& hit.transform.tag == "Mur")
		{
			distRight = hit.distance;
		}
		if(Physics.Raycast(diagLeftRay, out hit, maxDistance*10)&& hit.transform.tag == "Mur")
		{
			distDiagLeft = hit.distance;
		}
		if(Physics.Raycast(diagRightRay, out hit, maxDistance*10)&& hit.transform.tag == "Mur")
		{
			distDiagRight = hit.distance;
		}
	}
}
