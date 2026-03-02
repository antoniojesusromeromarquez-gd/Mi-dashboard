using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerController player;
    public DeliveryPort portal;

    [Header("Referencias de Texto")]
    public TextMeshProUGUI yellowText;
    public TextMeshProUGUI greenText;
    public TextMeshProUGUI blueText;
    public TextMeshProUGUI healthText;

    [Header("Objetivos del portal")]
    public TextMeshProUGUI goalRedText;
    public TextMeshProUGUI goalGreenText;
    public TextMeshProUGUI goalBlueText;

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            //Actualizo los textos con las variables del Player 
             yellowText.text="Y: "+player.redData;
            greenText.text="G: " +player.greenData;
            blueText.text="B: "+player.blueData;
            healthText.text="Vidas: "+player.health;
        }
      if(portal !=null)
      {
        //Mostramos (Entregados/Necesarios)
        goalRedText.text = "Objetivo Y: " + portal.GetRojosEntregados() + " / " + portal.rojosNecesarios;
        goalGreenText.text = "Objetivo V: " + portal.GetVerdesEntregados() + " / " + portal.verdesNecesarios;
            goalBlueText.text = "Objetivo A: " + portal.GetAzulesEntregados() + " / " + portal.azulesNecesarios;      
      }
    }
}
