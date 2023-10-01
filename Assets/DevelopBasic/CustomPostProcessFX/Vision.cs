using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(VisionRenderer), PostProcessEvent.BeforeStack, "Custom/Vision")]
public class Vision : PostProcessEffectSettings
{
    [Range(0, 1f)]
    public FloatParameter blendAmount = new FloatParameter{value = 0f};
    public TextureParameter MaskTex = new TextureParameter{value = null};
    public TextureParameter screenTex_1 = new TextureParameter{value = null};
    public TextureParameter screenTex_2 = new TextureParameter{value = null};
    public TextureParameter screenTex_3 = new TextureParameter{value = null};
    public TextureParameter screenTex_4 = new TextureParameter{value = null};
    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value && blendAmount > 0;
    }
}

public sealed class VisionRenderer: PostProcessEffectRenderer<Vision>{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Vision"));
        sheet.properties.SetFloat("_Blend", settings.blendAmount.value);

        var maskTex = settings.MaskTex.value==null?RuntimeUtilities.whiteTexture:settings.MaskTex.value;
        var screenTex_1 = settings.screenTex_1.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_1.value;
        var screenTex_2 = settings.screenTex_2.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_2.value;
        var screenTex_3 = settings.screenTex_3.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_3.value;
        var screenTex_4 = settings.screenTex_4.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_4.value;

        sheet.properties.SetTexture("_MaskTex", maskTex);
        sheet.properties.SetTexture("_Screen_1", screenTex_1);
        sheet.properties.SetTexture("_Screen_2", screenTex_2);
        sheet.properties.SetTexture("_Screen_3", screenTex_3);
        sheet.properties.SetTexture("_Screen_4", screenTex_4);
    //There might be better way to get temporary RT for using built-in postprocess effect
        var tempTex = RenderTexture.GetTemporary(context.width, context.height);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        RenderTexture.ReleaseTemporary(tempTex);
    }
}