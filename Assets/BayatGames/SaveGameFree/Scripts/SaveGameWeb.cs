using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;

using UnityEngine;
using UnityEngine.Networking;

namespace BayatGames.SaveGameFree
{


    public class SaveGameWeb
    {

        private static string m_DefaultUsername = "savegamefree";
        private static string m_DefaultPassword = "$@ve#game%free";
        private static string m_DefaultURL = "http://www.example.com";
        private static bool m_DefaultEncode = false;
        private static string m_DefaultEncodePassword = "h@e#ll$o%^";
        private static ISaveGameSerializer m_DefaultSerializer = new SaveGameJsonSerializer();
        private static ISaveGameEncoder m_DefaultEncoder = new SaveGameSimpleEncoder();
        private static Encoding m_DefaultEncoding = Encoding.UTF8;


        public static string DefaultUsername
        {
            get
            {
                return m_DefaultUsername;
            }
            set
            {
                m_DefaultUsername = value;
            }
        }

        public static string DefaultPassword
        {
            get
            {
                return m_DefaultPassword;
            }
            set
            {
                m_DefaultPassword = value;
            }
        }


        public static string DefaultURL
        {
            get
            {
                return m_DefaultURL;
            }
            set
            {
                m_DefaultURL = value;
            }
        }


        public static bool DefaultEncode
        {
            get
            {
                return m_DefaultEncode;
            }
            set
            {
                m_DefaultEncode = value;
            }
        }

        public static string DefaultEncodePassword
        {
            get
            {
                return m_DefaultEncodePassword;
            }
            set
            {
                m_DefaultEncodePassword = value;
            }
        }


        public static ISaveGameSerializer DefaultSerializer
        {
            get
            {
                if (m_DefaultSerializer == null)
                {
                    m_DefaultSerializer = new SaveGameJsonSerializer();
                }
                return m_DefaultSerializer;
            }
            set
            {
                m_DefaultSerializer = value;
            }
        }

        public static ISaveGameEncoder DefaultEncoder
        {
            get
            {
                if (m_DefaultEncoder == null)
                {
                    m_DefaultEncoder = new SaveGameSimpleEncoder();
                }
                return m_DefaultEncoder;
            }
            set
            {
                m_DefaultEncoder = value;
            }
        }


        public static Encoding DefaultEncoding
        {
            get
            {
                if (m_DefaultEncoding == null)
                {
                    m_DefaultEncoding = Encoding.UTF8;
                }
                return m_DefaultEncoding;
            }
            set
            {
                m_DefaultEncoding = value;
            }
        }

        protected string m_Username;
        protected string m_Password;
        protected string m_URL;
        protected bool m_Encode;
        protected string m_EncodePassword;
        protected ISaveGameSerializer m_Serializer;
        protected ISaveGameEncoder m_Encoder;
        protected Encoding m_Encoding;
        protected UnityWebRequest m_Request;
        protected bool m_IsError = false;
        protected string m_Error = "";


        public virtual string Username
        {
            get
            {
                return this.m_Username;
            }
            set
            {
                this.m_Username = value;
            }
        }


        public virtual string Password
        {
            get
            {
                return this.m_Password;
            }
            set
            {
                this.m_Password = value;
            }
        }


        public virtual string URL
        {
            get
            {
                return this.m_URL;
            }
            set
            {
                this.m_URL = value;
            }
        }


        public virtual bool Encode
        {
            get
            {
                return this.m_Encode;
            }
            set
            {
                this.m_Encode = value;
            }
        }


        public virtual string EncodePassword
        {
            get
            {
                return this.m_EncodePassword;
            }
            set
            {
                this.m_EncodePassword = value;
            }
        }


        public virtual ISaveGameSerializer Serializer
        {
            get
            {
                if (this.m_Serializer == null)
                {
                    this.m_Serializer = new SaveGameJsonSerializer();
                }
                return this.m_Serializer;
            }
            set
            {
                this.m_Serializer = value;
            }
        }


        public virtual ISaveGameEncoder Encoder
        {
            get
            {
                if (this.m_Encoder == null)
                {
                    this.m_Encoder = new SaveGameSimpleEncoder();
                }
                return this.m_Encoder;
            }
            set
            {
                this.m_Encoder = value;
            }
        }


        public virtual Encoding Encoding
        {
            get
            {
                if (this.m_Encoding == null)
                {
                    this.m_Encoding = Encoding.UTF8;
                }
                return this.m_Encoding;
            }
            set
            {
                this.m_Encoding = value;
            }
        }


        public virtual UnityWebRequest Request
        {
            get
            {
                return this.m_Request;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is error.
        /// </summary>
        /// <value><c>true</c> if this instance is error; otherwise, <c>false</c>.</value>
        public virtual bool IsError
        {
            get
            {
                return this.m_IsError;
            }
        }


        public virtual string Error
        {
            get
            {
                return this.m_Error;
            }
        }


        public SaveGameWeb() : this(DefaultUsername)
        {
        }


        public SaveGameWeb(string username) : this(username, DefaultPassword)
        {

        }


        public SaveGameWeb(string username, string password) : this(username, password, DefaultURL)
        {
        }

        public SaveGameWeb(string username, string password, string url) : this(username, password, url, DefaultEncode)
        {
        }


        public SaveGameWeb(string username, string password, string url, bool encode) : this(username, password, url, encode, DefaultEncodePassword)
        {
        }


        public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword) : this(username, password, url, encode, encodePassword, DefaultSerializer)
        {
        }

        public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer) : this(username, password, url, encode, encodePassword, serializer, DefaultEncoder)
        {
        }


        public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer, ISaveGameEncoder encoder) : this(username, password, url, encode, encodePassword, serializer, encoder, DefaultEncoding)
        {
        }


        public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding)
        {
            this.m_Username = username;
            this.m_Password = password;
            this.m_URL = url;
            this.m_Encode = encode;
            this.m_EncodePassword = encodePassword;
            this.m_Serializer = serializer;
            this.m_Encoder = encoder;
            this.m_Encoding = encoding;
        }

        /// <summary>
        /// Save the specified identifier and obj.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public virtual IEnumerator Save<T>(string identifier, T obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            Serializer.Serialize<T>(obj, memoryStream, Encoding);
            string data = Encoding.GetString(memoryStream.ToArray());
            if (Encode)
            {
                data = Encoder.Encode(data, EncodePassword);
            }
            yield return Send(identifier, data, "save");
            if (this.m_IsError)
            {
                Debug.LogError(this.m_Error);
            }
            else
            {
                Debug.Log("Data successfully saved.");
            }
        }

        /// <summary>
        /// Download the specified identifier.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public virtual IEnumerator Download(string identifier)
        {
            yield return Send(identifier, null, "load");
            if (this.m_IsError)
            {
                Debug.LogError(this.m_Error);
            }
            else
            {
                Debug.Log("Data successfully downloaded.");
            }
        }

        /// <summary>
        /// Load the specified identifier.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public virtual T Load<T>(string identifier)
        {
            return Load<T>(identifier, default(T));
        }

        /// <summary>
        /// Load the specified identifier and defaultValue.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public virtual T Load<T>(string identifier, T defaultValue)
        {
            if (defaultValue == null)
            {
                defaultValue = default(T);
            }
            T result = defaultValue;
            if (!this.m_IsError && !string.IsNullOrEmpty(this.m_Request.downloadHandler.text))
            {
                string data = this.m_Request.downloadHandler.text;
                if (Encode)
                {
                    data = Encoder.Decode(data, EncodePassword);
                }
                MemoryStream memoryStream = new MemoryStream(Encoding.GetBytes(data));
                result = Serializer.Deserialize<T>(memoryStream, Encoding);
                memoryStream.Dispose();
                if (result == null)
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        /// <summary>
        /// Send the specified identifier, data and action.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <param name="data">Data.</param>
        /// <param name="action">Action.</param>
        public virtual IEnumerator Send(string identifier, string data, string action)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>() { {
                    "identifier",
                    identifier
                }, {
                    "action",
                    action
                }, {
                    "username",
                    Username
                }
            };
            if (!string.IsNullOrEmpty(data))
            {
                formFields.Add("data", data);
            }
            if (!string.IsNullOrEmpty(Password))
            {
                formFields.Add("password", Password);
            }
            this.m_Request = UnityWebRequest.Post(URL, formFields);
#if UNITY_2019_1_OR_NEWER
            yield return this.m_Request.SendWebRequest();
#else
			yield return m_Request.Send ();
#endif
#if UNITY_2020_1_OR_NEWER
            if (this.m_Request.result != UnityWebRequest.Result.Success)
            {
                this.m_IsError = true;
                this.m_Error = this.m_Request.error;
            }
#elif UNITY_2017_1_OR_NEWER
            if (this.m_Request.isNetworkError || this.m_Request.isHttpError)
            {
                this.m_IsError = true;
                this.m_Error = this.m_Request.error;
            }
#else
			if ( m_Request.isError )
			{
				m_IsError = true;
				m_Error = m_Request.error;
			}
#endif
            else if (this.m_Request.downloadHandler.text.StartsWith("Error"))
            {
                this.m_IsError = true;
                this.m_Error = this.m_Request.downloadHandler.text;
            }
            else
            {
                this.m_IsError = false;
            }
        }

    }

}