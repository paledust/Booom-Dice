using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChannelData{
    public SpriteRenderer maskRenderer;
    public string mixerValueName;
}
public class MiniGameChannelController : MonoBehaviour
{
    [SerializeField] private ChannelData[] channelDatas;
}
