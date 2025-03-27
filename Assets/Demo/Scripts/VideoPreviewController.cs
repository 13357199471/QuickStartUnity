using bytertc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoPreviewController : MonoBehaviour {

    private YUVPlayer _YUVPlayer;
    private Text _UserIDText;
    private RemoteStreamKey _RemoteStreamKey;
    bool _IsActive = false;
    public void SetRemoteStreamKey(RemoteStreamKey key)
    {
        _RemoteStreamKey = key;
        if (_UserIDText)
        {
            string text = _RemoteStreamKey.UserID;
            if (key.streamIndex == StreamIndex.kStreamIndexScreen)
            {
                text += "_screen";
            }
            _UserIDText.text = text;
        }
    }

    public RemoteStreamKey GetRemoteStreamKey()
    {
        return _RemoteStreamKey;
    }

    public void SetActive(bool bActive)
    {
        gameObject.SetActive(bActive);
        _IsActive = bActive;
        if (_IsActive == false)
        {
            Reset();
        }
    }
    public bool GetActive()
    {
        return _IsActive;
    }
    public void Reset()
    {
        _RemoteStreamKey.RoomID = "";
        _RemoteStreamKey.UserID = "";
        if (_UserIDText != null)
        {
            _UserIDText.text = "";
            _YUVPlayer.Reset();
        }
    }
    // Use this for initialization
    void Awake ()
    {
        InitUI();
    }

    void InitUI()
    {
        _YUVPlayer = transform.Find("LayoutVideo/YUVPlayerBackGround/RawImage")?.GetComponent<YUVPlayer>();
        _UserIDText = transform.Find("LayoutTip/Text")?.GetComponent<Text>();
    }

    public void SetYUVFrame(VideoFrame frame)
    {
        _YUVPlayer?.SetYUVFrame(frame);
    }
}
