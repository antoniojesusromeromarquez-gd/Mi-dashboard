using UnityEngine;
using TMPro;

public class DeliveryPort : MonoBehaviour
{
    [Header("Objetivos del Nivel")]
    public int rojosNecesarios = 5;
    public int verdesNecesarios = 3;
    public int azulesNecesarios = 2;

    [Header("Referencias UI")]
    public GameObject mensajeVisual; 

    [Header("Progreso Actual")]
    private int rojosEntregados = 0;
    private int verdesEntregados = 0;
    private int azulesEntregados = 0;

    public int GetRojosEntregados(){return rojosEntregados;}
    public int GetVerdesEntregados(){return verdesEntregados;}
    public int GetAzulesEntregados(){return azulesEntregados;}

    private bool playerEnZona = false;
    private PlayerController playerScript;

    void Update()
    {
        // Si el jugador está cerca y pulsa E
        if (playerEnZona && Input.GetKeyDown(KeyCode.E))
        {
            EntregarTodo();
        }
    }

    void EntregarTodo()
    {
        if (playerScript == null) return; // Seguridad por si acaso

        // Entregamos los rojos
        rojosEntregados += playerScript.redData;
        playerScript.redData = 0;

        // Entregamos los verdes
        verdesEntregados += playerScript.greenData;
        playerScript.greenData = 0;

        // Entregamos los azules
        azulesEntregados += playerScript.blueData;
        playerScript.blueData = 0;

        Debug.Log($"DEPURACIÓN: R:{rojosEntregados}/{rojosNecesarios} | V:{verdesEntregados}/{verdesNecesarios} | A:{azulesEntregados}/{azulesNecesarios}");

        VerificarVictoria();
        playerScript.GetComponent<AudioSource>().PlayOneShot(playerScript.sonidoDeposito);
    }

    void VerificarVictoria()
    {
        if (rojosEntregados >= rojosNecesarios &&
            verdesEntregados >= verdesNecesarios &&
            azulesEntregados >= azulesNecesarios)
        {
            // Limpiamos los datos entregados para el siguiente nivel
            rojosEntregados = 0;
            verdesEntregados = 0;
            azulesEntregados = 0;

            // Subimos la dificultad de los objetivos 
            rojosNecesarios += 2;
            verdesNecesarios += 2;
            azulesNecesarios += 2;
            
            // Avisamos al GameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.NextLevel();
            }

            Debug.Log("¡Nivel completado!");
        }
    } // Aquí faltaba esta llave de cierre

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnZona = true;
            playerScript = other.GetComponent<PlayerController>();

            if (mensajeVisual != null) mensajeVisual.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnZona = false;
            playerScript = null;
            if (mensajeVisual != null) mensajeVisual.SetActive(false);
        }
    }
}