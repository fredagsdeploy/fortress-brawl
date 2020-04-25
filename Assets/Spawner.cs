using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    private GameObject _unit;
    private float _spawnRate = 1f;
    private Vector3 _destination;
    // Start is called before the first frame update
    void Start()
    {
        var config = GetComponentInParent<SpawnConfiguration>();
        _unit = config.unit;
        _destination = config.destination;
        _spawnRate = config.spawnRate;
        InvokeRepeating(nameof(Spawn), 0f, _spawnRate);
    }
    
    void Spawn()
    {
        var spawned = Instantiate(_unit, transform.position + new Vector3(10, 0, 10), transform.rotation);
        spawned.GetComponent<Movable>().SetDestination(_destination);
    }
}
