using System;
using System.Collections.Generic;
using System.Text;

namespace Sampler
{
    public class Harpsichord : Instrument
    {
        public override bool IsEnableVelocity => false;

        protected override string GetSampleFolder()
        {
            return "Samples/Harpsichord";
        }

        protected override int GetMinSample()
        {
            return 0;
        }

        protected override int GetMaxSample()
        {
            return 62;
        }

        protected override int GetFileDigitCount()
        {
            return 3;
        }

        protected override bool GetIsNoteOffOnRelease()
        {
            return false;
        }

        protected override bool GetIsLoop()
        {
            return false;
        }

        public override int GetOffset()
        {
            return -5;
        }
    }
}
