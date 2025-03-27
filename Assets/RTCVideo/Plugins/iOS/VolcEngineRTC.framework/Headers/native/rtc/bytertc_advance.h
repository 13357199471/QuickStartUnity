/*
 * Copyright (c) 2020 The VolcEngineRTC project authors. All Rights Reserved.
 * @brief VolcEngineRTC Advance API
*/

#pragma once

#include "../bytertc_video.h"

#include <stdint.h>
#ifdef __ANDROID__
#include "jni.h"
#endif


#ifdef __GNUC__
#    define attribute_deprecated __attribute__((deprecated))
#elif defined(_MSC_VER)
#    define attribute_deprecated __declspec(deprecated)
#else
#    define attribute_deprecated
#endif

namespace bytertc {

/**
 * 
 * @hidden for internal use only
 * @type
 * @brief RTC Audio Dump 节点配置。 <br>
 * 要与 bae_audio_sample_interface 中 <br>
 * 的 AudioSampleType 枚举和 AudioSampleConfigure 保持一致。
 */

struct RTCAudioSampleConfigure {
    bool aec_near_in_enable = false;
    bool aec_far_in_enable = false;
    bool aec_far_in_ref_enable = false;
    bool aec_out_enable = false;
    bool agc_out_enable = false;
    bool ns_out_enable = false;
    bool ainr_out_enable = false;
    bool mix_out_enable = false;
    bool send2encoder_enable = false;
    bool audio_share_in_enable = false;
    bool audio_share_win_ref_enable = false;
    bool audio_share_out_enable = false;
    bool external_audio_in_enable = false;
    bool pitchfilter_out_enable = false;
    bool decoder_out_enable = false;
    bool front_aec_near_ref_enable = false;
    bool postprocess_out_enable = false;
    bool remotefilter_out_enable = false;
    bool push_audio_frame_in_enable = false;
    bool loopback_device_in_enable = false;
    void reset() {
        aec_near_in_enable = false;
        aec_far_in_enable = false;
        aec_far_in_ref_enable = false;
        aec_out_enable = false;
        agc_out_enable = false;
        ns_out_enable = false;
        ainr_out_enable = false;
        mix_out_enable = false;
        send2encoder_enable = false;
        audio_share_in_enable = false;
        audio_share_win_ref_enable = false;
        audio_share_out_enable = false;
        external_audio_in_enable = false;
        pitchfilter_out_enable = false;
        decoder_out_enable = false;
        front_aec_near_ref_enable = false;
        postprocess_out_enable = false;
        remotefilter_out_enable = false;
        push_audio_frame_in_enable = false;
        loopback_device_in_enable = false;
    }
    void normalMode() {
        aec_near_in_enable = true;
        aec_far_in_ref_enable = true;
        mix_out_enable = true;
    }
    void allMode() {
        aec_near_in_enable = true;
        aec_far_in_enable = true;
        aec_far_in_ref_enable = true;
        aec_out_enable = true;
        agc_out_enable = true;
        ns_out_enable = true;
        ainr_out_enable = true;
        mix_out_enable = true;
        send2encoder_enable = true;
        audio_share_in_enable = true;
        audio_share_win_ref_enable = true;
        audio_share_out_enable = true;
        external_audio_in_enable = true;
        pitchfilter_out_enable = true;
        decoder_out_enable = true;
        front_aec_near_ref_enable = true;
        postprocess_out_enable = true;
        remotefilter_out_enable = true;
        push_audio_frame_in_enable = true;
        loopback_device_in_enable = true;
    }
};

/**
 * 
 * @hidden for internal use only
 * @type keytype
 * @brief SDK 的服务器环境。
 */

enum Env {
    /**
     * 
     * @brief 线上环境。
     */
    
    kEnvProduct = 0,
    /**
     * 
     * @brief BOE 环境。
     */
    
    kEnvBOE,
    /**
     * 
     * @brief 测试环境。
     */
    
    kEnvTest
};

attribute_deprecated
/**
 * 
 * @hidden for internal use only
 * @deprecated
 * @type api
 * @region 引擎管理
 * @brief 设置 SDK 当前使用的环境。
 * @param env SDK 使用的环境参数，详见：Env{@link #Env} 。
 * @return
 *        0： 设置成功 <br>
 *        -1：设置失败
 * @note
 *        1.本函数是可选函数，默认使用正式环境； <br>
 *        2.本函数必须在创建 RTCVideo 实例 createRTCVideo{@link #createRTCVideo} 前调用。
 */

BYTERTC_API BYTERTC_DEPRECATED int setEnv(Env env);

attribute_deprecated
/**
 * 
 * @hidden for internal use only
 * @deprecated
 * @brief 设置设备 ID
 * @param device_id app 根据当前软硬件环境生成的唯一设备 id
 */

BYTERTC_API BYTERTC_DEPRECATED void setDeviceId(const char* device_id);
/**
 * 
 * @hidden for internal use only
 * @brief 设置应用的状态
 * @param engine <br>
 *       要通知的引擎
 * @param app_state 应用状态字符串
 */

BYTERTC_API void setAppState(void* engine, const char* app_state);

attribute_deprecated
/**
 * 
 * @hidden for internal use only
 * @deprecated
 * @type api
 * @region 房间管理
 * @brief 私有接口
 * @param parameters JSON 字符串
 */

BYTERTC_API int setParameters(const char* parameters);

#ifdef __ANDROID__
/**
 * 
 * @hidden(iOS,macOS,Windows,Linux) for internal use only
 * @type api
 * @region 引擎管理
 * @brief 用于设置 Android 的 ApplicationContext 给 Engine 使用， <br>
 * 如果 Android 使用纯 C++接口，则该方法必须在引擎创建前设置
 * @param j_egl_context <br>
 *      传入 Java 的 android.content.Context 类型的 EGLContext
 */

BYTERTC_API void setApplicationContext(jobject j_application_context);
#endif

/**
 * 
 * @region 视频管理
 * @type keytype
 * @brief 硬件加速设备类型
 */

enum HWDeviceType {
    /**
     * 
     * @brief 未知的设备类型
     */
    
    kHWDeviceTypeNone,

    /**
     * 
     * @brief direct3d 11 视频加速设备
     */
    
    kHWDeviceTypeD3D11,

    /**
     * 
     * @brief cuda 硬件加速设备
     * @note cuda 是 nvidia 平台下硬件加速设备
     */
    
    kHWDeviceTypeCuda,

    /**
     * 
     * @brief intel 平台下 qsv 加速设备
     */
    
    kHWDeviceTypeQsv,

    /**
     * 
     * @brief windows 平台 dx 硬件加速设备
     */
    
    kHWDeviceTypeDxva2,

    /**
     * 
     * @brief Android 平台下硬件加速设备 mediacodec
     */
    
    kHWDeviceTypeMediaCodec,

    /**
     * 
     * @brief IOS、MACOS 平台下硬件加速设备 VideoToolbox
     */
    
    kHWDeviceTypeVideoToolbox,
     /**
     * 
     * @brief android、linux 平台下硬件加速设备 egl context
     */
    
    kHWDeviceTypeEglContext,

    /**
     * 
     * @brief linux、windows 平台下硬件加速设备 va context
     */
    
    kHWDeviceTypeVaapi,
};

/**
 * 
 * @hidden for internal use only
 * @type keytype
 * @brief 硬编码设备 context
 */

enum HWDeviceContextKey {
    /**
     * @brief cuda context
     */
    kHWDeviceContextKeyCudaContext,
    /**
     * 
     * @brief derict3D11 设备
     */
    
    kHWDeviceContextKeyD3D11Device,
    /**
     * 
     * @brief derict3D11 设备 context
     */
    
    kHWDeviceContextKeyD3D11DeviceContext,
    /**
     * 
     * @brief egl 设备 context
     */
    
    kHWDeviceContextKeyEglContext,
    /**
     * 
     * @brief vadisplay 设备 context
     */
    
    kHWDeviceContextKeyVADisplay,
};

/**
 * 
 * @type keytype
 * @brief 硬件加速设备 context
 */

class IHWDeviceContext {
public:
    virtual ~IHWDeviceContext() = default;
    /**
     * 
     * @brief 获取硬件加速设备 context 类型
     * @return 硬件加速设备类型，详见 HWDeviceType{@link #HWDeviceType}
     */
    
    virtual HWDeviceType deviceType() const = 0;
    /**
     * 
     * @brief 设置属性
     * @param key 硬件加速设备 context 类型，详见 ByteRTCHWDeviceContextKey:{@link #ByteRTCHWDeviceContextKey}
     * @param value 指向硬件加速设备 context 地址的指针
     */
    
    virtual void setProperty(HWDeviceContextKey key, void* value) = 0;
    /**
     * 
     * @brief 获取属性
     * @param key 硬件加速设备 context 类型，详见 ByteRTCHWDeviceContextKey:{@link #ByteRTCHWDeviceContextKey}
     * @return 返回硬件加速设备 context 地址的指针
     */
    
    virtual void* getProperty(HWDeviceContextKey key) = 0;
    /**
     * 
     * @brief 内部 context 是否由该实例拥有
     * @return
     *        - true: 硬件设备 context 被实例对象拥有
     *        - false: 硬件设备上 context 文不被实例对象拥有
     */
    
    virtual bool ownContext() const = 0;
    /**
     * 
     * @brief 设置 context 是否被实例对象拥有
     * @param own_context <br>
     *       - true: context 被实例对象拥有
     *       - false: context 不被实例对象拥有
     */
    
    virtual void setOwnContext(bool own_context) = 0;
    /**
     * 
     * @brief 转移硬件设备 context，
     * @return 指向硬件设备 context 的指针
     */
    
    virtual IHWDeviceContext* moveContext() = 0;
    /**
     * 
     * @brief 释放实例对象
     */
    
    virtual void release() = 0;
};

/**
 * @hidden for internal use only
 * @brief create a hardware device context with specified deviceType
 */
BYTERTC_API IHWDeviceContext* createHwDeviceContext(HWDeviceType device_type);

/**
 * 
 * @hidden for internal use only
 * @brief 设置视频源的设备上下文信息
 * @param engine <br>
 *       要设置的引擎，详见 IRTCVideo{@link #IRTCVideo}
 * @param hw_context <br>
 *        设置视频帧，详见：IHWDeviceContext{@link #IHWDeviceContext}
 * @note 用于硬件编码传入 GPU 内存时使用
 */

BYTERTC_API void setVideoSourceHWDeviceContext(void* engine, IHWDeviceContext* hw_context);

/**
 * 
 * @hidden for internal use only
 * @type api
 * @region 视频管理
 * @brief 发送外部源视频数据
 * @param engine <br>
 *       要设置的引擎，详见 IRTCVideo{@link #IRTCVideo}
 * @param frame <br>
 *        输入视频帧，详见 IVideoFrame{@link #IVideoFrame}
 * @note
 *      1.支持 YUV420P，NV12，D3D texture，Cuda 内存，OpenGL texture 等格式的视频帧。 <br>
 *      2.该函数运行在用户调用线程内
 */

BYTERTC_API bool pushExternalVideoFrame(void* engine, const IVideoFrame* frame);


/**
 * 
 * @hidden for internal use only
 * @type keytype
 * @brief 音频帧信息
 */

class IEncodedAudioFrame {
public:
    virtual ~IEncodedAudioFrame() {
    }
    virtual AudioCodecType codecType() const = 0;
    virtual void setCodecType(AudioCodecType codec_type) = 0;
    virtual int sampleRate() const = 0;
    virtual void setSampleRate(int sample_rate) = 0;
    virtual int channelsNum() const = 0;
    virtual void setChannelsNum(int channels_num) = 0;
    virtual int64_t timestampUs() const = 0;
    virtual void setTimestampUs(int64_t timestamp_us) = 0;
    virtual int64_t timestampPts() const = 0;
    virtual void setTimestampPts(int64_t timestamp_pts) = 0;
    virtual int timebaseNum() const = 0;
    virtual void setTimestampNum(int timebase_num) = 0;
    virtual int timebaseDen() const = 0;
    virtual void setTimestampDen(int timebase_den) = 0;
    virtual void setStreamMemory(const ManagedMemory& memory) = 0;
    virtual uint8_t* data() const = 0;
    virtual int dataSize() const = 0;
    virtual int frameLength() const = 0;
};

/**
 * @hidden for internal use only
 */
BYTERTC_API IEncodedAudioFrame* createEncodedAudioFrame();

/**
 * 
 * @hidden for internal use only
 * @type callback
 * @region 音频数据回调
 * @brief 本地音频帧监测器
 */

class ILocalEncodedAudioFrameObserver  {
public:
    virtual ~ILocalEncodedAudioFrameObserver() {
    }
    /**
     * 
     * @type callback
     * @region 音频数据回调
     * @brief 调用 RegisterLocalEncodedAudioFrameObserver{@link #RegisterLocalEncodedAudioFrameObserver} 后，SDK 收到本地音频帧信息时，回调该事件
     * @param type 本地音频帧类型，参看 StreamIndex{@link #StreamIndex}
     * @param audio_stream 本地音频帧信息，参看 IEncodedAudioFrame{@link #IEncodedAudioFrame}
     */
    
    virtual void onLocalEncodedAudioFrame(StreamIndex type, const IEncodedAudioFrame& audio_stream) = 0;
};

/**
 * 
 * @hidden for internal use only
 * @type callback
 * @region 音频数据回调
 * @brief 远端音频帧监测器
 */

class IRemoteEncodedAudioFrameObserver  {
public:
    virtual ~IRemoteEncodedAudioFrameObserver () {
    }
    /**
     * 
     * @type callback
     * @region 音频数据回调
     * @brief 调用 RegisterRemoteEncodedAudioFrameObserver{@link #RegisterRemoteEncodedAudioFrameObserver} 后，SDK 收到远端音频帧信息时，回调该事件
     * @param stream_id 收到的远端音频流的 ID
     * @param audio_stream 远端音频帧信息，参看 IEncodedAudioFrame{@link #IEncodedAudioFrame}
     * @param stream_info 收到的远端音频流的信息，参看 RemoteStreamKey{@link #RemoteStreamKey}
     */
    
    virtual void onRemoteEncodedAudioFrame(
            const char* stream_id, const IEncodedAudioFrame& audio_stream, const RemoteStreamKey& stream_info) = 0;
};


/**
 * 
 * @hidden for internal use only
 * @type api
 * @region 音频数据回调
 * @brief 注册远端音频帧监测器。 <br>
 *        调用该方法后，SDK 每监测到一帧远端音频帧时，都会将音频帧信息通过 onRemoteEncodedAudioFrame{@link #IRemoteEncodedAudioFrameObserver#onRemoteEncodedAudioFrame} 回调给用户
 * @param engine 需设置的引擎，参看 IRTCVideo{@link #IRTCVideo}
 * @param observer 远端音频帧监测器，参看 IRemoteEncodedAudioFrameObserver{@link #IRemoteEncodedAudioFrameObserver}
 * @note
 *       - 该方法建议在进房前调用。
 *       - 将参数设置为 nullptr 则取消注册。
 */

BYTERTC_API void registerRemoteEncodedAudioFrameObserver(void* engine, IRemoteEncodedAudioFrameObserver* observer);

/**
 * 
 * @hidden for internal use only
 * @type api
 * @region 音频数据回调
 * @brief 注册本地音频帧监测器。 <br>
 *        调用该方法后，SDK 每监测到一帧本地音频帧时，都会将音频帧信息通过 onLocalEncodedAudioFrame{@link #ILocalEncodedAudioFrameObserver#onLocalEncodedAudioFrame} 回调给用户
 * @param engine 需设置的引擎，参看 IRTCVideo{@link #IRTCVideo}
 * @param observer 本地音频帧监测器，参看 ILocalEncodedAudioFrameObserver{@link #ILocalEncodedAudioFrameObserver}
 * @note
 *       - 该方法建议在进房前调用。
 *       - 将参数设置为 nullptr 则取消注册。
 */

BYTERTC_API void registerLocalEncodedAudioFrameObserver(void* engine, ILocalEncodedAudioFrameObserver* observer);


/**
 * @hidden for internal use only
 */
BYTERTC_API bool pushExternalEncodedAudioFrame(void* engine, const IEncodedAudioFrame* audio_stream);

/**
 * @hidden for internal use only
 * @type api
 */
struct MediaIoOption {
    bool enable_audio_encoding = true;
    bool enable_video_encoding = true;
    bool enable_audio_decoding = true;
    bool enable_video_decoding = true;
};

/**
 * @hidden for internal use only
 * @type api
 */
BYTERTC_API void setupEngineMediaIo(void* engine, const MediaIoOption& option);

/**
 * 
 * @hidden for internal use only
 * @type keytype
 * @region 视频数据回调
 * @brief metadata 数据大小最大值
 */

const int MAX_METADATA_LENGTH = 1024;


/**
 * 
 * @hidden for internal use only
 * @type callback
 * @brief 音频设备事件回调
 */

class IAudioDeviceEventHandler {
public:
    /**
     * 
     * @type callback
     * @brief 获得麦克风音量
     * @param volume <br>
     *       音量大小
     */
    
    virtual void onRecordingAudioVolumeIndication(int volume) = 0;

    /**
     * 
     * @type callback
     * @brief 获得 Loopback 音量
     * @param volume <br>
     *       音量大小
     */
    
    virtual void onLoopbackAudioVolumeIndication(int volume) = 0;
};

/**
 * 
 * @type api
 * @region 音频设备管理
 * @brief 音频设备管理类 Ex
 */
class IAudioDeviceManagerEx : public IAudioDeviceManager {
public:
    /**
     * 
     * @region 音频设备管理
     * @brief 构造函数
     */
    
    IAudioDeviceManagerEx() = default;

    /**
     * 
     * @type api
     * @region 音频设备管理
     * @brief 析构当前对象，释放内存
     * @note 该结构不需要的时候应该调用本函数释放音频操作相关的资源
     */
    
    virtual void release() = 0;

    /**
     * 
     * @type api
     * @region 音频设备管理
     * @brief 切换音频播放到扬声器或者听筒，默认播放设备是扬声器
     * @param enable <br>
     *       - true：切换至扬声器
     *       - false：切换至听筒
     * @note 本方法只在移动设备上有效
     */
    
    virtual void setEnableSpeakerphone(bool enable) = 0;

    /**
     * 
     * @type api
     * @region 音频设备管理
     * @brief 启动音频采集设备测试。 <br>
     *        该方法测试音频采集设备是否能正常工作。启动测试后，会收到 `onLocalAudioPropertiesReport` 回调上报的音量信息。
     * @param indication_interval `onLocalAudioPropertiesReport` 回调的时间间隔，单位为毫秒。建议设置到大于 200 毫秒。最小不得少于 10 毫秒。小于 10 毫秒行为未定义。
     * @return 方法调用结果 <br>
     *        - 0：方法调用成功
     *        - < 0：方法调用失败
     * @note
     *       - 该方法必须在进房前调用，且不可与其它音频设备测试功能同时应用。
     *       - 你需调用 stopAudioCaptureDeviceTest{@link #IAudioDeviceManagerEx#stopAudioCaptureDeviceTest} 停止测试。
     */
    
    virtual int startAudioCaptureDeviceTest(int indication_interval) = 0;

    /**
     * 
     * @type api
     * @region 音频设备管理
     * @brief 停止音频采集设备测试。
     * @return 方法调用结果 <br>
     *        - 0：方法调用成功
     *        - < 0：方法调用失败
     * @note 调用 startAudioCaptureDeviceTest{@link #IAudioDeviceManagerEx#startAudioCaptureDeviceTest} 后，需调用本方法停止测试。
     */
    
    virtual int stopAudioCaptureDeviceTest() = 0;

    /**
     * 
     * @type api
     * @region 音频设备管理
     * @brief 开始音频设备回路测试。 <br>
     *        该方法测试音频采集设备和音频播放设备是否能正常工作。一旦测试开始，音频采集设备会采集本地声音 <br>
     *        并通过音频播放设备播放出来，同时用户 App 会收到 `onLocalAudioPropertiesReport` <br>
     *        回调上报的音量信息。
     * @param indication_interval `onLocalAudioPropertiesReport` 回调的时间间隔，单位为毫秒。建议设置到大于 200 毫秒。最小不得少于 10 毫秒。小于 10 毫秒行为未定义。
     * @return 方法调用结果 <br>
     *        - 0：方法调用成功
     *        - < 0：方法调用失败
     * @note
     *       - 该方法必须在进房前调用。且不可与其它音频设备测试功能同时应用。
     *       - 你需调用 stopAudioDeviceLoopbackTest{@link #IAudioDeviceManagerEx#stopAudioDeviceLoopbackTest} 停止测试。
     *       - 该方法仅在本地进行音频设备测试，不涉及网络连接。
     */
    
    virtual int startAudioDeviceLoopbackTest(int indication_interval) = 0;

    /**
     * 
     * @type api
     * @region 音频设备管理
     * @brief 停止音频设备回路测试。
     * @return 方法调用结果 <br>
     *        - 0：方法调用成功
     *        - < 0：方法调用失败
     * @note 调用 startAudioDeviceLoopbackTest{@link #IAudioDeviceManagerEx#startAudioDeviceLoopbackTest} 后，需调用本方法停止测试。
     */
    
    virtual int stopAudioDeviceLoopbackTest() = 0;

protected:
    /**
     * 
     * @hidden constructor/destructor
     * @brief 析构函数
     */
    
    virtual ~IAudioDeviceManagerEx() = default;
};

/**
 * 
 * @deprecated since 3.51 and will be deleted in 3.53, use getAudioDeviceManager{@link #IRTCVideo#getAudioDeviceManager} instead
 * @hidden for internal use only
 * @type api
 * @brief 音频设备管理接口创建，该接口不依赖引擎，主要用于无引擎下的音频设备管理
 * @param handler <br>
 *       音频设备的事件回调
 * @return 音频设备管理接口
 */

BYTERTC_API IAudioDeviceManagerEx* createAudioDeviceManager(IAudioDeviceEventHandler* handler);

}  

