using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class DirectorTrigger : MonoBehaviour
{
    public enum TipoTrigger
    {
        UmaVez, TempoTodo,
    }

    public GameObject triggeringGameObject;
    public PlayableDirector diretor;
    public TipoTrigger triggerType;
    public UnityEvent AoIniciarCutscene;
    public UnityEvent AoFinalizarCutscene;
    [HideInInspector]

    protected bool m_AlreadyTriggered;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != triggeringGameObject)
            return;

        if (triggerType == TipoTrigger.UmaVez && m_AlreadyTriggered)
            return;

        diretor.Play();
        m_AlreadyTriggered = true;
        AoIniciarCutscene.Invoke();
        Invoke("FinishInvoke", (float)diretor.duration);
    }

    void FinishInvoke()
    {
        AoFinalizarCutscene.Invoke();
    }

    public void OverrideAlreadyTriggered(bool alreadyTriggered)
    {
        m_AlreadyTriggered = alreadyTriggered;
    }
}