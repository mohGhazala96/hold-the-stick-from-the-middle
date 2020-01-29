using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;


public class controlText : MonoBehaviour {
    public Text TeamWork;
    public Text Honesty;
    public Text respectAuthority;
    public Text Independence;
    public Text Ambition;
    public Text StartBtn;

    // Use this for initialization
    void Start () {
        if(TeamWork != null)
        TeamWork.text = ArabicFixer.Fix("متعاون");
        if (Honesty != null)
            Honesty.text = ArabicFixer.Fix("أمين");
        if (respectAuthority != null)
            respectAuthority.text = ArabicFixer.Fix("يحترم السلم الوظيفى");
        if (Independence != null)
            Independence.text = ArabicFixer.Fix("مستقل");
        if (Ambition != null)
            Ambition.text = ArabicFixer.Fix("طموح");
        if (StartBtn != null)
            StartBtn.text = ArabicFixer.Fix("ابدأ");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
