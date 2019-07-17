using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationMapController : MonoBehaviour
{
    private SpriteRenderer  m_SpriteRenderer;
    
    public void Init()
    {
        m_SpriteRenderer = transform.Find("BackGround").GetComponent<SpriteRenderer>();
    }

    public void SetBgTexture(Sprite sprite)
    {
        m_SpriteRenderer.sprite = sprite;
    }
}
