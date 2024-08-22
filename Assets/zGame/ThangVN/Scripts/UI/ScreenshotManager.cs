using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    public static ScreenshotManager Instance;
    public Image targetImage;
    private string screenshotFilePath;
    public Camera screenshotCamera;
    int screenshotWidth;
    int screenshotHeight;
    private void Awake()
    {
        Instance = this;
    }


    public void CaptureScreenshot()
    {
        screenshotFilePath = Path.Combine(Application.dataPath, $"Resources/Image/room{SaveGame.CurrentRoom}.png");
        screenshotWidth = Screen.width;
        screenshotHeight = Screen.height;

        ScreenCapture.CaptureScreenshot(screenshotFilePath);
    }

    public void LoadScreenshot(Image img, int idRoom)
    {
        //screenshotFilePath = Path.Combine(Application.dataPath, $"Resources/Image/room{idRoom}.png");
        screenshotFilePath = Path.Combine(Application.persistentDataPath, $"room{SaveGame.CurrentRoom}.png");

        StartCoroutine(LoadScreenshot(img));

    }

    IEnumerator LoadScreenshot(Image img)
    {
        while (!File.Exists(screenshotFilePath))
        {
            yield return null;
        }

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + screenshotFilePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;
            }
        }
    }

    public void CaptureScreenshotWithoutUI()
    {
        //screenshotFilePath = Path.Combine(Application.dataPath, $"Resources/Image/room{SaveGame.CurrentRoom}.png");
        screenshotFilePath = Path.Combine(Application.persistentDataPath, $"room{SaveGame.CurrentRoom}.png");

        screenshotWidth = Screen.width;
        screenshotHeight = Screen.height;

        StartCoroutine(CaptureScreenshotCoroutine());
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        screenshotCamera.targetTexture = rt;

        screenshotCamera.Render();

        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        screenshot.Apply();

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(screenshotFilePath, bytes);

        Debug.Log($"Screenshot saved to: {screenshotFilePath}");

        yield return null;
    }
}
