using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f; // Velocidade do player

    void Update()
    {
        Vector2 move = Vector2.zero;

        // Lê WASD + setas
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            move.y += 1f;
        }
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            move.y -= 1f;
        }
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            move.x -= 1f;
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            move.x += 1f;
        }

        // Normaliza para diagonais
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // Aplica movimento
        Vector2 position = (Vector2)transform.position;
        position += move * speed * Time.deltaTime;
        transform.position = position;

        // Debug opcional
        Debug.Log(move);
    }
}