using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using TMPro;


class LegendasBehaviour : PlayableBehaviour
{
    public string DefaultText;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var textObject = playerData as TextMeshProUGUI;
        if (textObject == null)
            return;

        // given the current time, determine how much of the string will be displayed
        /*
        var progress = (float)(playable.GetTime() / playable.GetDuration());
        var subStringLength = Mathf.RoundToInt(Mathf.Clamp01(progress) * DefaultText.Length);
        textObject.text = DefaultText.Substring(0, subStringLength);*/
        textObject.text = DefaultText;
    }
}

public class TypewriterEffectPlayableAsset : PlayableAsset, IPropertyPreview
{
    public string Text = "This is the default text to display";
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LegendasBehaviour>.Create(graph);
        playable.GetBehaviour().DefaultText = Text;
        return playable;
    }

    // this will put the text field into preview mode when editing, avoids constant dirtying the scene
    public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        driver.AddFromName<TextMeshProUGUI>("m_Text");
    }
}
