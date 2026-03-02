using UnityEngine;

public class DataSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] dataPrefabs;
    public GameObject bugPrefab;

    [Header("Configuración de dificultad")]
    public float spawnRate = 2f;      // Segundos entre caídas
    [Range(0, 100)] 
    public int bugChance = 10;        // % de probabilidad de enemigo
    public float spawnRangeX = 8f;    // Ancho del área de caída

    private float timer;              // Temporizador manual

    void Update()
    {
        // Solo spawneamos si el juego está activo según el GameManager
        if (GameManager.instance != null && !GameManager.instance.gameActive) return;

        timer += Time.deltaTime;

        // Cuando el temporizador llega al spawnRate, soltamos algo
        if (timer >= spawnRate)
        {
            SpawnObject();
            timer = 0; // Reiniciamos el reloj
        }
    }

    void SpawnObject()
    {
        // Calculamos posición aleatoria en X
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);

        // Decidimos si cae un Bug o un Dato
        if (Random.Range(0, 100) < bugChance)
        {
            if (bugPrefab != null)
                Instantiate(bugPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            if (dataPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, dataPrefabs.Length);
                Instantiate(dataPrefabs[randomIndex], spawnPos, Quaternion.identity);
            }
        }
    }
}
