using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPadTask : MonoBehaviour
{
    public Text cardCode;

    public Text inputCode;

    public int codeLength = 6;

    public float codeTimeResetInSeconds = 0.5f;

    private bool isReseting = false;

    private void OnEnable()
    {
        string code = string.Empty;

        for (int i = 0; i < codeLength; i++)
        {
            code += Random.Range(1, 10);
        }

        cardCode.text = code;
        inputCode.text = string.Empty;
    }

    public void ButtonClick(int number)
    {
        if (isReseting) { return; }

        inputCode.text += number;
        
        if (inputCode.text == cardCode.text)
        {
            inputCode.text = "Success";
            StartCoroutine(ResetCode());
        }
        else if (inputCode.text.Length >= codeLength)
        {
            inputCode.text = "Failed";
            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator ResetCode()
    {
        isReseting = true;

        yield return new WaitForSeconds(codeTimeResetInSeconds);

        inputCode.text = string.Empty;

        isReseting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
