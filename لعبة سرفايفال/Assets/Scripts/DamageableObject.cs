using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour
{
    protected float health = 100f;
    public int level = 1;
    public string type = "Tree";
    public GameObject healthbar;
    public GameObject healthbarBackground;
    private Slider healthSlider;
    
    private void Start()
    {

        healthbar = GameObject.Find("Healthbar");
        healthbarBackground = healthbar.transform.Find("Background").gameObject;
       
    }
    public virtual void TakeDamage(float damage)
    {
        Debug.Log("taking damage");
        if (healthSlider == null) {
            healthSlider = healthbar.GetComponent<Slider>();
        }
        health -= damage;
        healthbarBackground.SetActive(true);
        healthSlider.value = health / 100;
        if (health <= 0)
        {
            healthbarBackground.SetActive(false);
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}

