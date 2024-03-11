using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Uiwadawa : MonoBehaviour
{
    public static Uiwadawa ins;
    public Text da;
    private void Awake()
    {
        ins = this;
    }
    public void UpdateUi(int bul)
    {
        da.text = bul.ToString();
    }
}
