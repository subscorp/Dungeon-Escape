/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;

public class Playgameservices : MonoBehaviour
{
    [SerializeField]
    Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        //PlayGamesPlatform.Instance
        PlayGamesPlatform.Activate();
        debugText.text = "playgames initialized";
        SignInUserWithPlaygames();
    }

    private void SignInUserWithPlaygames()
    {
        /*
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (success) =>
        {
            if (SignInStatus.Success != 0)

                    debugText.text = "sign in successfully";
            else
                    debugText.text = "sign in was not successfull";

        });
        */

/*
    }

    public void AchivementCompleted()
    {
        //Social.ReportProgress()
    }
}

*/