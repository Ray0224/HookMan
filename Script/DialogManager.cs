using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Vector2 dialogBoxOffset;
    public GameObject dialogBox;
    public Transform targetPos;
    public Text text;
    public TextAsset[] textFiles;
    public TextAsset[] ReuseFiles;
    public List<string> dialogText;
    public int index;
    public float wordFinishTime;
    public float lineFinishTime;
    public bool textFinished;
    // Start is called before the first frame update
    void Start()
    {
        textFinished = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        dialogBox.transform.position = new Vector3(targetPos.position.x + dialogBoxOffset.x, targetPos.position.y + dialogBoxOffset.y,0);
    }
    public void ShowDialogBox(TextAsset file)
    {
        ParseTextFile(file);
        StartCoroutine(ParseText());
    }
    public void CloseOldDialogBox()
    {
        textFinished = true;
        dialogBox.SetActive(false);
    }
    void ParseTextFile(TextAsset file)
    {
        dialogText.Clear();
        index = 0;
        var lineText = file.text.Split('\n');
        foreach(var line in lineText)
        {
            dialogText.Add(line);
        }
    }
    IEnumerator ParseText()
    {
        yield return new WaitForSecondsRealtime(lineFinishTime+0.1f);
        dialogBox.SetActive(true);
        textFinished = false;
        text.text = "";
        for (int i = 0; i < dialogText[index].Length; i++)
        {
            if(textFinished == false)
            {
                text.text += dialogText[index][i];
                yield return new WaitForSecondsRealtime(wordFinishTime);
            }
            else
            {
                yield break;
            }
        }
        yield return new WaitForSecondsRealtime(lineFinishTime);
        if(textFinished == false)
        {
            index++;
            if (index == dialogText.Count)
            {
                dialogBox.SetActive(false);
                index = 0;
            }
            else
            {
                StartCoroutine(ParseText());
            }
        }
        else
        {
            yield break;
        }
        
        
    }

}
