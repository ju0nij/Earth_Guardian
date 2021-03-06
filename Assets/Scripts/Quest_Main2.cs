using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class Quest_Main2 : MonoBehaviour
{
    [SerializeField]
    public Choice[] choices;
    public int[] answers;
    public string[] alerts;
    public Information_Cont theInfo;
    public ChoiceManager selection;
    private int count;
    public bool isAble = false;

    private int correctState = 0;
    private bool done = false;
    private LocalDBManager gameData;

    private PlayerManager thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        if (!LocalDBManager.Instance.isQuestCleared(2) && thePlayer.beforeMapName == "Prov_Info2")
        {
            isAble = true;
            thePlayer.questMode = true;
            theInfo.Load();
            selection.ShowChoice(choices[0]);
        }
        else selection = null;
    }

    public bool isOpenedDialog()
    {
        return theInfo.isOpenedDialog();
    }
    void Trans()
    {
        thePlayer.questMode = false;
        thePlayer.currentMapName = "M-2";
        thePlayer.beforeMapName = "Q-4";
        count = 0; correctState = 0;
        theInfo.Disappear();
        //Object.Destroy(quest.theInfo);
        SceneManager.LoadScene("M-2");
    }

    void Finish()
    {
        FadeObject obj = FindObjectOfType<FadeObject>();
        count++;
        if (theInfo.popup.GetBool("Appear") == false && done == false)
        {
            if (correctState == choices.Length)
            {
                theInfo.SetTitle("Quest Cleared!");
                theInfo.SetContent("You have cleared this stage! Congratulations. If you didn't cleared other stage, you should clear it!");
                theInfo.Appear();
            }
            else
            {
                theInfo.SetTitle("A little bit more...");
                theInfo.SetContent("You made some wrong answers.. you can retry this quest.");
                theInfo.Appear();
            }
            done = true;
        }
        if (done == true && !theInfo.isOpened)
        {
            if (correctState == choices.Length)
                LocalDBManager.Instance.QuestClear(2);
            obj.FadeIn(obj.fadeTime);
            Trans();
        }
    }

    private void Correct(int round)
    {
        correctState++;
        theInfo.SetTitle("Correct!");
        switch (round)
        {
            case 0:
                theInfo.SetContent(alerts[0]);
                theInfo.Appear();
                break;
            case 1:
                theInfo.SetContent(alerts[1]);
                theInfo.Appear();
                break;
            case 2:
                theInfo.SetContent(alerts[2]);
                theInfo.Appear();
                break;
            default:
                break;

        }
    }

    void Wrong(int round)
    {
        theInfo.SetTitle("Wrong..");
        switch (round)
        {
            case 0:
                theInfo.SetContent(alerts[0]);
                theInfo.Appear();
                break;
            case 1:
                theInfo.SetContent(alerts[1]);
                theInfo.Appear();
                break;
            case 2:
                theInfo.SetContent(alerts[2]);
                theInfo.Appear();
                break;
            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAble)
        {
            if (selection.isActive == false)
            {
                if (count < choices.Length)
                {
                    if (selection.GetResult() == answers[count])
                    {
                        Correct(count);
                    }
                    else Wrong(count);
                }
                if (count < choices.Length - 1)
                {
                    count++;
                    selection.ShowChoice(choices[count]);
                }
                else Finish();
            }
        }
    }
}


