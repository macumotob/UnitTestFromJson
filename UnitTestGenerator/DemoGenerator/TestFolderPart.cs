//  UnitTest class : BeProduct.Core.DataModel.Folder.Folder
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoGenerator
{

    public partial class TestFolder
    {
        public BeProduct.Core.DataModel.Folder.Folder CreateInstance()
        {
            BeProduct.Core.DataModel.Folder.Folder document = new BeProduct.Core.DataModel.Folder.Folder()
            {

                Id = "193ad45b-8563-4f46-9d8f-42612891bf5c",
                CompanyId = "3c9d1526-4a98-4d13-8c36-c0445e0aa483",
                MasterFolder = BeProduct.Core.DataModel.Enums.FolderType.Color,
                FolderType = "Rainbow",
                FolderDescription = "1-10",
                CreatedBy = new BeProduct.Core.DataModel.Info.UserInfo()
                {

                    user_id = "a3fd24ce-0ae1-423d-8a58-e4da6829ecb0",
                    user_name = "fastex"
                }
            ,
                CreatedAt = "2016-03-21T10:05:02.2526184+00:00",
                ModifiedBy = new BeProduct.Core.DataModel.Info.UserInfo()
                {

                    user_id = "62a806f8-7296-4408-9969-4b5e16d95f90",
                    user_name = "imvdesign"
                }
            ,
                ModifiedAt = "2016-06-01T12:12:03.4358839+00:00",
                Active = true,
                Schema = new List<BeProduct.Core.DataModel.Folder.FolderField>()
{
 new BeProduct.Core.DataModel.Folder.FolderField()
{

Id = "a27be371-79de-4ab4-9c34-792058ea81ba",
FieldId = "_1test_date_time",
FieldName = "-1Test_Date-Time",
FieldType = "DateTime",
Prefix = "Schema",
Fixed = false,
Properties =  new Dictionary<System.String,System.Object>()
{

 // element DefaultValue not found,
 // element TooltipMessage not found,
 // element Required not found
}

}

}
            ,
                Custom = new List<BeProduct.Core.DataModel.Folder.FolderField>()
                {

                }
            ,
                System = new List<BeProduct.Core.DataModel.Folder.FolderField>()
{
 new BeProduct.Core.DataModel.Folder.FolderField()
{

Id = "3c074e8d-445d-4f41-804a-84a1ba1a4efe",
FieldId = "header_number",
FieldName = "Header Number",
FieldType = "Text",
Prefix = "null",
Fixed = true,
Properties =  new Dictionary<System.String,System.Object>()
{

 // element DefaultValue not found,
 // element MaxValueCharacters not found,
 // element TooltipMessage not found,
 // element Required not found
}

}
, new BeProduct.Core.DataModel.Folder.FolderField()
{

Id = "20a4761c-5fa4-4c57-aba6-d4f97de9b795",
FieldId = "header_name",
FieldName = "Header Name",
FieldType = "Text",
Prefix = "null",
Fixed = true,
Properties =  new Dictionary<System.String,System.Object>()
{

 // element DefaultValue not found,
 // element MaxValueCharacters not found,
 // element TooltipMessage not found,
 // element Required not found
}

}
, new BeProduct.Core.DataModel.Folder.FolderField()
{

Id = "5babef85-0766-4e72-9475-b71c833cc3ad",
FieldId = "created_by",
FieldName = "Created",
FieldType = "UserLabel",
Prefix = "null",
Fixed = false,
Properties =  new Dictionary<System.String,System.Object>()
{

 // element DataType not found,
 // element Required not found,
 // element DefaultValue not found,
 // element Tooltip not found,
 // element FieldId2 not found,
 // element FieldId3 not found
}

}
, new BeProduct.Core.DataModel.Folder.FolderField()
{

Id = "590045be-fe6d-4364-8007-543372108d37",
FieldId = "modified_by",
FieldName = "Modified",
FieldType = "UserLabel",
Prefix = "null",
Fixed = false,
Properties =  new Dictionary<System.String,System.Object>()
{

 // element DataType not found,
 // element Required not found,
 // element DefaultValue not found,
 // element Tooltip not found,
 // element FieldId2 not found,
 // element FieldId3 not found
}

}
, new BeProduct.Core.DataModel.Folder.FolderField()
{

Id = "e7f893e2-c24b-49b6-a95c-8f6a8bfb10e7",
FieldId = "active",
FieldName = "Active",
FieldType = "TrueFalse",
Prefix = "null",
Fixed = false,
Properties =  new Dictionary<System.String,System.Object>()
{

 // element DataType not found,
 // element DefaultValue not found,
 // element Tooltip not found
}

}
, new BeProduct.Core.DataModel.Folder.FolderField()
{

Id = "0905cdff-17cf-47bd-aa58-6fe801d5f699",
FieldId = "version",
FieldName = "Version",
FieldType = "Text",
Prefix = "null",
Fixed = false,
Properties =  new Dictionary<System.String,System.Object>()
{

 // element DefaultValue not found,
 // element MaxValueCharacters not found,
 // element TooltipMessage not found,
 // element Enabled not found,
 // element Required not found
}

}

}
            ,
                Control = new BeProduct.Core.DataModel.Controls.SchemaControl()
                {

                    // element Form not found,
                    // element Grid not found,
                    // element Search not found,
                    // element List not found
                }
            ,
                permissions = new List<BeProduct.Core.DataModel.Permissions.UserRole>()
{
 new BeProduct.Core.DataModel.Permissions.UserRole()
{

user =  new BeProduct.Core.DataModel.Info.UserInfo()
{

user_id = "a7e0f38c-4536-481d-896a-703aaedb717c",
user_name = "JohnDoe"
}
,
role =  new BeProduct.Core.DataModel.Info.RoleInfo()
{

role_id = "ced122e0-251f-4352-8d26-14f3344a1136",
role_name = "ADMIN"
}
,
UserId = "a7e0f38c-4536-481d-896a-703aaedb717c",
FirstName = "John",
LastName = "Doe",
UserName = "JohnDoe",
full_name = "JohnDoe",
Title = "null",
Active = true,
order = 0

}
, new BeProduct.Core.DataModel.Permissions.UserRole()
{

user =  new BeProduct.Core.DataModel.Info.UserInfo()
{

user_id = "ac67f858-4a95-40c0-8aff-6ccc993d68a0",
user_name = "u.dolan"
}
,
role =  new BeProduct.Core.DataModel.Info.RoleInfo()
{

role_id = "",
role_name = ""
}
,
UserId = "ac67f858-4a95-40c0-8aff-6ccc993d68a0",
FirstName = "Uncle",
LastName = "Dolan",
UserName = "u.dolan",
full_name = "UncleDolan",
Title = "lll",
Active = true,
order = 5

}
, new BeProduct.Core.DataModel.Permissions.UserRole()
{

user =  new BeProduct.Core.DataModel.Info.UserInfo()
{

user_id = "a3fd24ce-0ae1-423d-8a58-e4da6829ecb0",
user_name = "fastex"
}
,
role =  new BeProduct.Core.DataModel.Info.RoleInfo()
{

role_id = "",
role_name = ""
}
,
UserId = "a3fd24ce-0ae1-423d-8a58-e4da6829ecb0",
FirstName = "Oleksiy",
LastName = "Voynov",
UserName = "fastex",
full_name = "OleksiyVoynov",
Title = "111",
Active = true,
order = 4

}
, new BeProduct.Core.DataModel.Permissions.UserRole()
{

user =  new BeProduct.Core.DataModel.Info.UserInfo()
{

user_id = "62a806f8-7296-4408-9969-4b5e16d95f90",
user_name = "imvdesign"
}
,
role =  new BeProduct.Core.DataModel.Info.RoleInfo()
{

role_id = "",
role_name = ""
}
,
UserId = "62a806f8-7296-4408-9969-4b5e16d95f90",
FirstName = "Igor",
LastName = "Morozov",
UserName = "imvdesign",
full_name = "IgorMorozov",
Title = "null",
Active = true,
order = 4

}
, new BeProduct.Core.DataModel.Permissions.UserRole()
{

user =  new BeProduct.Core.DataModel.Info.UserInfo()
{

user_id = "1eb5de33-bf73-4d64-95c9-f5bac011adba",
user_name = "m.tomara"
}
,
role =  new BeProduct.Core.DataModel.Info.RoleInfo()
{

role_id = "",
role_name = ""
}
,
UserId = "1eb5de33-bf73-4d64-95c9-f5bac011adba",
FirstName = "Michael",
LastName = "Tomara",
UserName = "m.tomara",
full_name = "MichaelTomara",
Title = "jjj",
Active = true,
order = 5

}

}

            }
            ; return document;
        }
    } //end of class
} //end of namespace
