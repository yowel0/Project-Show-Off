using System;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;

public class VectorInput
{
    float pressedBorder = 0.8f;

    Vector2 input;
    Vector2 oldInput;
    public VectorButton north = new VectorButton();
    public VectorButton east = new VectorButton();
    public VectorButton south = new VectorButton();
    public VectorButton west = new VectorButton();

    public VectorInput(){

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update(Vector2 vInput)
    {
        Console.WriteLine("print");
        oldInput = input;
        input = vInput;
        //north
        if (input.y > 0){
            if (input.y > pressedBorder){
                north.down = true;
                if (oldInput.y < pressedBorder){
                    north.pressed = true;
                }
                else{
                    north.pressed = false;
                }
            }
            else {
                north.down = false;
                if (oldInput.y > pressedBorder){
                    north.released = true;
                }
                else{
                    north.released = false;
                }
            }
        }
        //south
        else if (input.y < 0){
            if (-input.y > pressedBorder){
                south.down = true;
                if (-oldInput.y < pressedBorder){
                    south.pressed = true;
                }
                else{
                    south.pressed = false;
                }
            }
            else {
                south.down = false;
                if (-oldInput.y > pressedBorder){
                    south.released = true;
                }
                else{
                    south.released = false;
                }
            }
        }
        //east
        if (input.x > 0){
            if (input.x > pressedBorder){
                east.down = true;
                if (oldInput.x < pressedBorder){
                    east.pressed = true;
                }
                else{
                    east.pressed = false;
                }
            }
            else {
                east.down = false;
                if (oldInput.x > pressedBorder){
                    east.released = true;
                }
                else{
                    east.released = false;
                }
            }
        }
        //west
        else if (input.x < 0){
            if (-input.x > pressedBorder){
                west.down = true;
                if (-oldInput.x < pressedBorder){
                    west.pressed = true;
                }
                else{
                    west.pressed = false;
                }
            }
            else {
                west.down = false;
                if (-oldInput.x > pressedBorder){
                    west.released = true;
                }
                else{
                    west.released = false;
                }
            }
        }
    }
}

public class VectorButton
{
    public VectorButton(){

    }
    public bool pressed;
    public bool down;
    public bool released;
}