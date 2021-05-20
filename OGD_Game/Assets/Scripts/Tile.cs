
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer highlightSprite;
    public SpriteRenderer highlightEligibleSprite;
    public Color validColor;
    public Color wrongColor;
    public bool hasTree = false;

    private TreeEntity tree;

    public void SetHighlight(bool active, bool valid)
    {
        highlightSprite.gameObject.SetActive(active);

        highlightSprite.color = valid ? validColor : wrongColor;

        
    }

    public void SetEligibleHighlight(bool valid)
    {
        highlightEligibleSprite.gameObject.SetActive(valid);
    }

    public void Conquer(Team team)
    {
        tree = GetComponentInChildren<TreeEntity>();
        tree.SetConquerer(team);
    }
}
