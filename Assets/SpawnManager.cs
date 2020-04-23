using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameObject unit;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 2f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        var instantiate = Instantiate(unit, transform);
    }
}
