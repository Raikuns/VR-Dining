using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Basket : MonoBehaviour, IBasket {

    public bool positive;
    protected AudioSource source;

    [SerializeField]
    AudioClip[] feedback;

    private List<GameObject> LikeFood = new List<GameObject>();
    private List<GameObject> DislikeFood = new List<GameObject>();

    public virtual void AddToList(GameObject _food)
    {
        if (positive)
        {
            LikeFood.Add(_food);
            PlaySong();
        }

        else if (!positive)
        {
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
