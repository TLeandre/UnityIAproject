using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public Text NBG;
	public Text fitMoyen;
	public Text Population;
	public Text bestFit;
	public Text badFit;

	public GameObject catPrefab;			//Prefab de notre chat

	private bool isTraning = false;
	public int populationSize = 50;			//Nombre d'agent a faire spawner. Gardez un multiple de 4 svp
	public float timer = 10f;				//Temps par generation
	private int generationNumber = 0;		//Numero de la generation actuelle

	private List<NeuralNetwork> nets;		//Liste de neural networks de generation x
	private List<NeuralNetwork> newNets;	//Liste de neural networks de generation x+1
	public List<GameObject> catList = null;	//Liste des chats de notre generation


	private int[] layers = new int[] { 6, 8, 6, 2 }; //Dimension de nos reseaux de neurones

	private float fit=0;					//On calculera la moyenne de fitnes de la generation grace a cette variable

	void Start()
	{
			NBG.text = "Nombre de Generation : " + generationNumber;
			fitMoyen.text = "Fitness Moyen : " + fit;
			Population.text = "Population : " + populationSize;
			bestFit.text = "Meilleur Fitness : 0";
			badFit.text = "Pire Fitness : 0";

	}

	// Met fin a la generation actuelle (cheh)
	void Timer()
	{
		isTraning = false;
	}

	void Update ()
	{
		//Changement de generation
		if (isTraning == false)
		{
			// Si c'est la premiere generation, instancie les tichats
			if (generationNumber == 0)
			{
				InitCatNeuralNetworks();
				CreateCatBodies();
			}
			else
			{
				//Transfer le score de fitness du controleur vers le reseau de neurones
				for(int i=0; i<populationSize; i++)
				{
					NNController script = catList[i].GetComponent<NNController>();
					float fitness = script.fitness;
					nets[i].SetFitness(fitness);
				}

				//Trie les agents pour ne garder que les plus performants
				nets.Sort();
				nets.Reverse();

				//Affiche la moyenne de fitness de la generation
				fit = 0;
				for(int i=0; i<populationSize; i++){
					fit += nets[i].GetFitness();
				}
				fit/=populationSize;
				Debug.Log("Average fitness:");
				Debug.Log(fit);

				majInformations();

				//Instancie la liste de la generation suivante
				List<NeuralNetwork> newNets = new List<NeuralNetwork>();

				//Recupere les plus intelligent de nos tichats
				for (int i = 0; i < populationSize/4; i++)
				{
					NeuralNetwork net = new NeuralNetwork(nets[i]);
					newNets.Add (net);

				}

				//Recupere les plus intelligent de nos tichats et les fait  muter
				for (int i = 0; i < populationSize/4; i++)
				{
					NeuralNetwork net = new NeuralNetwork(nets[i]);
					net.Mutate(0.5f);
					newNets.Add (net);
				}

				//Recupere les plus intelligent de nos tichatset les fait plus muter
				for (int i = 0; i < populationSize/4; i++)
				{
					NeuralNetwork net = new NeuralNetwork(nets[i]);
					net.Mutate(2f);
					newNets.Add (net);

				}

				//Recupere les plus intelligent de nos tichats et leurs defonce le cerveau
				for (int i = 0; i < populationSize/4; i++)
				{
					NeuralNetwork net = new NeuralNetwork(nets[i]);
					net.Mutate(10f);
					newNets.Add (net);
				}

				//Changement d'agents entre les deux generation
				nets = newNets;

			}

			//A la fin du decompte du timer, passe a la generation suivante
			generationNumber++;
			Invoke("Timer",timer);
			CreateCatBodies();


			isTraning = true;
		}

		//------------------FEEDFORWARD
		//Transfer les informations du NNController vers l'input layer du reseau de neurones
		for (int i = 0; i < populationSize; i++)
		{
			NNController script = catList[i].GetComponent<NNController>();

			float[] result;
			float vel = script.currentVelocity;
			float distForward = script.distForward/script.maxDistance;
			float distLeft= script.distLeft/script.maxDistance;
			float distRight= script.distRight/script.maxDistance;
			float distDiagLeft= script.distDiagLeft/script.maxDistance;
			float distDiagRight= script.distDiagRight/script.maxDistance;

			float[] tInputs = new float[]{vel, distForward, distLeft, distRight, distDiagLeft, distDiagRight};
			result = nets[i].FeedForward(tInputs);
			script.results = result;//Envoie le resultat au controleur

		}

        //Les generation ne meurent pas assez vite pour vous? Voila la solution
		if (Input.GetKeyDown("space"))
		{
			generationNumber++;
			isTraning = true;
			Timer ();
			CreateCatBodies();

		}

	}

	//Instancie tout nos tichats
	private void CreateCatBodies()
	{
		//Detruit tout nos tichats (muahaha)
		for (int i = 0; i < catList.Count; i++)
		{
			Destroy(catList[i]);
		}

		//Recree une generation de tichats (mooooooh)
		catList = new List<GameObject>();
		for (int i = 0; i < populationSize; i++)
		{
			GameObject cat = Instantiate(catPrefab, new Vector3(12.5f, 0.1f, -10.5f),Quaternion.Euler(0, Random.Range(0, 380), 0));
			catList.Add(cat);
			catList[i] = cat;
		}
	}

	// Initialise notre liste d'agent
	void InitCatNeuralNetworks()
	{
		nets = new List<NeuralNetwork>();

		for (int i = 0; i < populationSize; i++)
		{
			NeuralNetwork net = new NeuralNetwork(layers);
			net.Mutate(0.5f);
			nets.Add(net);
		}
	}

	void majInformations()
	{
			NBG.text = "Nombre de Generation : " + generationNumber;
			fitMoyen.text = "Fitness Moyen : " + fit;
			bestFit.text = "Meilleur Fitness : " + nets[0].GetFitness();
			badFit.text = "Pire Fitness : " + nets[39].GetFitness();
	}


}
