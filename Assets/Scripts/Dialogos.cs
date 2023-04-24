using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.Events;
using Cinemachine;

using TMPro;

public class Dialogos : MonoBehaviour
{
    [Header("Personagens")]
    public GameObject npc; public GameObject jog;

    [Header("HUD")]
    public TextMeshProUGUI caixaDeTexto;
    public Image imagemDeFala;
    public float velocidadeDoTexto;

    [Header("Vozes")]
    public AudioClip[] vozesFantasma;


    [System.Serializable]
    public class EstagiosDeDialogo
    {
        [System.Serializable]
        public class LinhasDeDialogo
        {
            public string linha;
            public Sprite img;
            public enum Personagem { Sophia, Fantasma}
            public Personagem personagem;
        }
        [SerializeField]
        public LinhasDeDialogo[] linhasDeDialogo;
    }
    [Space(10)]

    [SerializeField]
    public EstagiosDeDialogo[] estagiosDialogo;
    

    


    public UnityEvent iniciarDialogo;
    public UnityEvent[] finalizarDialogo;

    private int index;
    public int indexEstagio;


    public bool conversando;

    AudioSource audioSource;

    AudioClip[] vozesatual;

    PlayerController player;
    void Start()
    {
        indexEstagio = 0;
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (conversando)
        {
            Vector3 direction = npc.transform.position - jog.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            jog.transform.rotation = Quaternion.Lerp(jog.transform.rotation, targetRotation, 5 * Time.deltaTime);



            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (caixaDeTexto.text == estagiosDialogo[indexEstagio].linhasDeDialogo[index].linha)
                {
                    ProximaLinha();
                }
                else
                {
                    StopAllCoroutines();
                    audioSource.Stop();
                    caixaDeTexto.text = estagiosDialogo[indexEstagio].linhasDeDialogo[index].linha;
                }
            }
            
        }
        
    }
    public void IniciarDialogo()
    {
        player.gameObject.GetComponent<AudioSource>().pitch = 1.4f;
        conversando = true;
        index = 0;
        iniciarDialogo.Invoke();
        StartCoroutine(DigitarLinha());
        imagemDeFala.gameObject.SetActive(true);

    }

    IEnumerator DigitarLinha()
    {
        imagemDeFala.sprite = estagiosDialogo[indexEstagio].linhasDeDialogo[index].img;
        if (estagiosDialogo[indexEstagio].linhasDeDialogo[index].personagem == EstagiosDeDialogo.LinhasDeDialogo.Personagem.Sophia)
        {
            vozesatual = player.vozesJogador;
            audioSource = player.gameObject.GetComponent<AudioSource>();
        }
        if (estagiosDialogo[indexEstagio].linhasDeDialogo[index].personagem == EstagiosDeDialogo.LinhasDeDialogo.Personagem.Fantasma)
        {
            vozesatual = vozesFantasma;
            audioSource = GetComponent<AudioSource>();
        }
        int fala = 1;

        /*StartCoroutine(LoopVoz(vozesatual));*/
        foreach (char c in estagiosDialogo[indexEstagio].linhasDeDialogo[index].linha.ToCharArray())
        {
            fala++;
            if (fala >= 2)
            {
                if (c.ToString() != " ")
                {
                    int i = Random.Range(0, vozesatual.Length);
                    audioSource.PlayOneShot(vozesatual[i]);
                    fala = 0;
                }
            }
            caixaDeTexto.text += c;
            yield return new WaitForSeconds(velocidadeDoTexto);
        }
    }
    void ProximaLinha()
    {
        if(index< estagiosDialogo[indexEstagio].linhasDeDialogo.Length - 1)
        {

            index++;
            caixaDeTexto.text = string.Empty;
            StartCoroutine(DigitarLinha());

        }
        else
        {
            finalizarDialogo[indexEstagio].Invoke();
            player.gameObject.GetComponent<AudioSource>().pitch = 1.0f;

            conversando = false;
            caixaDeTexto.text = string.Empty;
            imagemDeFala.sprite = null;
            imagemDeFala.gameObject.SetActive(false);

        }

    }
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Capsule")
        IniciarDialogo();

    }
    public void AumentarIndexEstagio()
    {
        indexEstagio++;
    }
}
