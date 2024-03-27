using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ActorManager : MonoBehaviour
{
    public Image actorImage1;
    public Image actorImage2;
    public Image actorImage3;
    public Image actorImage4;

    Image[] actorSprites;
    Actor[] currentActors;

    public float inactiveActorDarkening = 0.5f;
    //This is the function that darkens every other sprite except for whoever's apparently talking. The given ID must EXACTLY correspond to whoever you want to be brightened.
    public void setActive(int actorId) {

        if (actorId == 69) {
            for (int i = 0; i < actorSprites.Length; i++)
            {
                actorSprites[i].color = new Color(inactiveActorDarkening, inactiveActorDarkening, inactiveActorDarkening, 1f);
            }
        } else {
            for (int i = 0; i < actorSprites.Length; i++)
            {

                if (i == actorId) {

                    actorSprites[i].color = new Color(1f, 1f, 1f, 1f);
                    
                }

                if (i != actorId) {

                    if (currentActors[i].show == true) {

                        actorSprites[i].color = new Color(inactiveActorDarkening, inactiveActorDarkening, inactiveActorDarkening, 1f);

                    }
                }
            }
        }
        
    }

    //loadActorImages only works if it's explicitly given either two or four actors. If it's given two actors, it assigns sprites to actorImage2 and 3 and sets the others to null. So, the first sprite you can visually see is the second actorImage, and the second sprite is actually the third, so you need to identify them like that if you need to call on them programmatically.
    //If it's given four actors, it individually assigns each actor to each sprite image.
    //Both work based on the given order of the actor array, from left to right.

    public void loadActorImages(Actor[] actors) {
        if (actors.Length == 2) {
            actorImage2.sprite = actors[0].sprite;
            actorImage3.sprite = actors[1].sprite;

            actorImage1.color = new Color(0f, 0f, 0f, 0f);
            actorImage4.color = new Color(0f, 0f, 0f, 0f);
        } else if (actors.Length == 4) {

            currentActors = actors;

            for (int i = 0; i < 4; i++)
            {
                actorSprites[i].sprite = actors[i].sprite;
                if (actors[i].show == false) {
                    Debug.Log("Setting actor " + i + "'s color to 0f.");
                    actorSprites[i].color = new Color(0f, 0f, 0f, 0f);
                }
            }
        } else {
            throw new Exception("ActorManager was given an array of actors size " + actors.Length);
        }
    }

    void Start() {
        actorSprites = new Image[] {actorImage1, actorImage2, actorImage3, actorImage4};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
