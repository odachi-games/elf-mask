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
    public enum SaveGamePath
    {
        PersistentDataPath,
        DataPath
    }

    public static class SaveGame
    {
        public delegate void SaveHandler(object obj, string identifier, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path);
        public delegate void LoadHandler(object loadedObj, string identifier, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path);

        public static event SaveHandler OnSaved;
        public static event LoadHandler OnLoaded;

        public static SaveHandler SaveCallback;
        public static LoadHandler LoadCallback;

        private static ISaveGameSerializer m_Serializer = new SaveGameJsonSerializer();
        private static ISaveGameEncoder m_Encoder = new SaveGameSimpleEncoder();
        private static Encoding m_Encoding = Encoding.UTF8;
        private static bool m_Encode = false;
        private static SaveGamePath m_SavePath = SaveGamePath.PersistentDataPath;
        private static string m_EncodePassword = "h@e#ll$o%^";
        private static bool m_LogError = false;

        public static ISaveGameSerializer Serializer
        {
            get
            {
                if (m_Serializer == null)
                {
                    m_Serializer = new SaveGameJsonSerializer();
                }
                return m_Serializer;
            }
            set { m_Serializer = value; }
        }

        public static ISaveGameEncoder Encoder
        {
            get
            {
                if (m_Encoder == null)
                {
                    m_Encoder = new SaveGameSimpleEncoder();
                }
                return m_Encoder;
            }
            set { m_Encoder = value; }
        }

        public static Encoding DefaultEncoding
        {
            get
            {
                if (m_Encoding == null)
                {
                    m_Encoding = Encoding.UTF8;
                }
                return m_Encoding;
            }
            set { m_Encoding = value; }
        }

        public static bool Encode
        {
            get { return m_Encode; }
            set { m_Encode = value; }
        }

        public static SaveGamePath SavePath
        {
            get { return m_SavePath; }
            set { m_SavePath = value; }
        }

        public static string EncodePassword
        {
            get { return m_EncodePassword; }
            set { m_EncodePassword = value; }
        }

        public static bool LogError
        {
            get { return m_LogError; }
            set { m_LogError = value; }
        }

        public static void Save<T>(string identifier, T obj)
        {
            Save<T>(identifier, obj, Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath);
        }

        public static void Save<T>(string identifier, T obj, bool encode)
        {
            Save<T>(identifier, obj, encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath);
        }

        public static void Save<T>(string identifier, T obj, string encodePassword)
        {
            Save<T>(identifier, obj, Encode, encodePassword, Serializer, Encoder, DefaultEncoding, SavePath);
        }

        public static void Save<T>(string identifier, T obj, ISaveGameSerializer serializer)
        {
            Save<T>(identifier, obj, Encode, EncodePassword, serializer, Encoder, DefaultEncoding, SavePath);
        }

        public static void Save<T>(string identifier, T obj, ISaveGameEncoder encoder)
        {
            Save<T>(identifier, obj, Encode, EncodePassword, Serializer, encoder, DefaultEncoding, SavePath);
        }

        public static void Save<T>(string identifier, T obj, Encoding encoding)
        {
            Save<T>(identifier, obj, Encode, EncodePassword, Serializer, Encoder, encoding, SavePath);
        }

        public static void Save<T>(string identifier, T obj, SaveGamePath savePath)
        {
            Save<T>(identifier, obj, Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, savePath);
        }

        public static void Save<T>(string identifier, T obj, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new System.ArgumentNullException("identifier");
            }
            if (serializer == null)
            {
                serializer = SaveGame.Serializer;
            }
            if (encoding == null)
            {
                encoding = SaveGame.DefaultEncoding;
            }

            string filePath = GetFilePath(identifier, path);

            if (obj == null)
            {
                obj = default(T);
            }

            Stream stream = null;

#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
#if UNITY_WSA || UNITY_WINRT
            UnityEngine.Windows.Directory.CreateDirectory(filePath);
#else
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
#endif
#endif

            if (encode)
            {
                stream = new MemoryStream();
            }
            else
            {
#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
                if (IOSupported())
                {
#if UNITY_WSA || UNITY_WINRT
                    stream = new MemoryStream();
#else
                    stream = File.Create(filePath);
#endif
                }
                else
                {
                    stream = new MemoryStream();
                }
#else
                stream = new MemoryStream();
#endif
            }

            serializer.Serialize(obj, stream, encoding);

            if (encode)
            {
                string data = System.Convert.ToBase64String(((MemoryStream)stream).ToArray());
                string encoded = encoder.Encode(data, password);
#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
                if (IOSupported())
                {
#if UNITY_WSA || UNITY_WINRT
                    UnityEngine.Windows.File.WriteAllBytes(filePath, encoding.GetBytes(encoded));
#else
                    File.WriteAllText(filePath, encoded, encoding);
#endif
                }
                else
                {
                    PlayerPrefs.SetString(filePath, encoded);
                    PlayerPrefs.Save();
                }
#else
                PlayerPrefs.SetString(filePath, encoded);
                PlayerPrefs.Save();
#endif
            }
            else if (!IOSupported())
            {
                string data = encoding.GetString(((MemoryStream)stream).ToArray());
                PlayerPrefs.SetString(filePath, data);
                PlayerPrefs.Save();
            }

            stream.Dispose();
            SaveCallback?.Invoke(obj, identifier, encode, password, serializer, encoder, encoding, path);
            OnSaved?.Invoke(obj, identifier, encode, password, serializer, encoder, encoding, path);
        }

        public static T Load<T>(string identifier)
        {
            return Load<T>(identifier, default(T), Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath);
        }

        public static T Load<T>(string identifier, T defaultValue)
        {
            return Load<T>(identifier, defaultValue, Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath);
        }

        public static T Load<T>(string identifier, T defaultValue, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new System.ArgumentNullException("identifier");
            }
            if (serializer == null) serializer = SaveGame.Serializer;
            if (encoding == null) encoding = SaveGame.DefaultEncoding;

            T result = defaultValue;
            string filePath = GetFilePath(identifier, path);

            if (!Exists(filePath, path))
            {
                if (m_LogError)
                {
                    Debug.LogWarningFormat("Identifier {0} does not exist. Returning default.", identifier);
                }
                return result;
            }

            Stream stream = null;
            if (encode)
            {
                string data = "";
#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
                if (IOSupported())
                {
#if UNITY_WSA || UNITY_WINRT
                    data = encoding.GetString(UnityEngine.Windows.File.ReadAllBytes(filePath));
#else
                    data = File.ReadAllText(filePath, encoding);
#endif
                }
                else
                {
                    data = PlayerPrefs.GetString(filePath);
                }
#else
                data = PlayerPrefs.GetString(filePath);
#endif
                string decoded = encoder.Decode(data, password);
                stream = new MemoryStream(System.Convert.FromBase64String(decoded), true);
            }
            else
            {
#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
                if (IOSupported())
                {
#if UNITY_WSA || UNITY_WINRT
                    stream = new MemoryStream(UnityEngine.Windows.File.ReadAllBytes(filePath));
#else
                    stream = File.OpenRead(filePath);
#endif
                }
                else
                {
                    string data = PlayerPrefs.GetString(filePath);
                    stream = new MemoryStream(encoding.GetBytes(data));
                }
#else
                string data = PlayerPrefs.GetString(filePath);
                stream = new MemoryStream(encoding.GetBytes(data));
#endif
            }

            result = serializer.Deserialize<T>(stream, encoding);
            stream.Dispose();

            if (result == null) result = defaultValue;

            LoadCallback?.Invoke(result, identifier, encode, password, serializer, encoder, encoding, path);
            OnLoaded?.Invoke(result, identifier, encode, password, serializer, encoder, encoding, path);

            return result;
        }

        public static bool Exists(string identifier)
        {
            return Exists(identifier, SavePath);
        }

        public static bool Exists(string identifier, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier)) throw new System.ArgumentNullException("identifier");
            string filePath = GetFilePath(identifier, path);

#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
            if (IOSupported())
            {
                bool exists = false;
#if UNITY_WSA || UNITY_WINRT
                exists = UnityEngine.Windows.Directory.Exists(filePath) || UnityEngine.Windows.File.Exists(filePath);
#else
                exists = Directory.Exists(filePath) || File.Exists(filePath);
#endif
                return exists;
            }
            return PlayerPrefs.HasKey(filePath);
#else
            return PlayerPrefs.HasKey(filePath);
#endif
        }

        public static void Delete(string identifier)
        {
            Delete(identifier, SavePath);
        }

        public static void Delete(string identifier, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier)) throw new System.ArgumentNullException("identifier");
            string filePath = GetFilePath(identifier, path);

            if (!Exists(filePath, path)) return;

#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
            if (IOSupported())
            {
#if UNITY_WSA || UNITY_WINRT
                if (UnityEngine.Windows.File.Exists(filePath)) UnityEngine.Windows.File.Delete(filePath);
                else if (UnityEngine.Windows.Directory.Exists(filePath)) UnityEngine.Windows.Directory.Delete(filePath, true);
#else
                if (File.Exists(filePath)) File.Delete(filePath);
                else if (Directory.Exists(filePath)) Directory.Delete(filePath, true);
#endif
            }
            else PlayerPrefs.DeleteKey(filePath);
#else
            PlayerPrefs.DeleteKey(filePath);
#endif
        }

        public static void Clear() => DeleteAll(SavePath);

        public static void DeleteAll(SaveGamePath path)
        {
            string dirPath = (path == SaveGamePath.PersistentDataPath) ? Application.persistentDataPath : Application.dataPath;

#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
            if (IOSupported())
            {
#if UNITY_WSA || UNITY_WINRT
                UnityEngine.Windows.Directory.Delete(dirPath);
#else
                DirectoryInfo info = new DirectoryInfo(dirPath);
                foreach (FileInfo file in info.GetFiles()) file.Delete();
                foreach (DirectoryInfo dir in info.GetDirectories()) dir.Delete(true);
#endif
            }
            else PlayerPrefs.DeleteAll();
#else
            PlayerPrefs.DeleteAll();
#endif
        }

        private static string GetFilePath(string identifier, SaveGamePath path)
        {
            if (IsFilePath(identifier)) return identifier;
            string root = (path == SaveGamePath.PersistentDataPath) ? Application.persistentDataPath : Application.dataPath;
            return string.Format("{0}/{1}", root, identifier);
        }

        public static bool IOSupported()
        {
            return Application.platform != RuntimePlatform.WebGLPlayer &&
                   Application.platform != RuntimePlatform.WSAPlayerARM &&
                   Application.platform != RuntimePlatform.WSAPlayerX64 &&
                   Application.platform != RuntimePlatform.WSAPlayerX86 &&
#if !UNITY_2017_3_OR_NEWER
                   Application.platform != RuntimePlatform.SamsungTVPlayer &&
#endif
                   Application.platform != RuntimePlatform.tvOS &&
                   Application.platform != RuntimePlatform.PS4;
        }

        public static bool IsFilePath(string str)
        {
            bool result = false;
#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
            if (Path.IsPathRooted(str))
            {
                try
                {
                    Path.GetFullPath(str);
                    result = true;
                }
                catch { result = false; }
            }
#endif
            return result;
        }
    }
}