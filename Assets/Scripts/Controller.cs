using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;

using CVRect = OpenCVForUnity.CoreModule.Rect;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(WebCamTextureToMatHelper))]
public class Controller : MonoBehaviour
{
    public ResolutionPreset requestedResolution = ResolutionPreset._640x480;
    public FPSPreset requestedFPS = FPSPreset._30;
    Texture2D webcam;
    WebCamTextureToMatHelper webCamTextureToMatHelper;
    
    private CascadeClassifier headCascade;
    private CascadeClassifier palmCascade;

    private const string headCascadePath = "";
    private const string palmCascadePath = "";

    private List<Vector3> heads = new List<Vector3>();
    private List<Vector3> palms = new List<Vector3>();

    public List<Planet> Planets = new List<Planet>();
    // Start is called before the first frame update
    void Start()
    {
        webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper>();
        int width, height;
        Dimensions(requestedResolution, out width, out height);
        webCamTextureToMatHelper.requestedWidth = width;
        webCamTextureToMatHelper.requestedHeight = height;
        webCamTextureToMatHelper.requestedFPS = (int)requestedFPS;
        webCamTextureToMatHelper.Initialize();


        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            RectTransform rt = inputImage.GetComponent<RectTransform>();
            //rt.sizeDelta = new Vector2(720, 1280);
            //rt.localScale = new Vector3(1.6f, 1.6f, 1f);
            rt.sizeDelta = new Vector2(480, 640);
            rt.localScale = new Vector3(2.75f, 2.75f, 1f);
        }
        
        
        //////////////////////////////////////////
        // Cascade initiation
        //////////////////////////////////////////
        catCascade = new CascadeClassifier();
        catCascade.load(Utils.getFilePath(catCascadePath));
        if (catCascade.empty())
        {
            Debug.LogError("Cannot load cascade");
        }
        else
        {
            Debug.Log("Successfuly loaded cascade");
        }
        
        board.GetComponent<Image>().enabled = false;
        captureImage.GetComponent<RawImage>().enabled = false;
        cat.SetActive(false);
        classify.GetComponent<Button>().enabled = false;
        generate.GetComponent<Button>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
        {
            Mat webcam = webCamTextureToMatHelper.GetMat();
            if (Time.frameCount % 5 == 0)
            {
                // TODO: Detectar y actualizar
            }
        }
    
    
    
    
    
    
    ////////////////////////////////////////////
    /// This is for CAMERA only
    public void OnWebCamTextureToMatHelperInitialized()
    {
        Debug.Log("OnWebCamTextureToMatHelperInitialized");

        Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();

        webcam = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);

        Utils.fastMatToTexture2D(webCamTextureMat, webcam);

        gameObject.GetComponent<Renderer>().material.mainTexture = webcam;

        gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);
        Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

        float width = webCamTextureMat.width();
        float height = webCamTextureMat.height();

        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;
        if (widthScale < heightScale)
        {
            Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
        }
        else
        {
            Camera.main.orthographicSize = height / 2;
        }
    }
    public void OnWebCamTextureToMatHelperDisposed()
    {
        Debug.Log("OnWebCamTextureToMatHelperDisposed");

        if (webcam != null)
        {
            Texture2D.Destroy(webcam);
            webcam = null;
        }

        worker.Dispose();
    }
    public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode)
    {
        Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);

    }
    void OnDestroy()
    {
        webCamTextureToMatHelper.Dispose();
    }
    public void OnPlayButtonClick()
    {
        webCamTextureToMatHelper.Play();
    }
    public void OnPauseButtonClick()
    {
        webCamTextureToMatHelper.Pause();
    }
    public void OnStopButtonClick()
    {
        webCamTextureToMatHelper.Stop();
    }
    public void OnChangeCameraButtonClick()
    {
        webCamTextureToMatHelper.requestedIsFrontFacing = !webCamTextureToMatHelper.IsFrontFacing();
    }
    public void OnRequestedResolutionDropdownValueChanged(int result)
    {
        if ((int)requestedResolution != result)
        {
            requestedResolution = (ResolutionPreset)result;

            int width, height;
            Dimensions(requestedResolution, out width, out height);

            webCamTextureToMatHelper.Initialize(width, height);
        }
    }
    public void OnRequestedFPSDropdownValueChanged(int result)
    {
        string[] enumNames = Enum.GetNames(typeof(FPSPreset));
        int value = (int)System.Enum.Parse(typeof(FPSPreset), enumNames[result], true);

        if ((int)requestedFPS != value)
        {
            requestedFPS = (FPSPreset)value;

            webCamTextureToMatHelper.requestedFPS = (int)requestedFPS;
        }
    }
    public void OnRotate90DegreeToggleValueChanged()
    {
        if (rotate90DegreeToggle.isOn != webCamTextureToMatHelper.rotate90Degree)
        {
            webCamTextureToMatHelper.rotate90Degree = rotate90DegreeToggle.isOn;
        }
    }
    public void OnFlipVerticalToggleValueChanged()
    {
        if (flipVerticalToggle.isOn != webCamTextureToMatHelper.flipVertical)
        {
            webCamTextureToMatHelper.flipVertical = flipVerticalToggle.isOn;
        }


    }
    public void OnFlipHorizontalToggleValueChanged()
    {
        if (flipHorizontalToggle.isOn != webCamTextureToMatHelper.flipHorizontal)
        {
            webCamTextureToMatHelper.flipHorizontal = flipHorizontalToggle.isOn;
        }
    }
    public enum FPSPreset : int
    {
        _0 = 0,
        _1 = 1,
        _5 = 5,
        _10 = 10,
        _15 = 15,
        _30 = 30,
        _60 = 60,
    }
    public enum ResolutionPreset : byte
    {
        _50x50 = 0,
        _640x480,
        _1280x720,
        _1920x1080,
        _9999x9999,
    }
    private void Dimensions(ResolutionPreset preset, out int width, out int height)
    {
        switch (preset)
        {
            case ResolutionPreset._50x50:
                width = 50;
                height = 50;
                break;
            case ResolutionPreset._640x480:
                width = 640;
                height = 480;
                break;
            case ResolutionPreset._1280x720:
                width = 1280;
                height = 720;
                break;
            case ResolutionPreset._1920x1080:
                width = 1920;
                height = 1080;
                break;
            case ResolutionPreset._9999x9999:
                width = 9999;
                height = 9999;
                break;
            default:
                width = height = 0;
                break;
        }
    }

}