using Microsoft.Azure;
using Microsoft.SharePoint.Client;
using System.IO;
using System.Security;

namespace AXSMarineDataLoader
{
    internal class SharePointMethods
    {
        internal static void UploadFileToSharePoint(string clientSubFolder, string fileName, byte[] FileContent)
        {
            var siteUrl = CloudConfigurationManager.GetSetting("SharepointSite");
            var docLibrary = CloudConfigurationManager.GetSetting("SharepointLibrary");
            var login = CloudConfigurationManager.GetSetting("SharepointUserName");
            var password = EncDec.Decrypt(CloudConfigurationManager.GetSetting("SharepointPassword"), "Alo#B1pu_4Sj$G");

            var securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }
            var onlineCredentials = new SharePointOnlineCredentials(login, securePassword);

            using (ClientContext CContext = new ClientContext(siteUrl))
            {
                CContext.Credentials = onlineCredentials;
                Web web = CContext.Web;

                var newFile = new FileCreationInformation
                {
                    ContentStream = new MemoryStream(FileContent),
                    Url = fileName
                };

                List DocumentLibrary = web.Lists.GetByTitle(docLibrary);
                Folder clientFolder = DocumentLibrary.RootFolder.Folders.Add(clientSubFolder);
                clientFolder.Update();

                CContext.Load(DocumentLibrary);
                CContext.Load(clientFolder.Files);
                CContext.ExecuteQuery();

                foreach (var fl in clientFolder.Files)
                {
                    if (fl.Name == fileName)
                    {
                        fl.DeleteObject();
                        CContext.ExecuteQuery();
                    }
                }

                var uploadFile = clientFolder.Files.Add(newFile);

                CContext.Load(uploadFile);
                CContext.ExecuteQuery();
            }
        }
    }
}
