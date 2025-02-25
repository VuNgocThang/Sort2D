#if NATIVE_AD_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum Dimension
{
    Width,
    Height,
    Both
}

public class ItemAdNative : MonoBehaviour
{
    [SerializeField] private Dimension fitBg;
    [SerializeField] private Vector2 limitFitSize;
    [SerializeField] private bool keepBgSize;

    [SerializeField] private Image bg;

    [SerializeField] RawImage adIconGame; //icon
    [SerializeField] RawImage adChoices; //icon ad
    [SerializeField] RawImage adImage; //logo
    [SerializeField] Text txtAdHeadline;
    [SerializeField] Text txtAdBody;

    [SerializeField] Text txtAdAdvertiser;
    [SerializeField] Text txtAdCallToAction;
    [SerializeField] GameObject btnCallAction;


    [SerializeField] private GameObject noAdCover;

    [SerializeField] private Canvas canvas;
    [SerializeField] private bool autoUseBgSize;
    [SerializeField] private RectTransform itemRect;
    private float originalHeight = 0f;
    private bool isFirstSetupCallback = false;

    private void OnValidate()
    {
        if (autoUseBgSize && bg != null)
        {
            autoUseBgSize = false;
            limitFitSize = bg.rectTransform.sizeDelta;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        originalHeight = itemRect.sizeDelta.y;
        if (canvas != null)
        {
            //canvas.worldCamera = CameraController.Instance.GameCamera;
        }
    }
#if NATIVE_AD_ADMOB
    public void Parse(NativeAd nativeAd)
    {
        if (noAdCover != null)
        {
            if (nativeAd == null)
            {
                noAdCover.SetActive(true);
                return;
            }
            else
            {
                noAdCover.SetActive(false);
            }
        }

        List<Texture2D> listImages = nativeAd.GetImageTextures();
         if (Config.ACTIVE_DEBUG_LOG)
        {
        for (int i = 0; i < listImages.Count; i++)
        {
            Debug.Log("Image " + i + ": " + listImages[i].width + " : " + listImages[i].height);
        }
        }
        if (listImages.Count > 0 && adImage != null)
        {
            if (canvas != null)
            {
                canvas.gameObject.SetActive(true);
            }

            int rand = Random.Range(0, listImages.Count);
            float ratio = listImages[rand].width / (float)listImages[rand].height;
            Vector2 size = new Vector2(listImages[rand].width, listImages[rand].height);
            switch (fitBg)
            {
                case Dimension.Width:
                    size = new Vector2(bg.rectTransform.sizeDelta.x, bg.rectTransform.sizeDelta.x / ratio);
                    if (size.y > limitFitSize.y && limitFitSize.y > 0)
                    {
                        size.y = limitFitSize.y;
                        size.x = size.y * ratio;
                    }

                    break;
                case Dimension.Height:
                    size = new Vector2(ratio * bg.rectTransform.sizeDelta.y, bg.rectTransform.sizeDelta.y);
                    if (size.x > limitFitSize.x && limitFitSize.x > 0)
                    {
                        size.x = limitFitSize.x;
                        size.y = size.x / ratio;
                    }

                    break;
                case Dimension.Both:
                    float bgRatio = bg.rectTransform.sizeDelta.x / bg.rectTransform.sizeDelta.y;
                    if (ratio > bgRatio)
                    {
                        size = new Vector2(bg.rectTransform.sizeDelta.x, bg.rectTransform.sizeDelta.x / ratio);
                    }
                    else
                    {
                        size = new Vector2(bg.rectTransform.sizeDelta.y * ratio, bg.rectTransform.sizeDelta.y);
                    }

                    break;
                default:
                    break;
            }

            /*Debug.Log("ratio: " + ratio);
            Debug.Log("in-game image size: " + size);*/
            adImage.texture = listImages[rand];
            //adImage.rectTransform.sizeDelta = size - new Vector2(20, 20);
            //BoxCollider2D box = adImage.gameObject.GetComponent<BoxCollider2D>();
            //if (box == null)
            //{
            //    box = adImage.gameObject.AddComponent<BoxCollider2D>();
            //}
            //box.size = adImage.rectTransform.sizeDelta;
            if (!keepBgSize)
            {
                bg.rectTransform.sizeDelta = size;
            }

            if (!isFirstSetupCallback)
            {
                if (nativeAd.RegisterImageGameObjects(new List<GameObject>() { adImage.gameObject }) == 0)
                {
                    // Handle failure to register ad asset.
                    if (Config.ACTIVE_DEBUG_LOG)
        {
                    Debug.Log("admob native: failed to register image textures.");
                    }
                }

                if (!nativeAd.RegisterAdChoicesLogoGameObject(adChoices.gameObject))
                {
                    // Handle failure to register ad asset.
                    if (Config.ACTIVE_DEBUG_LOG)
                    {
                    Debug.Log("admob native: failed to register adchoice logo.");
                    }
                }

                if (!nativeAd.RegisterIconImageGameObject(adIconGame.gameObject))
                {
                    // Handle failure to register ad asset.
                    if (Config.ACTIVE_DEBUG_LOG)
                    {
                    Debug.Log("admob native: failed to register Icon .");
                    }
                }

                if (!nativeAd.RegisterCallToActionGameObject(btnCallAction.gameObject))
                {
                    // Handle failure to register ad asset.
                    if (Config.ACTIVE_DEBUG_LOG)
                    {
                    Debug.Log("admob native: failed to register Icon call to action .");
                    }
                }
            }
        }
        else
        {
            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        firstSetup = false;
        Destroy(gameObject);
    }

    private bool firstSetup = false;
    void Update()
    {
        if (AppOpenAdManager.Instance.nativeAd == null)
        {
            itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x, 0f);
            itemRect.localScale = Vector3.zero;
            return;
        }

        if (!firstSetup || AppOpenAdManager.Instance.nativeAdLoaded)
        {
            if (AppOpenAdManager.Instance.nativeAdLoaded)
            {
                isFirstSetupCallback = false;
            }

            firstSetup = true;
            //AppOpenAdManager.Instance.nativeAdLoaded = false;
            //get data
            Texture2D image = AppOpenAdManager.Instance.nativeAd.GetImageTextures()[0];
            Texture2D adchoice = AppOpenAdManager.Instance.nativeAd.GetAdChoicesLogoTexture();
            Texture2D iconTexture = AppOpenAdManager.Instance.nativeAd.GetIconTexture();
            string headline = AppOpenAdManager.Instance.nativeAd.GetHeadlineText();
            string bodyText = AppOpenAdManager.Instance.nativeAd.GetBodyText();
            string advertiser = AppOpenAdManager.Instance.nativeAd.GetAdvertiserText();
            string cta = AppOpenAdManager.Instance.nativeAd.GetCallToActionText();


            //assign values
            if (adIconGame != null)
            {
                adIconGame.texture = iconTexture;
            }

            if (adImage != null)
            {
                adImage.texture = image;
            }

            if (adChoices != null)
                adChoices.texture = adchoice;
            if (txtAdHeadline != null)
                txtAdHeadline.text = headline;
            if (txtAdBody != null)
                txtAdBody.text = bodyText;
            if (txtAdAdvertiser != null)
                txtAdAdvertiser.text = advertiser;
            if (txtAdCallToAction != null)
                txtAdCallToAction.text = cta;
            Parse(AppOpenAdManager.Instance.nativeAd);

            //Texture2D iconTexture = AppOpenAdManager.Instance.nativeAd.GetIconTexture();

            //adIconGame = GameObject.CreatePrimitive(PrimitiveType.Quad);
            //adIconGame.transform.position = new Vector3(1, 1, 1);
            //adIconGame.transform.localScale = new Vector3(1, 1, 1);
            //adIconGame.GetComponent<Renderer>().material.mainTexture = iconTexture;

            //// Register GameObject that will display icon asset of native ad.
            //if (!AppOpenAdManager.Instance.nativeAd.RegisterIconImageGameObject(adIconGame.gameObject))
            //{
            //    // Handle failure to register ad asset.
            //    Debug.Log("admob native: failed to register adchoice logo.");
            //}
        }
    }
#endif

    //private bool nativeAdLoaded;
    //private NativeAd nativeAd;
    //public void RequestNativeAd()
    //{
    //    AdLoader adLoader = new AdLoader.Builder(ConfigIdsAds._adNativeId)
    //        .ForNativeAd()
    //        .Build();
    //    adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
    //    adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
    //    adLoader.LoadAd(new AdRequest.Builder().Build());
    //}

    //private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //{
    //    Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
    //}
    //private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    //{
    //    Debug.Log("Native ad loaded.");
    //    this.nativeAd = args.nativeAd;
    //    this.nativeAdLoaded = true;
    //}
}