using System;
using UnityEngine;

namespace Unity.Labs.FacialRemote
{
    public interface IStreamSource
    {
        bool streamActive { get; }
        bool streamThreadActive { get; set; }
        Func<bool> IsStreamSource { get; set; }
        Func<StreamReader> getStreamReader { get; set; }

        Func<PlaybackData> getPlaybackData { get; set; }
        Func<bool> getUseDebug { get; set; }

        void StartStreamThread();
        void ActivateStreamSource();
        void DeactivateStreamSource();
        void SetReaderStreamSettings();
    }

    public interface IUseReaderActive
    {
        Func<bool> isStreamActive { get; set; }
        Func<bool> isTrackingActive { get; set; }
    }

    public interface IUseStreamSettings
    {
        Func<IStreamSettings> getStreamSettings { get; set; }
        Func<IStreamSettings> getReaderStreamSettings { get; set; } // TODO should try to always use active settings.
        void OnStreamSettingsChange();
    }

    public interface IUseReaderBlendShapes
    {
        Func<float[]> getBlendShapesBuffer { get; set; }
    }

    public interface IUseReaderHeadPose
    {
        Func<Pose> getHeadPose { get; set; }
    }

    public interface IUseReaderCameraPose
    {
        Func<Pose> getCameraPose { get; set; }
    }

    public interface IServerSettings
    {
        Func<int> getPortNumber { get; set; }
        Func<int> getFrameCatchupSize { get; set; }
        Func<int> getFrameCatchupThreshold { get; set; }
    }


    public abstract class StreamSource : IStreamSource, IUseStreamSettings
    {
        public bool isSource { get { return IsStreamSource(); } }
        public Func<bool> IsStreamSource { get; set; }
        public Func<IStreamSettings> getStreamSettings { get; set; }
        public Func<IStreamSettings> getReaderStreamSettings { get; set; }
        public Func<PlaybackData> getPlaybackData { get; set; }
        public Func<bool> getUseDebug { get; set; }
        public Func<StreamReader> getStreamReader { get; set; }

        public bool streamActive { get { return IsStreamActive(); } }
        public bool streamThreadActive { get; set; }

        protected StreamReader streamReader { get { return getStreamReader(); } }
        protected IStreamSettings streamSettings { get { return getStreamSettings(); } }
        protected PlaybackData playbackData { get { return getPlaybackData(); } }
        protected bool useDebug { get { return getUseDebug(); } }

        public abstract void StreamSourceUpdate();
        public abstract void OnStreamSettingsChange();
        public abstract void SetReaderStreamSettings();

        public virtual void ActivateStreamSource()
        {
            if (!isSource)
            {
                streamReader.UnSetStreamSource();
                streamReader.SetStreamSource(this);
            }
        }

        public virtual void DeactivateStreamSource()
        {
            if (isSource)
            {
                streamReader.UnSetStreamSource();
            }
        }

        protected abstract bool IsStreamActive();

        public abstract void StartStreamThread();
        public abstract void StartPlaybackDataUsage();
        public abstract void StopPlaybackDataUsage();
        public abstract void UpdateCurrentFrameBuffer(bool force = false);
    }
}
