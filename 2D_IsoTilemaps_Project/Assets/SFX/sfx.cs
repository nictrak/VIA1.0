using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx : MonoBehaviour
{
   public AudioSource Woosh1;
   public AudioSource Woosh2;
   public AudioSource Woosh3;

   public void playWoosh1(){
   	Woosh1.Play();
   }
   public void playWoosh2(){
   	Woosh2.Play();
   }
   public void playWoosh3(){
   	Woosh3.Play();
   }
}
