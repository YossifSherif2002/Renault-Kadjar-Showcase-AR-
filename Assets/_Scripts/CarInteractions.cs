using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class CarInteractions : MonoBehaviour
{
    [Header("Car Parts")]
    [SerializeField] private Transform left_front_door;
    [SerializeField] private Transform left_back_door;
    [SerializeField] private Transform right_front_door;
    [SerializeField] private Transform right_back_door;
    [SerializeField] private Transform trunk;
    [SerializeField] private Transform double_backseats;
    [SerializeField] private Transform single_backseat;

    [SerializeField] private GameObject car;
    [SerializeField] private Material headlights_mat;
    [SerializeField] private Material car_mat;
    [SerializeField] private List<Sprite> whole_car_interaction_btn_sprites;
    [SerializeField] private List<Sprite> lights_btn_sprites;
    [SerializeField] private List<GameObject> backseats_indicators;
    [SerializeField] private InputActionReference Rotate;
    [SerializeField] private InputActionReference Pinch;

    private bool left_f_door_opened;
    private bool left_b_door_opened;
    private bool right_f_door_opened;
    private bool right_b_door_opened;
    private bool trunk_opened;
    private bool car_opened;
    private bool lights_on;
    private bool double_backseats_open;
    private bool single_backseat_open;
    private bool is_changeing_color;

    private void Awake()
    {
        Rotate.action.performed += OnRotate;
        Pinch.action.performed += OnZoom;
        Rotate.action.Enable();
        Pinch.action.Enable();
    }

    private void Start()
    {
        left_f_door_opened = false;
        left_b_door_opened = false;
        right_f_door_opened = false;
        right_b_door_opened = false;
        trunk_opened = false;
        double_backseats_open = false;
        single_backseat_open = false;
        car_opened = false;
        lights_on = false;
        is_changeing_color = false;

        car_mat.color = Color.white;

        for (int i = 0; i < backseats_indicators.Count; i++)
        {
            backseats_indicators[i].SetActive(false);
        }

        MiddleMan.Instance.whole_car_interaction_btn.onClick.AddListener(WholeCarInteraction);
        MiddleMan.Instance.lights_btn.onClick.AddListener(LightsInteraction);
        MiddleMan.Instance.red_btn.onClick.AddListener(() => ChangeColor(Color.red));
        MiddleMan.Instance.black_btn.onClick.AddListener(() => ChangeColor(Color.black));
        MiddleMan.Instance.white_btn.onClick.AddListener(() => ChangeColor(Color.white));
        MiddleMan.Instance.grey_btn.onClick.AddListener(() => ChangeColor(Color.grey));
        MiddleMan.Instance.blue_btn.onClick.AddListener(() => ChangeColor(Color.blue));
    }

    void OnZoom(InputAction.CallbackContext ctx)
    {
        var touches = Touchscreen.current.touches;

        if (touches.Count(t => t.isInProgress) < 2)
            return;

        var touch0 = touches[0];
        var touch1 = touches[1];

        if (!touch0.isInProgress || !touch1.isInProgress)
            return;

        Vector2 touch0Pos = touch0.position.ReadValue();
        Vector2 touch1Pos = touch1.position.ReadValue();

        Vector2 touch0Prev = touch0Pos - touch0.delta.ReadValue();
        Vector2 touch1Prev = touch1Pos - touch1.delta.ReadValue();

        float prevDistance = Vector2.Distance(touch0Prev, touch1Prev);
        float currentDistance = Vector2.Distance(touch0Pos, touch1Pos);
        float deltaDistance = currentDistance - prevDistance;

        
        float zoomSpeed = 0.01f;
        car.transform.localScale += Vector3.one * deltaDistance * zoomSpeed;

        float minScale = 0.5f;
        float maxScale = 3f;
        car.transform.localScale = Vector3.one * Mathf.Clamp(car.transform.localScale.x, minScale, maxScale);
    }

    void OnRotate(InputAction.CallbackContext ctx)
    {
        var touches = Touchscreen.current.touches;

        if (touches.Count(t => t.isInProgress) > 1)
            return;

        Vector2 delta = ctx.ReadValue<Vector2>();
        car.transform.Rotate(Vector3.up, -delta.x * 0.2f, Space.World);
    }



    public void LeftFrontDoorInteraction()
    {
        if (!left_f_door_opened)
        {
            left_front_door.DOLocalRotate(new Vector3(-90, 0, 60), 0.5f).SetEase(Ease.InOutSine);
            left_f_door_opened = true;
        }
        else
        {
            left_front_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            left_f_door_opened = false;
        }
    }

    public void RightFrontDoorInteraction()
    {
        if (!right_f_door_opened)
        {
            right_front_door.DOLocalRotate(new Vector3(-90, 0, -60), 0.5f).SetEase(Ease.InOutSine);
            right_f_door_opened = true;
        }
        else
        {
            right_front_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            right_f_door_opened = false;
        }
    }

    public void LeftBackDoorInteraction()
    {
        if (!left_b_door_opened)
        {
            left_back_door.DOLocalRotate(new Vector3(-90, 0, 60), 0.5f).SetEase(Ease.InOutSine);
            left_b_door_opened = true;
        }
        else
        {
            left_back_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            left_b_door_opened = false;
        }
    }

    public void RightBackDoorInteraction()
    {
        if (!right_b_door_opened)
        {
            right_back_door.DOLocalRotate(new Vector3(-90, 0, -60), 0.5f).SetEase(Ease.InOutSine);
            right_b_door_opened = true;
        }
        else
        {
            right_back_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            right_b_door_opened = false;
        }
    }


    public void TrunkInteraction()
    {
        if (!trunk_opened)
        {
            trunk.DOLocalRotate(new Vector3(-165, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            trunk_opened = true;
            for (int i = 0; i < backseats_indicators.Count; i++)
            {
                backseats_indicators[i].SetActive(true);
            }
        }
        else
        {
            trunk.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            trunk_opened = false;
            for (int i = 0; i < backseats_indicators.Count; i++)
            {
                backseats_indicators[i].SetActive(false);
            }
        }
    }
    

    public void DoubleBackSeatsInteraction()
    {
        if (trunk_opened)
        {
            if (!double_backseats_open)
            {
                double_backseats.DOLocalRotate(new Vector3(-210, 0, 0), 0.5f).SetEase(Ease.InOutSine);
                double_backseats_open = true;
            }
            else
            {
                double_backseats.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
                double_backseats_open = false;
            }
        }
    }

    public void SingleBackSeatInteraction()
    {
        if (trunk_opened)
        {
            if (!single_backseat_open)
            {
                single_backseat.DOLocalRotate(new Vector3(-210, 0, 0), 0.5f).SetEase(Ease.InOutSine);
                single_backseat_open = true;
            }
            else
            {
                single_backseat.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
                single_backseat_open = false;
            }
        }
    }


    public void WholeCarInteraction()
    {
        car_opened = !car_opened; 

        if (car_opened)
        {
            left_front_door.DOLocalRotate(new Vector3(-90, 0, 60), 0.5f).SetEase(Ease.InOutSine);
            left_back_door.DOLocalRotate(new Vector3(-90, 0, 60), 0.5f).SetEase(Ease.InOutSine);
            right_front_door.DOLocalRotate(new Vector3(-90, 0, -60), 0.5f).SetEase(Ease.InOutSine);
            right_back_door.DOLocalRotate(new Vector3(-90, 0, -60), 0.5f).SetEase(Ease.InOutSine);
            trunk.DOLocalRotate(new Vector3(-165, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            double_backseats.DOLocalRotate(new Vector3(-210, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            single_backseat.DOLocalRotate(new Vector3(-210, 0, 0), 0.5f).SetEase(Ease.InOutSine);

            for (int i = 0; i < backseats_indicators.Count; i++)
            {
                backseats_indicators[i].SetActive(true);
            }

            MiddleMan.Instance.whole_car_interaction_btn.GetComponent<Image>().sprite = whole_car_interaction_btn_sprites[1];

            left_f_door_opened = left_b_door_opened = right_f_door_opened = right_b_door_opened = trunk_opened = double_backseats_open = single_backseat_open = true;
        }
        else
        {
            left_front_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            left_back_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            right_front_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            right_back_door.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            trunk.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            double_backseats.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);
            single_backseat.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetEase(Ease.InOutSine);

            for (int i = 0; i < backseats_indicators.Count; i++)
            {
                backseats_indicators[i].SetActive(false);
            }

            MiddleMan.Instance.whole_car_interaction_btn.GetComponent<Image>().sprite = whole_car_interaction_btn_sprites[0];

            left_f_door_opened = left_b_door_opened = right_f_door_opened = right_b_door_opened = trunk_opened = double_backseats_open = single_backseat_open = false;
        }
    }

    public void LightsInteraction()
    {
        lights_on = !lights_on;

        if (!lights_on)
        {
            headlights_mat.EnableKeyword("_EMISSION");
            MiddleMan.Instance.lights_btn.GetComponent<Image>().sprite = lights_btn_sprites[1];
        }
        else
        {
            headlights_mat.DisableKeyword("_EMISSION");
            MiddleMan.Instance.lights_btn.GetComponent<Image>().sprite = lights_btn_sprites[0];
        }
    }

    void ChangeColor(Color new_color)
    {
        if (car_mat.color == new_color || is_changeing_color)
            return;

        DOTween.Kill(car_mat);

        is_changeing_color = true;

        car_mat.DOColor(Color.white, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            car_mat.DOColor(new_color, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                is_changeing_color = false;
            });
        });
    }
}
