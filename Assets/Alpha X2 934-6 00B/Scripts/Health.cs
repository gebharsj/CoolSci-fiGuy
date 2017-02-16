using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public float baseHealth = 50;
    float health;

    void Start()
    {
        health = baseHealth;
    }

    public void TookDamage()
    {
        health--;
        if (health <= 0)
            Died();
    }

    void Died()
    {
        Destroy(gameObject);
    }
}
