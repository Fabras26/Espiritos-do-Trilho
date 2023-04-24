using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;


public class PlayerInteraction : MonoBehaviour
{
    PlayerController player;

    public float distanciaRay =  5f;
    public float velocidadeDeRotacao = 200f;

    public Transform verObjeto;
    public Transform bolsoObjeto;

    private Camera myCam;

    public UnityEvent onView;
    public UnityEvent onFinishView;



    private bool isViewing;
    private bool canFinish;

    private Interectable currentInterectable;
    private Porta portaAtual;
    private CaixaParaEmpurrar caixa;
    private Vector3 originPosition;
    private Quaternion originRotation;

    //private AudioPlayer audioPlayer;

    private AudioSource sfx;


    Inventario inventario;
    float velOrigin;
    private void Awake()
    {
        //audioPlayer = GetComponent<AudioPlayer>();
        sfx = GameObject.Find("Efeitos Sonoros").GetComponent<AudioSource>();
    }
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        inventario = FindObjectOfType<Inventario>();
        velOrigin = player.MoveSpeed;
        myCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInterectable();
    }
    void CheckInterectable()
    {
        if(isViewing)
        {
            if (currentInterectable.item.inspecionavel)
            {
                if (Input.GetMouseButton(1))
                {
                    RotacionarObjeto();
                }
                if (canFinish && Input.GetMouseButton(0))
                {

                    FinishView();
                }

            }
            if (currentInterectable.item.coletavel)
            {
                if (Input.GetMouseButton(1))
                {
                    RotacionarObjeto();
                }
                if (canFinish && Input.GetMouseButton(0))
                {
                    FinishView();
                    UIManager.instance.TextoColetado();
                    inventario.AddItem(currentInterectable.item.idInventario, currentInterectable.item.imgInventario);
                }

            }
        }
        RaycastHit hit;
        Vector3 rayOrigin = myCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.2f));
        if (Physics.Raycast(rayOrigin, myCam.transform.forward, out hit, distanciaRay))
        {
            NPC npc = hit.collider.GetComponent<NPC>();
            if(npc != null)
            {
                if (npc.dialogoDisponivel)
                {
                    UIManager.instance.SetBalao(true);
                    if (Input.GetMouseButtonDown(0) && !npc.GetComponent < Dialogos >(). conversando)
                    {

                        if (inventario.ChecarItem(2))
                        {
                            npc.GetComponent<Dialogos>().indexEstagio++;
                            inventario.UseItem(2);
                        }
                        npc.Dialogar();
                    }
                }
                else
                {
                    UIManager.instance.SetBalao(false);
                }
            }
            else
            {
                UIManager.instance.SetBalao(false);
            }
            Porta porta = hit.collider.GetComponent<Porta>();
            if (porta != null)
            {
                portaAtual = porta;
                if (portaAtual.trancada)
                {
                    UIManager.instance.SetHudTrancada(true);

                    if (inventario.ChecarItem(1) && Input.GetMouseButtonDown(0))
                    {
                        portaAtual.trancada = false;
                        portaAtual.Abrir(transform.position);
                        inventario.UseItem(1);

                    }

                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (portaAtual.aberta)
                    {
                        portaAtual.Fechar();
                    }
                    if (!portaAtual.aberta)
                    {
                        portaAtual.Abrir(transform.position);
                    }
                    if(!portaAtual.trancada) portaAtual.aberta = !portaAtual.aberta;


                }

            }
            Interectable interectable = hit.collider.GetComponent<Interectable>();
            if (interectable != null)
            {
                if (Input.GetMouseButtonDown(0) && isViewing == false)
                {
                    if(interectable.isMoving)
                    {
                        return;
                    }

                    onView.Invoke();

                    currentInterectable = interectable;

                    isViewing = true;

                    Interact(currentInterectable.item);

                    if (currentInterectable.item.inspecionavel)
                    {
                        originPosition = currentInterectable.transform.position;
                        originRotation = currentInterectable.transform.rotation;
                        StartCoroutine(MovendoObjeto(currentInterectable, verObjeto.position, verObjeto.rotation));
                    }

                    if (currentInterectable.item.coletavel)
                    {
                        originPosition = currentInterectable.transform.position;
                        originRotation = currentInterectable.transform.rotation;
                        StartCoroutine(MovendoObjeto(currentInterectable, verObjeto.position, verObjeto.rotation));
                    }
                }
            }
            if ((porta != null) || interectable != null)
            {
                
                if ((porta != null) && portaAtual.trancada)
                {
                    UIManager.instance.SetHudTrancada(true);
                    UIManager.instance.SetHandCursor(false);
                }
                else
                {
                    UIManager.instance.SetHudTrancada(false);
                    UIManager.instance.SetHandCursor(true);
                }

            }
            else
            {
                UIManager.instance.SetHudTrancada(false);
                UIManager.instance.SetHandCursor(false);
            }
        }
        else
        {
            UIManager.instance.SetBalao(false);
            UIManager.instance.SetHandCursor(false);
        }
        if (Physics.Raycast(rayOrigin, myCam.transform.forward, out hit, 0.2f))
        {
            caixa = hit.collider.GetComponent<CaixaParaEmpurrar>();
            if (caixa != null)
            {
                UIManager.instance.SetHudEmpurrar(true);

                if (Input.GetKey(KeyCode.LeftShift))
                {

                    UIManager.instance.SetHudEmpurrar(false);
                    caixa.Move();

                    player.movendoCaixa = true;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    UIManager.instance.SetHudEmpurrar(true);
                    caixa.Soltar();
                    player.movendoCaixa = false;

                }

            }
            else
            {

                UIManager.instance.SetHudEmpurrar(false);
                player.movendoCaixa = false;
                player.gameObject.transform.SetParent(null);

            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                caixa.Soltar();
                player.movendoCaixa = false;

            }
        }
        else
        {
            player.movendoCaixa = false;
            player.gameObject.transform.SetParent(null);
            UIManager.instance.SetHudEmpurrar(false);
        }
    }
    IEnumerator MovendoObjeto(Interectable obj, Vector3 position, Quaternion rotation)
    {
        obj.isMoving = true;
        float timer = 0;
        while (timer < 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, position, Time.deltaTime * 5);
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, rotation, Time.deltaTime * 5);
            timer += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = position;
        obj.isMoving = false;
    }
    void RotacionarObjeto()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        currentInterectable.transform.Rotate(myCam.transform.right, -Mathf.Deg2Rad * y * velocidadeDeRotacao, Space.World);
        currentInterectable.transform.Rotate(myCam.transform.up, -Mathf.Deg2Rad * x * velocidadeDeRotacao, Space.World);

    }

    void Interact(Item item)
    {
        //audioPlayer.PlayAudio(item.audioClip);

        sfx.PlayOneShot(item.audioClip);
        UIManager.instance.SetTextoDescricao(item.Text);
        Invoke("CanFinish", item.audioClip.length + 0.5f);
    }
    void CanFinish()
    {
        canFinish = true;
    }
    void FinishView()
    {
        //audioPlayer.PlayAudio(currentInterectable.item.audioClip);
        sfx.PlayOneShot(currentInterectable.item.audiocolect);

        canFinish = false;
        isViewing = false;
        if (currentInterectable.item.inspecionavel)
        {
            StartCoroutine(MovendoObjeto(currentInterectable, originPosition,originRotation));
        }
        if (currentInterectable.item.coletavel)
        {
            StartCoroutine(MovendoObjeto(currentInterectable, bolsoObjeto.position, bolsoObjeto.rotation));
            StartCoroutine(ColetandoObjeto(currentInterectable));
        }
        onFinishView.Invoke();
        UIManager.instance.SetTextoDescricao("");
    }
    IEnumerator ColetandoObjeto(Interectable obj)
    {
       
        float timer = 0;
        while (timer < 1)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, Vector3.zero, Time.deltaTime * 5);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(obj.gameObject);
    }

}
