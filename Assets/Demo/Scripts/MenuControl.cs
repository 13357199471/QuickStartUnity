using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bytertc
{
    public class MenuControl : MonoBehaviour
    {
        public Button audioBtn;
        public Button cameraBtn;
        public Button voiceBtn;
        public Image audioImage;
        public Image cameraImage;
        public Image voiceImage;

        private Dictionary<Image, bool> imageMap = new Dictionary<Image, bool>();
        void Start()
        {
            audioBtn = transform.Find("AudioBtn")?.GetComponent<Button>();
            audioBtn.onClick.AddListener(OpenAudioClick);
            cameraBtn = transform.Find("CameraBtn")?.GetComponent<Button>();
            cameraBtn.onClick.AddListener(OpenCameraClick);
            voiceBtn = transform.Find("VoiceBtn")?.GetComponent<Button>();
            voiceBtn.onClick.AddListener(OpenVoiceClick);

            audioImage = transform.Find("AudioBtn")?.GetComponent<Image>();
            cameraImage = transform.Find("CameraBtn")?.GetComponent<Image>();
            voiceImage = transform.Find("VoiceBtn")?.GetComponent<Image>();
            imageMap.Add(audioImage, true);
            imageMap.Add(cameraImage, true);
            imageMap.Add(voiceImage, true);
        }
        public void OpenAudioClick()
        {
            Image currentImage = audioImage;
            bool isOpen = imageMap[currentImage];
            imageMap[currentImage] = !isOpen;
            if (isOpen)
            {
                currentImage.sprite = Resources.Load("audio_disable", typeof(Sprite)) as Sprite;
                SpriteState spriteBtnState;
                spriteBtnState.highlightedSprite = null;
                audioBtn.spriteState = spriteBtnState;

                EngineManager.instance.GetVideoEngine().StopAudioCapture();
                EngineManager.instance.GetVideoRoom().UnpublishStream(MediaStreamType.kMediaStreamTypeAudio);
            }
            else
            {
                currentImage.sprite = Resources.Load("audio", typeof(Sprite)) as Sprite;
                SpriteState spriteBtnState;
                spriteBtnState.highlightedSprite = Resources.Load("audio_hover", typeof(Sprite)) as Sprite;
                audioBtn.spriteState = spriteBtnState;

                EngineManager.instance.GetVideoEngine().StartAudioCapture();
                EngineManager.instance.GetVideoRoom().PublishStream(MediaStreamType.kMediaStreamTypeAudio);
            }
        }
        public void OpenCameraClick()
        {
            Image currentImage = cameraImage;
            bool isOpen = imageMap[currentImage];
            imageMap[currentImage] = !isOpen;
            if (isOpen)
            {
                currentImage.sprite = Resources.Load("video_disable", typeof(Sprite)) as Sprite;
                SpriteState spriteBtnState;
                spriteBtnState.highlightedSprite = null;
                cameraBtn.spriteState = spriteBtnState;
                EngineManager.instance.GetVideoEngine().StopVideoCapture();
            }
            else
            {
                currentImage.sprite = Resources.Load("video", typeof(Sprite)) as Sprite;
                SpriteState spriteBtnState;
                spriteBtnState.highlightedSprite = Resources.Load("video_hover", typeof(Sprite)) as Sprite;
                cameraBtn.spriteState = spriteBtnState;
                EngineManager.instance.GetVideoEngine().StartVideoCapture();
            }
        }
        public void OpenVoiceClick()
        {
            Image currentImage = voiceImage;
            bool isOpen = imageMap[currentImage];
            imageMap[currentImage] = !isOpen;
            if (isOpen)
            {
                currentImage.sprite = Resources.Load("voice_disable", typeof(Sprite)) as Sprite;
                SpriteState spriteBtnState;
                spriteBtnState.highlightedSprite = null;
                voiceBtn.spriteState = spriteBtnState;
                EngineManager.instance.GetVideoRoom().PauseAllSubscribedStream(PauseResumeControlMediaType.kRTCPauseResumeControlMediaTypeAudio);
            }
            else
            {
                currentImage.sprite = Resources.Load("voice", typeof(Sprite)) as Sprite;
                //SpriteState spriteBtnState;
                //spriteBtnState.highlightedSprite = Resources.Load("video_hover", typeof(Sprite)) as Sprite;
                //voiceBtn.spriteState = spriteBtnState;
                EngineManager.instance.GetVideoRoom().ResumeAllSubscribedStream(PauseResumeControlMediaType.kRTCPauseResumeControlMediaTypeAudio);
            }
        }


    }
}
