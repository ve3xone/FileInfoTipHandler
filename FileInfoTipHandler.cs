using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Attributes;
using SharpShell;
using SharpShell.SharpInfoTipHandler;

namespace FolderInfoTipHandler
{
    /// <summary>
    /// The FolderInfoTip handler is an example SharpInfoTipHandler that provides an info tip
    /// for folders that shows the number of items in the folder.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".exe", ".dll", ".sys")]
    public class FileInfoTipHandler : SharpInfoTipHandler
    {
        /// <summary>
        /// Gets info for the selected item (SelectedItemPath).
        /// </summary>
        /// <param name="infoType">Type of info to return.</param>
        /// <param name="singleLine">if set to <c>true</c>, put the info in a single line.</param>
        /// <returns>
        /// Specified info for the selected file.
        /// </returns>
        protected override string GetInfo(RequestedInfoType infoType, bool singleLine)
        {
            //  Switch on the tip of info we need to provide.
            switch (infoType)
            {
                case RequestedInfoType.InfoTip:
                    using (Process process = Process.Start(new ProcessStartInfo {
                               FileName = "E:\\Apps\\retoolkit\\die\\diec.exe",
                               Arguments = $"\"{Path.GetFullPath(SelectedItemPath)}\"",
                               UseShellExecute = false,
                               CreateNoWindow = true,
                               RedirectStandardOutput = true
                           })){
                        //  Format the formatted info tip.
                        return string.Format(/*singleLine*/proverka(Path.GetFullPath(SelectedItemPath)) + "Date created: " + File.GetCreationTime(Path.GetFullPath(SelectedItemPath)) + "\nSize: " + sizeConver(Path.GetFullPath(SelectedItemPath)) + "\nDIEC: \n" + process.StandardOutput.ReadToEnd());
                    }

                case RequestedInfoType.Name:
                    
                    //  Return the name of the folder.
                    return string.Format("Folder '{0}'", Path.GetFileName(SelectedItemPath));
                    
                default:

                    //  We won't be asked for anything else, like shortcut paths, for folders, so we 
                    //  can return an empty string in the default case.
                    return string.Empty;
            }
        }
        public string sizeConver(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                FileInfo info = new FileInfo(filePath);
                long size = info.Length;
                string[] sizeletters = new string[] { "bytes", "KB", "MB", "GB", "TB" };
                for (int i = 0; i < 5; i++)
                {
                    if (size < 1024)
                    {
                        string fileSize = size.ToString() + sizeletters[i];
                        return fileSize;
                    }
                    size /= 1024;
                }
            }
            return "";
        } 
        public string proverka(string path)
        {
            string fdes;
            string compname;
            string fver;
            if (FileVersionInfo.GetVersionInfo(path).FileDescription == null)
            {
                fdes = null;
            }
            else if (FileVersionInfo.GetVersionInfo(path).FileDescription == "")
            {
                fdes = null;
            }
            else
            {
                fdes = "File Description: " + FileVersionInfo.GetVersionInfo(path).FileDescription + "\n";
            }
            if (FileVersionInfo.GetVersionInfo(path).CompanyName == null)
            {
                compname = null;
            }
            else if (FileVersionInfo.GetVersionInfo(path).CompanyName == null)
            {
                compname = null;
            }
            else
            {
                compname = "Company: " + FileVersionInfo.GetVersionInfo(path).CompanyName + "\n";
            }
            if (FileVersionInfo.GetVersionInfo(path).FileVersion == null)
            {
                fver = null;
            }
            else if (FileVersionInfo.GetVersionInfo(path).FileVersion == null)
            {
                fver = null;
            }
            else
            {
                fver = "Version: " + FileVersionInfo.GetVersionInfo(path).FileVersion + "\n";
            }
            return fdes + compname + fver;
        }
    }
}
