using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechManager : MonoBehaviour {

   
    public Speech speeches;  //This will show the array in the editor, which we can then manipulate in editor.
	
}


/// <summary>
/// The class: Speech creates our BuddySpeech system
/// This class contains to arrays of audioclips that are being used
/// to store feedback clips the buddy can "say" based on the player's actions.
/// </summary>
[System.Serializable]
public class Speech
{
    public AudioClip[] PositveFeedback;
    public AudioClip[] NegativeFeedback;

    public AudioSource source;

    /// <summary>
    /// This method gives the audiosource a random clip out of the 
    /// positive feedback array
    /// </summary>
    public void PlayPositive()
    {
        source.clip = PositveFeedback[Random.Range(0, PositveFeedback.Length)];
        source.Play();
    }

    /// <summary>
    /// This method gives the audiosource a random clip out of the 
    /// negative feeback array
    /// </summary>
    public void PlayNegative()
    {
        source.clip = NegativeFeedback[Random.Range(0, NegativeFeedback.Length)];
        source.Play();
    }
}
