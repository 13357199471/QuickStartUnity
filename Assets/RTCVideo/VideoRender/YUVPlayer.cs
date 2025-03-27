using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using bytertc;

public class YUVPlayer : MonoBehaviour
{
    private Material renderMaterial = null;

    Texture2D textureY = null;
    Texture2D textureU = null;
    Texture2D textureV = null;

    float yCutPercent = 1.0f;
    float uCutPercent = 1.0f;
    float vCutPercent = 1.0f;

    private RectTransform rectTransform;
    private RawImage rawImage;

    private float originWidth;
    private float originHeight;
    private float yuvRatio;

    public RectTransform backgrounfTranform = null;
    int _VideoWidth = 0;
    int _VideoHeight = 0;
    int _Rotation = 0;
    int[] _LineSize; 
    void Start()
    {
        GetBgRect();

        rawImage = GetComponent<RawImage>();
        if (rawImage == null)
        {
            Debug.LogError("请设置渲染对象");
            return;
        }
        rawImage.enabled = false;
        var material = Instantiate(rawImage.material);
        rawImage.material = material;
        renderMaterial = material;
        _LineSize = new int[3];
    }

    private void GetBgRect()
    {
        rectTransform = GetComponent<RectTransform>();
        originWidth = rectTransform.rect.width;
        originHeight = rectTransform.rect.height;
        backgrounfTranform = rectTransform.parent.GetComponent<RectTransform>();
        if (backgrounfTranform != null)
        {
            originWidth = backgrounfTranform.sizeDelta.x;
            originHeight = backgrounfTranform.sizeDelta.y;
        }
    }

    private void InitYUVTexture(int[] lineSize, int height)
    {
        Reset();
        textureY = new Texture2D(lineSize[0], height, TextureFormat.Alpha8, false);
        textureU = new Texture2D(lineSize[1], height / 2, TextureFormat.Alpha8, false);
        textureV = new Texture2D(lineSize[2], height / 2, TextureFormat.Alpha8, false);
        rawImage.gameObject.SetActive(true);
    }

    bool LineSizeIsChange(int[] newLineSize)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_LineSize[i] != newLineSize[i])
            {
                return true;
            }
        }
        return false;
    }

    public void SetYUVFrame(VideoFrame frame)
    {
        if (_VideoWidth != frame.Width ||
        _VideoHeight != frame.Height ||
        _Rotation != (int)frame.Rotation||
        LineSizeIsChange(frame.PlaneLineSize))
        {
            InitYUVTexture(frame.PlaneLineSize, frame.Height);
            _VideoWidth = frame.Width;
            _VideoHeight = frame.Height;
            _Rotation = (int)frame.Rotation;
            _LineSize = frame.PlaneLineSize;
        }

        textureY.LoadRawTextureData(frame.PlaneData[0]);
        textureU.LoadRawTextureData(frame.PlaneData[1]);
        textureV.LoadRawTextureData(frame.PlaneData[2]);

        textureY.Apply();
        textureU.Apply();
        textureV.Apply();

        yCutPercent = (float)frame.Width / frame.PlaneLineSize[0];
        uCutPercent = (float)(frame.Width >> 1) / frame.PlaneLineSize[1];
        vCutPercent = (float)(frame.Width >> 1) / frame.PlaneLineSize[2];

        UpdateYUVRender();

        yuvRatio = (float)frame.Width / frame.Height;

        UpdateYUVRect();

    }
    private void UpdateYUVRect()
    {
        GetBgRect();
        rectTransform.localEulerAngles = new Vector3(0, 0, -_Rotation);
        if (_Rotation == 90 || _Rotation == 270)
        {
            yuvRatio = 1.0f / yuvRatio;
            if (yuvRatio * originHeight < originWidth)
            {
                rectTransform.sizeDelta = new Vector2(originHeight, yuvRatio * originHeight);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(originWidth / yuvRatio, originWidth);
            }
        }
        else
        {
            if (yuvRatio * originHeight < originWidth)
            {
                rectTransform.sizeDelta = new Vector2(yuvRatio * originHeight, originHeight);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(originWidth, originWidth / yuvRatio);
            }
        }
        rawImage.enabled = true;
    }
    private void UpdateYUVRender()
    {
        renderMaterial.SetTexture("_YTex", textureY);
        renderMaterial.SetTexture("_UTex", textureU);
        renderMaterial.SetTexture("_VTex", textureV);
        renderMaterial.SetFloat("_YCutPercent", yCutPercent);
        renderMaterial.SetFloat("_UCutPercent", uCutPercent);
        renderMaterial.SetFloat("_VCutPercent", vCutPercent);
    }

    public void Reset()
    {
        if (rawImage == null)
        {
            Start();
        }
        rawImage.enabled = true;
        rawImage.gameObject.SetActive(false);
        if (textureY != null)
        {
            Destroy(textureY);
            Destroy(textureU);
            Destroy(textureV);
        }
        _VideoWidth = 0;
        _VideoHeight = 0;
        
        yCutPercent = 1.0f;
        uCutPercent = 1.0f;
        vCutPercent = 1.0f;

        _Rotation = 0;

        rectTransform.localEulerAngles = new Vector3(0, 0, 0);

    }
    private void OnDestroy()
    {
        if (textureY != null)
        {
            Destroy(textureY);
            Destroy(textureU);
            Destroy(textureV);
        }
    }


}
