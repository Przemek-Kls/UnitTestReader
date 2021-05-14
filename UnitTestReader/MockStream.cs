using System;
using System.IO;

namespace UnitTestReader
{
    internal class MockStream : MemoryStream
    {
        protected override void Dispose(Boolean disposing)
        {
            if (!IsDisposeCalled)
            {
                if (disposing)
                {

                }
                base.Dispose(disposing);
                IsDisposeCalled = true;
            }

        }

        public Boolean IsDisposeCalled
        {
            get; private set;
        }
    }
}
