using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTestReader
{
    [TestClass]
    public class TestReader
    {
        private static readonly Byte[] testData = new Byte[] { 0x00, 0x12, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0xFF };

        private Reader reader;

        [TestInitialize]
        public void Initialize()
        {
            reader = new Reader( new MemoryStream( testData ), false );
        }

        [TestCleanup]
        public void Cleanup()
        {
            reader.Dispose();
            reader = null;
        }

        [TestMethod]
        public void Reader_TestConstructor()
        {
            Assert.IsTrue( reader.IsOpen );
            Assert.IsTrue( reader.BytesRead == 0 );
            Assert.IsTrue( reader.CanRead );
            Assert.IsFalse( reader.CanSeek );
            Assert.IsFalse( reader.CanWrite );
        }

        [TestMethod]
        public void Reader_TestConstructor_LeaveInnerStreamOpen_Positive()
        {
            using ( var stream = new MockStream() )
            {
                using ( var reader = new Reader( stream, true ) )
                {
                }

                Assert.IsFalse( stream.IsDisposeCalled );
            }
        }

        [TestMethod]
        public void Reader_TestConstructor_LeaveInnerStreamOpen_Negative()
        {
            using ( var stream = new MockStream() )
            {
                using ( var reader = new Reader( stream, false ) )
                {
                }

                Assert.IsTrue( stream.IsDisposeCalled );
            }
        }

        [TestMethod]
        public void Reader_TestDispose()
        {
            reader.Dispose();

            Assert.IsFalse( reader.IsOpen );
            Assert.IsTrue( reader.CanRead );
            Assert.IsFalse( reader.CanSeek );
            Assert.IsFalse( reader.CanWrite );
        }

        [TestMethod]
        [ExpectedException( typeof ( ArgumentNullException ) )]
        public void Reader_TestConstructor_NullStream()
        {
            new Reader( null, false );
        }

        [TestMethod]
        [ExpectedException( typeof ( ArgumentException ) )]
        public void Reader_TestConstructor_InvalidStream()
        {
            var invalidStream = new Mock<Stream>();

            invalidStream.SetupGet( s => s.CanRead ).Returns( false );

            new Reader( invalidStream.Object, false );
        }

        [TestMethod]
        public void Reader_TestRead()
        {
            var buffer = new Byte[ testData.Length ];

            Assert.IsTrue( reader.Read( buffer, 0, testData.Length ) == testData.Length );
            Assert.IsTrue( buffer.SequenceEqual( testData ) );
            Assert.IsTrue( reader.BytesRead == testData.Length );
            Assert.IsTrue( reader.Read( buffer, 0, 1 ) == 0 );
            Assert.IsTrue( reader.BytesRead == testData.Length );
        }

        [TestMethod]
        public void Reader_TestRead_Offset()
        {
            var buffer = new Byte[ testData.Length + 2 ];

            Assert.IsTrue( reader.Read( buffer, 2, testData.Length ) == testData.Length );
            Assert.IsTrue( buffer.SequenceEqual( new Byte[] { 0, 0 }.Concat( testData ) ) );
            Assert.IsTrue( reader.BytesRead == testData.Length );
            Assert.IsTrue( reader.Read( buffer, 0, 1 ) == 0 );
            Assert.IsTrue( reader.BytesRead == testData.Length );
        }

        [TestMethod]
        public void Reader_TestRead_Count()
        {
            var buffer1 = new Byte[ testData.Length - 2 ];

            Assert.IsTrue( reader.Read( buffer1, 0, testData.Length - 2 ) == testData.Length - 2 );
            Assert.IsTrue( buffer1.SequenceEqual( testData.Take( testData.Length - 2 ) ) );
            Assert.IsTrue( reader.BytesRead == testData.Length - 2 );

            var buffer2 = new Byte[ 2 ];

            Assert.IsTrue( reader.Read( buffer2, 0, 2 ) == 2 );
            Assert.IsTrue( buffer2.SequenceEqual( testData.Skip( testData.Length - 2 ).Take( 2 ) ) );
            Assert.IsTrue( reader.BytesRead == testData.Length );

            Assert.IsTrue( reader.Read( buffer2, 0, 1 ) == 0 );
            Assert.IsTrue( reader.BytesRead == testData.Length );
        }

        [TestMethod]
        [ExpectedException( typeof ( ObjectDisposedException ) )]
        public void Reader_TestRead_Disposed()
        {
            reader.Dispose();

            var buffer = new Byte[ 0 ];

            reader.Read( buffer, 0, 0 );
        }

        [TestMethod]
        [ExpectedException( typeof ( NotSupportedException ) )]
        public void Reader_TestSeek()
        {
            reader.Seek( 0, SeekOrigin.Begin );
        }

        [TestMethod]
        [ExpectedException( typeof ( ObjectDisposedException ) )]
        public void Reader_TestSeek_Disposed()
        {
            reader.Dispose();

            reader.Seek( 0, SeekOrigin.Begin );
        }

        [TestMethod]
        [ExpectedException( typeof ( NotSupportedException ) )]
        public void Reader_TestWrite()
        {
            reader.Write( new Byte[ 4 ], 0, 4 );
        }

        [TestMethod]
        [ExpectedException( typeof ( ObjectDisposedException ) )]
        public void Reader_TestWrite_Disposed()
        {
            reader.Dispose();

            reader.Write( new Byte[ 4 ], 0, 4 );
        }

        [TestMethod]
        [ExpectedException( typeof ( NotSupportedException ) )]
        public void Reader_TestSetLength()
        {
            reader.SetLength( 4 );
        }

        [TestMethod]
        [ExpectedException( typeof ( ObjectDisposedException ) )]
        public void Reader_TestSetLength_Disposed()
        {
            reader.Dispose();

            reader.SetLength( 4 );
        }

        [TestMethod]
        public void Reader_TestFlush()
        {
            reader.Flush();
        }

        [TestMethod]
        [ExpectedException( typeof ( NotSupportedException ) )]
        public void Reader_TestGetPosition()
        {
            var position = reader.Position;
        }

        [TestMethod]
        [ExpectedException( typeof ( ObjectDisposedException ) )]
        public void Reader_TestGetPosition_Disposed()
        {
            reader.Dispose();

            var position = reader.Position;
        }

        [TestMethod]
        [ExpectedException( typeof ( NotSupportedException ) )]
        public void Reader_TestSetPosition()
        {
            reader.Position = 0;
        }

        [TestMethod]
        [ExpectedException( typeof ( ObjectDisposedException ) )]
        public void Reader_TestSetPosition_Disposed()
        {
            reader.Dispose();

            reader.Position = 0;
        }

        [TestMethod]
        [ExpectedException( typeof ( NotSupportedException ) )]
        public void Reader_TestGetLength()
        {
            var length = reader.Length;
        }

        [TestMethod]
        [ExpectedException( typeof ( ObjectDisposedException ) )]
        public void Reader_TestGetLength_Disposed()
        {
            reader.Dispose();

            var length = reader.Length;
        }
    }
}
