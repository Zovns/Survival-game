using System.Collections;
using UnityEngine;

public class FarmableObject : DamageableObject
{
   
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health > 0f)
        {
            StartCoroutine(ScaleOnHit());
        }
    }

    private IEnumerator ScaleOnHit()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        float duration = 0.1f;
        float time = 0f;

        // Scale up
        while (time < duration)
        {
            float t = time / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        // Hold briefly at larger scale (optional)
        yield return new WaitForSeconds(0.05f);

        // Scale back to original
        time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }


}
