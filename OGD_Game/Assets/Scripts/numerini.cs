using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numerini : MonoBehaviour
{
    public Transform bar;
    public Vector3 offset;

    public TextMesh damage;
    public TextMesh health;

    private float maxHealth;
    private float maxdamage;
    Transform target;

    public void Setup(Transform target, float maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateBar(maxHealth);
        this.target = target;
    }

    public void UpdateBar(float newValue)
    {
        damage.text= "" + maxdamage;
        health.text = "" + newValue;
        Vector3 scale = bar.transform.localScale;
        bar.transform.localScale = scale;
    }

    public void HideBar()
    {
        //this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target != null)
            this.transform.position = target.position + offset;
    }
}
