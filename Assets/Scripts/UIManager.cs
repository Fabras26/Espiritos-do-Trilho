using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI textoDescricao;

    public TextMeshProUGUI textoItemColetado;

    public GameObject handCursor;

    public GameObject empurrarCaixaHud;

    public GameObject bolinha;
    public GameObject trancado;

    public GameObject balao;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SetHandCursor(bool state)
    {
        handCursor.SetActive(state);
        bolinha.SetActive(!state);
    }
    public void SetHudEmpurrar(bool state)
    {
        empurrarCaixaHud.SetActive(state);
        bolinha.SetActive(!state);

    }
    public void SetHudTrancada(bool state)
    {
        trancado.SetActive(state);
        bolinha.SetActive(!state);
    }

    public void SetTextoDescricao(string texto)
    {
        textoDescricao.text = texto;
    }
    public void TextoColetado()
    {
        StartCoroutine(FadingText());
    }
    public void SetBalao(bool state)
    {
        balao.SetActive(state);
        bolinha.SetActive(!state);
    }
    IEnumerator FadingText()
    {
        Color newColor = textoItemColetado.color;
        while(newColor.a < 1)
        {
            newColor.a += Time.deltaTime;
            textoItemColetado.color = newColor;
            yield return null;

        }
        yield return new WaitForSeconds(1f); 
        while (newColor.a > 0)
        {
            newColor.a -= Time.deltaTime;
            textoItemColetado.color = newColor;
            yield return null;
        }
    }
}
