using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Speech
{
    public AudioClip[] PositiveFeedback; //Array of all positive feedback lines.
    public AudioClip[] NegativeFeedback;
    

    private AudioSource source; //The source that will play our lines

    public void SetSource(AudioSource _source)
    {
        source = _source;
    }

    public void Positive()
    {
        SetSource(source);
        source.clip = PositiveFeedback[Random.Range(0, PositiveFeedback.Length)];
        source.Play();
    }

    public void Negative()
    {
        SetSource(source);
        source.clip = NegativeFeedback[Random.Range(0, NegativeFeedback.Length)];
        source.Play();
    }

}

public class BuddyBehaviour : MonoBehaviour
{
    [SerializeField]
    Speech[] BuddySpeech;
}






