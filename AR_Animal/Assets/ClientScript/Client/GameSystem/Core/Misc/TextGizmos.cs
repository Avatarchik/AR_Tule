//-----------------------------------------------------------------------
// <copyright file="TextGizmos.cs" company="Taomee Inc.">
//     Copyright (c) 2011 Taomee Inc. All rights reserved.
// </copyright>
// <author email="ce@sixthsensor.dk">Carl Emil Carlsen</author>
// <author email="alexsu@taomee.com">Su Yong</author>
//-----------------------------------------------------------------------

namespace Misc
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The TextGizmos class provides function to draw text with gizmos.
    /// See http://forum.unity3d.com/threads/27902-Text-Gizmo for details.
    /// See http://csharpindepth.com/Articles/General/Singleton.aspx for
    /// singleton pattern in C#.
    /// </summary>
    public class TextGizmos
    {
        #region Fields

        /// <summary>
        /// The height of the character texture.
        /// </summary>
        public const int CharTextureHeight = 8; // TODO: line breaks

        /// <summary>
        /// The width of the character texture.
        /// </summary>
        public const int CharTextureWidth = 6;

        /// <summary>
        /// The supported characters.
        /// </summary>
        private const string Characters = "abcdefghijklmnopqrstuvwxyz0123456789 !#%(),.'-;_{}~+=:\\?\"/*";

        /// <summary>
        /// The mapping from the character which can not be used in the file name on Windows to the name string.
        /// </summary>
        private static readonly Dictionary<char, string> unsafeChar2NameMapping = new Dictionary<char, string>()
        {
            { ':', "colon" },
            { '\\', "backslash" },
            { '?', "questionmark" },
            { '"', "quotes" },
            { '/', "slash" },
            { '*', "star" },
        };

        /// <summary>
        /// The mapping from character to path string.
        /// </summary>
        private Dictionary<char, string> char2TexturePathMapping;

        /// <summary>
        /// The editor camera.
        /// </summary>
        private Camera editorCamera = null;

        #endregion

        #region Constructors
        /// <summary>
        /// Prevents a default instance of the TextGizmos class from being created.
        /// </summary>
        private TextGizmos()
        {
            this.editorCamera = Camera.current;
            this.char2TexturePathMapping = new Dictionary<char, string>();
            for (int i = 0; i < Characters.Length; ++i)
            {
                char c = Characters[i];
                string name;
                if (!unsafeChar2NameMapping.TryGetValue(c, out name))
                {
                    name = c.ToString();
                }

                this.char2TexturePathMapping.Add(c, "TextGizmos/text_" + name + ".png");
            }
        }
        #endregion

        #region Enumerators
        #endregion

        #region Properties
        /// <summary>
        /// Gets the instance of the singleton.
        /// </summary>
        public static TextGizmos Instance
        {
            get { return Nested.Instance; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Draw text with gizmos.
        /// </summary>
        /// <param name="position">The 3D position of the text.</param>
        /// <param name="text">The text to draw.</param>
        public void Draw(Vector3 position, string text)
        {
            if (this.editorCamera != null)
            {
                string lowerText = text.ToLower();
                Vector3 screenPoint = this.editorCamera.WorldToScreenPoint(position);
                int offset = 20;
                for (int c = 0; c < lowerText.Length; ++c)
                {
                    if (this.char2TexturePathMapping.ContainsKey(lowerText[c]))
                    {
                        Vector3 worldPoint = this.editorCamera.ScreenToWorldPoint(new Vector3(screenPoint.x + offset, screenPoint.y, screenPoint.z));
                        Gizmos.DrawIcon(worldPoint, this.char2TexturePathMapping[lowerText[c]]);
                        offset += CharTextureWidth;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// The nested class.
        /// </summary>
        private class Nested
        {
            /// <summary>
            /// The static member which holds the instance.
            /// </summary>
            internal static readonly TextGizmos Instance = new TextGizmos();

            /// <summary>
            /// Initializes static members of the Nested class.
            /// Explicit static constructor to tell C# compiler
            /// not to mark type as beforefieldinit.
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1409:RemoveUnnecessaryCode", Justification = "ForSingleton")]
            static Nested()
            {
            }
        }
    }
}
