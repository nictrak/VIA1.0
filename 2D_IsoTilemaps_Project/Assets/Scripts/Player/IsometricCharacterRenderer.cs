using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IsometricCharacterRenderer : MonoBehaviour
{
	
	public enum States { none, first, second, third };
    public static readonly string[] dashDirections = { "VIA Dash N", "VIA Dash NW", "VIA Dash W", "VIA Dash SW", "VIA Dash S", "VIA Dash SE", "VIA Dash E", "VIA Dash NE" };
    public static readonly string[] staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    public static readonly string[] runDirections = {"Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE"};   
    public static readonly string[] attackDirections =  {"Attack N", "Attack NW", "Attack W", "Attack SW", "Attack S", "Attack SE", "Attack E", "Attack NE"};
	public static readonly string[] attack1Directions = {"Attack1 N", "Attack1 NW", "Attack1 W", "Attack1 SW", "Attack1 S", "Attack1 SE", "Attack1 E", "Attack1 NE"};
	public static readonly string[] attack2Directions = {"Attack2 N", "Attack2 NW", "Attack2 W", "Attack2 SW", "Attack2 S", "Attack2 SE", "Attack2 E", "Attack2 NE"};
	
	Animator animator;
    public int LastDirection;

    private void Awake()
    {
        //cache the animator component
        animator = GetComponent<Animator>();
    }


    public void SetDirection(Vector2 direction){

        //use the Run states by default
        string[] directionArray = null;

        //measure the magnitude of the input.
        if (direction.magnitude < .01f)
        {
            //if we are basically standing still, we'll use the Static states
            //we won't be able to calculate a direction if the user isn't pressing one, anyway!
            directionArray = staticDirections;
        }
        else
        {
            //we can calculate which direction we are going in
            //use DirectionToIndex to get the index of the slice from the direction vector
            //save the answer to lastDirection
            directionArray = runDirections;
            LastDirection = DirectionToIndex(direction, 8);
        }

        //tell the animator to play the requested state
        animator.Play(directionArray[LastDirection]);
    }

    //helper functions

    //this function converts a Vector2 direction to an index to a slice around a circle
    //this goes in a counter-clockwise direction.
    public static int DirectionToIndex(Vector2 dir, int sliceCount){
        //get the normalized direction
        Vector2 normDir = dir.normalized;
        //calculate how many degrees one slice is
        float step = 360f / sliceCount;
        //calculate how many degress half a slice is.
        //we need this to offset the pie, so that the North (UP) slice is aligned in the center
        float halfstep = step / 2;
        //get the angle from -180 to 180 of the direction vector relative to the Up vector.
        //this will return the angle between dir and North.
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        //add the halfslice offset
        angle += halfstep;
        //if angle is negative, then let's make it positive by adding 360 to wrap it around.
        if (angle < 0){
            angle += 360;
        }
        //calculate the amount of steps required to reach this angle
        float stepCount = angle / step;
        //round it, and we have the answer!
        return Mathf.FloorToInt(stepCount);
    }







    //this function converts a string array to a int (animator hash) array.
    public static int[] AnimatorStringArrayToHashArray(string[] animationArray)
    {
        //allocate the same array length for our hash array
        int[] hashArray = new int[animationArray.Length];
        //loop through the string array
        for (int i = 0; i < animationArray.Length; i++)
        {
            //do the hash and save it to our hash array
            hashArray[i] = Animator.StringToHash(animationArray[i]);
        }
        //we're done!
        return hashArray;
    }
	
	public void AttackDirection(Vector2 direction, States attackState){

        //use the Run states by default
        string[] directionArray = null;
		
        if(attackState != States.none)
        {
            if (attackState == States.first)
            {
                directionArray = attackDirections;
            }
            else if (attackState == States.second)
            {
                directionArray = attack1Directions;
            }
            else
            {
                directionArray = attack2Directions;
            }

            if (direction.magnitude >= .01f)
            {
                LastDirection = DirectionToIndex(direction, 8);
            }

            animator.Play(directionArray[LastDirection]);
        }
    }

    public void DashDirection(Vector2 direction){

        //use the Run states by default
        string[] directionArray = null;
		directionArray = dashDirections;
		
		if (direction.magnitude >= .01f)
		{
			LastDirection = DirectionToIndex(direction, 8);
		} else {
            LastDirection = DirectionToIndex(direction, 8);
        }

        animator.Play(directionArray[LastDirection]);
    }


}
