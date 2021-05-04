using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName;
    public string startPos;

    [Header("Animation Components")]
    private UIManager uim;

    private PlayerController pc;

    private DataSaver dataSaver;
    public void Start()
    {
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        uim = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        dataSaver = GameObject.FindWithTag("DataSaver").GetComponent<DataSaver>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            pc.startPos = startPos;
            StartCoroutine(SceneTransition());
            dataSaver.SaveScene();
        }
    }

    private IEnumerator SceneTransition()
    {
        uim.SceneTransitionFadeIn();
        yield return new WaitForSecondsRealtime(uim.anim.speed + 0.1f); //(+0.1) para que a personagem se consiga teleportar com a OnLoadScene do PlayerController
        SceneManager.LoadScene(sceneName);
        uim.SceneTransitionFadeOut();
    }
}
