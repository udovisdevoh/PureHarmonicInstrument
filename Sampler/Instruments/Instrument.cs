using Sampler.Instruments;
using SdlDotNet.Audio;
using System;

namespace Sampler
{
    public abstract class Instrument
    {
        #region Constants
        private const int channelCount = 8;
        #endregion

        #region Members
        private Sample[] samples;

        private Channel[] channels;

        private Channel[] soundChannelMap;

        private int minSample;

        private int maxSample;

        private int nextChannelId = 0;

        private bool isLoop;

        private bool isNoteOffOnRelease;
        #endregion

        #region Constructors
        public Instrument()
        {
            string sampleFolder = this.GetSampleFolder();
            this.minSample = this.GetMinSample();
            this.maxSample = this.GetMaxSample();
            int digitCount = this.GetFileDigitCount();
            this.isLoop = this.GetIsLoop();
            this.isNoteOffOnRelease = this.GetIsNoteOffOnRelease();

            Mixer.ChannelsAllocated = channelCount;

            this.samples = new Sample[maxSample + 1];
            this.soundChannelMap = new Channel[128];

            this.channels = new Channel[channelCount];
            for (int channelId = 0; channelId < channelCount; ++channelId)
            {
                this.channels[channelId] = Mixer.CreateChannel(channelId);
            }

            for (int sampleId = minSample; sampleId < maxSample + 1; ++sampleId)
            {
                Sample sample = new Sample(sampleId, sampleFolder + "/" + this.FormatDigit(sampleId, digitCount) + ".ogg");
                this.samples[sampleId] = sample;
            }
        }
        #endregion

        public bool IsNoteOffOnRelease
        {
            get { return this.isNoteOffOnRelease; }
            set { this.isNoteOffOnRelease = value; }
        }

        public abstract bool IsEnableVelocity { get; }

        public void Play(int pitch, int velocity)
        {
            pitch += this.GetOffset();
            pitch += 3; // Pitch correction WTF

            while (pitch > this.maxSample)
            {
                pitch -= 12;
            }

            if (pitch < this.minSample || pitch < 0 || pitch > 127)
            {
                return;
            }

            velocity = Math.Min(127, Math.Max(0, velocity));

            if (!this.IsEnableVelocity)
            {
                velocity = 127;
            }

            Sample sample = this.samples[pitch];

            /*
            if (sample.LastChannel != null)
            {
                sample.LastChannel.Stop();
                sample.LastChannel = null;
            }
            */

            Channel selectedChannel = this.channels[this.GetCurrentChannel()];
            selectedChannel.Stop();

            this.soundChannelMap[pitch] = selectedChannel;

            //selectedChannel.Volume = velocity;
            sample.Sound.Volume = velocity / (channelCount / 2);

            //sample.LastChannel = selectedChannel;

            selectedChannel.Play(sample.Sound, this.isLoop);
        }

        public abstract int GetOffset();

        public void Stop(int pitch)
        {
            if (pitch < this.minSample || pitch > maxSample || pitch < 0 || pitch > 127)
            {
                return;
            }

            if (this.soundChannelMap[pitch] != null)
            {
                this.soundChannelMap[pitch].Stop();
            }
        }

        public void StopAll()
        {
            for (int channelId = 0; channelId < channelCount; ++channelId)
            {
                this.channels[channelId].Stop();
            }
        }

        public int GetCurrentChannel()
        {
            int currentChannelId = this.nextChannelId;
            ++this.nextChannelId;

            if (this.nextChannelId >= channelCount)
            {
                this.nextChannelId = 0;
            }

            return currentChannelId;
        }

        public string FormatDigit(int sampleId, int digitCount)
        {
            string formated = sampleId.ToString();

            while (formated.Length < digitCount)
            {
                formated = "0" + formated;
            }

            return formated;
        }

        public void Dispose()
        {
            foreach (Channel channel in this.channels)
            {
                channel.Dispose();
            }

            foreach (Sample sample in this.samples)
            {
                sample.Dispose();
            }
        }

        protected abstract bool GetIsNoteOffOnRelease();

        protected abstract bool GetIsLoop();

        protected abstract string GetSampleFolder();

        protected abstract int GetMinSample();

        protected abstract int GetMaxSample();

        protected abstract int GetFileDigitCount();
    }
}
