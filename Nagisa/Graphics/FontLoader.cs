/* Nagisa - 2D Game Library with OpenTK */

/* LICENSE - The MIT License (MIT)
 
Copyright (c) 2014 Tomona Nanase

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Nagisa.Graphics
{
    /// <summary>
    /// ファイルからフォントをロードするためのローダです。
    /// </summary>
    public class FontLoader : IDisposer
    {
        #region -- Private Fields --
        private PrivateFontCollection fontCollection;
        private IReadOnlyList<FontFamily> families;

        private bool isDisposed = false;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// フォントから読み込みに成功したフォントファミリのリストを取得します。
        /// </summary>
        public IReadOnlyList<FontFamily> Families { get { return this.families; } }

        /// <summary>
        /// オブジェクトが破棄されたかを表す真偽値を取得します。
        /// </summary>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 読み込むファイルを指定して新しい FontLoader クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="filename">読み込まれるフォントファイル。</param>
        public FontLoader(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            this.fontCollection = new PrivateFontCollection();
            this.fontCollection.AddFontFile(filename);
            this.families = Array.AsReadOnly(this.fontCollection.Families);
        }

        /// <summary>
        /// 読み込むストリームを指定して新しい FontLoader クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み取られる Stream。</param>
        /// <param name="size">読み取られるバイト数。</param>
        public FontLoader(Stream stream, int size)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            if (!stream.CanRead)
                throw new ArgumentException();

            IntPtr buffer = Marshal.AllocHGlobal(size);
            const int ReadSegmentSize = 1024 * 16;
            int readSize;
            int readTotal = 0;
            byte[] readBuffer = new byte[ReadSegmentSize];

            do
            {
                readSize = stream.Read(readBuffer, 0, ReadSegmentSize);
                Marshal.Copy(readBuffer, 0, buffer + readTotal, readSize);
                readTotal += readSize;
            } while (readSize > 0);

            if (readTotal != size)
                throw new ArgumentException();

            this.fontCollection = new PrivateFontCollection();
            this.fontCollection.AddMemoryFont(buffer, size);
            this.families = Array.AsReadOnly(this.fontCollection.Families);

            Marshal.FreeHGlobal(buffer);
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// このオブジェクトで使用されているリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// このオブジェクトによって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">
        /// マネージリソースとアンマネージリソースの両方を解放する場合は true。アンマネージリソースだけを解放する場合は false。
        /// </param>
        protected void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    if (this.families != null)
                        foreach (var item in this.families)
                            item.Dispose();

                    if (this.fontCollection != null)
                        this.fontCollection.Dispose();
                }

                this.families = null;
                this.fontCollection = null;

                this.isDisposed = true;
            }
        }
        #endregion

        #region -- Destructors --
        ~FontLoader()
        {
            this.Dispose(false);
        }
        #endregion
    }
}