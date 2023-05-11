using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public GameObject spikePrefab;
    public float spawnInterval = 0.2f;
    public float speed = 5f;

    private Vector3 spawnPoint;
    private Collider ceilingCollider;
    private GameObject[] spikePool;
    private int currentSpike = 0;
    private float timer = 0.0f;

    void Start()
    {
        // Get the collider of the ceiling object
        ceilingCollider = GetComponent<Collider>();

        // Create a pool of spikes
        spikePool = new GameObject[50];
        for (int i = 0; i < spikePool.Length; i++)
        {
            spikePool[i] = Instantiate(spikePrefab);
            spikePool[i].SetActive(false);
        }
    }

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // If the timer has exceeded the spawn interval, spawn a spike
        if (timer >= spawnInterval)
        {
            timer = 0.0f;
            SpawnSpike();
        }
    }

    void SpawnSpike()
    {
        // Get a random spawn point within the ceiling object's bounds
        float x = Random.Range(ceilingCollider.bounds.min.x, ceilingCollider.bounds.max.x);
        float z = Random.Range(ceilingCollider.bounds.min.z, ceilingCollider.bounds.max.z);
        spawnPoint = new Vector3(x, ceilingCollider.bounds.max.y, z);

        // Activate a spike from the pool
        spikePool[currentSpike].SetActive(true);
        spikePool[currentSpike].transform.position = spawnPoint;

        // Move the spike downward
        Rigidbody spikeRigidbody = spikePool[currentSpike].GetComponent<Rigidbody>();
        spikeRigidbody.velocity = Vector3.down * speed;

        // Increment the current spike index
        currentSpike = (currentSpike + 1) % spikePool.Length;
    }


    // If a spike collides with anything, deactivate it
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
