using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Basket : MonoBehaviour, IBasket {

    public bool positive;
    public AudioSource source;

    [SerializeField]
    AudioClip[] feedback;

    private List<Food> LikeFood = new List<Food>();
    private List<Food> DislikeFood = new List<Food>();
    //PreferenceContainer preferenceContainer;

    public virtual void AddToList(Food _food)
    {
        if (positive)
        {
            _food.gameObject.SetActive(false);
            LikeFood.Add(_food);
            PlaySong();
           // preferenceContainer.add(_food)
        }

        else if (!positive)
        {
            _food.gameObject.SetActive(false);
            DislikeFood.Add(_food);
            PlaySong();
        }
    }

    public virtual void PlaySong()
    {
        if(source != null)
        {
            source.clip = feedback[Random.Range(0, feedback.Length)];
            source.Play();
        }
    }
    

   


}
