using System;
using System.IO;

namespace UnitTestReader
{
    /// <summary>
    /// Reprezentuje czytnik strumienia.
    /// </summary>
    public class Reader : Stream
    {
        private Stream stream;
        private Boolean leaveInnerStreamOpen;
        private Int64 bytesRead;
        private bool IsDisposed = false;

        /// <summary>
        /// Zwalnia zasoby.
        /// </summary>
        /// <param name="disposing">n/a</param>
        protected override void Dispose(Boolean disposing)
        {
            if (!IsDisposed)
            {
                if (disposing && stream != null && !leaveInnerStreamOpen)
                {
                    stream.Close();
                }
                if (stream != null)
                {
                    stream = null;
                    base.Dispose(disposing);
                }
                IsOpen = false;
                bytesRead = 0;
                IsDisposed = true;
            }

        }

        /// <summary>
        /// Tworzy instancję klasy <see cref="Reader"/>.
        /// </summary>
        /// <param name="stream">Strumień danych.</param>
        /// <param name="leaveInnerStreamOpen">Wartość [true] jeśli strumień danych powinien pozostać 
        /// otwarty po wywołaniu metody <see cref="Stream.Dispose()"/>.</param>
        public Reader(Stream stream,
                       Boolean leaveInnerStreamOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException();
            }
            if (!stream.CanRead)
            {
                throw new ArgumentException();
            }


            this.stream = stream;
            this.leaveInnerStreamOpen = leaveInnerStreamOpen;

            bytesRead = 0;
            IsOpen = true;

        }


        /// <summary>
        /// n/a
        /// </summary>
        /// <param name="buffer">n/a</param>
        /// <param name="offset">n/a</param>
        /// <param name="count">n/a</param>
        /// <returns>n/a</returns>
        public override Int32 Read(Byte[] buffer,
                                    Int32 offset,
                                    Int32 count)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("Reader.cs object has been disposed");
            }
            var result = stream.Read(buffer, offset, count);
            bytesRead += result;
            return result;
        }

        /// <summary>
        /// n/a
        /// </summary>
        /// <param name="offset">n/a</param>
        /// <param name="origin">n/a</param>
        /// <returns>n/a</returns>
        public override Int64 Seek(Int64 offset,
                                    SeekOrigin origin)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("Reader.cs object has been disposed");
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// n/a
        /// </summary>
        /// <param name="buffer">n/a</param>
        /// <param name="offset">n/a</param>
        /// <param name="count">n/a</param>
        public override void Write(Byte[] buffer,
                                    Int32 offset,
                                    Int32 count)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("Reader.cs object has been disposed");
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// n/a
        /// </summary>
        /// <param name="value">n/a</param>
        public override void SetLength(Int64 value)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("Reader.cs object has been disposed");
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// n/a
        /// </summary>
        public override void Flush()
        {
            //Not implemented write method.
        }

        /// <summary>
        /// Wartość [true] jeśli czytnik jest otwarty.
        /// </summary>
        public Boolean IsOpen
        {
            get; private set;
        }

        /// <summary>
        /// Rozmiar danych odczytanych ze strumienia.
        /// </summary>
        public Int64 BytesRead
        {
            get
            {
                return bytesRead;
            }
        }

        /// <summary>
        /// n/a
        /// </summary>
        public override Boolean CanRead
        {
            get
            {
                try
                {
                    Read(new byte[] { 0x01 }, 0, 0);
                    return true;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return true;
                }

            }
        }

        /// <summary>
        /// n/a
        /// </summary>
        public override Boolean CanSeek
        {
            get
            {
                try
                {
                    Seek(0, SeekOrigin.Begin);
                    return true;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// n/a
        /// </summary>
        public override Boolean CanWrite
        {
            get
            {
                try
                {
                    Write(new Byte[4], 0, 4);
                    return true;
                }
                catch (NotSupportedException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// n/a
        /// </summary>
        public override Int64 Position
        {

            get
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException("Reader.cs object has been disposed");
                }
                throw new NotSupportedException();
            }

            set
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException("Reader.cs object has been disposed");
                }
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// n/a
        /// </summary>
        public override Int64 Length
        {
            get
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException("Reader.cs object has been disposed");
                }
                throw new NotSupportedException();
            }
        }
    }
}
