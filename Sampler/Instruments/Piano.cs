using System;
using System.Collections.Generic;
using System.Text;

namespace Sampler
{
    public class Piano : Instrument
    {
        public override bool IsEnableVelocity => true;

        protected override string GetSampleFolder()
        {
            return "Samples/Piano";
        }

        protected override int GetMinSample()
        {
            return 0;
        }

        protected override int GetMaxSample()
        {
            return 87;
        }

        protected override int GetFileDigitCount()
        {
            return 3;
        }

        protected override bool GetIsNoteOffOnRelease()
        {
            return true;
        }

        protected override bool GetIsLoop()
        {
            return false;
        }

        public override int GetOffset()
        {
            return 0;
        }
    }
}
