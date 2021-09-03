using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeUI : MonoBehaviour
{
    private static ShakeUI instance;
    public static ShakeUI Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ShakeUI>();
            return instance;
        }
    }

    public CanvasGroup canvasGroup;

    public GameObject InfoGo;
    public Text InfoText;
    public float cameraShake = 10;
    private bool isShake;

    private Vector3 oldPosition;
    private void Awake()
    {
        instance = this;
        oldPosition = transform.position;
        canvasGroup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShake)
        {
            float a = Random.Range(0f, cameraShake) - cameraShake * 0.5f;
            //XY¶b¾_°Ê
            transform.position = new Vector3(transform.position.x, transform.position.y + a, transform.position.z);
            //Z¶b¾_°Ê
            transform.position = new Vector3(transform.position.x, transform.position.y, (Random.Range(0f, cameraShake)) - cameraShake * 0.5f);
            cameraShake = cameraShake / 1.05f;
            if (cameraShake < 0.05f)
            {
                isShake = false;
                cameraShake = 5;
                transform.position = oldPosition;
            }
            return;
        }

        canvasGroup.alpha -= Time.deltaTime;
        if (canvasGroup.alpha == 0)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }

    public void SetText(string info)
    {
        InfoText.text = info;
        canvasGroup.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 1;
        isShake = true;
    }

}
