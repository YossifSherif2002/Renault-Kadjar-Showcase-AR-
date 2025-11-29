using UnityEngine;

public class ColorBtnInteraction : MonoBehaviour
{
    [SerializeField] private Animator colors_btn_anim;

    private bool colors_btns_active;

    private void Start()
    {
        colors_btns_active = false;
    }


    public void ColorsBtnSintearction()
    {
        if (!colors_btns_active)
        {
            colors_btn_anim.Play("ColorsBtnsInteraction 0");
            colors_btns_active = true;
        }
        else
        {
            colors_btn_anim.Play("ColorsBtnsInteraction");
            colors_btns_active = false;
        }
    }
}
