﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dev2.Studio.Core.Interfaces;

namespace Dev2.Studio.Core.InterfaceImplementors
{
    /// <summary>
    /// A directory persistence provider.
    /// </summary>
    public class DirectoryPersistenceProvider : IDirectoryPersistenceProvider
    {
        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        public string DirectoryPath
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets an enumeration of all content from the files in <see cref="DirectoryPath"/>.
        /// </summary>
        /// <returns>An enumeration of all content from the files in <see cref="DirectoryPath"/></returns>
        public IEnumerable<string> Read()
        {
            var di = new DirectoryInfo(DirectoryPath);
            return di.EnumerateFiles().Select(fi => File.ReadAllText(fi.FullName));
        }

        /// <summary>
        /// Writes the given data to a text file.
        /// </summary>
        /// <param name="fileName">The name of the file to be written excluding extension.</param>
        /// <param name="data">The data to be written.</param>
        public void Write(string fileName, string data)
        {
            File.WriteAllText(GetFilePath(fileName), data);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted excluding extension.</param>
        public void Delete(string fileName)
        {
            File.Delete(GetFilePath(fileName));
        }


        /// <summary>
        /// Reads the contents of the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to be read, excluding extension.</param>
        /// <returns>The content of the given file.</returns>
        public string Read(string fileName)
        {
            return File.ReadAllText(GetFilePath(fileName));
        }

        string GetFilePath(string containerName)
        {
            return Path.Combine(DirectoryPath, containerName + ".xml");
        }
    }
}
