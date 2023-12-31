using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(VisionRenderer), PostProcessEvent.BeforeStack, "Custom/Vision", false)]
public class Vision : PostProcessEffectSettings
{
[Header("Parameter")]
    [Range(0,1)] public FloatParameter blendAmount = new FloatParameter{value = 0f};
//     public FloatParameter maskFadeSpeed = new FloatParameter{value = 0.25f};
//     public FloatParameter redChannelOffset = new FloatParameter{value = 0};
//     public FloatParameter greenChannelOffset = new FloatParameter{value = 0.6f};
//     public FloatParameter blueChannelOffset = new FloatParameter{value = 1.2f};
// [Header("maskFade")]
//     [Range(0,1)] public FloatParameter redSinWeight = new FloatParameter{value = 1};
//     [Range(0,1)] public FloatParameter greenSinWeight = new FloatParameter{value = 1};
//     [Range(0,1)] public FloatParameter blueSinWeight = new FloatParameter{value = 1};
[Space(20), Header("Texture")]
    public TextureParameter MaskTex = new TextureParameter{value = null};
    public TextureParameter screenTex_0 = new TextureParameter{value = null};
    public TextureParameter screenTex_1 = new TextureParameter{value = null};
    public TextureParameter screenTex_2 = new TextureParameter{value = null};
    public TextureParameter screenTex_3 = new TextureParameter{value = null};
    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value && blendAmount > 0;
    }
}

public sealed class VisionRenderer: PostProcessEffectRenderer<Vision>{
    private const string blendName = "_Blend";
    private const string maskTexName = "_MaskTex";
    private const string screen_0_Name = "_Screen_0";
    private const string screen_1_Name = "_Screen_1";
    private const string screen_2_Name = "_Screen_2";
    private const string screen_3_Name = "_Screen_3";

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Vision"));
        sheet.properties.SetFloat(blendName, settings.blendAmount.value);

        var maskTex = settings.MaskTex.value==null?RuntimeUtilities.whiteTexture:settings.MaskTex.value;
        var screenTex_0 = settings.screenTex_0.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_0.value;
        var screenTex_1 = settings.screenTex_1.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_1.value;
        var screenTex_2 = settings.screenTex_2.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_2.value;
        var screenTex_3 = settings.screenTex_3.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_3.value;

        sheet.properties.SetTexture(maskTexName, maskTex);
        sheet.properties.SetTexture(screen_0_Name, screenTex_0);
        sheet.properties.SetTexture(screen_1_Name, screenTex_1);
        sheet.properties.SetTexture(screen_2_Name, screenTex_2);
        sheet.properties.SetTexture(screen_3_Name, screenTex_3);
    //There might be better way to get temporary RT for using built-in postprocess effect
        var tempTex = RenderTexture.GetTemporary(context.width, context.height);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        RenderTexture.ReleaseTemporary(tempTex);
    }
}