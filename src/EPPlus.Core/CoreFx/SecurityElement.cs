using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security
{

    public sealed class SecurityElement
    {
        private delegate void ToStringHelperFunc(object obj, string str);

        private const string s_strIndent = "   ";

        private const int c_AttributesTypical = 8;

        private const int c_ChildrenTypical = 1;

        internal string m_strTag;

        internal string m_strText;

        private ArrayList m_lChildren;

        internal ArrayList m_lAttributes;

        private static readonly char[] s_tagIllegalCharacters = new char[]
        {
            ' ',
            '<',
            '>'
        };

        private static readonly char[] s_textIllegalCharacters = new char[]
        {
            '<',
            '>'
        };

        private static readonly char[] s_valueIllegalCharacters = new char[]
        {
            '<',
            '>',
            '"'
        };

        private static readonly string[] s_escapeStringPairs = new string[]
        {
            "<",
            "&lt;",
            ">",
            "&gt;",
            "\"",
            "&quot;",
            "'",
            "&apos;",
            "&",
            "&amp;"
        };

        private static readonly char[] s_escapeChars = new char[]
        {
            '<',
            '>',
            '"',
            '\'',
            '&'
        };

        /// <summary>Gets or sets the tag name of an XML element.</summary>
        /// <returns>The tag name of an XML element.</returns>
        /// <exception cref="T:System.ArgumentNullException">The tag is null. </exception>
        /// <exception cref="T:System.ArgumentException">The tag is not valid in XML. </exception>
        public string Tag
        {
            get
            {
                return this.m_strTag;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Tag");
                }
                if (!SecurityElement.IsValidTag(value))
                {
                    throw new ArgumentException(string.Format("Argument_InvalidElementTag", new object[]
                    {
                        value
                    }));
                }
                this.m_strTag = value;
            }
        }




        internal ArrayList InternalChildren
        {
            get
            {
                return this.m_lChildren;
            }
        }

        internal SecurityElement()
        {
        }








        /// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityElement" /> class with the specified tag and text.</summary>
        /// <param name="tag">The tag name of the XML element. </param>
        /// <param name="text">The text content within the element. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="tag" /> parameter is null. </exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="tag" /> parameter or <paramref name="text" /> parameter is invalid in XML. </exception>
        public SecurityElement(string tag, string text)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }
            if (!SecurityElement.IsValidTag(tag))
            {
                throw new ArgumentException(string.Format( "Argument_InvalidElementTag", new object[]
                {
                    tag
                }));
            }
            if (text != null && !SecurityElement.IsValidText(text))
            {
                throw new ArgumentException(string.Format("Argument_InvalidElementText", new object[]
                {
                    text
                }));
            }
            this.m_strTag = tag;
            this.m_strText = text;
        }


        /// <summary>Creates and returns an identical copy of the current <see cref="T:System.Security.SecurityElement" /> object.</summary>
        /// <returns>A copy of the current <see cref="T:System.Security.SecurityElement" /> object.</returns>
        [ComVisible(false)]
        public SecurityElement Copy()
        {
            return new SecurityElement(this.m_strTag, this.m_strText)
            {
                m_lChildren = (this.m_lChildren == null) ? null : new ArrayList(this.m_lChildren),
                m_lAttributes = (this.m_lAttributes == null) ? null : new ArrayList(this.m_lAttributes)
            };
        }

        /// <summary>Determines whether a string is a valid tag.</summary>
        /// <returns>true if the <paramref name="tag" /> parameter is a valid XML tag; otherwise, false.</returns>
        /// <param name="tag">The tag to test for validity. </param>
        public static bool IsValidTag(string tag)
        {
            return tag != null && tag.IndexOfAny(SecurityElement.s_tagIllegalCharacters) == -1;
        }

        /// <summary>Determines whether a string is valid as text within an XML element.</summary>
        /// <returns>true if the <paramref name="text" /> parameter is a valid XML text element; otherwise, false.</returns>
        /// <param name="text">The text to test for validity. </param>
        public static bool IsValidText(string text)
        {
            return text != null && text.IndexOfAny(SecurityElement.s_textIllegalCharacters) == -1;
        }

        /// <summary>Determines whether a string is a valid attribute name.</summary>
        /// <returns>true if the <paramref name="name" /> parameter is a valid XML attribute name; otherwise, false.</returns>
        /// <param name="name">The attribute name to test for validity. </param>
        public static bool IsValidAttributeName(string name)
        {
            return SecurityElement.IsValidTag(name);
        }

        /// <summary>Determines whether a string is a valid attribute value.</summary>
        /// <returns>true if the <paramref name="value" /> parameter is a valid XML attribute value; otherwise, false.</returns>
        /// <param name="value">The attribute value to test for validity. </param>
        public static bool IsValidAttributeValue(string value)
        {
            return value != null && value.IndexOfAny(SecurityElement.s_valueIllegalCharacters) == -1;
        }

        private static string GetEscapeSequence(char c)
        {
            int num = SecurityElement.s_escapeStringPairs.Length;
            for (int i = 0; i < num; i += 2)
            {
                string text = SecurityElement.s_escapeStringPairs[i];
                string result = SecurityElement.s_escapeStringPairs[i + 1];
                if (text[0] == c)
                {
                    return result;
                }
            }
            return c.ToString();
        }

        /// <summary>Replaces invalid XML characters in a string with their valid XML equivalent.</summary>
        /// <returns>The input string with invalid characters replaced.</returns>
        /// <param name="str">The string within which to escape invalid characters. </param>
        public static string Escape(string str)
        {
            if (str == null)
            {
                return null;
            }
            StringBuilder stringBuilder = null;
            int length = str.Length;
            int num = 0;
            while (true)
            {
                int num2 = str.IndexOfAny(SecurityElement.s_escapeChars, num);
                if (num2 == -1)
                {
                    break;
                }
                if (stringBuilder == null)
                {
                    stringBuilder = new StringBuilder();
                }
                stringBuilder.Append(str, num, num2 - num);
                stringBuilder.Append(SecurityElement.GetEscapeSequence(str[num2]));
                num = num2 + 1;
            }
            if (stringBuilder == null)
            {
                return str;
            }
            stringBuilder.Append(str, num, length - num);
            return stringBuilder.ToString();
        }

        private static string GetUnescapeSequence(string str, int index, out int newIndex)
        {
            int num = str.Length - index;
            int num2 = SecurityElement.s_escapeStringPairs.Length;
            for (int i = 0; i < num2; i += 2)
            {
                string result = SecurityElement.s_escapeStringPairs[i];
                string text = SecurityElement.s_escapeStringPairs[i + 1];
                int length = text.Length;
                if (length <= num && string.Compare(text, 0, str, index, length, StringComparison.Ordinal) == 0)
                {
                    newIndex = index + text.Length;
                    return result;
                }
            }
            newIndex = index + 1;
            return str[index].ToString();
        }

        private static string Unescape(string str)
        {
            if (str == null)
            {
                return null;
            }
            StringBuilder stringBuilder = null;
            int length = str.Length;
            int num = 0;
            while (true)
            {
                int num2 = str.IndexOf('&', num);
                if (num2 == -1)
                {
                    break;
                }
                if (stringBuilder == null)
                {
                    stringBuilder = new StringBuilder();
                }
                stringBuilder.Append(str, num, num2 - num);
                stringBuilder.Append(SecurityElement.GetUnescapeSequence(str, num2, out num));
            }
            if (stringBuilder == null)
            {
                return str;
            }
            stringBuilder.Append(str, num, length - num);
            return stringBuilder.ToString();
        }

        private static void ToStringHelperStringBuilder(object obj, string str)
        {
            ((StringBuilder)obj).Append(str);
        }

        private static void ToStringHelperStreamWriter(object obj, string str)
        {
            ((StreamWriter)obj).Write(str);
        }


        /// <summary>Finds an attribute by name in an XML element.</summary>
        /// <returns>The value associated with the named attribute, or null if no attribute with <paramref name="name" /> exists.</returns>
        /// <param name="name">The name of the attribute for which to search. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
        public string Attribute(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (this.m_lAttributes == null)
            {
                return null;
            }
            int count = this.m_lAttributes.Count;
            for (int i = 0; i < count; i += 2)
            {
                string a = (string)this.m_lAttributes[i];
                if (string.Equals(a, name))
                {
                    string str = (string)this.m_lAttributes[i + 1];
                    return SecurityElement.Unescape(str);
                }
            }
            return null;
        }

        /// <summary>Finds a child by its tag name.</summary>
        /// <returns>The first child XML element with the specified tag value, or null if no child element with <paramref name="tag" /> exists.</returns>
        /// <param name="tag">The tag for which to search in child elements. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="tag" /> parameter is null. </exception>
        public SecurityElement SearchForChildByTag(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }
            if (this.m_lChildren == null)
            {
                return null;
            }
            IEnumerator enumerator = this.m_lChildren.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SecurityElement securityElement = (SecurityElement)enumerator.Current;
                if (securityElement != null && string.Equals(securityElement.Tag, tag))
                {
                    return securityElement;
                }
            }
            return null;
        }
#if false
        internal IPermission ToPermission(bool ignoreTypeLoadFailures)
        {
            IPermission permission = XMLUtil.CreatePermission(this, PermissionState.None, ignoreTypeLoadFailures);
            if (permission == null)
            {
                return null;
            }
            permission.FromXml(this);
            PermissionToken.GetToken(permission);
            return permission;
        }

        internal object ToSecurityObject()
        {
            string strTag;
            if ((strTag = this.m_strTag) != null && strTag == "PermissionSet")
            {
                PermissionSet permissionSet = new PermissionSet(PermissionState.None);
                permissionSet.FromXml(this);
                return permissionSet;
            }
            return this.ToPermission(false);
        }
#endif
        internal string SearchForTextOfLocalName(string strLocalName)
        {
            if (strLocalName == null)
            {
                throw new ArgumentNullException("strLocalName");
            }
            if (this.m_strTag == null)
            {
                return null;
            }
            if (this.m_strTag.Equals(strLocalName) || this.m_strTag.EndsWith(":" + strLocalName, StringComparison.Ordinal))
            {
                return SecurityElement.Unescape(this.m_strText);
            }
            if (this.m_lChildren == null)
            {
                return null;
            }
            IEnumerator enumerator = this.m_lChildren.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string text = ((SecurityElement)enumerator.Current).SearchForTextOfLocalName(strLocalName);
                if (text != null)
                {
                    return text;
                }
            }
            return null;
        }

    }
}
