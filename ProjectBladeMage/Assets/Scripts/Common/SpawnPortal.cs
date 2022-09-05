using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnPortal : MonoBehaviour
{
    private bool initiated = false;     // Whether or not the initial spawn animation began
    private float portalSize = 1;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawn()
    {
        if (!initiated)
        {
            transform.DOScale(portalSize, 1).SetEase(Ease.OutBounce);
            initiated = true;
        }
    }

    public void EndSpawn()
    {
        transform.DOScale(0, 1).SetEase(Ease.InBack);
        initiated = false;
    }
}
