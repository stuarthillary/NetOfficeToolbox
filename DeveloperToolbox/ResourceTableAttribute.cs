﻿using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace NetOffice.DeveloperToolbox
{
    /// <summary>
    /// Contains information about coresponding localization resource file
    /// </summary>
    public class ResourceTableAttribute : System.Attribute
    {
        /// <summary>
        /// Localization Resource File Adress
        /// </summary>
        public readonly string Address;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="address">localization resource file adress</param>
        public ResourceTableAttribute(string address)
        {
            Address = address;
        }

        /// <summary>
        /// Return all names and values for localization based on language
        /// </summary>
        /// <param name="control">instance must have a ResourceTable attribute</param>
        /// <param name="languageID">target language id</param>
        /// <returns>target names and values</returns>
        public static Dictionary<string, string> GetResourceValues(Control control, int languageID)
        {
            Type type = control.GetType();
            Assembly assembly = type.Assembly;
            object[] obj = type.GetCustomAttributes(typeof(ResourceTableAttribute), false);
            ResourceTableAttribute attrib = obj[0] as ResourceTableAttribute;
            return Translation.Translator.GetTranslateResources(control, attrib.Address, languageID);
        }

        /// <summary>
        /// Returns all names for localization
        /// </summary>
        /// <param name="type">type of instance(must have ResourceTableAttribute)</param>
        /// <returns>target names</returns>
        public static string[] GetResourceNames(Type type)
        {
            object[] obj = type.GetCustomAttributes(typeof(ResourceTableAttribute), false);
            ResourceTableAttribute attrib = obj[0] as ResourceTableAttribute;
            Stream stream = type.Assembly.GetManifestResourceStream(type.Assembly.GetName().Name + "." + attrib.Address);
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Dispose();
            stream.Dispose();
            return Translation.Translator.ReadResourceNames(content);
        }
    }
}
