﻿/* Nagisa - 2D Game Library with OpenTK */

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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Nagisa.Graphics
{
    /// <summary>
    /// ビットマップデータを読み込み、内部データに簡易的にアクセスします。
    /// </summary>
    public class BitmapLoader : IDisposer
    {
        #region -- Private Fields --
        private Bitmap bitmap;
        private System.Drawing.Graphics graphics;
        private IntPtr bitmapData;
        private int bitmapBytes;

        private bool isDisposed = false;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// ビットマップの元となった Bitmap オブジェクトを取得します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public Bitmap BaseBitmap
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.bitmap;
            }
        }

        /// <summary>
        /// ビットマップを操作する Graphics オブジェクトを取得します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public System.Drawing.Graphics Graphics
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.graphics;
            }
        }

        /// <summary>
        /// ビットマップデータの先頭ポインタを取得します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public IntPtr Scan0
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.bitmapData;
            }
        }

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
        /// ビットマップを使用して BitmapLoader クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="baseBitmap">元となるビットマップオブジェクト。</param>
        /// <exception cref="ArgumentNullException">ビットマップオブジェクトが null です。</exception>
        public BitmapLoader(Bitmap baseBitmap)
        {
            if (baseBitmap == null)
                throw new ArgumentNullException("baseBitmap");

            this.Initalize(baseBitmap);
        }

        /// <summary>
        /// ストリームを読み取って BitmapLoader クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み取り可能なストリーム。</param>
        /// <exception cref="ArgumentNullException">ストリームオブジェクトが null です。</exception>
        /// <exception cref="NotSupportedException">ストリームが読み取りに対応していません。</exception>
        public BitmapLoader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
                throw new NotSupportedException();

            this.Initalize(new Bitmap(stream));
        }

        /// <summary>
        /// ファイルを読み取って BitmapLoader クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="filename">読み取られるファイル名。</param>
        /// <exception cref="ArgumentNullException">ファイル名が無効です。</exception>
        public BitmapLoader(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            this.Initalize(new Bitmap(filename));
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// ビットマップの変更を適用し、ヒープにコピーします。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public void Flush()
        {
            if (this.isDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            this.graphics.Flush();

            var src = this.bitmap.LockBits(new Rectangle(Point.Empty, this.bitmap.Size),
                                           ImageLockMode.ReadOnly,
                                           PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* p_src = (byte*)src.Scan0;
                byte* p_dst = (byte*)this.bitmapData;

                for (int i = 0; i < this.bitmapBytes; i++, p_src++, p_dst++)
                    *p_dst = *p_src;
            }

            this.bitmap.UnlockBits(src);
        }

        /// <summary>
        /// このオブジェクトによって使用されているすべてのリソースを解放します。
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
        ///     マネージリソースとアンマネージリソースの両方を解放する場合は true。
        ///     アンマネージリソースだけを解放する場合は false。
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    if (this.graphics != null)
                        this.graphics.Dispose();

                    if (this.bitmap != null)
                        this.bitmap.Dispose();

                    if (this.bitmapData != IntPtr.Zero)
                        Marshal.FreeHGlobal(this.bitmapData);
                }

                this.bitmapData = IntPtr.Zero;
                this.graphics = null;
                this.bitmap = null;

                this.isDisposed = true;
            }
        }
        #endregion

        #region -- Private Methods --
        private void Initalize(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            this.graphics = System.Drawing.Graphics.FromImage(bitmap);

            if (8L * bitmap.Width * bitmap.Height >= (long)int.MaxValue)
                throw new ArgumentException("画像サイズが大きすぎます。");

            this.bitmapBytes = 4 * bitmap.Width * bitmap.Height;
            this.bitmapData = Marshal.AllocHGlobal(this.bitmapBytes);

            this.Flush();
        }
        #endregion

        #region -- Destructors --
        ~BitmapLoader()
        {
            this.Dispose(false);
        }
        #endregion
    }
}