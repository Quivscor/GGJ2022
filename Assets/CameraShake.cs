using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 startPos;
    WaitForEndOfFrame wait;
    public float shakePower;
    public float shakeDuration;

    private void Start()
    {
        startPos = transform.localPosition;

        wait = new WaitForEndOfFrame();
    }

    public void StartShake(float value)
    {
        StartCoroutine(Shake(shakeDuration));
    }

    private IEnumerator Shake(float time)
    {
        while(time > 0)
        {
            time -= Time.deltaTime;
            transform.localPosition = startPos + Random.insideUnitSphere * shakePower;
            yield return wait;
        }
        
        transform.localPosition = startPos;
    }
}
