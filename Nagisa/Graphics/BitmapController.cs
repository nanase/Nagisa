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

namespace Nagisa.Graphics
{
    /// <summary>
    /// ビットマップの生データ操作を提供します。
    /// </summary>
    public class BitmapController : IDisposer
    {
        #region -- Private Fields --
        private BitmapData data;
        private Bitmap bitmap;
        private IntPtr scan0;

        private bool isDisposed = false;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 座標を指定して Color 構造体を取得または設定します。
        /// </summary>
        /// <param name="x">取得する X 座標。</param>
        /// <param name="y">取得する Y 座標。</param>
        /// <returns>座標が指し示す Color 構造体。</returns>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        /// <exception cref="ArgumentOutOfRangeException">X または Y 座標が画像データの範囲外です。</exception>
        public unsafe Color this[int x, int y]
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (x < 0 || x >= this.data.Width)
                    throw new ArgumentOutOfRangeException("x");

                if (y < 0 || y >= this.data.Height)
                    throw new ArgumentOutOfRangeException("y");

                return Color.FromArgb(*((int*)this.scan0 + (this.data.Width * y + x)));
            }
            set
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (x < 0 || x >= this.data.Width)
                    throw new ArgumentOutOfRangeException("x");

                if (y < 0 || y >= this.data.Height)
                    throw new ArgumentOutOfRangeException("y");

                int* dst = (int*)this.scan0 + (this.data.Width * y + x);
                *dst = value.ToArgb();
            }
        }

        /// <summary>
        /// 生データを格納する BitmapData オブジェクトを取得します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public BitmapData BitmapData
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.data;
            }
        }

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
        /// ビットマップデータの先頭ポインタを取得します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public IntPtr Scan0
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.scan0;
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
        /// ビットマップとフラグをもとに BitmapController クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="bitmap">元となるビットマップ。</param>
        /// <param name="flags">メモリロックフラグ。</param>
        /// <exception cref="ArgumentNullException">bitmap に null が指定されました。</exception>
        public BitmapController(Bitmap bitmap, ImageLockMode flags)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            this.bitmap = bitmap;
            this.data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), flags, PixelFormat.Format32bppArgb);
            this.scan0 = data.Scan0;
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// このオブジェクトで使用されているすべてのリソースを解放します。ビットマップは解放されません。
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
        /// この操作で元となった Bitmap オブジェクトは解放されません。
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
                    if (this.data != null)
                        this.bitmap.UnlockBits(this.data);

                    // このクラスではビットマップを解放しない
                    // if (this.bitmap != null)
                    //    this.bitmap.Dispose();
                }

                this.data = null;
                this.bitmap = null;
                this.scan0 = IntPtr.Zero;

                this.isDisposed = true;
            }
        }
        #endregion

        #region -- Destructors --
        ~BitmapController()
        {
            this.Dispose(false);
        }
        #endregion
    }
}