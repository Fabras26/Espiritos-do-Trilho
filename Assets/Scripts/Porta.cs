using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Porta : MonoBehaviour
{
    public bool trancada;
    public bool aberta;

    [SerializeField]
    float velocidadeAbertura = 2f;
    [Header("Configuração de Rotação")]
    [SerializeField]
    private float rotacao = 90;
    [SerializeField]
    private float direcao;

    public Quaternion rotInicial;
    public Vector3 frente;

    public AudioClip portaAbrindo;

    public AudioClip portaFechando;
    public AudioClip portaBatendo;
    public AudioClip portaTrancada;



    private Coroutine coroutine;

    public UnityEvent acoes;
    void Start()
    {
        rotInicial = transform.rotation;

        frente = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Abrir(Vector3 PosicaoPlayer)
    {

        if (!trancada)
        {
                if(coroutine !=null)
                {
                    StopCoroutine(coroutine);
                }
                float ponto = Vector3.Dot(frente, (PosicaoPlayer - transform.position).normalized);
                coroutine = StartCoroutine(CoroutineAbrir(ponto));
            acoes.Invoke();



        }
        else
        {
            GameObject.Find("Efeitos Sonoros").GetComponent<AudioSource>().PlayOneShot(portaTrancada);
        }



    }
    public void Fechar()
    {
        if (!trancada)
        {

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            GameObject.Find("Efeitos Sonoros").GetComponent<AudioSource>().PlayOneShot(portaFechando);

            coroutine = StartCoroutine(CoroutineFechar());
        }

    }

    IEnumerator CoroutineAbrir(float ponto)
    {
        Quaternion inicio = transform.rotation;
        Quaternion fim;


        GameObject.Find("Efeitos Sonoros").GetComponent<AudioSource>().PlayOneShot(portaAbrindo);
        GameObject.Find("Efeitos Sonoros").GetComponent<AudioSource>().PlayOneShot(portaFechando);
        if (ponto >= direcao)
        {
            fim = Quaternion.Euler(new Vector3(0, rotInicial.y - rotacao, 0));
        }

        else
        {
            fim = Quaternion.Euler(new Vector3(0, rotInicial.y + rotacao, 0));
        }

        float timer = 0;
        while (timer < 1)
        {
            transform.rotation = Quaternion.Slerp(inicio, fim, timer);
            yield return null;

            timer += Time.deltaTime * velocidadeAbertura;
        }
    }
    IEnumerator CoroutineFechar()
    {
        Quaternion inicio = transform.rotation;
        Quaternion fim = rotInicial;

        float timer = 0;
        while (timer < 1)
        {
            transform.rotation = Quaternion.Slerp(inicio, fim, timer);
            yield return null;

            timer += Time.deltaTime * velocidadeAbertura;
        }
        GameObject.Find("Efeitos Sonoros").GetComponent<AudioSource>().PlayOneShot(portaBatendo);

    }
}
