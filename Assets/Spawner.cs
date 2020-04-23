using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public GameObject unit;
    public float spawnRate = 1f;
    public Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        var spawned = Instantiate(unit, transform.position + new Vector3(10, 0, 10), transform.rotation);
        spawned.GetComponent<Selectable>().SetDestination(destination);
    }
}
