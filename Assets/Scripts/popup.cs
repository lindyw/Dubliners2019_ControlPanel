using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class popup : MonoBehaviour {

    public Text mTitle;
    public Text sTitle;

    public void Fade(bool b)
    {
        this.GetComponent<Animator>().SetBool("Close", b);
    }

    public void updateTitle()
    {
        sTitle.text = mTitle.text;
    }

}
