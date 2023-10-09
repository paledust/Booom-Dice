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
    public FloatParameter maskFadeSpeed = new FloatParameter{value = 0.25f};
    public FloatParameter redChannelOffset = new FloatParameter{value = 0};
    public FloatParameter greenChannelOffset = new FloatParameter{value = 0.6f};
    public FloatParameter blueChannelOffset = new FloatParameter{value = 1.2f};
[Header("maskFade")]
    [Range(0,1)] public FloatParameter redSinWeight = new FloatParameter{value = 1};
    [Range(0,1)] public FloatParameter greenSinWeight = new FloatParameter{value = 1};
    [Range(0,1)] public FloatParameter blueSinWeight = new FloatParameter{value = 1};
[Space(20), Header("Texture")]
    public TextureParameter MaskTex = new TextureParameter{value = null};
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
    private const string screen_1_Name = "_Screen_1";
    private const string screen_2_Name = "_Screen_2";
    private const string screen_3_Name = "_Screen_3";
    private const string redSinWeight_Name = "_RedSinWeight";
    private const string greenSinWeight_Name = "_GreenSinWeight";
    private const string blueSinWeight_Name = "_BlueSinWeight";
    private const string fadeSpeed_Name = "_Speed";
    private const string offsetRed_Name = "_OffsetRed";
    private const string offsetGreen_Name = "_OffsetGreen";
    private const string offsetBlue_Name = "_OffsetBlue";

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Vision"));
        sheet.properties.SetFloat(blendName, settings.blendAmount.value);

        sheet.properties.SetFloat(fadeSpeed_Name, settings.maskFadeSpeed.value);
        sheet.properties.SetFloat(offsetRed_Name, settings.redChannelOffset.value);
        sheet.properties.SetFloat(offsetGreen_Name, settings.greenChannelOffset.value);
        sheet.properties.SetFloat(offsetBlue_Name, settings.blueChannelOffset.value);
        sheet.properties.SetFloat(redSinWeight_Name, settings.redSinWeight.value);
        sheet.properties.SetFloat(greenSinWeight_Name, settings.greenSinWeight.value);
        sheet.properties.SetFloat(blueSinWeight_Name, settings.blueSinWeight.value);

        var maskTex = settings.MaskTex.value==null?RuntimeUtilities.whiteTexture:settings.MaskTex.value;
        var screenTex_1 = settings.screenTex_1.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_1.value;
        var screenTex_2 = settings.screenTex_2.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_2.value;
        var screenTex_3 = settings.screenTex_3.value==null?RuntimeUtilities.whiteTexture:settings.screenTex_3.value;

        sheet.properties.SetTexture(maskTexName, maskTex);
        sheet.properties.SetTexture(screen_1_Name, screenTex_1);
        sheet.properties.SetTexture(screen_2_Name, screenTex_2);
        sheet.properties.SetTexture(screen_3_Name, screenTex_3);
    //There might be better way to get temporary RT for using built-in postprocess effect
        var tempTex = RenderTexture.GetTemporary(context.width, context.height);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        RenderTexture.ReleaseTemporary(tempTex);
    }
}