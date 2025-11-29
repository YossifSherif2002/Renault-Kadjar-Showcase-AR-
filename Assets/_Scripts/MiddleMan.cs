using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddleMan : MonoBehaviour
{
    public static MiddleMan Instance;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    public Button whole_car_interaction_btn;
    public Button lights_btn;
    public Button red_btn;
    public Button blue_btn;
    public Button black_btn;
    public Button white_btn;
    public Button grey_btn;
}
