using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteLightSource : MonoBehaviour
{
    [SerializeField] private float lightRadius = 1;
    [SerializeField] private float lightLength = 1;
    [SerializeField] private float lightIntensity = 1;
    [SerializeField, ColorUsage(false)] private Color lightColor;

    private static int SpriteLightPosID;
    private static int SpriteLightRadiusID;
    private static int SpriteLightLengthID;
    private static int SpriteLightIntensityID;
    private static int SpriteLightColorID;

    void Awake(){
        SpriteLightPosID = Shader.PropertyToID("SpriteLightPos");
        SpriteLightRadiusID = Shader.PropertyToID("SpriteLightRadius");
        SpriteLightLengthID = Shader.PropertyToID("SpriteLightLength");
        SpriteLightIntensityID = Shader.PropertyToID("SpriteLightIntensity");
        SpriteLightColorID = Shader.PropertyToID("SpriteLightColor");
    }
    void Update(){
        Shader.SetGlobalFloat(SpriteLightRadiusID, lightRadius);
        Shader.SetGlobalFloat(SpriteLightLengthID, lightLength);
        Shader.SetGlobalFloat(SpriteLightIntensityID, lightIntensity);
        Shader.SetGlobalColor(SpriteLightColorID, lightColor);
        Shader.SetGlobalVector(SpriteLightPosID, transform.position);
    }
}