



















// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `ConStr_Primary`
//     Provider:               `System.Data.SqlClient`
//     Connection String:      `Data Source=.;Initial Catalog=Vote;Integrated Security=True`
//     Schema:                 ``
//     Include Views:          `True`



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPoco;

namespace MVote
{

	public partial class VoteDB : Database
	{
		public VoteDB() 
			: base("ConStr_Primary")
		{
			CommonConstruct();
		}

        public VoteDB(string connectionStringName) 
			: base(connectionStringName)
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
            VoteDB GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static VoteDB GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
                return new VoteDB();
        }

        [ThreadStatic]
        static VoteDB _instance;
		
		protected override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}

        protected override void OnCompleteTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
        

	}
	



    

	[TableName("dbo.UserInfo")]



	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class UserInfo  
    {



		[Column] public int ID { get; set; }





		[Column] public string UserName { get; set; }





		[Column] public string UserDese { get; set; }





		[Column] public string UserImg { get; set; }





		[Column] public int? UserIndex { get; set; }





		[Column] public int? VsID { get; set; }





		[Column] public int? ForB { get; set; }





		[Column] public string Numer { get; set; }





		[Column] public string Hospital { get; set; }





		[Column] public string Mobile { get; set; }





		[Column] public string UserTitle { get; set; }
        [ResultColumn]
        public int SumVote { get; set; }
	}

    

	[TableName("dbo.Vote_session")]



	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class Vote_session  
    {



		[Column] public int ID { get; set; }





		[Column] public string Title { get; set; }





		[Column] public string TitleDesc { get; set; }





		[Column] public string FaceTitle { get; set; }





		[Column] public string BackTitle { get; set; }





		[Column] public bool? VState { get; set; }





		[Column] public DateTime? StartDate { get; set; }





		[Column] public DateTime? EndSDate { get; set; }





		[Column] public int? VIndex { get; set; }



	}

    

	[TableName("dbo.Vote_User")]



	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class Vote_User  
    {



		[Column] public int ID { get; set; }





		[Column] public int? UserID { get; set; }





		[Column] public int? VsID { get; set; }





		[Column] public DateTime? CreateDate { get; set; }





		[Column] public string Vscode { get; set; }



	}

    

	[TableName("dbo.VsCode")]



	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class VsCode  
    {



		[Column] public int ID { get; set; }





		[Column] public string Vscode { get; set; }



	}

    

	[TableName("dbo.View_Tb")]



	[ExplicitColumns]
    public partial class View_Tb  
    {



		[Column] public int? ID { get; set; }





		[Column] public int? VoteIndex { get; set; }





		[Column] public bool? VoteState { get; set; }





		[Column] public string VoteDesc { get; set; }





		[Column] public string VoteTitle { get; set; }



	}


}



