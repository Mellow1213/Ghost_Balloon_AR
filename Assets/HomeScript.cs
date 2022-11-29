using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("BalloonScene");
    }
}
