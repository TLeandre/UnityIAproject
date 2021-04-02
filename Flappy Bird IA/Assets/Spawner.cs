using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public GameObject solPrefab;
    public float delai = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPipe", 0f, delai);
        InvokeRepeating("SpawnSol", 0f, delai);
    }

    void SpawnPipe()
    {
        GameObject pipe = Instantiate(pipePrefab);
        Destroy(pipe, 5f);
    }
    void SpawnSol()
    {
        GameObject sol = Instantiate(solPrefab);
        Destroy(sol, 3f);
    }
}
