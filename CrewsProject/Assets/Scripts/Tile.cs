using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] ColorSettings colorSettings;
    [SerializeField] GameObject mouseOverlay;

    [SerializeField] GameObject selectOverlay;
    [SerializeField] int radius;

    public ColorSettings ColorSettings => colorSettings;

    public Vector2 tilePosition { get; private set; }

    void Awake()
    {
        mouseOverlay.SetActive(false);
    }

    private void OnMouseDown()
    {
        foreach (var _tile in GridManager.Instance.CurrentGrid)
        {
            _tile.selectOverlay.SetActive(false);
            if (_tile == this) continue; 

            if ((Mathf.Abs(tilePosition.x - _tile.tilePosition.x) + Mathf.Abs(tilePosition.y - _tile.tilePosition.y)) <= radius)
            {
                _tile.selectOverlay.SetActive(true);
            }
        }
    }
    public void OnMouseOver()
    {
        mouseOverlay.SetActive(true);
    }

    public void OnMouseExit()
    {
        mouseOverlay.SetActive(false);
    }

    public void SetPosition(Vector2 _position)
    {
        tilePosition = _position;
    }
}

[System.Serializable]
public class ColorSettings
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color lightColor = Color.white;
    [SerializeField] Color darkColor = Color.gray;

    public Color LightColor => lightColor;
    public Color DarkColor => darkColor;
    public void SetColor(bool _isLighterColor)
    {
        spriteRenderer.color = _isLighterColor ? lightColor : darkColor;
    }
}
