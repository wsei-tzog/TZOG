using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class DamageScreenEffect : MonoBehaviour
{

    public Image damageImage;

    private void Start()
    {
        damageImage.enabled = false;
    }


    public IEnumerator Blooding(float flashDuration, float Health)
    {

        if (Health > 40)
        {
            damageImage.enabled = true;
            yield return new WaitForSeconds(flashDuration);
            damageImage.enabled = false;
        }
        else if (Health < 40 && Health > 25)
        {
            damageImage.enabled = true;
            Color originalColor = damageImage.color;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.12f);
            damageImage.color = newColor;
        }
        else if (Health < 25)
        {
            damageImage.enabled = true;
            Color originalColor = damageImage.color;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
            damageImage.color = newColor;
        }
    }

    public IEnumerator Dead()
    {
        damageImage.enabled = true;
        Color originalColor = damageImage.color;
        Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        damageImage.color = newColor;

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("master");
        Camera.main.GetComponent<AudioListener>().enabled = false;
    }
}
