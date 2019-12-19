using UnityEngine;
using System;
using System.Collections;

public class Target : MonoBehaviour
{

    public float health = 100f;

    private WaitForSeconds visualTime = new WaitForSeconds(0.1f);
    private new Renderer renderer;
    private Color baseColor;
    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        baseColor = renderer.material.GetColor("_Color");
    }

    public void TakeDamage(float amount) {
        health -= amount;

        StartCoroutine(visualDamage());

        if(health <= 0)
        {
            Die();
        }
    }

    void Die() {

        Destroy(gameObject);
    }

    private IEnumerator visualDamage() {

        Debug.Log("trafiony");
        renderer.material.SetColor("_Color", Color.red);
        yield return visualTime;
        renderer.material.SetColor("_Color", baseColor);
        Debug.Log("zatopiony");
    }

}
