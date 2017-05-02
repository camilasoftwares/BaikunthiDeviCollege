using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

[ServiceContract(Namespace = "")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class FileUpload
{
    [OperationContract(Action = "Post")]
    public UploadedFile Upload(Stream Uploading)
    {
        try
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                Uploading.CopyTo(memStream);
                Byte[] bytes = memStream.ToArray();
                CollegeMSEntities cme = new CollegeMSEntities();
                Upload upload = cme.Uploads.Create();
                upload.File = bytes;
                upload.CreatedDate = DateTime.Now;
                upload = cme.Uploads.Add(upload);
                cme.SaveChanges();
                return new UploadedFile() { ID = upload.ID, Message = "Uploaded successfully.", ResponseCode = 0 };
            }  
        }
        catch (Exception ex)
        {
            return new UploadedFile() { UploadType = "Uplod failed", Message = ex.Message, ResponseCode = -1, ID = 0 };
        }
    }

    //[OperationContract(Action = "Post")]
    //public UploadedFile UploadDocument(int RegistrationID, Stream Uploading, string FileName, int ExaminationId)
    //{
    //    try
    //    {
    //        string filePath = FileName;
    //        string filename = Path.GetFileName(filePath);
    //        string ext = Path.GetExtension(filename);
    //        string contenttype = String.Empty;

    //        switch (ext)
    //        {
    //            case ".doc":
    //                contenttype = "application/vnd.ms-word";
    //                break;
    //            case ".docx":
    //                contenttype = "application/vnd.ms-word";
    //                break;
    //            case ".xls":
    //                contenttype = "application/vnd.ms-excel";
    //                break;
    //            case ".xlsx":
    //                contenttype = "application/vnd.ms-excel";
    //                break;
    //            case ".jpg":
    //                contenttype = "image/jpg";
    //                break;
    //            case ".png":
    //                contenttype = "image/png";
    //                break;
    //            case ".gif":
    //                contenttype = "image/gif";
    //                break;
    //            case ".pdf":
    //                contenttype = "application/pdf";
    //                break;
    //        }
    //        if (contenttype == String.Empty)
    //        {
    //            return new UploadedFile() { UploadType = "Registration Documents", Message = "File format not recognized.", ResponseCode = 1, ID = RegistrationID.ToString() };
    //        }

    //        Stream fs = Uploading;
    //        BinaryReader br = new BinaryReader(fs);
    //        Byte[] bytes = br.ReadBytes((Int32)fs.Length);

    //        CollegeMSEntities cme = new CollegeMSEntities();
    //        IlligibilityDocument document = cme.IlligibilityDocuments.Create();
    //        //document.DocImage = bytes;
    //        document.RegistrationID = RegistrationID;
    //        document.ExaminationID = ExaminationId;
    //        document.DocumentName = cme.IlligibilityExaminations.Find(document.ExaminationID).Name + "." + filename.Split('.')[1];

    //        cme.SaveChanges();

    //        return new UploadedFile() { FileName = document.DocumentName, UploadType = "Registration Documents", Message = "Success", ResponseCode = 0, ID = RegistrationID.ToString() };
    //    }
    //    catch (Exception ex)
    //    {
    //        return new UploadedFile() { UploadType = "Registration Documents", Message = ex.Message, ResponseCode = -1, ID = RegistrationID.ToString() };
    //    }
    //}

    [OperationContract(Action = "Post")]
    public UploadedFile Transform(UploadedFile Uploading, string FileName)
    {
        //Uploading.FileName = FileName;
        return Uploading;
    }
}

[DataContract]
public class UploadedFile
{
    [DataMember]
    public int ID { get; set; }

    [DataMember]
    public string UploadType { get; set; }

    [DataMember]
    public string Message { get; set; }

    [DataMember]
    public int ResponseCode { get; set; }
}
