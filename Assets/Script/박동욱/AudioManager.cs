using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{


    [HideInInspector]
    public AudioSource m_audioSrc;
    public AudioClip[] m_audioClip;

    public float m_volume = 1.0f;
    public bool m_loop = false;
    public bool m_backgroundSound = false;
    public bool m_effectSound = false;
    private void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        m_audioSrc = gameObject.AddComponent<AudioSource>();

        m_audioSrc.loop = m_loop;
        m_audioSrc.volume = m_volume;
        m_audioSrc.panStereo = 0.0f;

        if(!m_effectSound && m_backgroundSound)
            PlaySound();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySound(int NeedSoundNum = 0)
    {
        m_audioSrc.clip = m_audioClip[NeedSoundNum];

        if (!m_effectSound && m_backgroundSound)
            m_audioSrc.Play();
        else
            m_audioSrc.PlayOneShot(m_audioClip[NeedSoundNum]);
    }
}
