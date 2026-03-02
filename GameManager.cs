using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para las Corrutinas

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Ajustes de Nivel")]
    public int level = 1;
    public float timeRemaining = 60f;
    public bool gameActive = true;

    [Header("Referencias UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public GameObject mensajeNivelObjeto; // <-- La variable que te faltaba
    public float tiempoMensaje = 2f;

    [Header("Referencias Objetos")]
    public DataSpawner spawner;

    public GameObject panelPausa;

    private AudioSource audioSource;
    public AudioClip sonidoVictoria;
        
void Start()
{
    audioSource = GetComponent<AudioSource>();
}
    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        if (gameActive)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = "Tiempo: " + Mathf.RoundToInt(timeRemaining).ToString();
            }
            else
            {
                GameOver();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
        
       
    }

    public void NextLevel()
    {
        level++;
        timeRemaining += 30f;
        levelText.text = "Nivel: " + level;

        // Escalado de dificultad
        if (spawner != null)
        {
            spawner.spawnRate = Mathf.Max(0.5f, spawner.spawnRate - 0.2f);
            spawner.bugChance = Mathf.Min(60, spawner.bugChance + 5);
        }

        StartCoroutine(MostrarMensajeNivel());

        if(audioSource != null && sonidoVictoria !=null)
        {
            audioSource.PlayOneShot(sonidoVictoria);
        }

    }

    IEnumerator MostrarMensajeNivel()
    {
        if (mensajeNivelObjeto != null)
        {
            // Actualizamos el texto para que diga el nivel actual
            TextMeshProUGUI textoPrompt = mensajeNivelObjeto.GetComponent<TextMeshProUGUI>();
            if(textoPrompt != null) textoPrompt.text = "¡NIVEL " + level + "!";

            mensajeNivelObjeto.SetActive(true);
            yield return new WaitForSeconds(tiempoMensaje);
            mensajeNivelObjeto.SetActive(false);
        }
    }

    void GameOver()
    {
        gameActive = false;
        Debug.Log("¡TIEMPO AGOTADO!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
     public void TogglePause()
        {
            gameActive =!gameActive;
            panelPausa.SetActive(!gameActive);

            Time.timeScale = gameActive ? 1:0;
        }

        public void Reanudar()
        {
            TogglePause();
        }

        //Función para el botón salir 

        public void IrAlMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
}