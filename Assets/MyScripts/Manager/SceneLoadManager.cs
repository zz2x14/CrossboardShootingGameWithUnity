using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : PersisentSingletonTool<SceneLoadManager>
{
    private const string SCENENAME_GAMEPLAY = "Gameplay";

    [SerializeField] private Image faderImage;
    [SerializeField] private float faderTime;

    private Color faderColor;

    IEnumerator LoadSceneCor(string sceneName)
    {
        faderImage.gameObject.SetActive(true);

        var loadSceneOpreation = SceneManager.LoadSceneAsync(sceneName);
        //使用场景异步加载 - 该bool为false时，场景不会完全加载完毕，达到等到淡入淡出效果结束后进入下个场景
        loadSceneOpreation.allowSceneActivation = false;

        while(faderColor.a < 1f)
        {
            //淡出 - 透明度逐渐增加 - 限制在合理范围内
            faderColor.a = Mathf.Clamp(faderColor.a + 1 / faderTime * Time.unscaledDeltaTime, 0f, 1f);
            faderImage.color = faderColor;
            yield return null;
        }

        loadSceneOpreation.allowSceneActivation = true; //淡出结束后让场景正确加载完毕

        while (faderColor.a > 0f)
        {
            //淡出 - 透明度逐渐增加 - 限制在合理范围内
            faderColor.a = Mathf.Clamp(faderColor.a - 1 / faderTime * Time.unscaledDeltaTime, 0f, 1f);
            faderImage.color = faderColor;
            yield return null;
        }

        faderImage.gameObject.SetActive(false);
    }


    public void LoadGameplayScene()
    {
        StartCoroutine(LoadSceneCor(SCENENAME_GAMEPLAY));
    }
}
