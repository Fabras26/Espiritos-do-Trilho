using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class LegendasPlayableAsset : PlayableAsset
{
    [TextArea(15, 20)]
    public string falasTexto;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<LegendasBehaviour>.Create(graph);

        LegendasBehaviour falasBehaviour = playable.GetBehaviour();

        falasBehaviour.DefaultText = falasTexto;

        return playable;
    }
}
