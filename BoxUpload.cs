using Box.V2;
using Box.V2.Config;
using Box.V2.Exceptions;
using Box.V2.JWTAuth;
using Box.V2.Managers;
using Box.V2.Models;
using Box.V2.Models.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace BoxUploadLibrary
{
    public class BoxUpload
    {
        const string BOX_FOLDER = "/Documents"; //for this example code, make sure this folder structure exists in Box

        static BoxClient client;
        static BoxSearchManager _searchManager;
        public async Task UploadData(byte[] stringBytes, Dictionary<string, object> metadataValues, string json)
        {
            var config1 = BoxConfig.CreateFromJsonString(json);
            var boxJWT = new BoxJWTAuth(config1);
            var sdk = new BoxJWTAuth(config1);
            var token = sdk.AdminToken();
            client = sdk.AdminClient(token);

            string fileId = "";
            System.Random random = new System.Random();
            var boxFolderId = await FindBoxFolderId(BOX_FOLDER);

            using (MemoryStream stream = new MemoryStream(stringBytes))
            {
                // Create request object with name and parent folder the file should be uploaded to
                BoxFileRequest request = new BoxFileRequest()
                {
                    Name = "File-" + random.Next(10, 50) + ".pdf",
                    Parent = new BoxRequestEntity() { Id = boxFolderId }
                };

                BoxFile file = client.FilesManager.UploadAsync(request, stream).Result;

                fileId = file.Id;

                //Listing all metadata templates
                BoxEnterpriseMetadataTemplateCollection<BoxMetadataTemplate> templates = await client.MetadataManager
    .GetEnterpriseMetadataAsync("enterprise");

                bool bTemplate = false;

                for (int i = 0; i < templates.Entries.Count; i++)
                {
                    if (templates.Entries[1].TemplateKey.ToString() == "ShipmentInfo")
                    {
                        bTemplate = true;
                        break;
                    }
                }

                if (bTemplate == false)
                {
                    //Create a custom metadata template - start
                    var templateParams = new BoxMetadataTemplate()
                    {
                        TemplateKey = "ShipmentInfo",
                        DisplayName = "Shipment Info",
                        Scope = "enterprise",
                        Fields = new List<BoxMetadataTemplateField>()
                {
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SRCLOCATION",
                    DisplayName = "SRCLOCATION"
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPTYPE",
                    DisplayName = "SHIPTYPE",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPNUM",
                    DisplayName = "SHIPNUM",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "BOLNUMBER",
                    DisplayName = "BOLNUMBER",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPDATE",
                    DisplayName = "SHIPDATE",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "APPTMNT",
                    DisplayName = "APPTMNT",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SCAC",
                    DisplayName = "SCAC",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CARRIERNAME",
                    DisplayName = "CARRIERNAME",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "PRONUMBER",
                    DisplayName = "PRONUMBER",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "TRAILERNUMBER",
                    DisplayName = "TRAILERNUMBER",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SEALNUMBER",
                    DisplayName = "SEALNUMBER",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPPER",
                    DisplayName = "SHIPPER",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPADDRESSLINE1",
                    DisplayName = "SHIPADDRESSLINE1",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPADDRESSLINE2",
                    DisplayName = "SHIPADDRESSLINE2",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPCITY",
                    DisplayName = "SHIPCITY",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPSTATE",
                    DisplayName = "SHIPSTATE",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPZIP",
                    DisplayName = "SHIPZIP",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPFOB",
                    DisplayName = "SHIPFOB",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "SHIPCOUNTRY",
                    DisplayName = "SHIPCOUNTRY",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CONSIGNEE",
                    DisplayName = "CONSIGNEE",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CONSIGNEEADDRESSLINE1",
                    DisplayName = "CONSIGNEEADDRESSLINE1",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CONSIGNEEADDRESSLINE2",
                    DisplayName = "CONSIGNEEADDRESSLINE2",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CONSIGNEECITY",
                    DisplayName = "CONSIGNEECITY",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CONSIGNEESTATE",
                    DisplayName = "CONSIGNEESTATE",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CONSIGNEEZIP",
                    DisplayName = "CONSIGNEEZIP",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "CONSIGNEECOUNTRY",
                    DisplayName = "CONSIGNEECOUNTRY",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "UNITS",
                    DisplayName = "UNITS",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "UNITTYPE",
                    DisplayName = "UNITTYPE",
                  },
                  new BoxMetadataTemplateField()
                  {
                    Type = "string",
                    Key = "WEIGHT",
                    DisplayName = "WEIGHT",
                  },
                }
                    };
                    BoxMetadataTemplate template = await client.MetadataManager.CreateMetadataTemplate(templateParams);
                }
                //Create a custom metadata template - End
            }

            Dictionary<string, object> metadata = await client.MetadataManager
                    .CreateFileMetadataAsync(fileId: fileId.ToString(), metadataValues, "enterprise", "ShipmentInfo");

            //return "File uploaded successfully";
        }
        static async Task<String> FindBoxFolderId(string path)
        {
            var folderNames = path.Split('/');
            folderNames = folderNames.Where((f) => !String.IsNullOrEmpty(f)).ToArray(); //get rid of leading empty entry in case of leading slash

            var currFolderId = "0"; //the root folder is always "0"
            foreach (string folderName in folderNames)
            {
                var folderInfo = await client.FoldersManager.GetInformationAsync(currFolderId);
                var foundFolder = folderInfo.ItemCollection.Entries.OfType<BoxFolder>().First((f) => f.Name == folderName);
                currFolderId = foundFolder.Id;
            }

            return currFolderId;
        }
    }
}