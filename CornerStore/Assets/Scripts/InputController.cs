using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerController playController;

    void Awake ()
    {
        playController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        //get input values
        int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        bool jump = Input.GetKey(KeyCode.Space);

        playController.ForwardInput = vertical;
        playController.TurnInput = horizontal;
        playController.JumpInput = jump;
    }
}
