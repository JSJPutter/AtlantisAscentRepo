using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class AudioTestScript : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SFXManager.instance.PlaySFXClip(audioClip, 1f, this.transform);
        }
    }
}