using System.IO;
using FiveSQD.WebVerse.Utilities;
using UnityEngine;

namespace FiveSQD.WebVerse.Handlers.File
{
    public class FileHandler : BaseHandler
    {
        public string fileDirectory { get; private set; }

        public static string ToFileURI(string uri)
        {
            return uri.Replace(":", "~");

        }

        public static string FromFileURI(string fileURI)
        {
            return fileURI.Replace("http~", "http:").Replace("https~", "https:");
        }

        public override void Initialize()
        {
            Logging.LogError("[FileHandler->Initialize] Initialize must be called with a file directory.");
        }

        public void Initialize(string fileDirectory)
        {
            base.Initialize();

            this.fileDirectory = fileDirectory;
            if (Directory.Exists(this.fileDirectory))
            {
                Logging.Log("[FileHandler->Initialize] File directory exists.");
            }
            else
            {
                Logging.Log("[FileHandler->Initialize] File directory does not exist. Creating...");
                Directory.CreateDirectory(this.fileDirectory);
            }
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        public bool FileExistsInFileDirectory(string file)
        {
            return System.IO.File.Exists(Path.Combine(fileDirectory, file));
        }

        public void CreateFileInFileDirectory(string fileName, byte[] data)
        {
            if (FileExistsInFileDirectory(fileName))
            {
                Logging.LogWarning("[FileHandler->CreateFileInFileDirectory] File already exists: " + fileName);
                return;
            }

            CreateDirectoryStructure(Path.Combine(fileDirectory, fileName));
            System.IO.File.WriteAllBytes(Path.Combine(fileDirectory, fileName), data);
        }

        public void CreateFileInFileDirectory(string fileName, Texture2D image)
        {
            if (FileExistsInFileDirectory(fileName))
            {
                Logging.LogWarning("[FileHandler->CreateFileInFileDirectory] File already exists: " + fileName);
                return;
            }

            CreateDirectoryStructure(Path.Combine(fileDirectory, fileName));

            System.IO.File.WriteAllBytes(Path.Combine(fileDirectory, fileName), image.EncodeToPNG());
        }

        public void DeleteFileInFileDirectory(string fileName)
        {
            if (!FileExistsInFileDirectory(fileName))
            {
                Logging.LogWarning("[FileHandler->DeleteFileInFileDirectory] No file: " + fileName);
                return;
            }

            System.IO.File.Delete(Path.Combine(fileDirectory, fileName));
        }

        public byte[] GetFileInFileDirectory(string fileName)
        {
            if (!FileExistsInFileDirectory(fileName))
            {
                Logging.LogWarning("[FileHandler->GetFileInFileDirectory] No file: " + fileName);
                return null;
            }

            return System.IO.File.ReadAllBytes(Path.Combine(fileDirectory, fileName));
        }

        private void CreateDirectoryStructure(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
        }
    }
}