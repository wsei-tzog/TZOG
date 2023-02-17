using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YouDontHaveKey : MonoBehaviour
{
    public Image ydhk;
    private void Start()
    {
        ydhk.enabled = false;
    }
    public IEnumerator YouDontHaveKeyDisplay()
    {
        ydhk.enabled = true;
        Debug.Log(" cor wait");
        yield return new WaitForSeconds(4);
        Debug.Log(" cor after wait");
        ydhk.enabled = false;
    }
}
