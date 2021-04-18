using SdlDotNet.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sampler.Instruments
{
    public class Sample
    {
        private Sound sound;

        private Channel lastChannel = null;

        private int pitch;

        public Sample(int pitch, string filename)
        {
            this.pitch = pitch;
            this.sound = new Sound(filename);
        }

        public Channel LastChannel
        {
            get { return this.lastChannel; }
            set { this.lastChannel = value; }
        }

        public Sound Sound
        {
            get { return this.sound; }
        }

        public void Dispose()
        {
            this.sound.Dispose();
        }
    }
}
