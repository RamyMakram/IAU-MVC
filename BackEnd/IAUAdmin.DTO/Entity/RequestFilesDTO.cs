using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class RequestFilesDTO
	{
        public int ID { get; set; }
        public string File_Name { get; set; }
        public string File_Path { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int Request_ID { get; set; }
        public Nullable<int> RequiredDoc_ID { get; set; }

        public virtual RequiredDocsDTO Required_Documents { get; set; }
    }
}
