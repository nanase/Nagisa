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
using System.Collections.Generic;
using System.Drawing;

namespace Nagisa.Graphics
{
    /// <summary>
    /// テキストの描画に用いられるオプションを格納します。
    /// </summary>
    public class TextRendererOptions : IDisposer
    {
        #region -- Private Fields --
        private float lineHeight;
        private IList<Brush> brushes;
        private Font font;
        private int shadowIndex = 0;
        private StringFormat format;

        private bool isDisposed = false;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 行の高さを取得または設定します。
        /// </summary>
        public float LineHeight
        {
            get
            {
                return this.lineHeight;
            }
            set
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                    throw new ArgumentOutOfRangeException("value");

                if (value < 0.0)
                    throw new ArgumentOutOfRangeException("value");

                this.lineHeight = value;
            }
        }

        /// <summary>
        /// 描画時に文字に塗りつぶされるブラシオブジェクトのリストを取得します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public IList<Brush> Brushes
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.brushes;
            }
        }

        /// <summary>
        /// アンチエイリアスを使用するかの真偽値を取得または設定します。
        /// </summary>
        public bool Antialiasing { get; set; }

        /// <summary>
        /// 影の描画を行うかの真偽値を取得または設定します。
        /// </summary>
        public bool DrawShadow { get; set; }

        /// <summary>
        /// 影の描画色のインデクスを取得または設定します。
        /// </summary>
        public int ShadowIndex
        {
            get
            {
                return this.shadowIndex;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                this.shadowIndex = value;
            }
        }

        /// <summary>
        /// フォントを取得または設定します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public Font Font
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.font;
            }
            set
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (value == null)
                    throw new ArgumentNullException("value");

                this.font = value;
            }
        }

        /// <summary>
        /// 文字列の描画時に用いられる StringFormat オブジェクトを取得または設定します。
        /// </summary>
        /// <exception cref="ObjectDisposedException">オブジェクトが破棄された後に操作が実行されました。</exception>
        public StringFormat Format
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                return this.format;
            }
            set
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (value == null)
                    throw new ArgumentNullException("value");

                this.format = value;
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
        /// フォントと行の高さを指定して新しい TextOptions クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="font">描画に用いるフォント。</param>
        /// <param name="lineHeight">行の高さ。</param>
        /// <exception cref="ArgumentNullException">font に null が指定されました。</exception>
        /// <exception cref="ArgumentOutOfRangeException">lineHeight に負値が指定されました。</exception>
        public TextRendererOptions(Font font, int lineHeight)
        {
            if (font == null)
                throw new ArgumentNullException("font");

            if (lineHeight < 0)
                throw new ArgumentOutOfRangeException("lineHeight");

            this.font = font;
            this.LineHeight = lineHeight;
            this.brushes = new List<Brush> { new SolidBrush(System.Drawing.Color.White) };
            this.format = new StringFormat(StringFormat.GenericTypographic)
            {
                FormatFlags = StringFormatFlags.NoWrap
            };
        }

        /// <summary>
        /// フォントファミリを指定して新しい TextOptions クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="fontFamily">フォントファミリ。</param>
        /// <param name="fontSize">フォントのサイズ。単位はピクセルです。</param>
        /// <param name="lineHeight">行の高さ。</param>
        /// <param name="style">フォントのスタイル。</param>
        public TextRendererOptions(
            FontFamily fontFamily,
            int fontSize,
            int lineHeight,
            FontStyle style = FontStyle.Regular)
            : this(new Font(fontFamily, fontSize, style, GraphicsUnit.Pixel), lineHeight)
        {
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
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    if (this.font != null)
                        this.font.Dispose();

                    if (this.format != null)
                        this.format.Dispose();

                    if (this.brushes != null)
                        foreach (var brush in this.brushes)
                            brush.Dispose();
                }

                this.font = null;
                this.format = null;
                this.brushes = null;

                this.isDisposed = true;
            }
        }
        #endregion

        #region -- Destructors --
        ~TextRendererOptions()
        {
            this.Dispose(false);
        }
        #endregion
    }
}