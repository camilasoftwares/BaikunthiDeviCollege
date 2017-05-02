using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Drawing;
using System.Globalization;

[ServiceContract(Namespace = "")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class CollegeService
{
    // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
    // To create an operation that returns XML,
    //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
    //     and include the following line in the operation body:
    //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
    [OperationContract]
    public void DoWork()
    {
        // Add your operation implementation here
        return;
    }

    [OperationContract(Action = "Get")]
    public string GetCategories()
    {

        CollegeMSEntities cme = new CollegeMSEntities();
        var categoryList = from p in cme.Categories
                         where p.OrgId == 1
                         select new { p.ID, p.Name };

        return Newtonsoft.Json.JsonConvert.SerializeObject(categoryList.ToList());
    }

    [OperationContract(Action ="Get")]
    public string GetCourses()
    {
        CollegeMSEntities cme = new CollegeMSEntities();
        var courseList = from p in cme.Courses
                         where p.OrgId == 1
                         select new { p.ID, p.Name };
                         
        return Newtonsoft.Json.JsonConvert.SerializeObject(courseList.ToList());
    }

    [OperationContract(Action = "Post")]
    public string SaveRegistration(string FName, string LName, string DOBDD, string DOBMM, string DOBYY, string CourseAppliedFor, string Email, string MobileNumber, string Category,
       string Nationality, string Address, string District, string Pincode, string DurationInUP, string MotherName, string FatherName, string MotherOccupation, string FatherOccupation, 
       string MotherIncome, string FatherIncome, string MotherMobile, string FatherMobile, string Cast, string PhoneNumber, string Religion, int IsMinority)
    {
        decimal MIncome, FIncome;
        decimal.TryParse(MotherIncome, out MIncome);
        decimal.TryParse(FatherIncome, out FIncome);
        using (CollegeMSEntities cme = new CollegeMSEntities())
        {
            Registration registration = cme.Registrations.Create();
            registration.FirstName = FName;
            registration.LastName = LName;
            registration.DateOfBirth = DateTime.ParseExact(DOBDD + "/" + DOBMM + "/" + DOBYY, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            registration.CourseId = long.Parse(CourseAppliedFor);
            registration.Email = Email;
            registration.MobileNumber = MobileNumber;
            registration.CategoryID = int.Parse(Category);
            registration.Nationality = Nationality;
            registration.AddressLine1 = Address;
            registration.AddressLine2 = District;
            registration.Pincode = Pincode;
            registration.Duration_in_UP = decimal.Parse(DurationInUP);
            registration.MotherName = MotherName;
            registration.FatherName = FatherName;
            registration.MotherOccupation = MotherOccupation;
            registration.FatherOccupation = FatherOccupation;
            registration.MotherIncome = MIncome;
            registration.FatherIncome = FIncome;
            registration.MotherMobileNumber = MotherMobile;
            registration.FatherMobileNumber = FatherMobile;
            registration.Cast = Cast;
            registration.PhoneNumber = PhoneNumber;
            registration.Religion = Religion;
            registration.IsMinority = (1== IsMinority); 

            registration = cme.Registrations.Add(registration);
            cme.SaveChanges();
            return Newtonsoft.Json.JsonConvert.SerializeObject(new {ID=registration.ID, FirstName = registration.FirstName, LastName = registration.LastName, ResponseCode=0, Message="Success" });
        }
            
        
    }

    [OperationContract(Action = "Post")]
    public string GetRegistration(int RegistrationId)
    {
        using (CollegeMSEntities cme = new CollegeMSEntities())
        {
            Registration registration = cme.Registrations.Find(RegistrationId);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                ID = registration.ID,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                DOB = registration.DateOfBirth != null ? registration.DateOfBirth.Value.ToString("dd/MM/yyyy") : "00/00/00",
                CourseId = registration.CourseId != null ? registration.CourseId.Value : 0,
                Email = registration.Email,
                MobileNumber = registration.MobileNumber,
                CategoryID = registration.CategoryID != null ? registration.CategoryID.Value : 0,
                Nationality = registration.Nationality,
                AddressLine1 = registration.AddressLine1,
                AddressLine2 = registration.AddressLine2,
                Pincode = registration.Pincode,
                Duration_in_UP = registration.Duration_in_UP != null ? registration.Duration_in_UP.Value.ToString() : "",
                MotherName = registration.MotherName,
                FatherName = registration.FatherName,
                MotherOccupation = registration.MotherOccupation,
                FatherOccupation = registration.FatherOccupation,
                MotherIncome = registration.MotherIncome!=null ? registration.MotherIncome.Value.ToString() : "",
                FatherIncome = registration.FatherIncome!=null ? registration.FatherIncome.Value.ToString() : "",
                MotherMobileNumber = registration.MotherMobileNumber,
                FatherMobileNumber = registration.FatherMobileNumber,
                Cast = registration.Cast,
                PhoneNumber = registration.PhoneNumber,
                Religion = registration.Religion,
                IsMinority = (registration.IsMinority!=null && registration.IsMinority.Value)?1:0,
                ResponseCode = 0,
                Message = "Success"
            });
        }
    }

    [OperationContract(Action = "Post")]
    public string UpdateRegistration(int RegistrationId, int UploadId, string FilePath)
    {
        try
        {
            string filename = Path.GetFileName(FilePath);
            string ext = Path.GetExtension(filename);
            string contenttype = String.Empty;

            switch (ext)
            {
                case ".doc":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".docx":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".xls":
                    contenttype = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    contenttype = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                    contenttype = "image/jpg";
                    break;
                case ".png":
                    contenttype = "image/png";
                    break;
                case ".gif":
                    contenttype = "image/gif";
                    break;
                case ".pdf":
                    contenttype = "application/pdf";
                    break;
            }
            if (contenttype == String.Empty)
            {
                return "File format not recognized.";
            }

            CollegeMSEntities cme = new CollegeMSEntities();
            Registration registration = cme.Registrations.Find(RegistrationId);
            registration.UploadId = UploadId;
            registration.PhotoFileName = registration.FirstName + "_" + registration.LastName + "_" + RegistrationId + ext;
            cme.SaveChanges();
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    // Add more operations here and mark them with [OperationContract]
}
