using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace bytertc
{
	public class EngineManager
	{
		private IRTCVideo _VideoEngineInstance;  
		private IRTCVideoRoom _RTCVideoRoom; 

		static object locker = new object();
		private static EngineManager engineManager;
		public static EngineManager instance
        {
            get
            {
                if (engineManager == null)
                {
                    lock (locker)
                    {
                        if (engineManager == null)
                        {
                            engineManager = new EngineManager();
                        }
                    }
                }
                return engineManager;
            }
		}
        public IRTCVideo GetVideoEngine()
        {
            return _VideoEngineInstance;
        }
        public IRTCVideoRoom GetVideoRoom()
        {
            return _RTCVideoRoom;
        }

        Action<StreamIndex, VideoFrame>          LocalVideoSinkOnFrameCallback;
        Action<RemoteStreamKey, VideoFrame> RemoteVideoSinkOnFrameCallback;

        Action<string> UserEnterCallback;
        Action<string> UserLeaveCallback;
        Action ReleaseEngineCallback;
        public void RegisterLocalVideoSinkOnFrameCallback(Action<StreamIndex, VideoFrame> action)
        {
            LocalVideoSinkOnFrameCallback = action;
        }
        public void RegisterRemoteVideoSinkOnFrameCallback(Action<RemoteStreamKey, VideoFrame> action)
        {
            RemoteVideoSinkOnFrameCallback = action;
        }
        public void RegisterUserEnterCallback(Action<string> action)
        {
            UserEnterCallback = action;
        }
        public void RegisterUserLeaveCallback(Action<string> action)
        {
            UserLeaveCallback = action;
        }
        public void RegisterReleaseEngineCallback(Action action)
        {
             ReleaseEngineCallback = action;
        }

        public string GetSDKVersion()
        {
            if (_VideoEngineInstance != null)
            {
                string sdkVersion = _VideoEngineInstance.GetSDKVersion();
                return sdkVersion;
            }
            return "";
        }
		public void CreateEngine()
        {
            if (_VideoEngineInstance != null)
            {
                Debug.LogWarning("engine is already create!");
                return;
            }

            _VideoEngineInstance = new RTCVideo();

            RTCVideoEngineParams initParams = new RTCVideoEngineParams();
            initParams.Params = new Dictionary<string, object>();
            initParams.AppID = Constants.APP_ID;
// #if UNITY_ANDROID
//             RTCVideo.GetCurrentEGLContext();
// #endif        
            _VideoEngineInstance.CreateRTCVideo(initParams);

            VideoCaptureConfig config = new VideoCaptureConfig();
            config.CapturePreference = CapturePreference.KManual;
#if !UNITY_STANDALONE
        config.Width = 360;
        config.Height = 640;
#else
            config.Width = 640;
            config.Height = 360;
#endif
            config.FrameRate = 15;
            _VideoEngineInstance.SetVideoCaptureConfig(config);
            _VideoEngineInstance.StartVideoCapture();
            _VideoEngineInstance.StartAudioCapture();
            RegisterEngineEvent();
        }
        public void ReleaseEngine()
        {
            if (_VideoEngineInstance != null)
            {
                if (ReleaseEngineCallback != null)
                {
                    ReleaseEngineCallback();
                }
                UnRegisterEngineEvent();
                UnRegisterRoomEvent();
                if (_RTCVideoRoom != null)
                {
                    _RTCVideoRoom.Destroy();
                    _RTCVideoRoom = null;
                }

                _VideoEngineInstance.Release();
            }
            _VideoEngineInstance = null;
        }
        public void CreateRoom(string room_id)
		{
            _RTCVideoRoom = _VideoEngineInstance.CreateRTCRoom(room_id);
            RegisterRoomEvent();
        }

		public void JoinRoom(string user_id)
        {
            if (_RTCVideoRoom != null)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.UserID = user_id;
                userInfo.ExtraInfo = "";
                MultiRoomConfig roomConfig = new MultiRoomConfig();
                roomConfig.roomProfileType = RoomProfileType.kRoomProfileTypeCommunication;
                roomConfig.isAutoSubscribeAudio = true;
                roomConfig.isAutoSubscribeVideo = true;
                roomConfig.remoteVideoConfig = new RemoteVideoConfig();
                string token = Constants.TOKEN;
                int nRet = _RTCVideoRoom.JoinRoom(token, userInfo, roomConfig);
            }else
            {
                Debug.LogWarning("room is not exist");
            }
        }

        public void RegisterRoomEvent()
        {
            if (_RTCVideoRoom == null)
            {
                Debug.Log("please create room");
                return;
            }
            _RTCVideoRoom.OnUserJoinedEvent += (string roomID, UserInfo userInfo, int elapsed) =>
            {
                Debug.Log(string.Format("OnUserJoinedEvent rec,  roomID:{0} userID:{1} elapsed:{2}", roomID, userInfo.UserID, elapsed));
            };
            _RTCVideoRoom.OnUserLeaveEvent += (string roomID, string userID, UserOfflineReason reason) =>
            {
                Debug.Log(string.Format("OnUserLeaveEvent rec,  roomID:{0} userID:{1} UserOfflineReason:{2}", roomID, userID, reason));
                UserLeave(userID);
            };
            _RTCVideoRoom.OnRoomErrorEvent += (string roomID, int error) =>
            {
                Debug.Log(string.Format("OnRoomWarningEvent rec,  roomID:{0} error:{1}", roomID, error));
            };
        }
        public void UnRegisterRoomEvent()
        {
            if (_RTCVideoRoom == null)
            {
                Debug.Log("please create room");
                return;
            }
            _RTCVideoRoom.OnUserJoinedEvent -= (string roomID, UserInfo userInfo, int elapsed) =>
            {
                Debug.Log(string.Format("OnUserJoinedEvent rec,  roomID:{0} userID:{1} elapsed:{2}", roomID, userInfo.UserID, elapsed));
            };
            _RTCVideoRoom.OnUserLeaveEvent -= (string roomID, string userID, UserOfflineReason reason) =>
            {
                Debug.Log(string.Format("OnUserLeaveEvent rec,  roomID:{0} userID:{1} UserOfflineReason:{2}", roomID, userID, reason));
                UserLeave(userID);
            };
            _RTCVideoRoom.OnRoomErrorEvent -= (string roomID, int error) =>
            {
                Debug.Log(string.Format("OnRoomWarningEvent rec,  roomID:{0} error:{1}", roomID, error));
            };
        }
        public void RegisterEngineEvent()
        {
            if (_VideoEngineInstance == null)
            {
                Debug.Log("please create rtcEngine");
                return;
            }
            _VideoEngineInstance.OnFirstLocalVideoFrameCapturedEvent += (StreamIndex index, VideoFrameInfo info) =>
            {
                string logInfo = string.Format("OnFirstLocalVideoFrameCaptured, index: {0}, width: {1}, height: {2}, rotation: {3}", index, info.width, info.height, info.rotation);
                Debug.Log(logInfo);
            };

            _VideoEngineInstance.OnFirstRemoteVideoFrameDecodedEvent += (RemoteStreamKey key, VideoFrameInfo info) =>
            {
                Debug.Log(string.Format("OnFirstRemoteVideoFrameDecodedEvent rec,   key:{0}, w:{1} h:{2}", key, info.width, info.height));
                UserEnter(key.UserID);
            };
            _VideoEngineInstance.OnLocalVideoSinkOnFrameEvent += OnLocalVideoSinkOnFrameEventHandler;
            _VideoEngineInstance.OnRemoteVideoSinkOnFrameEvent += OnRemoteVideoSinkOnFrameEventHandler;
        }
        public void UnRegisterEngineEvent()
        {
            if (_VideoEngineInstance == null)
            {
                Debug.Log("please create rtcEngine");
                return;
            }
            _VideoEngineInstance.OnFirstLocalVideoFrameCapturedEvent -= (StreamIndex index, VideoFrameInfo info) =>
            {
                string logInfo = string.Format("OnFirstLocalVideoFrameCaptured, index: {0}, width: {1}, height: {2}, rotation: {3}", index, info.width, info.height, info.rotation);
                Debug.Log(logInfo);
            };

            _VideoEngineInstance.OnFirstRemoteVideoFrameDecodedEvent -= (RemoteStreamKey key, VideoFrameInfo info) =>
            {
                Debug.Log(string.Format("OnFirstRemoteVideoFrameDecodedEvent rec,   key:{0}, w:{1} h:{2}", key, info.width, info.height));
                UserEnter(key.UserID);
            };
            _VideoEngineInstance.OnLocalVideoSinkOnFrameEvent -= OnLocalVideoSinkOnFrameEventHandler;
            _VideoEngineInstance.OnRemoteVideoSinkOnFrameEvent -= OnRemoteVideoSinkOnFrameEventHandler;
        }
        public int SwitchCamera(CameraID cameraID)
        {
            return _VideoEngineInstance.SwitchCamera(cameraID);
        }

        public void SetLocalVideoMirroType(MirrorType mirrorType)
        {
            Debug.Log("set mirrorType =" + mirrorType);
            _VideoEngineInstance.SetLocalVideoMirrorType(mirrorType);
        }


        bool OnLocalVideoSinkOnFrameEventHandler(StreamIndex index, VideoFrame videoFrame)
        {
            if (LocalVideoSinkOnFrameCallback != null)
            {
                LocalVideoSinkOnFrameCallback(index, videoFrame);
            }
            return true;
        }

        bool OnRemoteVideoSinkOnFrameEventHandler(RemoteStreamKey key, VideoFrame videoFrame)
        {
            if (_RTCVideoRoom == null)
            {
                return false;
            }
            if (RemoteVideoSinkOnFrameCallback != null)
            {
                RemoteVideoSinkOnFrameCallback(key, videoFrame);
            }
            return true;
        }

        private void UserEnter(string user_id)
        {
            if (UserEnterCallback != null)
            {
                UserEnterCallback(user_id);
            }
        }
        private void UserLeave(string user_id)
        {
            if (UserLeaveCallback != null)
            {
                UserLeaveCallback(user_id);
            }
        }

    }
}
